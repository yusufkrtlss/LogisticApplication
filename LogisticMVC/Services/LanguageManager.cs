using System.Globalization;

namespace LogisticMVC.Services
{
    public class LanguageManager : ILanguageService
    {
        private string _currentLanguage;
        public string CurrentLanguage
        {
            get { return _currentLanguage ?? CultureInfo.CurrentCulture.Name; }
            set { _currentLanguage = value; }
        }
    }
}
