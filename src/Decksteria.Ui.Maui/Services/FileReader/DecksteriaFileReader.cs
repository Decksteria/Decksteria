﻿namespace Decksteria.Ui.Maui.Services.FileReader;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core.Data;
using Decksteria.Ui.Maui.Services.FileLocator;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Networking;

internal sealed class DecksteriaFileReader : IDecksteriaFileReader
{
    private readonly HttpClient httpClient;

    private readonly IDecksteriaFileLocator fileLocator;

    private readonly ILogger<DecksteriaFileReader> logger;

    private readonly ConcurrentDictionary<string, SemaphoreSlim> lockedFiles;

    private readonly HashSet<string> verifiedFiles;

    public DecksteriaFileReader(IDecksteriaFileLocator fileLocator, IHttpClientFactory httpClientFactory, ILogger<DecksteriaFileReader> logger)
    {
        this.httpClient = httpClientFactory.CreateClient();
        this.fileLocator = fileLocator;
        this.logger = logger;
        lockedFiles = [];
        verifiedFiles = [];
    }

    public string BuildConnectionString(string fileName, IDictionary<string, string> connectionProperties, string downloadURL, string? md5Checksum = null, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public string GetExpectedFileLocation(string fileName)
    {
        return fileLocator.GetExpectedFileLocation(fileName);
    }

    public string GetExpectedImageLocation(string fileName)
    {
        return fileLocator.GetExpectedImageLocation(fileName);
    }

    public async Task<string> GetFileLocationAsync(string fileName, string downloadURL, string? md5Checksum = null, CancellationToken cancellationToken = default)
    {
        // Gets the file path used by the .NET Maui Application
        var filePath = GetExpectedFileLocation(fileName);

        // If the device does not have internet, return the expected file path assuming it was already downloaded.
        // Exception handling for a missing file will be handled on the application side.
        if (!ValidateNetworkAccess())
        {
            return filePath;
        }

        if (File.Exists(filePath) && await VerifyChecksum(filePath, md5Checksum))
        {
            return filePath;
        }

        // Create directory if the directory is missing.
        var directory = Path.GetDirectoryName(filePath);
        if (directory is not null && !Directory.Exists(directory))
        {
            _ = Directory.CreateDirectory(directory);
        }

        // Download file and implement retry policy
        await DownloadRetryAsync(fileName, DownloadAsync, () => VerifyChecksum(filePath, md5Checksum));
        return filePath;

        async Task DownloadAsync()
        {
            using var httpStream = await httpClient.GetStreamAsync(downloadURL, cancellationToken);
            using var fileStream = File.Create(filePath);
            await httpStream.CopyToAsync(fileStream, cancellationToken);
            fileStream.Close();
        }
    }

    public async Task<string> GetImageLocationAsync(string fileName, string downloadURL, string? md5Checksum = null, CancellationToken cancellationToken = default)
    {
        // Gets the file path used by the .NET Maui Application
        var filePath = GetExpectedImageLocation(fileName);

        // If the device does not have internet, return the expected file path assuming it was already downloaded.
        // Exception handling for a missing file will be handled on the application side.
        if (!ValidateNetworkAccess())
        {
            return filePath;
        }

        if (File.Exists(filePath) && await VerifyChecksum(filePath, md5Checksum))
        {
            return filePath;
        }

        // Create directory if the directory is missing.
        var directory = Path.GetDirectoryName(filePath);
        if (directory is not null && !Directory.Exists(directory))
        {
            _ = Directory.CreateDirectory(directory);
        }

        // Download file and implement retry policy
        await DownloadRetryAsync(fileName, DownloadAsync, () => VerifyChecksum(filePath, md5Checksum));
        return filePath;

        async Task DownloadAsync()
        {
            using var httpStream = await httpClient.GetStreamAsync(downloadURL, cancellationToken);
            using var fileStream = File.Create(filePath);
            await httpStream.CopyToAsync(fileStream, cancellationToken);
            fileStream.Close();
        }
    }

    public async Task<byte[]> ReadByteFileAsync(string fileName, string downloadURL, string? md5Checksum = null, CancellationToken cancellationToken = default)
    {
        var filePath = await GetFileLocationAsync(fileName, downloadURL, md5Checksum, cancellationToken);
        return await File.ReadAllBytesAsync(filePath, cancellationToken);
    }

    public async Task<byte[]> ReadImageAsync(string fileName, string downloadURL, string? md5Checksum = null, CancellationToken cancellationToken = default)
    {
        var filePath = await GetImageLocationAsync(fileName, downloadURL, md5Checksum, cancellationToken);
        return await File.ReadAllBytesAsync(filePath, cancellationToken);
    }

    public async Task<string?> ReadOnlineTextAsync(string URL, CancellationToken cancellationToken = default)
    {
        if (!ValidateNetworkAccess())
        {
            return null;
        }

        try
        {
            return await httpClient.GetStringAsync(URL, cancellationToken);
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, e.Message);
            return null;
        }
    }

