namespace Decksteria.Ui.Maui.Shared.Controls;

using System.Collections.Generic;
using System;
using Microsoft.Maui.Controls;
using Plainer.Maui.Controls;
using UraniumUI.Pages;
using UraniumUI.Views;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using UraniumUI.Resources;
using System.ComponentModel;

using System.Windows.Input;
using UraniumUI.Material.Controls;
using CommunityToolkit.Maui.Behaviors;

[ContentProperty(nameof(Validations))]
internal partial class NumericField : InputField
{
    public EntryView EntryView => ( Content as EntryView )!;

    public override View Content { get; set; } = new EntryView
    {
        Margin = new Thickness(10, 0),
        BackgroundColor = Colors.Transparent,
        Keyboard = Keyboard.Numeric,
        VerticalOptions = LayoutOptions.Center
    };

    protected StatefulContentView iconClear = new()
    {
        VerticalOptions = LayoutOptions.Center,
        HorizontalOptions = LayoutOptions.End,
        IsVisible = false,
        Padding = new Thickness(5, 0),
        Margin = new Thickness(0, 0, 5, 0),
        Content = new Path
        {
            StyleClass = new[] { "NumericField.ClearIcon" },
            Data = UraniumShapes.X,
            Fill = ColorResource.GetColor("OnBackground", "OnBackgroundDark", Colors.DarkGray).WithAlpha(.5f),
        }
    };

