namespace Decksteria.Ui.Maui.Shared.Controls;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using InputKit.Shared.Validations;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Plainer.Maui.Controls;
using UraniumUI.Material.Controls;
using UraniumUI.Resources;

[ContentProperty(nameof(Validations))]
internal partial class NumericField : InputField
{
    public EntryView EntryView => (Content as EntryView)!;

    public override View Content { get; set; } = new EntryView
    {
        Margin = new Thickness(10, 0),
        BackgroundColor = Colors.Transparent,
        Keyboard = Keyboard.Numeric,
        VerticalOptions = LayoutOptions.Center,
        Text = "0"
    };

    protected MinValueValidation minValueValidation = new();

    protected MaxValueValidation maxValueValidation = new();

    public int Value
    {
        get
        {
            var value = (int) GetValue(ValueProperty);
            return value;
        }
        set
        {
            SetValue(ValueProperty, value);
            EntryView.Text = value.ToString();
        }
    }

    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value),
        typeof(int),
        typeof(NumericField),
        0,
        BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            if (bindable is not NumericField numericField)
            {
                return;
            }

            if (newValue is int intNewValue)
            {
                numericField.Value = intNewValue;
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
            if (bindable is not NumericField numericField)
            {
                return;
            }

            numericField.EntryView.TextColor = (Color) newValue;
        });

    public string FontFamily { get => (string) GetValue(FontFamilyProperty); set => SetValue(FontFamilyProperty, value); }

    public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
        nameof(FontFamily),
        typeof(string),
        typeof(NumericField),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is not NumericField numericField)
            {
                return;
            }

            numericField.EntryView.FontFamily = (string) newValue;
            numericField.labelTitle.FontFamily = (string) newValue;
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
            if (bindable is not NumericField numericField)
            {
                return;
            }

            numericField.EntryView.CharacterSpacing = (double) newValue;
        });

    public ReturnType ReturnType { get => (ReturnType) GetValue(ReturnTypeProperty); set => SetValue(ReturnTypeProperty, value); }

    public static readonly BindableProperty ReturnTypeProperty = BindableProperty.Create(
        nameof(ReturnType),
        typeof(ReturnType),
        typeof(NumericField),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is not NumericField numericField)
            {
                return;
            }

            numericField.EntryView.ReturnType = (ReturnType) newValue;
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
            if (bindable is not NumericField numericField)
            {
                return;
            }

            numericField.EntryView.IsTextPredictionEnabled = (bool) newValue;
        });

    public int Min
    {
        get => (int) GetValue(MinProperty);
        set
        {
            SetValue(MinProperty, value);
            
            if (Value < value)
            {
                Value = value;
            }
        }
    }

    public static readonly BindableProperty MinProperty = BindableProperty.Create(
        nameof(Min),
        typeof(int),
        typeof(NumericField),
        defaultBindingMode: BindingMode.TwoWay);

    public int Max
    {
        get => (int) GetValue(MaxProperty);
        set
        {
            SetValue(MaxProperty, value);

            if (Value > value)
            {
                Value = value;
            }
        }
    }

    public static readonly BindableProperty MaxProperty = BindableProperty.Create(
        nameof(Max),
        typeof(int),
        typeof(NumericField),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is not NumericField numericField)
            {
                return;
            }

            numericField.EntryView.MaxLength = numericField.MaxDigits;
        });

    protected int MaxDigits => Max.ToString().Length + (AllowNegatives ? 1 : 0);

    public bool AllowNegatives { get => (bool) GetValue(AllowNegativesProperty); set => SetValue(AllowNegativesProperty, value); }

    public static BindableProperty AllowNegativesProperty = BindableProperty.Create(
        nameof(AllowNegatives),
        typeof(bool), typeof(NumericField),
        false,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is not NumericField numericField)
            {
                return;
            }

            numericField.EntryView.MaxLength = numericField.MaxDigits;
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
            if (bindable is not NumericField numericField)
            {
                return;
            }

            numericField.EntryView.FontSize = (double) newValue;
        });

    public FontAttributes FontAttributes { get => (FontAttributes) GetValue(FontAttributesProperty); set => SetValue(FontAttributesProperty, value); }

    public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(
        nameof(FontAttributes),
        typeof(FontAttributes),
        typeof(NumericField),
        defaultValue: Entry.FontAttributesProperty.DefaultValue,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is not NumericField numericField)
            {
                return;
            }

            numericField.EntryView.FontAttributes = (FontAttributes) newValue;
        });

    public TextAlignment HorizontalTextAlignment { get => (TextAlignment) GetValue(HorizontalTextAlignmentProperty); set => SetValue(HorizontalTextAlignmentProperty, value); }

    public static readonly BindableProperty HorizontalTextAlignmentProperty = BindableProperty.Create(
        nameof(HorizontalTextAlignment),
        typeof(TextAlignment),
        typeof(NumericField),
        defaultValue: Entry.HorizontalTextAlignmentProperty.DefaultValue,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is not NumericField numericField)
            {
                return;
            }

            numericField.EntryView.HorizontalTextAlignment = (TextAlignment) newValue;
        });

    public override bool HasValue => !string.IsNullOrEmpty(EntryView.Text);

    public IList<Behavior> EntryBehaviors => EntryView.Behaviors;

    public event EventHandler<TextChangedEventArgs>? TextChanged;
    public event EventHandler? Completed;

    public NumericField()
    {
        minValueValidation.SetBinding(MinValueValidation.MinValueProperty, new Binding(nameof(Min), BindingMode.TwoWay, source: this));
        maxValueValidation.SetBinding(MaxValueValidation.MaxValueProperty, new Binding(nameof(Max), BindingMode.TwoWay, source: this));
        Validations.Add(minValueValidation);
        Validations.Add(maxValueValidation);

        EntryView.SetBinding(Entry.ReturnCommandParameterProperty, new Binding(nameof(ReturnCommandParameter), BindingMode.TwoWay, source: this));
        EntryView.SetBinding(Entry.ReturnCommandProperty, new Binding(nameof(ReturnCommand), BindingMode.TwoWay, source: this));
        EntryView.SetBinding(Entry.SelectionLengthProperty, new Binding(nameof(SelectionLength), BindingMode.TwoWay, source: this));
        EntryView.SetBinding(Entry.CursorPositionProperty, new Binding(nameof(CursorPosition), BindingMode.TwoWay, source: this));
        EntryView.SetBinding(IsEnabledProperty, new Binding(nameof(IsEnabled), BindingMode.OneWay, source: this));
        EntryView.SetBinding(InputView.IsReadOnlyProperty, new Binding(nameof(IsReadOnly), BindingMode.OneWay, source: this));
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
        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            Value = Min;
        }
        else if ((AllowNegatives && e.NewTextValue.Length == MaxDigits && !e.NewTextValue.StartsWith('-')) || !int.TryParse(e.NewTextValue, out var intValue))
        {
            EntryView.Text = e.OldTextValue;
            return;
        }
        else
        {
            Value = intValue;
        }

        if (string.IsNullOrEmpty(e.OldTextValue) || string.IsNullOrEmpty(e.NewTextValue))
        {
            UpdateState();
        }

        if (e.NewTextValue != null)
        {
            CheckAndShowValidations();
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
            Value = Min;
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

    public override void ResetValidation()
    {
        EntryView.Text = string.Empty;
        base.ResetValidation();
    }
}