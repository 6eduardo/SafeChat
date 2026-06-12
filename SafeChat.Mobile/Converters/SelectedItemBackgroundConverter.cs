using System.Globalization;

namespace SafeChat.Mobile.Converters;

public class SelectedItemBackgroundConverter : IValueConverter
{
  public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
  {
    var isSelected = value is true;
    return isSelected
      ? Application.Current?.Resources["SafeChatConversationsSelectedBg"] ?? Colors.Transparent
      : Colors.White;
  }

  public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
    throw new NotSupportedException();
}
