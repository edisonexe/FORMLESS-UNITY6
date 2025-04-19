using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using Formless.UI.Menu;

public class LanguageSwitcher : MonoBehaviour
{
    public Dropdown languageDropdown; // Ссылка на Dropdown
    public Settings settings; // Ссылка на объект Settings

    private void Start()
    {
        // Инициализация Dropdown
        if (languageDropdown != null)
        {
            // Заполняем Dropdown значениями (если это не сделано вручную)
            languageDropdown.ClearOptions();
            languageDropdown.AddOptions(new System.Collections.Generic.List<string> { "English", "Русский" });

            // Устанавливаем обработчик события выбора
            languageDropdown.onValueChanged.AddListener(OnLanguageChanged);

            // Устанавливаем язык по умолчанию (например, первый в списке)
            string savedLanguage = PlayerPrefs.GetString("SelectedLanguage", "en");
            SetLanguage(savedLanguage);

            // Устанавливаем Dropdown на сохранённый язык
            var localeIndex = LocalizationSettings.AvailableLocales.Locales.FindIndex(l => l.Identifier.Code == savedLanguage);
            if (localeIndex >= 0)
            {
                languageDropdown.value = localeIndex;
            }
        }
    }

    private void OnLanguageChanged(int index)
    {
        var selectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        if (selectedLocale != null)
        {
            LocalizationSettings.SelectedLocale = selectedLocale;

            // Сохраняем выбранный язык
            if (settings != null)
            {
                settings.SaveLanguage(selectedLocale.Identifier.Code);
            }
        }
    }

    private void SetLanguage(string localeCode)
    {
        // Находим локаль по коду и устанавливаем её
        var selectedLocale = LocalizationSettings.AvailableLocales.Locales.Find(l => l.Identifier.Code == localeCode);
        if (selectedLocale != null)
        {
            LocalizationSettings.SelectedLocale = selectedLocale;
        }
    }
}