using System.Globalization;

namespace SafeChat.Mobile.Converters;

public class IntGreaterThanZeroConverter : IValueConverter
{
  public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
    value is int count && count > 0;

  public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
    throw new NotSupportedException();
}
