﻿namespace Decksteria.Ui.Maui.Services.FileReader;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Decksteria.Core.Data;

internal sealed class DecksteriaFileReader : IDecksteriaFileReader
{
    public string BuildConnectionString(string fileName, IDictionary<string, string> connectionProperties, string downloadURL) => throw new NotImplementedException();

    public string GetFileLocation(string fileName, string downloadURL) => throw new NotImplementedException();

    public Task<byte[]> ReadByteFileAsync(string fileName, string downloadURL) => throw new NotImplementedException();

    public Task<string> ReadTextFileAsync(string fileName, string downloadURL) => throw new NotImplementedException();
}
