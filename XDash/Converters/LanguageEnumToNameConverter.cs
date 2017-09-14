using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;
using XDash.Framework.Models;

namespace XDash.Converters
{
    public class LanguageEnumToNameConverter : IValueConverter
    {
        private static readonly Dictionary<Language, string> _languageNames = new Dictionary<Language, string>
        {
            { Language.EN, "English" },
            { Language.RO, "Romanian" }
        };

        private static readonly Dictionary<string, Language> _reverseLanguageNames
            = _languageNames.ToDictionary(x => x.Value, x => x.Key);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _languageNames[(Language)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null
                ? Language.EN
                : _reverseLanguageNames[value as string];
        }
    }
}
