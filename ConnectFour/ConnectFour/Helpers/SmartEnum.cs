using System.Reflection;

namespace ConnectFour.Helpers;

public abstract class SmartEnum<T> where T : SmartEnum<T>
{
    /// <summary>
    /// The underlying string value.
    /// </summary>
    public string Value { get; }

    protected SmartEnum(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Override in derived types to change matching behavior.
    /// Default: Ordinal (case sensitive).
    /// Use OrdinalIgnoreCase for case-insensitive matching.
    /// </summary>
    protected virtual StringComparison Comparison => StringComparison.Ordinal;

    public override string ToString() => Value;

    #region ---------- Equality ----------
    public override bool Equals(object? obj)
        => obj is SmartEnum<T> other &&
           string.Equals(Value, other.Value, Comparison);

    public override int GetHashCode()
        => StringComparer.FromComparison(Comparison).GetHashCode(Value);

    public static bool operator ==(SmartEnum<T>? a, SmartEnum<T>? b)
        => a?.Equals(b) ?? b is null;

    public static bool operator !=(SmartEnum<T>? a, SmartEnum<T>? b)
        => !(a == b);

    #endregion

    #region ---------- Value list discovery (cached) ----------
    private static readonly Lazy<IReadOnlyList<T>> _list = new(() =>
        typeof(T)
            .GetFields(BindingFlags.Public |
                       BindingFlags.Static |
                       BindingFlags.DeclaredOnly)
            .Where(f => f.FieldType == typeof(T))
            .Select(f => (T)f.GetValue(null)!)
            .ToList()
    );

    public static IReadOnlyList<T> List => _list.Value;

    #endregion

    #region ---------- Parsing ----------
    public static T FromValue(string value)
    {
        var item = TryFromValue(value);
        if (item is null)
        {
            string allowed = string.Join(", ", List.Select(x => x.Value));
            throw new ArgumentException(
                $"Invalid value '{value}' for {typeof(T).Name}. " +
                $"Allowed values: {allowed}"
            );
        }

        return item;
    }

    public static T? TryFromValue(string value)
    {
        // Use each item's comparison setting to evaluate equality
        foreach (var item in List)
        {
            if (string.Equals(item.Value, value, item.Comparison))
                return item;
        }
        return null;
    }
    #endregion
}
