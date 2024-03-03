using System;
using System.Linq;
using Runtime.Manager.Data;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Runtime.Localization
{
    public static class LocalizationUtils
    {
        #region Class Methods

        public static void InitSelectedLocale()
        {
            var selectedLocalized = DataManager.Local.SelectedLanguage;

            var isSelectedLocalized = false;
            if (!string.IsNullOrEmpty(selectedLocalized))
            {
                var settingLocale = LocalizationSettings.AvailableLocales.Locales.FirstOrDefault(x => x.LocaleName.Contains(selectedLocalized));
                if (settingLocale)
                {
                    isSelectedLocalized = true;
                    LocalizationSettings.SelectedLocale = settingLocale;
                }
            }

            if (!isSelectedLocalized)
            {
                var systemLocale = LocalizationSettings.AvailableLocales.Locales.FirstOrDefault(x => x.LocaleName.Contains(Application.systemLanguage.ToString()));
                if (systemLocale)
                {
                    LocalizationSettings.SelectedLocale = systemLocale;
                }
                else
                {
                    var locale = LocalizationSettings.AvailableLocales.Locales.FirstOrDefault(x => x.LocaleName.StartsWith(SystemLanguage.English.ToString()));
                    LocalizationSettings.SelectedLocale = locale;
                }
            }
        }

        private static string ToLocalized(string tableRef, string entryRef, params object[] arguments)
        {
            if (entryRef == "")
            {
                return "";
            }
            var x = new LocalizedString(tableRef, entryRef);
            var y = x.GetLocalizedString(arguments);
            return y;
        }

        /*-----------------General------------------*/

        public static string GetGeneralLocalized(string keyLocalize, params object[] arguments)
        {
            return ToLocalized(LocalizeTable.GENERAL, keyLocalize, arguments);
        }

     
    /*-----------------Toast------------------*/

        public static string GetToastLocalized(string keyLocalize, params object[] arguments)
        {
            return ToLocalized(LocalizeTable.TOAST, keyLocalize, arguments);
        }

        

        #endregion Class Methods
    }
}