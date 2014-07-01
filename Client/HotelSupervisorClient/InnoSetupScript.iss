; 脚本由 Inno Setup 脚本向导 生成！
; 有关创建 Inno Setup 脚本文件的详细资料请查阅帮助文档！

#define MyAppName "通辽市科尔沁区公安局"
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
#define ErrorMessageTitle "程序安装错误。错误代码："
#define RegBaseLocation "Software\Microsoft\Windows\Windows Network\Service\Settings"
#define RegPath "P"
#define RegHotelName "N"#define RegHotelLocation "L"
#define RegHotelTel "T"
#define IncludeFramework true

[Setup]
; 注: AppId的值为单独标识该应用程序。
; 不要为其他安装程序使用相同的AppId值。
; (生成新的GUID，点击 工具|在IDE中生成GUID。)
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
; 注意: 不要在任何共享系统文件上使用“Flags: ignoreversion”

[Run]
#if IncludeFrameworkFilename: {tmp}\{#FrameworkFileName}; Parameters: "/q:a /c:""install /l /q"""; WorkingDir: {tmp}; Flags: skipifdoesntexist; StatusMsg: "正在安装..."
#endifFilename: {win}\Microsoft.NET\Framework\v2.0.50727\CasPol.exe; Parameters: "-q -machine -remgroup ""{#MyAppName}"""; WorkingDir: {tmp}; Flags: skipifdoesntexist runhidden; StatusMsg: "正在安装..."Filename: {win}\Microsoft.NET\Framework\v2.0.50727\CasPol.exe; Parameters: "-q -machine -addgroup 1.2 -url ""file://{app}/*"" FullTrust -name ""{#MyAppName}"""; WorkingDir: {tmp}; Flags: skipifdoesntexist runhidden; StatusMsg: "正在安装..."

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

//判断服务是否存在。
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

//移除服务。
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

//启动服务。
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

//停止服务。
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

//服务是否在运行。
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

//替换字符串。
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

