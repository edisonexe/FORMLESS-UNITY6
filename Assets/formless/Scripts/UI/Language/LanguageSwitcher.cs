using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using Formless.UI.Menu;

public class LanguageSwitcher : MonoBehaviour
{
    public Dropdown languageDropdown; // ������ �� Dropdown
    public Settings settings; // ������ �� ������ Settings

    private void Start()
    {
        // ������������� Dropdown
        if (languageDropdown != null)
        {
            // ��������� Dropdown ���������� (���� ��� �� ������� �������)
            languageDropdown.ClearOptions();
            languageDropdown.AddOptions(new System.Collections.Generic.List<string> { "English", "�������" });

            // ������������� ���������� ������� ������
            languageDropdown.onValueChanged.AddListener(OnLanguageChanged);

            // ������������� ���� �� ��������� (��������, ������ � ������)
            string savedLanguage = PlayerPrefs.GetString("SelectedLanguage", "en");
            SetLanguage(savedLanguage);

            // ������������� Dropdown �� ���������� ����
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

            // ��������� ��������� ����
            if (settings != null)
            {
                settings.SaveLanguage(selectedLocale.Identifier.Code);
            }
        }
    }

    private void SetLanguage(string localeCode)
    {
        // ������� ������ �� ���� � ������������� �
        var selectedLocale = LocalizationSettings.AvailableLocales.Locales.Find(l => l.Identifier.Code == localeCode);
        if (selectedLocale != null)
        {
            LocalizationSettings.SelectedLocale = selectedLocale;
        }
    }
}