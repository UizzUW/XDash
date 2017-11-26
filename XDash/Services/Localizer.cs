using System.Collections.Generic;
using System.Linq;
using System.Resources;
using MVPathway.Messages.Abstractions;
using XDash.Framework.Models;
using XDash.Messages;
using XDash.Resources.Strings;
using XDash.Services.Contracts;

namespace XDash.Services
{
    public class Localizer : ILocalizer
    {
        private readonly IMessenger _messenger;

        private readonly Dictionary<Language, ResourceManager> _resourceManagers = new Dictionary<Language, ResourceManager>()
        {
            { Language.EN, Strings_EN.ResourceManager },
            { Language.RO, Strings_RO.ResourceManager }
        };

        public Language[] SupportedLanguages => _resourceManagers.Keys.ToArray();

        private Language _currentLanguage;
        public Language CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                _currentLanguage = value;
                _messenger.Send(new LanguageChangedMessage());
            }
        }


        public Localizer(IMessenger messenger)
        {
            _messenger = messenger;
        }

        public string GetLocalizedString(string key) => _resourceManagers[CurrentLanguage].GetString(key);
    }
}
