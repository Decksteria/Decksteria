namespace Decksteria.Ui.Maui.Services.FileReader;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core.Data;
using Microsoft.Maui.Storage;

internal sealed class DecksteriaFileReader(IHttpClientFactory httpClientFactory) : IDecksteriaFileReader
{
    private readonly HttpClient httpClient = httpClientFactory.CreateClient();

    public string BuildConnectionString(string fileName, string gameName, IDictionary<string, string> connectionProperties, string downloadURL, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public Task<byte[]> ReadImageAsync(string fileName, string gameName, string downloadURL, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetFileLocationAsync(string fileName, string gameName, string downloadURL, CancellationToken cancellationToken = default)
    {
        var filePath = @$"{FileSystem.AppDataDirectory}\{gameName}\{fileName}";
        if (!File.Exists(filePath))
        {
            using var httpStream = await httpClient.GetStreamAsync(downloadURL);
            using var fileStream = File.Create(filePath);
            httpStream.CopyTo(fileStream);
        }

        return filePath;
    }

    public Task<byte[]> ReadByteFileAsync(string fileName, string gameName, string downloadURL, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public async Task<string> ReadTextFileAsync(string fileName, string gameName, string downloadURL, CancellationToken cancellationToken = default)
    {
        var fileLocation = await GetFileLocationAsync(fileName, gameName, downloadURL, cancellationToken);
        return await File.ReadAllTextAsync(fileLocation, cancellationToken);
    }
}
