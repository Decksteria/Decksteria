namespace Decksteria.Ui.Maui.Services.LoggingProvider;

using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;

internal sealed class LoggingProvider : ILoggerProvider
{
    private readonly string logFilePath;

    private readonly TimeProvider timeProvider;

    public LoggingProvider(TimeProvider timeProvider)
    {
        logFilePath = Path.Combine(FileSystem.AppDataDirectory, "logs", $"log_{timeProvider.GetUtcNow():yyyy-MM-dd_HH-mm-ss}.log");
        this.timeProvider = timeProvider;
    }

    public ILogger CreateLogger(string categoryName)
    {
        var baseDirectory = Path.GetDirectoryName(logFilePath) ?? FileSystem.AppDataDirectory;
        if (!Directory.Exists(baseDirectory))
        {
            Directory.CreateDirectory(baseDirectory);
        }

        return new FileLogger(logFilePath, timeProvider);
    }

    public void Dispose()
    {
        // Nothing to dispose.
    }
}

public class FileLogger : ILogger
{
    private readonly string filePath;

    private readonly TimeProvider timeProvider;

    private static readonly object _lock = new();

    public FileLogger(string filePath, TimeProvider timeProvider)
    {
        this.filePath = filePath;
        this.timeProvider = timeProvider;
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
            var fullMessage = $"[{typeof(TState).Name}] - {timeProvider.GetLocalNow():yyyy/MM/dd HH:mm:ss.fff zzz}: {logLevel} - {message}";
            AddMessage(fullMessage);
        }
    }

    private void AddMessage(string message)
    {
        Console.WriteLine(message);
        File.AppendAllText(filePath, $"{message}\n");
    }
}