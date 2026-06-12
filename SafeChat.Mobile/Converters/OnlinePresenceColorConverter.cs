using System.Globalization;

namespace SafeChat.Mobile.Converters;

public class OnlinePresenceColorConverter : IValueConverter
{
  public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
  {
    var isOnline = value is true;
    var key = isOnline ? "SafeChatPresenceText" : "SafeChatTextMuted";
    return Application.Current?.Resources[key] ?? Colors.Gray;
  }

  public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
    throw new NotSupportedException();
}
