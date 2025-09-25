#define TRuntime "win-x64"

[Setup]
AppName=TSXMLEdit
AppVersion=0.0.0.1
DefaultDirName={pf}\TSXMLEdit
DefaultGroupName=TSXMLEdit
UninstallDisplayIcon={app}\TSXMLEdit.exe
OutputBaseFilename=TSXMLEditSetup
Compression=lzma
SolidCompression=yes

[Files]
; Use the runtime constant for path flexibility
Source: "bin\Release\net9.0-windows7.0\{#TRuntime}\publish\TSXMLEdit.exe"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\TSXMLEdit"; Filename: "{app}\TSXMLEdit.exe"
Name: "{commondesktop}\TSXMLEdit"; Filename: "{app}\TSXMLEdit.exe"

[Registry]
; --- TSNDJ association ---
Root: HKCR; Subkey: ".tsndj"; ValueType: string; ValueName: ""; ValueData: "TSXMLEdit.TSNDJ"; Flags: uninsdeletevalue
Root: HKCR; Subkey: "TSXMLEdit.TSNDJ"; ValueType: string; ValueData: "TS Form Content"; Flags: uninsdeletekey
Root: HKCR; Subkey: "TSXMLEdit.TSNDJ\DefaultIcon"; ValueType: string; ValueData: "{app}\TSXMLEdit.exe,0"
Root: HKCR; Subkey: "TSXMLEdit.TSNDJ\shell\open\command"; ValueType: string; ValueData: """{app}\TSXMLEdit.exe"" ""%1"""

; --- TSXML association ---
Root: HKCR; Subkey: ".tsxml"; ValueType: string; ValueName: ""; ValueData: "TSXMLEdit.TSXML"; Flags: uninsdeletevalue
Root: HKCR; Subkey: "TSXMLEdit.TSXML"; ValueType: string; ValueData: "TS Form Description"; Flags: uninsdeletekey
Root: HKCR; Subkey: "TSXMLEdit.TSXML\DefaultIcon"; ValueType: string; ValueData: "{app}\TSXMLEdit.exe,0"
Root: HKCR; Subkey: "TSXMLEdit.TSXML\shell\open\command"; ValueType: string; ValueData: """{app}\TSXMLEdit.exe"" ""%1"""
