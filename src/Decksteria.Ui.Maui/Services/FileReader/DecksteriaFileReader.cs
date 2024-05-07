namespace Decksteria.Ui.Maui.Services.FileReader;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core.Data;
using Microsoft.Maui.Storage;

internal sealed class DecksteriaFileReader(string gameName, IHttpClientFactory httpClientFactory) : IDecksteriaFileReader
{
    private readonly HttpClient httpClient = httpClientFactory.CreateClient();

    private readonly string gameName = gameName;

    private List<string> VerifiedFiles = new();

    public string BuildConnectionString(string fileName, IDictionary<string, string> connectionProperties, string downloadURL, string? md5Checksum = null, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public async Task<string> GetFileLocationAsync(string fileName, string downloadURL, string? md5Checksum = null, CancellationToken cancellationToken = default)
    {
        // Gets the file path used by the .NET Maui Application
        var filePath = @$"{FileSystem.AppDataDirectory}\{gameName}\{fileName}";
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

        // Download file
        await DownloadAsync(true);

        return filePath;

        async Task DownloadAsync(bool performChecksumValidation)
        {
            using var httpStream = await httpClient.GetStreamAsync(downloadURL, cancellationToken);
            using var fileStream = File.Create(filePath);
            await httpStream.CopyToAsync(fileStream, cancellationToken);
            fileStream.Close();

            // If checksum is not to be validated, or checksum was completed return.
            if (!performChecksumValidation || await VerifyChecksum(filePath, md5Checksum))
            {
                return;
            }

            // If checksum validation failed, download again without checksum validation.
            // If this retry fails, the checksum will be presumed to be parsed in incorrectly.
            await DownloadAsync(false);
            VerifiedFiles.Add(filePath);
        }
    }

    public Task<byte[]> ReadByteFileAsync(string fileName, string downloadURL, string? md5Checksum = null, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public Task<byte[]> ReadImageAsync(string fileName, string downloadURL, string? md5Checksum = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<string> ReadTextFileAsync(string? fileName, string downloadURL, string? md5Checksum = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return await httpClient.GetStringAsync(downloadURL, cancellationToken);
        }

        var fileLocation = await GetFileLocationAsync(fileName, downloadURL, md5Checksum, cancellationToken);
        return await File.ReadAllTextAsync(fileLocation, cancellationToken);
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
            return hash.Replace("-", "").ToUpper();
        }
    }
}
