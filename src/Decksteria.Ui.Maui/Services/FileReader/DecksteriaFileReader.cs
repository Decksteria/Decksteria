namespace Decksteria.Ui.Maui.Services.FileReader;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Networking;
using Microsoft.Maui.Storage;

internal sealed class DecksteriaFileReader : IDecksteriaFileReader
{
    private readonly HttpClient httpClient;

    private readonly string gameName;

    private readonly ILogger<DecksteriaFileReader> logger;

    private List<string> VerifiedFiles;

    public DecksteriaFileReader(string gameName, IHttpClientFactory httpClientFactory, ILogger<DecksteriaFileReader> logger)
    {
        this.httpClient = httpClientFactory.CreateClient();
        this.gameName = gameName;
        this.logger = logger;
        VerifiedFiles = new();
    }

    public string BuildConnectionString(string fileName, IDictionary<string, string> connectionProperties, string downloadURL, string? md5Checksum = null, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public async Task<string> GetFileLocationAsync(string fileName, string downloadURL, string? md5Checksum = null, CancellationToken cancellationToken = default)
    {
        // Gets the file path used by the .NET Maui Application
        var filePath = @$"{FileSystem.AppDataDirectory}\{gameName}\{fileName}";

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
        await DownloadRetryAsync(DownloadAsync, () => VerifyChecksum(filePath, md5Checksum));
        return filePath;

        async Task DownloadAsync()
        {
            using var httpStream = await httpClient.GetStreamAsync(downloadURL, cancellationToken);
            using var fileStream = File.Create(filePath);
            await httpStream.CopyToAsync(fileStream, cancellationToken);
            fileStream.Close();
        }
    }

    public Task<byte[]> ReadByteFileAsync(string fileName, string downloadURL, string? md5Checksum = null, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public Task<byte[]> ReadImageAsync(string fileName, string downloadURL, string? md5Checksum = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
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

    private async Task DownloadRetryAsync(Func<Task> DownloadAsync, Func<Task<bool>> ValidateChecksum)
    {
        // Add custom retry policy for HTTP Request
        for (var i = 0; i < 3; i++)
        {
            try
            {
                await DownloadAsync();
                var checksumValid = await ValidateChecksum();

                if (checksumValid)
                {
                    return;
                }

                logger.LogWarning("File checksum validation did not match. Retry: {RetryCount}.", i);
            }
            catch (HttpRequestException e)
            {
                logger.LogError(e, "File Download failed, {ExceptionMessage}. Retry: {RetryCount}.", e.Message, i);
                continue;
            }
        }
    }

    private async Task<bool> VerifyChecksum(string filePath, string? md5Checksum)
    {
        // If the plug-in did not provide a checksum, always assume it was downloaded correctly.
        // If a file has already been verified, it does not need to be verified again.
        if (string.IsNullOrWhiteSpace(md5Checksum) || VerifiedFiles.Contains(filePath))
        {
            VerifiedFiles.Add(filePath);
            return true;
        }

        // Compute MD5 Checksum
        using var md5 = MD5.Create();
        using var fileStream = File.OpenRead(filePath);
        var checksumBytes = await md5.ComputeHashAsync(fileStream);
        var checksum = BitConverter.ToString(checksumBytes);

        if (StandardiseHash(checksum) == StandardiseHash(md5Checksum))
        {
            // File is already verified and does not need to be re-verified again.
            VerifiedFiles.Add(filePath);
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
