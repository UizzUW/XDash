using System;
using System.Globalization;
using Xamarin.Forms;
using OperatingSystem = XDash.Framework.Models.OperatingSystem;

namespace XDash.Converters
{
    public class OperatingSystemToLogoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            var os = (OperatingSystem)value;
            switch (os)
            {
                case OperatingSystem.Windows:
                    return ImageSource.FromResource("XDash.Resources.Images.xdash_windows_nobg_icon.png");
                case OperatingSystem.Android:
                    return ImageSource.FromResource("XDash.Resources.Images.xdash_droid_nobg_icon.png");
                case OperatingSystem.iOS:
                    return ImageSource.FromResource("XDash.Resources.Images.xdash_ios_nobg_icon.png");
                case OperatingSystem.macOS:
                    return ImageSource.FromResource("XDash.Resources.Images.xdash_mac_nobg_icon.png");
                case OperatingSystem.Linux:
                    return ImageSource.FromResource("XDash.Resources.Images.xdash_linux_nobg_icon.png");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