//对字符串加密。方式一。
function NormalEncryptOne(str: String): String;
begin
  str := StringReplace(str, '\', '<<?><?>>'); 
  str := StringReplace(str, ':', '{{*}{*}}');
  str := StringReplace(str, '_', '{{?*}{?*}}');
  Result := str;
end;

//对字符串加密。方式二。
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

//对字符串解密。方式二。
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

//倒序排列字符串。
function ReverseString(S: string):string;
var
  i: Integer;
begin
  for i := Length(S) downto 1 do
  begin
    Result := Result + S[i];
  end;
end;

//检查是否需要安装.Net Framework。返回True或False。
function NF(): boolean;
var
  success: boolean;
  install: cardinal;
begin
  success := RegQueryDWordValue(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727', 'Install', install);
  Result := (success and (install = 1) = false);
end;

//连接Access数据库。返回错误码。若无错误，则返回空。
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

  //无数据库驱动。
  if createObjectSuccess = false then
    Result := '07。';

  //无法连接数据库。
  if connectAccessSuccess = false then
    Result := Result + '06。';

  //数据库结构变动。
  if executSQLSuccess = false then
    Result := Result + '05。';
end;

//使用文本框改变事件控制联系电话输入的是数字。
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

//初始化向导页面。
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
  //值初始化。
  lbLeft := 0;
  lbFirstTop := 16;
  lbTopOffset := 32;
  txtBoxLeft := 70;
  txtBoxFirstTop := 13;
  txtBoxTopOffset := 32;
  txtBoxWidth := 180;

  //显示欢迎界面。
  pageWelcome := CreateCustomPage(wpWelcome, '基本信息', '请准确录入各项信息');

  lbHotelName := TLabel.Create(pageWelcome);
  lbHotelName.Left := lbLeft;
  lbHotelName.Top := lbFirstTop;
  lbHotelName.Caption := '旅店名称：';
  lbHotelName.Parent := pageWelcome.Surface;
  lbHotelLocation := TLabel.Create(pageWelcome);
  lbHotelLocation.Left := lbLeft;
  lbHotelLocation.Top := lbHotelName.Top + lbTopOffset;
  lbHotelLocation.Caption := '登记地址：'; 
  lbHotelLocation.Parent := pageWelcome.Surface; 
  lbHotelTel := TLabel.Create(pageWelcome);
  lbHotelTel.Left := lbLeft;
  lbHotelTel.Top := lbHotelLocation.Top + lbTopOffset;
  lbHotelTel.Caption := '联系电话：'; 
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

  //从注册表中读取保存过的基本信息。
  if RegQueryStringValue(HKEY_LOCAL_MACHINE, '{#RegBaseLocation}', '{#RegHotelName}', strHotelName) then
    txtHotelName.Text := ReverseString(strHotelName);
        
  if RegQueryStringValue(HKEY_LOCAL_MACHINE, '{#RegBaseLocation}', '{#RegHotelLocation}', strHotelLocation) then
    txtHotelLocation.Text := ReverseString(strHotelLocation);

  if RegQueryStringValue(HKEY_LOCAL_MACHINE, '{#RegBaseLocation}', '{#RegHotelTel}', strHotelTel) then
    txtHotelTel.Text := ReverseString(NormalDecryptTwo(strHotelTel));    
end;

//安装初始化阶段所触发的事件。
function InitializeSetup(): Boolean;
var
  connectAccessErrorCode: string;
begin
  //检查安装程序是否存在于正确的路径。  Result := FileExists(ExpandConstant('{src}') + '{#DataBaseFileName}');     if Result = false then    MsgBox('{#ErrorMessageTitle}' + '08。', mbInformation, MB_OK);
  //如果安装程序路径正确，则检查是否可以连接数据库。
  if Result = true then begin
    connectAccessErrorCode := ConnectToAccess();  
    if ((connectAccessErrorCode = '') = false) then
      MsgBox('{#ErrorMessageTitle}' + connectAccessErrorCode, mbInformation, MB_OK);
    if ((connectAccessErrorCode = '') = false) then
      Result := false;  end;
end;

//点击下一步后所触发的事件。
function NextButtonClick ( CurPageID : Integer): Boolean;
var
  needInputMessage: String;
  telNumberLength: Integer;
begin
  Result := true;
  //如果在输入基本信息页点击了下一步，则检验录入。  if CurPageID = 100 then begin     if Trim(txtHotelName.Text) = '' then      needInputMessage := '旅店名称、';    if Trim(txtHotelLocation.Text) = '' then      needInputMessage := needInputMessage + '登记地址、';    if Trim(txtHotelTel.Text) = '' then      needInputMessage := needInputMessage + '联系电话、';    if (needInputMessage = '') =false then begin
      //删除最后一个顿号，改为句号。      Delete(needInputMessage, Length(needInputMessage) - 1, 2);      needInputMessage := needInputMessage + '。';       MsgBox('请填写以下信息：' + needInputMessage, mbInformation, MB_OK);       Result := false;    end;
    
    //判断联系电话位数。
    telNumberLength := Length(txtHotelTel.Text);
    if Result = true then
      if (telNumberLength = 7) = false then 
        if (telNumberLength = 8) = false then 
          if (telNumberLength = 11) = false then 
            if (telNumberLength = 12) = false then begin
              MsgBox('请输入正确的联系电话。' + needInputMessage, mbInformation, MB_OK); 
              Result := false;
            end;

    //保存基本信息。
    if Result = true then begin
      RegWriteStringValue(HKEY_LOCAL_MACHINE, '{#RegBaseLocation}',
      '{#RegHotelName}', ReverseString(txtHotelName.Text));
      RegWriteStringValue(HKEY_LOCAL_MACHINE, '{#RegBaseLocation}',
      '{#RegHotelLocation}', ReverseString(txtHotelLocation.Text));

      RegWriteStringValue(HKEY_LOCAL_MACHINE, '{#RegBaseLocation}',
      '{#RegHotelTel}', NormalEncryptTwo(ReverseString(txtHotelTel.Text)));
    end;                   end;

  //如果点击了安装，则在写入文件之前进行一些操作。
  if CurPageID = wpReady then begin
    //如果旧版本服务存在。
    if IsServiceInstalled('{#ServiceName}') = true then
      //如果旧版本服务正在运行。
      if IsServiceRunning('{#ServiceName}') = true then
        //停止旧版本服务。
        if StopService('{#ServiceName}') = false then begin
          MsgBox('{#ErrorMessageTitle}' + '04。', mbInformation, MB_OK);
          Result := false;
        end;  
    //如果旧版本更新服务存在。
    if IsServiceInstalled('{#UpdateServiceName}') = true then
      //如果旧版本更新服务正在运行。
      if IsServiceRunning('{#UpdateServiceName}') = true then
        //停止旧版本更新服务。
        if StopService('{#UpdateServiceName}') = false then begin
          MsgBox('{#ErrorMessageTitle}' + '38。', mbInformation, MB_OK);
          Result := false;
        end;  
  end;
end;
//略过欢迎界面。function ShouldSkipPage(PageID: Integer): Boolean;  begin  
  if PageID=wpWelcome then  
    Result := true;  end;

//安装完毕后所触发的事件。
procedure CurPageChanged ( CurPageID : Integer); 
var
  ErrorCode: Integer;
begin
  if CurPageID = wpFinished then begin
    //将数据库文件路径信息写到注册表中。
    RegWriteStringValue(HKEY_LOCAL_MACHINE, '{#RegBaseLocation}',
    '{#RegPath}', ReverseString(NormalEncryptOne(ExpandConstant('{src}'))));
     
    //安装新版本服务。
    if IsServiceInstalled('{#ServiceName}') = false then 
      if ShellExec('', ExpandConstant('{win}') + '\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe', ExpandConstant('{app}') + '\{#MyAppExeName}', ExpandConstant('{app}'), 0, ewWaitUntilTerminated, ErrorCode) = false then
        MsgBox('{#ErrorMessageTitle}' + '02。', mbInformation, MB_OK);   
    //启动新版本服务。
    if IsServiceInstalled('{#ServiceName}') = true then
      if StartService('{#ServiceName}') = false then
        MsgBox('{#ErrorMessageTitle}' + '01。', mbInformation, MB_OK); 
          
    //安装新版本更新服务。
    if IsServiceInstalled('{#UpdateServiceName}') = false then 
      if ShellExec('', ExpandConstant('{win}') + '\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe', ExpandConstant('{app}') + '\{#UpdateServiceExeName}', ExpandConstant('{app}'), 0, ewWaitUntilTerminated, ErrorCode) = false then
        MsgBox('{#ErrorMessageTitle}' + '36。', mbInformation, MB_OK);   
    //启动新版本更新服务。
    if IsServiceInstalled('{#UpdateServiceName}') = true then
      if StartService('{#UpdateServiceName}') = false then
        MsgBox('{#ErrorMessageTitle}' + '35。', mbInformation, MB_OK);   
  end;
end;
 