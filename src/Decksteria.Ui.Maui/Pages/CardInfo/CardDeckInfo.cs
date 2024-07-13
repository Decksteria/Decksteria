namespace Decksteria.Ui.Maui.Pages.CardInfo;

using System;
using System.ComponentModel;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

internal sealed class CardDeckInfo : INotifyPropertyChanged
{
    private const double AddRemoveButtonWidth = 25.0;

    private bool canAddCard;

    private int count;

    public bool CanAddCard
    {
        get => canAddCard;
        set
        {
            canAddCard = value;
            OnPropertyChanged(nameof(CanAddCard));
        }
    }

    public bool CanRemoveCard => Count > 0;

    public int Count
    {
        get => count;
        set
        {
            count = value;
            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(nameof(CanRemoveCard));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public VisualElement CreateVisualElement(string deckLabel, Action<object?, EventArgs> AddAction, Action<object?, EventArgs> RemoveAction)
    {
        var titleLabel = new Label
        {
            HorizontalTextAlignment = TextAlignment.Center,
            StyleClass = ["FilledButton"],
            Text = $"{deckLabel}: ",
            VerticalTextAlignment = TextAlignment.Center
        };
        titleLabel.SetValue(Label.FontSizeProperty, "Subtitle");

        var removeButton = new Button
        {
            HeightRequest = AddRemoveButtonWidth,
            Padding = 0,
            Text = "-",
            WidthRequest = AddRemoveButtonWidth
        };
        removeButton.Pressed += (sender, e) => RemoveAction(sender, e);
        removeButton.SetBinding(Button.IsEnabledProperty, new Binding(nameof(CardDeckInfo.CanRemoveCard), BindingMode.OneWay, source: this));

        var countLabel = new Label
        {
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center
        };
        countLabel.SetBinding(Label.TextProperty, new Binding(nameof(CardDeckInfo.Count), BindingMode.OneWay, source: this));

        var addButton = new Button
        {
            HeightRequest = AddRemoveButtonWidth,
            Padding = 0,
            StyleClass = ["FilledButton"],
            Text = "+",
            WidthRequest = AddRemoveButtonWidth
        };
        addButton.Pressed += (sender, e) => AddAction(sender, e);
        addButton.SetBinding(Button.IsEnabledProperty, new Binding(nameof(CardDeckInfo.CanAddCard), BindingMode.OneWay, source: this));

        var horizontalStack = new HorizontalStackLayout
        {
            titleLabel,
            removeButton,
            countLabel,
            addButton
        };

        return horizontalStack;
    }

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
};