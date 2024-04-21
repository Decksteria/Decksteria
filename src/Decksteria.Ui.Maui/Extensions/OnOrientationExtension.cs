namespace Decksteria.Ui.Maui.Extensions;

using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Devices;
using System.ComponentModel;
using System;
using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.Messaging;

/// <summary>
/// OnOrientationExtension courtesy of FlavioGoncalves-Cayas from:
/// https://gist.github.com/FlavioGoncalves-Cayas/841a13ca1f2904ef1f732c159b4c5f5d#file-onorientationextension-cs
/// </summary>
[ContentProperty(nameof(Default))]
public class OnOrientationExtension : IMarkupExtension<BindingBase>
{
    public Type? TypeConverter { get; set; }
    public object? Default { get; set; }
    public object? Landscape { get; set; }
    public object? Portrait { get; set; }

    static OnOrientationExtension()
    {
        DeviceDisplay.MainDisplayInfoChanged += (_, _) => WeakReferenceMessenger.Default.Send(new OrientationChangedMessage());
    }

    public BindingBase ProvideValue(IServiceProvider serviceProvider)
    {
        var typeConverter = TypeConverter is not null ? Activator.CreateInstance(TypeConverter) as TypeConverter : null;

        var defaultString = Default as string;
        var defaultValue = (defaultString is not null ? typeConverter?.ConvertFromInvariantString(defaultString) : null ) ?? Default;
        var orientationSource = new OnOrientationSource
        {
            DefaultValue = defaultValue
        };
        orientationSource.PortraitValue = Portrait is null ? orientationSource.DefaultValue : typeConverter?.ConvertFromInvariantString((string) Portrait) ?? Portrait;
        orientationSource.LandscapeValue = Landscape is null ? orientationSource.DefaultValue : typeConverter?.ConvertFromInvariantString((string) Landscape) ?? Landscape;

        return new Binding
        {
            Mode = BindingMode.OneWay,
            Path = nameof(OnOrientationSource.Value),
            Source = orientationSource
        };
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);

    public class OrientationChangedMessage
    {
    }
}