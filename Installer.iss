[Setup]
AllowNoIcons=yes
AllowRootDirectory=yes
AppCopyright=© 2011 maul.esel
AppName=ChameleonCoder
AppVerName=ChameleonCoder v0.0.0.1 alpha 2
AppPublisher=maul.esel
AppPublisherURL=http://www.autohotkey.com/forum/viewtopic.php?t=72874
AppSupportURL=http://www.autohotkey.com/forum/viewtopic.php?t=72874
ChangesAssociations=yes
DefaultDirName={pf}\ChameleonCoder
DefaultGroupName=ChameleonCoder
LicenseFile=license.txt
SourceDir=Deploy
OutputDir=..\
Uninstallable=yes

[Languages]

[Types]
Name: "full"; Description: "{cm:FullInstall}"
Name: "custom"; Description: "{cm:CustInstall}"; Flags: iscustom

[Components]
Name: "main";         Description: "{cm:CompMain}";   Types: full custom; Flags: fixed
Name: "ext";          Description: "{cm:CompExts}";   Types: full custom
Name: "com";          Description: "{cm:CompCOMS}";                       Flags: fixed
Name: "language";     Description: "{cm:CompLang}";   Types: full custom
Name: "GuidCreator";  Description: "{cm:CompGuid}";   Types: full custom
Name: "help";         Description: "{cm:CompHelp}";   Types: full custom
Name: "help\english"; Description: "{cm:CompHelpEN}"; Types: full custom
Name: "debug";        Description: "{cm:CompTest}"

[Dirs]
Name: "{app}\Components"
Name: "{app}\de-DE"; Components: language
Name: "{app}\Components\de-DE"; Components: language

[Files]
Source: "ChameleonCoder.exe";         DestDir: "{app}"; Flags: ignoreversion; Components: main
Source: "ChameleonCoder.exe.config";  DestDir: "{app}"; Flags: ignoreversion; Components: main
Source: "ICSharpCode.AvalonEdit.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: main
Source: "license.txt";                DestDir: "{app}"; Flags: ignoreversion; Components: main
Source: "Odyssey.dll";                DestDir: "{app}"; Flags: ignoreversion; Components: main
Source: "Components\ChameleonCoder.ComponentCore.dll"; DestDir: "{app}\Components"; Flags: ignoreversion; Components: main

Source: "de-DE\ChameleonCoder.resources.dll"; DestDir: "{app}\de-DE"; Flags: ignoreversion; Components: language
Source: "Components\de-DE\ChameleonCoder.ComponentCore.resources.dll"; DestDir: "{app}\Components\de-DE"; Flags: ignoreversion; Components: language

Source: "Components\GuidCreator.dll"; DestDir: "{app}\Components"; Flags: ignoreversion; Components: GuidCreator

Source: "..\Documentation\Documentation.chm"; DestDir: "{app}"; Flags: ignoreversion; Components: help\english; Permissions: users-full

Source: "test.ccr"; DestDir: "{app}"; Flags: ignoreversion; Components: debug

[Registry]
Root: HKCR; Subkey: ".ccp"; ValueType: string; ValueName: ""; ValueData: "{cm:ExtsCCP}"; Flags: uninsdeletekey; Components: ext
Root: HKCR; Subkey: ".ccp\Shell\Open\command"; ValueType: string; ValueName: ""; ValueData: "{app}\ChameleonCoder.exe ""%1"""; Flags: uninsdeletekey; Components: ext
Root: HKCR; Subkey: ".ccp\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\ChameleonCoder.exe,0"; Flags: uninsdeletekey; Components: ext

Root: HKCR; Subkey: ".ccr"; ValueType: string; ValueName: ""; ValueData: "{cm:ExtsCCR}"; Flags: uninsdeletekey; Components: ext
Root: HKCR; Subkey: ".ccr\Shell\Open\command"; ValueType: string; ValueName: ""; ValueData: "{app}\ChameleonCoder.exe ""%1"""; Flags: uninsdeletekey; Components: ext
Root: HKCR; Subkey: ".ccp\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\ChameleonCoder.exe,1"; Flags: uninsdeletekey; Components: ext

[Tasks]
Name: desktopicon; Description: "Create a desktop icon"; Components: main

[Icons]
Name: "{commondesktop}\ChameleonCoder"; Filename: "{app}\ChameleonCoder.exe"; WorkingDir: "{app}"; Flags: createonlyiffileexists; Tasks: desktopicon

[Languages]
Name: "en"; MessagesFile: "compiler:Default.isl"
Name: "de"; MessagesFile: "compiler:German.isl"

[CustomMessages]
en.FullInstall="full installation"
en.CustInstall="custom installation"
en.CompMain="ChameleonCoder and integrated resource types (required)"
en.CompExts="file extensions for *.ccr and *.ccp"
en.CompCOMS="COM support"
en.CompLang="additional languages"
en.CompGuid="GuidCreator plugin"
en.CompHelp="help files"
en.CompHelpEN="english help file"
en.CompTest="test file"
en.ExtsCCR="ChameleonCoder resource file"
en.ExtsCCP="ChameleonCoder resource pack"





