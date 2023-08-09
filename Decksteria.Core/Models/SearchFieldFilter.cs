namespace Decksteria.Core.Models;

public record SearchFieldFilter : SearchField
{
    public SearchFieldFilter(string fieldName, FieldType fieldType, ComparisonType comparison, object value) : base(fieldName, fieldType)
    {
        Comparison = comparison;
        Value = value;
    }

    public ComparisonType Comparison { get; set; }

    public object Value { get; set; }
}

