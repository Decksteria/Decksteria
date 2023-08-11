namespace Decksteria.Core.Data;

public interface IDecksteriaFileReader
{
    Task<string> ReadTextFileAsync(string fileName);

    Task<byte[]> ReadByteFileAsync(string fileName);

    string GetFileLocation(string fileName);

    string BuildConnectionString(string fileName, IDictionary<string, string> connectionProperties);
}
