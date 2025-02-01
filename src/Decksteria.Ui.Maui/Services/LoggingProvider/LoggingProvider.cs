namespace Decksteria.Ui.Maui.Services.LoggingProvider;

using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;

internal sealed class LoggingProvider : ILoggerProvider
{
    private readonly string logFilePath;

    public LoggingProvider(TimeProvider timeProvider)
    {
        logFilePath = Path.Combine(FileSystem.AppDataDirectory, "logs", $"log_{timeProvider.GetUtcNow()}.log");
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new FileLogger(logFilePath);
    }

    public void Dispose()
    {
        // Nothing to dispose.
    }
}

public class FileLogger : ILogger
{
    private readonly string filePath;
    private static readonly object _lock = new();

    public FileLogger(string filePath)
    {
        this.filePath = filePath;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (formatter is null)
        {
            return;
        }

        var message = formatter(state, exception);
        lock (_lock)
        {
            var fullMessage = $"[{typeof(TState).Name}]: {logLevel} - {message}";
            AddMessage(fullMessage);
        }
    }

    private void AddMessage(string message)
    {
        Console.WriteLine(message);
        File.AppendAllText(filePath, $"{message}\n");
    }
}