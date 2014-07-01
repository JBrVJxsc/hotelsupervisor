; �ű��� Inno Setup �ű��� ���ɣ�
; �йش��� Inno Setup �ű��ļ�����ϸ��������İ����ĵ���

#define MyAppName "ͨ���пƶ�����������"
#define MyAppVersion "1.0"
#define MyAppPublisher "Kerchin Police"
#define MyAppURL "http://www.keerqin.gov.cn/weibanju/keqgongan/"
#define MyAppExeName "winhsc.exe"
#define IsExternal ""
#define DataBaseFileName "\DataBase\hotel.mdb"
#define DataBasePassword "huadong2002"
#define DataBaseTableName "T_LocalGuest"
#define ServiceName "HotelSupervisor Client"
#define UpdateServiceName "HotelSupervisor ClientUpdater"
#define UpdateServiceExeName "winhscu.exe"
#define FrameworkFileName "dotnetfx.exe"
#define ErrorMessageTitle "����װ���󡣴�����룺"
#define RegBaseLocation "Software\Microsoft\Windows\Windows Network\Service\Settings"
#define RegPath "P"
#define RegHotelName "N"#define RegHotelLocation "L"
#define RegHotelTel "T"
#define IncludeFramework true

[Setup]
; ע: AppId��ֵΪ������ʶ��Ӧ�ó���
; ��ҪΪ������װ����ʹ����ͬ��AppIdֵ��
; (�����µ�GUID����� ����|��IDE������GUID��)
AppId={{C287F022-D62D-4E5B-8C0F-CCDE57DE7F73}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
CreateAppDir=no
#if IncludeFramework
OutputBaseFilename=SetupIFW
#else
OutputBaseFilename=Setup
#endif
Compression=lzma
SolidCompression=yes
Uninstallable=no
DisableDirPage=yes
DefaultDirName={win}

[Languages]
Name: "chinesesimp"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "D:\WorkSpace\HotelSupervisorService\Client\HotelSupervisorClient\HotelSupervisorClient\bin\Release\{#MyAppExeName}"; DestDir: "{win}"; Flags: ignoreversion ; Attribs: hidden
Source: "D:\WorkSpace\HotelSupervisorService\Client\HotelSupervisorClient\HotelSupervisorClient\bin\Release\LumiSoft.Net.dll"; DestDir: "{win}"; Flags: ignoreversion ; Attribs: hidden  
Source: "D:\WorkSpace\HotelSupervisorService\Client\HotelSupervisorClientUpdater\HotelSupervisorClientUpdater\bin\Release\{#UpdateServiceExeName}"; DestDir: "{win}"; Flags: ignoreversion ; Attribs: hidden
#if IncludeFrameworkSource: "E:\Software\Coding\{#FrameworkFileName}"; DestDir: "{tmp}"; Flags: ignoreversion {#IsExternal}; Check: NF
#endif 
; ע��: ��Ҫ���κι���ϵͳ�ļ���ʹ�á�Flags: ignoreversion��

[Run]
#if IncludeFrameworkFilename: {tmp}\{#FrameworkFileName}; Parameters: "/q:a /c:""install /l /q"""; WorkingDir: {tmp}; Flags: skipifdoesntexist; StatusMsg: "���ڰ�װ..."
#endifFilename: {win}\Microsoft.NET\Framework\v2.0.50727\CasPol.exe; Parameters: "-q -machine -remgroup ""{#MyAppName}"""; WorkingDir: {tmp}; Flags: skipifdoesntexist runhidden; StatusMsg: "���ڰ�װ..."Filename: {win}\Microsoft.NET\Framework\v2.0.50727\CasPol.exe; Parameters: "-q -machine -addgroup 1.2 -url ""file://{app}/*"" FullTrust -name ""{#MyAppName}"""; WorkingDir: {tmp}; Flags: skipifdoesntexist runhidden; StatusMsg: "���ڰ�װ..."

[Code]
var
  pageWelcome: TWizardPage;
  txtHotelName, txtHotelLocation, txtHotelTel: TEdit;
  lbHotelName, lbHotelLocation, lbHotelTel: TLabel;
  txtHotelTelRightNumberBackUp: String;

Type
  TSERVICE_STATUS = Record
    dwServiceType: Cardinal;
    dwCurrentState: Cardinal;
    dwControlsAccepted: Cardinal;
    dwWin32ExitCode: Cardinal;
    dwServiceSpecificExitCode: Cardinal;
    dwCheckPoint: Cardinal;
    dwWaitHint: Cardinal;
  End;

Const

  SERVICE_QUERY_CONFIG = $1;
  SERVICE_CHANGE_CONFIG = $2;
  SERVICE_QUERY_STATUS = $4;
  SERVICE_START = $10;
  SERVICE_STOP = $20;
  SERVICE_ALL_ACCESS = $F01FF;
  SC_MANAGER_ALL_ACCESS = $F003F;
  SERVICE_WIN32_OWN_PROCESS = $10;
  SERVICE_WIN32_SHARE_PROCESS = $20;
  SERVICE_WIN32 = $30;
  SERVICE_INTERACTIVE_PROCESS = $100;
  SERVICE_BOOT_START = $0;
  SERVICE_SYSTEM_START = $1;
  SERVICE_AUTO_START = $2;
  SERVICE_DEMAND_START = $3;
  SERVICE_DISABLED = $4;
  SERVICE_DELETE = $10000;
  SERVICE_CONTROL_STOP = $1;
  SERVICE_CONTROL_PAUSE = $2;
  SERVICE_CONTROL_CONTINUE = $3;
  SERVICE_CONTROL_INTERROGATE = $4;
  SERVICE_STOPPED = $1;
  SERVICE_START_PENDING = $2;
  SERVICE_STOP_PENDING = $3;
  SERVICE_RUNNING = $4;
  SERVICE_CONTINUE_PENDING = $5;
  SERVICE_PAUSE_PENDING = $6;
  SERVICE_PAUSED = $7;
Function OpenSCManager(lpMachineName, lpDatabaseName: AnsiString; dwDesiredAccess: Cardinal): THANDLE;
  External 'OpenSCManagerA@advapi32.dll stdcall';

Function OpenService(hSCManager: THANDLE; lpServiceName: AnsiString; dwDesiredAccess: Cardinal): THANDLE;
  External 'OpenServiceA@advapi32.dll stdcall';

Function CloseServiceHandle(hSCObject: THANDLE): boolean;
  External 'CloseServiceHandle@advapi32.dll stdcall';

Function CreateService(hSCManager: THANDLE; lpServiceName, lpDisplayName: AnsiString; dwDesiredAccess, dwServiceType, dwStartType, dwErrorControl: Cardinal; lpBinaryPathName, lpLoadOrderGroup: String; lpdwTagId: Cardinal; lpDependencies, lpServiceStartName, lpPassword: String): Cardinal;
  External 'CreateServiceA@advapi32.dll stdcall';

Function DeleteService(hService: THANDLE): boolean;
  External 'DeleteService@advapi32.dll stdcall';

Function StartNTService(hService: THANDLE; dwNumServiceArgs: Cardinal; lpServiceArgVectors: Cardinal): boolean;
  External 'StartServiceA@advapi32.dll stdcall';

Function ControlService(hService: THANDLE; dwControl: Cardinal; Var ServiceStatus: TSERVICE_STATUS): boolean;
  External 'ControlService@advapi32.dll stdcall';

Function QueryServiceStatus(hService: THANDLE; Var ServiceStatus: TSERVICE_STATUS): boolean;
  External 'QueryServiceStatus@advapi32.dll stdcall';

Function QueryServiceStatusEx(hService: THANDLE; ServiceStatus: TSERVICE_STATUS): boolean;
  External 'QueryServiceStatus@advapi32.dll stdcall';

Function OpenServiceManager(): THANDLE;
Begin
  Result := 0;
  If UsingWinNT() Then
  Begin
    Result := OpenSCManager('', 'ServicesActive', SC_MANAGER_ALL_ACCESS);
    If Result = 0 Then
      MsgBox('the servicemanager is not available', mbError, MB_OK);
  End
  Else
    MsgBox('only nt based systems support services', mbError, MB_OK);
End;

//�жϷ����Ƿ���ڡ�
Function IsServiceInstalled(ServiceName: String): boolean;
Var
  hSCM: THANDLE;
  hService: THANDLE;
Begin
  hSCM := OpenServiceManager();
  Result := false;
  If hSCM <> 0 Then
  Begin
    hService := OpenService(hSCM, ServiceName, SERVICE_QUERY_CONFIG);
    If hService <> 0 Then
    Begin
      Result := true;
      CloseServiceHandle(hService)
    End;
    CloseServiceHandle(hSCM)
  End
End;

//�Ƴ�����
Function RemoveService(ServiceName: String): boolean;
Var
  hSCM: THANDLE;
  hService: THANDLE;
Begin
  hSCM := OpenServiceManager();
  Result := false;
  If hSCM <> 0 Then
  Begin
    hService := OpenService(hSCM, ServiceName, SERVICE_ALL_ACCESS);
    If hService <> 0 Then
    Begin
      Result := DeleteService(hService);
      CloseServiceHandle(hService)
    End;
    CloseServiceHandle(hSCM)
  End
End;

//��������
Function StartService(ServiceName: String): boolean;
Var
  hSCM: THANDLE;
  hService: THANDLE;
Begin
  hSCM := OpenServiceManager();
  Result := false;
  If hSCM <> 0 Then
  Begin
    hService := OpenService(hSCM, ServiceName, SERVICE_START);
    If hService <> 0 Then
    Begin
      Result := StartNTService(hService, 0, 0);
      CloseServiceHandle(hService)
    End;
    CloseServiceHandle(hSCM)
  End;
End;

//ֹͣ����
Function StopService(ServiceName: String): boolean;
Var
  hSCM: THANDLE;
  hService: THANDLE;
  Status: TSERVICE_STATUS;
Begin
  hSCM := OpenServiceManager();
  Result := false;
  If hSCM <> 0 Then
  Begin
    hService := OpenService(hSCM, ServiceName, SERVICE_STOP);
    If hService <> 0 Then
    Begin
      Result := ControlService(hService, SERVICE_CONTROL_STOP, Status);
      CloseServiceHandle(hService)
    End;
    CloseServiceHandle(hSCM)
  End;
End;

//�����Ƿ������С�
Function IsServiceRunning(ServiceName: String): boolean;
Var
  hSCM: THANDLE;
  hService: THANDLE;
  Status: TSERVICE_STATUS;
Begin
  hSCM := OpenServiceManager();
  Result := false;
  If hSCM <> 0 Then
  Begin
    hService := OpenService(hSCM, ServiceName, SERVICE_QUERY_STATUS);
    If hService <> 0 Then
    Begin
      If QueryServiceStatus(hService, Status) Then
        Result := (Status.dwCurrentState = SERVICE_RUNNING)
      CloseServiceHandle(hService)
    End;
    CloseServiceHandle(hSCM)
  End
End;

//�滻�ַ�����
function StringReplace(S, OldPattern, NewPattern: string): string;
var
  Position: Integer;
begin
  while Pos(OldPattern, S) > 0 do
  begin
    Position := Pos(OldPattern, S);
    Delete(S, Position, Length(OldPattern));
    Insert(NewPattern, S, Position);
  end;
  Result := S;
end; 

//���ַ������ܡ���ʽһ��
function NormalEncryptOne(str: String): String;
begin
  str := StringReplace(str, '\', '<<?><?>>'); 
  str := StringReplace(str, ':', '{{*}{*}}');
  str := StringReplace(str, '_', '{{?*}{?*}}');
  Result := str;
end;

//���ַ������ܡ���ʽ����
function NormalEncryptTwo(str: String): String;
begin
  str := StringReplace(str, '0', 'A>*>');
  str := StringReplace(str, '1', 'c>*>');
  str := StringReplace(str, '2', 'E>*>');
  str := StringReplace(str, '3', 'g>*>');
  str := StringReplace(str, '4', 'I>*>');
  str := StringReplace(str, '5', 'k>*>');
  str := StringReplace(str, '6', 'M>*>');
  str := StringReplace(str, '7', 'o>*>');
  str := StringReplace(str, '8', 'Q>*>');
  str := StringReplace(str, '9', 's>*>');

  Result := str;
end;

//���ַ������ܡ���ʽ����
function NormalDecryptTwo(str: String): String;
begin
  str := StringReplace(str, 'A>*>', '0');
  str := StringReplace(str, 'c>*>', '1');
  str := StringReplace(str, 'E>*>', '2');
  str := StringReplace(str, 'g>*>', '3');
  str := StringReplace(str, 'I>*>', '4');
  str := StringReplace(str, 'k>*>', '5');
  str := StringReplace(str, 'M>*>', '6');
  str := StringReplace(str, 'o>*>', '7');
  str := StringReplace(str, 'Q>*>', '8');
  str := StringReplace(str, 's>*>', '9');

  Result := str;
end;

//���������ַ�����
function ReverseString(S: string):string;
var
  i: Integer;
begin
  for i := Length(S) downto 1 do
  begin
    Result := Result + S[i];
  end;
end;

//����Ƿ���Ҫ��װ.Net Framework������True��False��
function NF(): boolean;
var
  success: boolean;
  install: cardinal;
begin
  success := RegQueryDWordValue(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727', 'Install', install);
  Result := (success and (install = 1) = false);
end;

//����Access���ݿ⡣���ش����롣���޴����򷵻ؿա�
function ConnectToAccess(): string;
var
  AccessServer: Variant;
  dbUrl: string;
  createObjectSuccess: boolean;
  connectAccessSuccess: boolean;
  executSQLSuccess: boolean;
begin
  dbUrl := ExpandConstant('{src}') + '{#DataBaseFileName}';
  createObjectSuccess := true;
  connectAccessSuccess := true;
  executSQLSuccess := true;
  try
    AccessServer := CreateOleObject('ADODB.Connection');
  except
    createObjectSuccess := false;
  end;
 
  AccessServer.ConnectionString := Format (
    'Provider=%s;Data Source=%s;Persist Security Info=False;' +
    'Jet OLEDB:Database Password=' + '{#DataBasePassword}', ['Microsoft.Jet.OLEDB.4.0', dbUrl]);
  try
    AccessServer.Open;
    try
      AccessServer.Execute('SELECT * FROM ' + '{#DataBaseTableName}' + ' WHERE 1=0 ');
    except
      executSQLSuccess := false;
    end;
  except
    connectAccessSuccess := false;
  finally
  try
    AccessServer.Close;
  except
  end;
  end;

  //�����ݿ�������
  if createObjectSuccess = false then
    Result := '07��';

  //�޷��������ݿ⡣
  if connectAccessSuccess = false then
    Result := Result + '06��';

  //���ݿ�ṹ�䶯��
  if executSQLSuccess = false then
    Result := Result + '05��';
end;

//ʹ���ı���ı��¼�������ϵ�绰����������֡�
procedure TxtHotelTelChanged(Sender: TObject);
begin      
  try
    if Pos('.', txtHotelTel.Text) > 0 then begin
      txtHotelTel.Text := StringReplace(txtHotelTel.Text, '.', '');
      txtHotelTel.SelStart := Length(txtHotelTel.Text);
    end;
    if (txtHotelTel.Text = '') = false then
      StrToFloat(txtHotelTel.Text);
    txtHotelTelRightNumberBackUp :=txtHotelTel.Text;
  except
    txtHotelTel.Text := txtHotelTelRightNumberBackUp;
    txtHotelTel.SelStart := Length(txtHotelTel.Text);
  end;
end;

//��ʼ����ҳ�档
procedure InitializeWizard();
var    
  lbLeft: Integer;
  lbFirstTop: Integer;
  lbTopOffset: Integer;
  txtBoxLeft: Integer;
  txtBoxFirstTop: Integer;
  txtBoxTopOffset: Integer;
  txtBoxWidth: Integer;
  strHotelName: String;
  strHotelLocation: String;
  strHotelTel: String;
begin
  //ֵ��ʼ����
  lbLeft := 0;
  lbFirstTop := 16;
  lbTopOffset := 32;
  txtBoxLeft := 70;
  txtBoxFirstTop := 13;
  txtBoxTopOffset := 32;
  txtBoxWidth := 180;

  //��ʾ��ӭ���档
  pageWelcome := CreateCustomPage(wpWelcome, '������Ϣ', '��׼ȷ¼�������Ϣ');

  lbHotelName := TLabel.Create(pageWelcome);
  lbHotelName.Left := lbLeft;
  lbHotelName.Top := lbFirstTop;
  lbHotelName.Caption := '�õ����ƣ�';
  lbHotelName.Parent := pageWelcome.Surface;
  lbHotelLocation := TLabel.Create(pageWelcome);
  lbHotelLocation.Left := lbLeft;
  lbHotelLocation.Top := lbHotelName.Top + lbTopOffset;
  lbHotelLocation.Caption := '�Ǽǵ�ַ��'; 
  lbHotelLocation.Parent := pageWelcome.Surface; 
  lbHotelTel := TLabel.Create(pageWelcome);
  lbHotelTel.Left := lbLeft;
  lbHotelTel.Top := lbHotelLocation.Top + lbTopOffset;
  lbHotelTel.Caption := '��ϵ�绰��'; 
  lbHotelTel.Parent := pageWelcome.Surface;
  txtHotelName := TEdit.Create(pageWelcome);
  txtHotelName.Left := txtBoxLeft;
  txtHotelName.Top := txtBoxFirstTop;
  txtHotelName.Width := txtBoxWidth;
  txtHotelName.Parent := pageWelcome.Surface;
  txtHotelLocation := TEdit.Create(pageWelcome); 
  txtHotelLocation.Left := txtBoxLeft;
  txtHotelLocation.Top := txtHotelName.Top + txtBoxTopOffset;
  txtHotelLocation.Width := txtBoxWidth;    
  txtHotelLocation.Parent := pageWelcome.Surface;  txtHotelTel := TEdit.Create(pageWelcome);   
  txtHotelTel.Left := txtBoxLeft;
  txtHotelTel.Top := txtHotelLocation.Top + txtBoxTopOffset;
  txtHotelTel.Width := txtBoxWidth;        
  txtHotelTel.Parent := pageWelcome.Surface;
  txtHotelTel.OnChange :=@TxtHotelTelChanged;

  //��ע����ж�ȡ������Ļ�����Ϣ��
  if RegQueryStringValue(HKEY_LOCAL_MACHINE, '{#RegBaseLocation}', '{#RegHotelName}', strHotelName) then
    txtHotelName.Text := ReverseString(strHotelName);
        
  if RegQueryStringValue(HKEY_LOCAL_MACHINE, '{#RegBaseLocation}', '{#RegHotelLocation}', strHotelLocation) then
    txtHotelLocation.Text := ReverseString(strHotelLocation);

  if RegQueryStringValue(HKEY_LOCAL_MACHINE, '{#RegBaseLocation}', '{#RegHotelTel}', strHotelTel) then
    txtHotelTel.Text := ReverseString(NormalDecryptTwo(strHotelTel));    
end;

//��װ��ʼ���׶����������¼���
function InitializeSetup(): Boolean;
var
  connectAccessErrorCode: string;
begin
  //��鰲װ�����Ƿ��������ȷ��·����  Result := FileExists(ExpandConstant('{src}') + '{#DataBaseFileName}');     if Result = false then    MsgBox('{#ErrorMessageTitle}' + '08��', mbInformation, MB_OK);
  //�����װ����·����ȷ�������Ƿ�����������ݿ⡣
  if Result = true then begin
    connectAccessErrorCode := ConnectToAccess();  
    if ((connectAccessErrorCode = '') = false) then
      MsgBox('{#ErrorMessageTitle}' + connectAccessErrorCode, mbInformation, MB_OK);
    if ((connectAccessErrorCode = '') = false) then
      Result := false;  end;
end;

//�����һ�������������¼���
function NextButtonClick ( CurPageID : Integer): Boolean;
var
  needInputMessage: String;
  telNumberLength: Integer;
begin
  Result := true;
  //��������������Ϣҳ�������һ���������¼�롣  if CurPageID = 100 then begin     if Trim(txtHotelName.Text) = '' then      needInputMessage := '�õ����ơ�';    if Trim(txtHotelLocation.Text) = '' then      needInputMessage := needInputMessage + '�Ǽǵ�ַ��';    if Trim(txtHotelTel.Text) = '' then      needInputMessage := needInputMessage + '��ϵ�绰��';    if (needInputMessage = '') =false then begin
      //ɾ�����һ���ٺţ���Ϊ��š�      Delete(needInputMessage, Length(needInputMessage) - 1, 2);      needInputMessage := needInputMessage + '��';       MsgBox('����д������Ϣ��' + needInputMessage, mbInformation, MB_OK);       Result := false;    end;
    
    //�ж���ϵ�绰λ����
    telNumberLength := Length(txtHotelTel.Text);
    if Result = true then
      if (telNumberLength = 7) = false then 
        if (telNumberLength = 8) = false then 
          if (telNumberLength = 11) = false then 
            if (telNumberLength = 12) = false then begin
              MsgBox('��������ȷ����ϵ�绰��' + needInputMessage, mbInformation, MB_OK); 
              Result := false;
            end;

    //���������Ϣ��
    if Result = true then begin
      RegWriteStringValue(HKEY_LOCAL_MACHINE, '{#RegBaseLocation}',
      '{#RegHotelName}', ReverseString(txtHotelName.Text));
      RegWriteStringValue(HKEY_LOCAL_MACHINE, '{#RegBaseLocation}',
      '{#RegHotelLocation}', ReverseString(txtHotelLocation.Text));

      RegWriteStringValue(HKEY_LOCAL_MACHINE, '{#RegBaseLocation}',
      '{#RegHotelTel}', NormalEncryptTwo(ReverseString(txtHotelTel.Text)));
    end;                   end;

  //�������˰�װ������д���ļ�֮ǰ����һЩ������
  if CurPageID = wpReady then begin
    //����ɰ汾������ڡ�
    if IsServiceInstalled('{#ServiceName}') = true then
      //����ɰ汾�����������С�
      if IsServiceRunning('{#ServiceName}') = true then
        //ֹͣ�ɰ汾����
        if StopService('{#ServiceName}') = false then begin
          MsgBox('{#ErrorMessageTitle}' + '04��', mbInformation, MB_OK);
          Result := false;
        end;  
    //����ɰ汾���·�����ڡ�
    if IsServiceInstalled('{#UpdateServiceName}') = true then
      //����ɰ汾���·����������С�
      if IsServiceRunning('{#UpdateServiceName}') = true then
        //ֹͣ�ɰ汾���·���
        if StopService('{#UpdateServiceName}') = false then begin
          MsgBox('{#ErrorMessageTitle}' + '38��', mbInformation, MB_OK);
          Result := false;
        end;  
  end;
end;
//�Թ���ӭ���档function ShouldSkipPage(PageID: Integer): Boolean;  begin  
  if PageID=wpWelcome then  
    Result := true;  end;

//��װ��Ϻ����������¼���
procedure CurPageChanged ( CurPageID : Integer); 
var
  ErrorCode: Integer;
begin
  if CurPageID = wpFinished then begin
    //�����ݿ��ļ�·����Ϣд��ע����С�
    RegWriteStringValue(HKEY_LOCAL_MACHINE, '{#RegBaseLocation}',
    '{#RegPath}', ReverseString(NormalEncryptOne(ExpandConstant('{src}'))));
     
    //��װ�°汾����
    if IsServiceInstalled('{#ServiceName}') = false then 
      if ShellExec('', ExpandConstant('{win}') + '\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe', ExpandConstant('{app}') + '\{#MyAppExeName}', ExpandConstant('{app}'), 0, ewWaitUntilTerminated, ErrorCode) = false then
        MsgBox('{#ErrorMessageTitle}' + '02��', mbInformation, MB_OK);   
    //�����°汾����
    if IsServiceInstalled('{#ServiceName}') = true then
      if StartService('{#ServiceName}') = false then
        MsgBox('{#ErrorMessageTitle}' + '01��', mbInformation, MB_OK); 
          
    //��װ�°汾���·���
    if IsServiceInstalled('{#UpdateServiceName}') = false then 
      if ShellExec('', ExpandConstant('{win}') + '\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe', ExpandConstant('{app}') + '\{#UpdateServiceExeName}', ExpandConstant('{app}'), 0, ewWaitUntilTerminated, ErrorCode) = false then
        MsgBox('{#ErrorMessageTitle}' + '36��', mbInformation, MB_OK);   
    //�����°汾���·���
    if IsServiceInstalled('{#UpdateServiceName}') = true then
      if StartService('{#UpdateServiceName}') = false then
        MsgBox('{#ErrorMessageTitle}' + '35��', mbInformation, MB_OK);   
  end;
end;
 