    public async Task<string> ReadTextFileAsync(string fileName, string downloadURL, string? md5Checksum = null, CancellationToken cancellationToken = default)
    {
        var fileLocation = await GetFileLocationAsync(fileName, downloadURL, md5Checksum, cancellationToken);
        return await File.ReadAllTextAsync(fileLocation, cancellationToken);
    }

    private async Task DownloadRetryAsync(string fileName, Func<Task> DownloadAsync, Func<Task<bool>> ValidateChecksum)
    {
        var fileLock = lockedFiles.GetOrAdd(fileName, new SemaphoreSlim(1, 1));

        await fileLock.WaitAsync();
        if (await ValidateChecksum())
        {
            fileLock.Release();
            lockedFiles.TryRemove(fileName, out _);
            return;
        }

        // Add custom retry policy for HTTP Request
        for (var i = 0; i < 3; i++)
        {
            try
            {
                await DownloadAsync();
                var checksumValid = await ValidateChecksum();
                if (checksumValid)
                {
                    break;
                }

                logger.LogWarning("File checksum validation did not match. Retry: {RetryCount}.", i);
            }
            catch (HttpRequestException e)
            {
                logger.LogError(e, "File Download failed, {ExceptionMessage}. Retry: {RetryCount}.", e.Message, i);
                continue;
            }
        }

        fileLock.Release();
        lockedFiles.TryRemove(fileName, out _);
    }

    private async Task<bool> VerifyChecksum(string filePath, string? md5Checksum)
    {
        // Since there is no file to open validation, simply skip and return false.
        if (!File.Exists(filePath))
        {
            return false;
        }

        // If the plug-in did not provide a checksum, always assume it was downloaded correctly.
        // If a file has already been verified, it does not need to be verified again.
        if (string.IsNullOrWhiteSpace(md5Checksum) || verifiedFiles.Contains(filePath))
        {
            verifiedFiles.Add(filePath);
            return true;
        }

        if (!File.Exists(filePath))
        {
            return false;
        }

        // Compute MD5 Checksum
        using var md5 = MD5.Create();
        using var fileStream = File.OpenRead(filePath);
        var checksumBytes = await md5.ComputeHashAsync(fileStream);
        var checksum = BitConverter.ToString(checksumBytes);

        if (StandardiseHash(checksum) == StandardiseHash(md5Checksum))
        {
            // File is already verified and does not need to be re-verified again.
            verifiedFiles.Add(filePath);
            return true;
        }

        return false;

        static string StandardiseHash(string hash)
        {
            return hash.Replace("-", "").ToUpperInvariant().Trim();
        }
    }

    private bool ValidateNetworkAccess()
    {
        if (Connectivity.Current.NetworkAccess is NetworkAccess.Internet)
        {
            return true;
        }

        logger.LogInformation("The device does not have internet.");
        return false;
    }
}
