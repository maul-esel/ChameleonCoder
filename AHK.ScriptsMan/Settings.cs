﻿namespace AHKScriptsMan.Properties {
    
    
    // Diese Klasse ermöglicht die Behandlung bestimmter Ereignisse der Einstellungsklasse:
    //  Das SettingChanging-Ereignis wird ausgelöst, bevor der Wert einer Einstellung geändert wird.
    //  Das PropertyChanged-Ereignis wird ausgelöst, nachdem der Wert einer Einstellung geändert wurde.
    //  Das SettingsLoaded-Ereignis wird ausgelöst, nachdem die Einstellungswerte geladen wurden.
    //  Das SettingsSaving-Ereignis wird ausgelöst, bevor die Einstellungswerte gespeichert werden.
    internal sealed partial class Settings {
        
        internal Settings() {
            // // Heben Sie die  Auskommentierung der unten angezeigten Zeilen auf, um Ereignishandler zum Speichern und Ändern von Einstellungen hinzuzufügen:
            //
            // this.SettingChanging += this.SettingChangingEventHandler;
            //
            // this.SettingsSaving += this.SettingsSavingEventHandler;
            //
        }
        
        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e) {
            if (e.SettingName == "Language")
            {
                Program.OnLanguageChanged(e.NewValue.ToString());
            }
        }
        
        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e) {
            // Fügen Sie hier Code zum Behandeln des SettingsSaving-Ereignisses hinzu.
        }
    }
}
