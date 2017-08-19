using XDash.Models.Enums;

namespace XDash.Services.Contracts
{
    public interface ILocalizer
    {
        Language[] SupportedLanguages { get; }
        Language CurrentLanguage { get; set; }
        string GetLocalizedString(string key);
    }
}
