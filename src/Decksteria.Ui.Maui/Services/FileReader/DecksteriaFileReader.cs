namespace Decksteria.Ui.Maui.Services.FileReader;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core.Data;
using Microsoft.Maui.Storage;

internal sealed class DecksteriaFileReader(string gameName, IHttpClientFactory httpClientFactory) : IDecksteriaFileReader
{
    private readonly HttpClient httpClient = httpClientFactory.CreateClient();

    private readonly string gameName = gameName;

    public string BuildConnectionString(string fileName, IDictionary<string, string> connectionProperties, string downloadURL, string? md5Checksum = null, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public async Task<string> GetFileLocationAsync(string fileName, string downloadURL, string? md5Checksum = null, CancellationToken cancellationToken = default)
    {
        var filePath = @$"{FileSystem.AppDataDirectory}\{gameName}\{fileName}";
        if (!File.Exists(filePath))
        {
            using var httpStream = await httpClient.GetStreamAsync(downloadURL, cancellationToken);
            var directory = Path.GetDirectoryName(filePath);
            if (directory is not null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using var fileStream = File.Create(filePath);
            await httpStream.CopyToAsync(fileStream, cancellationToken);
        }

        return filePath;
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
}
