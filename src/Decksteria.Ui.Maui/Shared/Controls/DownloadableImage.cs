namespace Decksteria.Ui.Maui.Shared.Controls;

using Decksteria.Ui.Maui.Services.CardImageService;
using Microsoft.Maui.Controls;
using System;
using System.ComponentModel;
using System.IO;

internal sealed class DownloadableImage : ContentView
{
    private readonly Image imageControl;

    private IDecksteriaCardImageService? cardImageService;

    private string? fullImagePath;

    public static readonly BindableProperty AllowDownloadProperty =
        BindableProperty.Create(nameof(AllowDownload), typeof(bool), typeof(DownloadableImage), false);

    public static readonly BindableProperty ImageUrlProperty =
        BindableProperty.Create(nameof(ImageUrl), typeof(string), typeof(DownloadableImage), string.Empty);

    public static readonly BindableProperty FileNameProperty =
        BindableProperty.Create(nameof(FileName), typeof(string), typeof(DownloadableImage), string.Empty);

    public event EventHandler? Initialised;

    public bool AllowDownload
    {
        get => (bool) GetValue(AllowDownloadProperty);
        set => SetValue(AllowDownloadProperty, value);
    }

    public string ImageUrl
    {
        get => (string) GetValue(ImageUrlProperty);
        set => SetValue(ImageUrlProperty, value);
    }

    public string FileName
    {
        get => (string) GetValue(FileNameProperty);
        set => SetValue(FileNameProperty, value);
    }

    public DownloadableImage()
    {
        imageControl = new Image();
        Content = imageControl;
        PropertyChanged += OnPropertiesChanged;
    }

    public void SetCardImageService(IDecksteriaCardImageService cardImageService)
    {
        this.cardImageService = cardImageService;
    }

    private async void OnPropertiesChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not (nameof(AllowDownload)) and not (nameof(ImageUrl)) and not (nameof(FileName)))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(ImageUrl))
        {
            return;
        }

        // If the Image Control is told to never use the local file.
        if (!AllowDownload || cardImageService is null)
        {
            imageControl.Source = ImageSource.FromUri(new Uri(ImageUrl));
            return;
        }

        fullImagePath = cardImageService.GetExpectedCardImageLocation(FileName);
        if (!File.Exists(fullImagePath))
        {
            fullImagePath = await cardImageService.GetCardImageLocationAsync(FileName, ImageUrl, null);
        }

        imageControl.Source = ImageSource.FromFile(fullImagePath);
    }
}