    public int? Value
    {
        get
        {
            var value = (string) GetValue(ValueProperty);
            return int.TryParse(value, out var intResult) ? intResult : null;
        }
        set => SetValue(ValueProperty, value is null ? string.Empty : value.ToString());
    }

    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value),
        typeof(string),
        typeof(NumericField),
        string.Empty,
        BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            if (bindable is NumericField numericField)
            {
                numericField.UpdateClearIconState();
            }
        });

    public Color TextColor { get => (Color) GetValue(TextColorProperty); set => SetValue(TextColorProperty, value); }

    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
        nameof(TextColor),
        typeof(Color),
        typeof(NumericField),
        ColorResource.GetColor("OnBackground", "OnBackgroundDark", Colors.DarkGray),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is NumericField numericField)
            {
                numericField.EntryView.TextColor = (Color) newValue;
            }
        });

    public string FontFamily { get => (string) GetValue(FontFamilyProperty); set => SetValue(FontFamilyProperty, value); }

    public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
        nameof(FontFamily),
        typeof(string),
        typeof(NumericField),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is NumericField numericField)
            {
                numericField.EntryView.FontFamily = (string) newValue;
                numericField.labelTitle.FontFamily = (string) newValue;
            }
        });

    public ClearButtonVisibility ClearButtonVisibility { get => (ClearButtonVisibility) GetValue(ClearButtonVisibilityProperty); set => SetValue(ClearButtonVisibilityProperty, value); }

    public static readonly BindableProperty ClearButtonVisibilityProperty = BindableProperty.Create(
        nameof(ClearButtonVisibility),
        typeof(ClearButtonVisibility),
        typeof(NumericField),
        ClearButtonVisibility.WhileEditing,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is NumericField numericField)
            {
                numericField.EntryView.ClearButtonVisibility = (ClearButtonVisibility) newValue;
            }
        });

    public bool IsPassword { get => (bool) GetValue(IsPasswordProperty); set => SetValue(IsPasswordProperty, value); }

    public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
        nameof(IsPassword),
        typeof(bool),
        typeof(NumericField),
        false,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is NumericField numericField)
            {
                numericField.EntryView.IsPassword = (bool) newValue;
            }
        });

    public object ReturnCommandParameter { get => GetValue(ReturnCommandParameterProperty); set => SetValue(ReturnCommandParameterProperty, value); }

    public static readonly BindableProperty ReturnCommandParameterProperty = BindableProperty.Create(
        nameof(ReturnCommandParameter),
        typeof(object),
        typeof(NumericField),
        defaultBindingMode: BindingMode.TwoWay);

    public ICommand ReturnCommand { get => (ICommand) GetValue(ReturnCommandProperty); set => SetValue(ReturnCommandProperty, value); }

    public static readonly BindableProperty ReturnCommandProperty = BindableProperty.Create(
        nameof(ReturnCommand),
        typeof(ICommand),
        typeof(NumericField),
        defaultBindingMode: BindingMode.TwoWay);

    public double CharacterSpacing { get => (double) GetValue(CharacterSpacingProperty); set => SetValue(CharacterSpacingProperty, value); }

    public static readonly BindableProperty CharacterSpacingProperty = BindableProperty.Create(
        nameof(CharacterSpacing),
        typeof(double),
        typeof(NumericField),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is NumericField numericField)
            {
                numericField.EntryView.CharacterSpacing = (double) newValue;
            }
        });

    public ReturnType ReturnType { get => (ReturnType) GetValue(ReturnTypeProperty); set => SetValue(ReturnTypeProperty, value); }

    public static readonly BindableProperty ReturnTypeProperty = BindableProperty.Create(
        nameof(ReturnType),
        typeof(ReturnType),
        typeof(NumericField),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is NumericField numericField)
            {
                numericField.EntryView.ReturnType = (ReturnType) newValue;
            }
        });

    public int SelectionLength { get => (int) GetValue(SelectionLengthProperty); set => SetValue(SelectionLengthProperty, value); }

    public static readonly BindableProperty SelectionLengthProperty = BindableProperty.Create(
        nameof(SelectionLength),
        typeof(int),
        typeof(NumericField),
        defaultBindingMode: BindingMode.TwoWay);

    public int CursorPosition { get => (int) GetValue(CursorPositionProperty); set => SetValue(CursorPositionProperty, value); }

    public static readonly BindableProperty CursorPositionProperty = BindableProperty.Create(
        nameof(CursorPosition),
        typeof(int),
        typeof(NumericField),
        defaultBindingMode: BindingMode.TwoWay);

    public bool IsTextPredictionEnabled { get => (bool) GetValue(IsTextPredictionEnabledProperty); set => SetValue(IsTextPredictionEnabledProperty, value); }

    public static readonly BindableProperty IsTextPredictionEnabledProperty = BindableProperty.Create(
        nameof(IsTextPredictionEnabled),
        typeof(bool),
        typeof(NumericField),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is NumericField numericField)
            {
                numericField.EntryView.IsTextPredictionEnabled = (bool) newValue;
            }
        });

    public int MaxLength { get => (int) GetValue(MaxLengthProperty); set => SetValue(MaxLengthProperty, value); }

    public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
        nameof(MaxLength),
        typeof(int),
        typeof(NumericField),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is NumericField numericField)
            {
                numericField.EntryView.MaxLength = (int) newValue;
            }
        });

    public bool AllowClear { get => (bool) GetValue(AllowClearProperty); set => SetValue(AllowClearProperty, value); }

    public static BindableProperty AllowClearProperty = BindableProperty.Create(
        nameof(AllowClear),
        typeof(bool), typeof(NumericField),
        false,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is NumericField numericField)
            {
                numericField.OnAllowClearChanged();
            }
        });

    public new bool IsReadOnly { get => (bool) GetValue(IsReadOnlyProperty); set => SetValue(IsReadOnlyProperty, value); }

    public static readonly BindableProperty IsReadOnlyProperty = BindableProperty.Create(
        nameof(IsReadOnly),
        typeof(bool),
        typeof(NumericField),
        false);

    [TypeConverter(typeof(FontSizeConverter))]
    public double FontSize { get => (double) GetValue(FontSizeProperty); set => SetValue(FontSizeProperty, value); }

    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
        nameof(FontSize),
        typeof(double),
        typeof(InputField),
        defaultValue: Label.FontSizeProperty.DefaultValue,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is NumericField numericField)
            {
                numericField.EntryView.FontSize = (double) newValue;
            }
        });

    public FontAttributes FontAttributes { get => (FontAttributes) GetValue(FontAttributesProperty); set => SetValue(FontAttributesProperty, value); }

    public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(
        nameof(FontAttributes),
        typeof(FontAttributes),
        typeof(NumericField),
        defaultValue: Entry.FontAttributesProperty.DefaultValue,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is NumericField numericField)
            {
                numericField.EntryView.FontAttributes = (FontAttributes) newValue;
            }
        });

    public TextAlignment HorizontalTextAlignment { get => (TextAlignment) GetValue(HorizontalTextAlignmentProperty); set => SetValue(HorizontalTextAlignmentProperty, value); }

    public static readonly BindableProperty HorizontalTextAlignmentProperty = BindableProperty.Create(
        nameof(HorizontalTextAlignment),
        typeof(TextAlignment),
        typeof(NumericField),
        defaultValue: Entry.HorizontalTextAlignmentProperty.DefaultValue,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is NumericField numericField)
            {
                numericField.EntryView.HorizontalTextAlignment = (TextAlignment) newValue;
            }
        });

    public override bool HasValue => !string.IsNullOrEmpty(EntryView.Text);

    public IList<Behavior> EntryBehaviors => EntryView.Behaviors;

    public event EventHandler<TextChangedEventArgs>? TextChanged;
    public event EventHandler? Completed;

    public NumericField()
    {
        iconClear.TappedCommand = new Command(OnClearTapped);

        UpdateClearIconState();

        EntryView.SetBinding(Entry.TextProperty, new Binding(nameof(EntryView.Text), BindingMode.TwoWay, source: this));
        EntryView.SetBinding(Entry.ReturnCommandParameterProperty, new Binding(nameof(EntryView.ReturnCommandParameter), BindingMode.TwoWay, source: this));
        EntryView.SetBinding(Entry.ReturnCommandProperty, new Binding(nameof(EntryView.ReturnCommand), BindingMode.TwoWay, source: this));
        EntryView.SetBinding(Entry.SelectionLengthProperty, new Binding(nameof(EntryView.SelectionLength), BindingMode.TwoWay, source: this));
        EntryView.SetBinding(Entry.CursorPositionProperty, new Binding(nameof(EntryView.CursorPosition), BindingMode.TwoWay, source: this));
        EntryView.SetBinding(IsEnabledProperty, new Binding(nameof(EntryView.IsEnabled), BindingMode.OneWay, source: this));
        EntryView.SetBinding(InputView.IsReadOnlyProperty, new Binding(nameof(EntryView.IsReadOnly), BindingMode.OneWay, source: this));

        AfterConstructor();
    }

    private void AfterConstructor()
    {
        EntryBehaviors.Add(new NumericValidationBehavior());
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (Handler is null)
        {
            EntryView.TextChanged -= EntryView_TextChanged;
            EntryView.Completed -= EntryView_Completed;
        }
        else
        {
            EntryView.TextChanged += EntryView_TextChanged;
            EntryView.Completed += EntryView_Completed;

            ApplyAttachedProperties();
        }
    }

    protected virtual void ApplyAttachedProperties()
    {

    }

    private void EntryView_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.OldTextValue) || string.IsNullOrEmpty(e.NewTextValue))
        {
            UpdateState();
        }

        if (e.NewTextValue != null)
        {
            CheckAndShowValidations();
        }

        if (AllowClear)
        {
            iconClear.IsVisible = !string.IsNullOrEmpty(e.NewTextValue);
        }

        TextChanged?.Invoke(this, e);
    }

    private void EntryView_Completed(object? sender, EventArgs e)
    {
        Completed?.Invoke(this, e);
    }

    public void ClearValue()
    {
        if (IsEnabled)
        {
            Value = null;
        }
    }

    protected override object GetValueForValidator()
    {
        return EntryView.Text;
    }

    protected virtual void OnClearTapped()
    {
        EntryView.Text = string.Empty;
    }

    protected virtual void OnAllowClearChanged()
    {
        UpdateClearIconState();
    }

    protected virtual void UpdateClearIconState()
    {
        if (AllowClear)
        {
            if (!endIconsContainer.Contains(iconClear))
            {
                endIconsContainer.Add(iconClear);
            }
        }
        else
        {
            endIconsContainer.Remove(iconClear);
        }
    }

    public override void ResetValidation()
    {
        EntryView.Text = string.Empty;
        base.ResetValidation();
    }
}