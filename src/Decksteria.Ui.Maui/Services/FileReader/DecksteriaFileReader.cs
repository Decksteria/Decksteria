namespace Decksteria.Ui.Maui.Services.FileReader;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Decksteria.Core.Data;
using Decksteria.Services.PlugInFactory;
using Microsoft.Maui.Storage;

internal sealed class DecksteriaFileReader(IDecksteriaPlugInFactory plugInFactory, HttpClient httpClient) : IDecksteriaFileReader
{
    private readonly IDecksteriaPlugInFactory plugInFactory = plugInFactory;
    private readonly HttpClient httpClient = httpClient;

    public string BuildConnectionString(string fileName, IDictionary<string, string> connectionProperties, string downloadURL, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public Task<byte[]> ReadImageAsync(string fileName, string downloadURL, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetFileLocationAsync(string fileName, string downloadURL, CancellationToken cancellationToken = default)
    {
        var plugIn = plugInFactory.GetSelectedFormat();
        var filePath = @$"{FileSystem.AppDataDirectory}\{plugIn.Game}\{fileName}";
        if (!File.Exists(filePath))
        {
            using var httpStream = await httpClient.GetStreamAsync(downloadURL);
            using var fileStream = File.Create(filePath);
            httpStream.CopyTo(fileStream);
        }

        return filePath;
    }

    public Task<byte[]> ReadByteFileAsync(string fileName, string downloadURL, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public async Task<string> ReadTextFileAsync(string fileName, string downloadURL, CancellationToken cancellationToken = default)
    {
        var fileLocation = await GetFileLocationAsync(fileName, downloadURL, cancellationToken);
        return await File.ReadAllTextAsync(fileLocation, cancellationToken);
    }
}
