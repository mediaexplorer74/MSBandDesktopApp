// Strings

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace DesktopSyncApp
{

  public class Strings
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Strings()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static ResourceManager ResourceManager
    {
      get
      {
        if (Strings.resourceMan == null)
          Strings.resourceMan = new ResourceManager("DesktopSyncApp.ResourceLinks.Strings", typeof (Strings).Assembly);
        return Strings.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static CultureInfo Culture
    {
      get => Strings.resourceCulture;
      set => Strings.resourceCulture = value;
    }

    public static string DefaultMessageCannedResponses1 => Strings.ResourceManager.GetString(nameof (DefaultMessageCannedResponses1), Strings.resourceCulture);

    public static string DefaultMessageCannedResponses2 => Strings.ResourceManager.GetString(nameof (DefaultMessageCannedResponses2), Strings.resourceCulture);

    public static string DefaultPhoneCannedResponses1 => Strings.ResourceManager.GetString(nameof (DefaultPhoneCannedResponses1), Strings.resourceCulture);

    public static string DefaultPhoneCannedResponses2 => Strings.ResourceManager.GetString(nameof (DefaultPhoneCannedResponses2), Strings.resourceCulture);

    public static string DefaultPhoneCannedResponses3 => Strings.ResourceManager.GetString(nameof (DefaultPhoneCannedResponses3), Strings.resourceCulture);

    public static string DOW_Friday => Strings.ResourceManager.GetString(nameof (DOW_Friday), Strings.resourceCulture);

    public static string DOW_Monday => Strings.ResourceManager.GetString(nameof (DOW_Monday), Strings.resourceCulture);

    public static string DOW_Saturday => Strings.ResourceManager.GetString(nameof (DOW_Saturday), Strings.resourceCulture);

    public static string DOW_Sunday => Strings.ResourceManager.GetString(nameof (DOW_Sunday), Strings.resourceCulture);

    public static string DOW_Thursday => Strings.ResourceManager.GetString(nameof (DOW_Thursday), Strings.resourceCulture);

    public static string DOW_Tuesday => Strings.ResourceManager.GetString(nameof (DOW_Tuesday), Strings.resourceCulture);

    public static string DOW_Wednesday => Strings.ResourceManager.GetString(nameof (DOW_Wednesday), Strings.resourceCulture);

    public static string FirmwareVersionDebugAbbr => Strings.ResourceManager.GetString(nameof (FirmwareVersionDebugAbbr), Strings.resourceCulture);

    public static string FirmwareVersionReleaseAbbr => Strings.ResourceManager.GetString(nameof (FirmwareVersionReleaseAbbr), Strings.resourceCulture);

    public static string Format_AppVersion => Strings.ResourceManager.GetString(nameof (Format_AppVersion), Strings.resourceCulture);

    public static string Format_FirmwareVersion => Strings.ResourceManager.GetString(nameof (Format_FirmwareVersion), Strings.resourceCulture);

    public static string Format_FirmwareVersionShort => Strings.ResourceManager.GetString(nameof (Format_FirmwareVersionShort), Strings.resourceCulture);

    public static string Format_LastSync_Long => Strings.ResourceManager.GetString(nameof (Format_LastSync_Long), Strings.resourceCulture);

    public static string Format_LastSync_Short => Strings.ResourceManager.GetString(nameof (Format_LastSync_Short), Strings.resourceCulture);

    public static string Label_AppVersion => Strings.ResourceManager.GetString(nameof (Label_AppVersion), Strings.resourceCulture);

    public static string Label_Button_DFSupport => Strings.ResourceManager.GetString(nameof (Label_Button_DFSupport), Strings.resourceCulture);

    public static string Label_Button_PrivacyPolicy => Strings.ResourceManager.GetString(nameof (Label_Button_PrivacyPolicy), Strings.resourceCulture);

    public static string Label_Button_Support => Strings.ResourceManager.GetString(nameof (Label_Button_Support), Strings.resourceCulture);

    public static string Label_Button_TermsOfUse => Strings.ResourceManager.GetString(nameof (Label_Button_TermsOfUse), Strings.resourceCulture);

    public static string Label_Button_ThirdPartyNotices => Strings.ResourceManager.GetString(nameof (Label_Button_ThirdPartyNotices), Strings.resourceCulture);

        public static string Label_CheckBox_Checked
        {
            get
            {
                return Strings.ResourceManager.GetString(nameof(Label_CheckBox_Checked), Strings.resourceCulture);
            }
        }

        public static string Label_CheckBox_Unchecked => Strings.ResourceManager.GetString(nameof (Label_CheckBox_Unchecked), Strings.resourceCulture);

    public static string Label_Color => Strings.ResourceManager.GetString(nameof (Label_Color), Strings.resourceCulture);

    public static string Label_DeviceFWVersion => Strings.ResourceManager.GetString(nameof (Label_DeviceFWVersion), Strings.resourceCulture);

    public static string Label_DeviceSerialNumber => Strings.ResourceManager.GetString(nameof (Label_DeviceSerialNumber), Strings.resourceCulture);

    public static string Label_Gender_Female => Strings.ResourceManager.GetString(nameof (Label_Gender_Female), Strings.resourceCulture);

    public static string Label_Gender_Male => Strings.ResourceManager.GetString(nameof (Label_Gender_Male), Strings.resourceCulture);

    public static string Label_Profile_Age => Strings.ResourceManager.GetString(nameof (Label_Profile_Age), Strings.resourceCulture);

    public static string Label_Profile_Birthdate => Strings.ResourceManager.GetString(nameof (Label_Profile_Birthdate), Strings.resourceCulture);

    public static string Label_Profile_DeviceName => Strings.ResourceManager.GetString(nameof (Label_Profile_DeviceName), Strings.resourceCulture);

    public static string Label_Profile_DeviceName_Required => Strings.ResourceManager.GetString(nameof (Label_Profile_DeviceName_Required), Strings.resourceCulture);

    public static string Label_Profile_Gender => Strings.ResourceManager.GetString(nameof (Label_Profile_Gender), Strings.resourceCulture);

    public static string Label_Profile_Height => Strings.ResourceManager.GetString(nameof (Label_Profile_Height), Strings.resourceCulture);

    public static string Label_Profile_Name => Strings.ResourceManager.GetString(nameof (Label_Profile_Name), Strings.resourceCulture);

    public static string Label_Profile_Name_Required => Strings.ResourceManager.GetString(nameof (Label_Profile_Name_Required), Strings.resourceCulture);

    public static string Label_Profile_Temperature => Strings.ResourceManager.GetString(nameof (Label_Profile_Temperature), Strings.resourceCulture);

    public static string Label_Profile_Weight => Strings.ResourceManager.GetString(nameof (Label_Profile_Weight), Strings.resourceCulture);

    public static string Label_Profile_ZipCode => Strings.ResourceManager.GetString(nameof (Label_Profile_ZipCode), Strings.resourceCulture);

    public static string Label_Profile_ZipCode_Optional => Strings.ResourceManager.GetString(nameof (Label_Profile_ZipCode_Optional), Strings.resourceCulture);

    public static string Label_UoM_Celsius => Strings.ResourceManager.GetString(nameof (Label_UoM_Celsius), Strings.resourceCulture);

    public static string Label_UoM_Fahrenheit => Strings.ResourceManager.GetString(nameof (Label_UoM_Fahrenheit), Strings.resourceCulture);

    public static string Label_UoM_Kilograms => Strings.ResourceManager.GetString(nameof (Label_UoM_Kilograms), Strings.resourceCulture);

    public static string Label_UoM_Metric => Strings.ResourceManager.GetString(nameof (Label_UoM_Metric), Strings.resourceCulture);

    public static string Label_UoM_Pounds => Strings.ResourceManager.GetString(nameof (Label_UoM_Pounds), Strings.resourceCulture);

    public static string Label_UoM_Standard => Strings.ResourceManager.GetString(nameof (Label_UoM_Standard), Strings.resourceCulture);

    public static string Label_Wallpaper => Strings.ResourceManager.GetString(nameof (Label_Wallpaper), Strings.resourceCulture);

    public static string Message_DeviceConnectErrorOccurred => Strings.ResourceManager.GetString(nameof (Message_DeviceConnectErrorOccurred), Strings.resourceCulture);

    public static string Message_DevicePairingErrorOccurred => Strings.ResourceManager.GetString(nameof (Message_DevicePairingErrorOccurred), Strings.resourceCulture);

    public static string Message_DeviceUnpairingErrorOccurred => Strings.ResourceManager.GetString(nameof (Message_DeviceUnpairingErrorOccurred), Strings.resourceCulture);

    public static string Message_Error_ConnectReconnect => Strings.ResourceManager.GetString(nameof (Message_Error_ConnectReconnect), Strings.resourceCulture);

    public static string Message_ErrorOccurred => Strings.ResourceManager.GetString(nameof (Message_ErrorOccurred), Strings.resourceCulture);

    public static string Message_LoginDialog_Working => Strings.ResourceManager.GetString(nameof (Message_LoginDialog_Working), Strings.resourceCulture);

    public static string Message_LoginErrorOccurred => Strings.ResourceManager.GetString(nameof (Message_LoginErrorOccurred), Strings.resourceCulture);

    public static string Message_LogoutErrorOccurred => Strings.ResourceManager.GetString(nameof (Message_LogoutErrorOccurred), Strings.resourceCulture);

    public static string Message_Page_CantRegister_Reset => Strings.ResourceManager.GetString(nameof (Message_Page_CantRegister_Reset), Strings.resourceCulture);

    public static string Message_Page_CantRegister_Unregister => Strings.ResourceManager.GetString(nameof (Message_Page_CantRegister_Unregister), Strings.resourceCulture);

    public static string Message_Page_CantRegister_UnregisterReset => Strings.ResourceManager.GetString(nameof (Message_Page_CantRegister_UnregisterReset), Strings.resourceCulture);

    public static string Message_Page_FirmwareUpdateReady => Strings.ResourceManager.GetString(nameof (Message_Page_FirmwareUpdateReady), Strings.resourceCulture);

    public static string Message_Page_FirmwareUpdating => Strings.ResourceManager.GetString(nameof (Message_Page_FirmwareUpdating), Strings.resourceCulture);

    public static string Message_Page_ForgetDeviceConfirm_01 => Strings.ResourceManager.GetString(nameof (Message_Page_ForgetDeviceConfirm_01), Strings.resourceCulture);

    public static string Message_Page_ForgetDeviceConfirm_02 => Strings.ResourceManager.GetString(nameof (Message_Page_ForgetDeviceConfirm_02), Strings.resourceCulture);

    public static string Message_Page_InstallSoftware => Strings.ResourceManager.GetString(nameof (Message_Page_InstallSoftware), Strings.resourceCulture);

    public static string Message_Page_Multiple => Strings.ResourceManager.GetString(nameof (Message_Page_Multiple), Strings.resourceCulture);

    public static string Message_Page_NewAppDescription => Strings.ResourceManager.GetString(nameof (Message_Page_NewAppDescription), Strings.resourceCulture);

    public static string Message_Page_PairConfirm => Strings.ResourceManager.GetString(nameof (Message_Page_PairConfirm), Strings.resourceCulture);

    public static string Message_Page_PairDevice_01 => Strings.ResourceManager.GetString(nameof (Message_Page_PairDevice_01), Strings.resourceCulture);

    public static string Message_Page_PairDevice_02 => Strings.ResourceManager.GetString(nameof (Message_Page_PairDevice_02), Strings.resourceCulture);

    public static string Message_Page_Profile_OOBE => Strings.ResourceManager.GetString(nameof (Message_Page_Profile_OOBE), Strings.resourceCulture);

    public static string Message_Page_UninstallPart1 => Strings.ResourceManager.GetString(nameof (Message_Page_UninstallPart1), Strings.resourceCulture);

    public static string Message_Page_UninstallPart2 => Strings.ResourceManager.GetString(nameof (Message_Page_UninstallPart2), Strings.resourceCulture);

    public static string Message_ProfileRefreshErrorOccurred => Strings.ResourceManager.GetString(nameof (Message_ProfileRefreshErrorOccurred), Strings.resourceCulture);

    public static string Message_ProfileSaveErrorOccurred => Strings.ResourceManager.GetString(nameof (Message_ProfileSaveErrorOccurred), Strings.resourceCulture);

    public static string Message_WebErrorOccurred => Strings.ResourceManager.GetString(nameof (Message_WebErrorOccurred), Strings.resourceCulture);

    public static string MissingDeviceName => Strings.ResourceManager.GetString(nameof (MissingDeviceName), Strings.resourceCulture);

    public static string OobeBandNameString => Strings.ResourceManager.GetString(nameof (OobeBandNameString), Strings.resourceCulture);

    public static string Settings_Bike_DataPoint0 => Strings.ResourceManager.GetString(nameof (Settings_Bike_DataPoint0), Strings.resourceCulture);

    public static string Settings_Bike_DataPoint1 => Strings.ResourceManager.GetString(nameof (Settings_Bike_DataPoint1), Strings.resourceCulture);

    public static string Settings_Bike_DataPoint2 => Strings.ResourceManager.GetString(nameof (Settings_Bike_DataPoint2), Strings.resourceCulture);

    public static string Settings_Bike_DataPoint3 => Strings.ResourceManager.GetString(nameof (Settings_Bike_DataPoint3), Strings.resourceCulture);

    public static string Settings_Bike_DataPoint4 => Strings.ResourceManager.GetString(nameof (Settings_Bike_DataPoint4), Strings.resourceCulture);

    public static string Settings_Bike_DataPoint5 => Strings.ResourceManager.GetString(nameof (Settings_Bike_DataPoint5), Strings.resourceCulture);

    public static string Settings_Bike_DataPoint6 => Strings.ResourceManager.GetString(nameof (Settings_Bike_DataPoint6), Strings.resourceCulture);

    public static string Settings_Bike_DataPoint7 => Strings.ResourceManager.GetString(nameof (Settings_Bike_DataPoint7), Strings.resourceCulture);

    public static string Settings_Bike_DataPoint8 => Strings.ResourceManager.GetString(nameof (Settings_Bike_DataPoint8), Strings.resourceCulture);

    public static string Settings_Bike_DataPoint9 => Strings.ResourceManager.GetString(nameof (Settings_Bike_DataPoint9), Strings.resourceCulture);

    public static string Settings_Bike_MainDataGrouping => Strings.ResourceManager.GetString(nameof (Settings_Bike_MainDataGrouping), Strings.resourceCulture);

    public static string Settings_Bike_SecondaryDataGrouping => Strings.ResourceManager.GetString(nameof (Settings_Bike_SecondaryDataGrouping), Strings.resourceCulture);

    public static string Settings_Bike_SplitMarkerHeader => Strings.ResourceManager.GetString(nameof (Settings_Bike_SplitMarkerHeader), Strings.resourceCulture);

    public static string Settings_Bike_SplitMarkerSubHeader => Strings.ResourceManager.GetString(nameof (Settings_Bike_SplitMarkerSubHeader), Strings.resourceCulture);

    public static string Settings_Bike_SubHeading1 => Strings.ResourceManager.GetString(nameof (Settings_Bike_SubHeading1), Strings.resourceCulture);

    public static string Settings_Bike_SubHeading2 => Strings.ResourceManager.GetString(nameof (Settings_Bike_SubHeading2), Strings.resourceCulture);

    public static string Settings_Bike_SubHeading3 => Strings.ResourceManager.GetString(nameof (Settings_Bike_SubHeading3), Strings.resourceCulture);

    public static string Settings_Bike_Title => Strings.ResourceManager.GetString(nameof (Settings_Bike_Title), Strings.resourceCulture);

    public static string Settings_BikeRun_DataTitle => Strings.ResourceManager.GetString(nameof (Settings_BikeRun_DataTitle), Strings.resourceCulture);

    public static string Settings_Calendar_SubHeading1 => Strings.ResourceManager.GetString(nameof (Settings_Calendar_SubHeading1), Strings.resourceCulture);

    public static string Settings_Calendar_Title => Strings.ResourceManager.GetString(nameof (Settings_Calendar_Title), Strings.resourceCulture);

    public static string Settings_Calls_SubHeading1 => Strings.ResourceManager.GetString(nameof (Settings_Calls_SubHeading1), Strings.resourceCulture);

    public static string Settings_Calls_SubHeading2 => Strings.ResourceManager.GetString(nameof (Settings_Calls_SubHeading2), Strings.resourceCulture);

    public static string Settings_Calls_Title => Strings.ResourceManager.GetString(nameof (Settings_Calls_Title), Strings.resourceCulture);

    public static string Settings_Cortana_SubHeading1 => Strings.ResourceManager.GetString(nameof (Settings_Cortana_SubHeading1), Strings.resourceCulture);

    public static string Settings_Cortana_Title => Strings.ResourceManager.GetString(nameof (Settings_Cortana_Title), Strings.resourceCulture);

    public static string Settings_Customize => Strings.ResourceManager.GetString(nameof (Settings_Customize), Strings.resourceCulture);

    public static string Settings_Email_SubHeading1 => Strings.ResourceManager.GetString(nameof (Settings_Email_SubHeading1), Strings.resourceCulture);

    public static string Settings_Email_Title => Strings.ResourceManager.GetString(nameof (Settings_Email_Title), Strings.resourceCulture);

    public static string Settings_Facebook_SubHeading1 => Strings.ResourceManager.GetString(nameof (Settings_Facebook_SubHeading1), Strings.resourceCulture);

    public static string Settings_Facebook_Title => Strings.ResourceManager.GetString(nameof (Settings_Facebook_Title), Strings.resourceCulture);

    public static string Settings_FacebookMessenger_SubHeading1 => Strings.ResourceManager.GetString(nameof (Settings_FacebookMessenger_SubHeading1), Strings.resourceCulture);

    public static string Settings_FacebookMessenger_Title => Strings.ResourceManager.GetString(nameof (Settings_FacebookMessenger_Title), Strings.resourceCulture);

    public static string Settings_Metrics_SelectionError => Strings.ResourceManager.GetString(nameof (Settings_Metrics_SelectionError), Strings.resourceCulture);

    public static string Settings_NotificationCenter_SubHeading1 => Strings.ResourceManager.GetString(nameof (Settings_NotificationCenter_SubHeading1), Strings.resourceCulture);

    public static string Settings_NotificationCenter_Title => Strings.ResourceManager.GetString(nameof (Settings_NotificationCenter_Title), Strings.resourceCulture);

    public static string Settings_PrimaryData => Strings.ResourceManager.GetString(nameof (Settings_PrimaryData), Strings.resourceCulture);

    public static string Settings_Reply1 => Strings.ResourceManager.GetString(nameof (Settings_Reply1), Strings.resourceCulture);

    public static string Settings_Reply2 => Strings.ResourceManager.GetString(nameof (Settings_Reply2), Strings.resourceCulture);

    public static string Settings_Reply3 => Strings.ResourceManager.GetString(nameof (Settings_Reply3), Strings.resourceCulture);

    public static string Settings_Reply4 => Strings.ResourceManager.GetString(nameof (Settings_Reply4), Strings.resourceCulture);

    public static string Settings_Run_DataPoint0 => Strings.ResourceManager.GetString(nameof (Settings_Run_DataPoint0), Strings.resourceCulture);

    public static string Settings_Run_DataPoint1 => Strings.ResourceManager.GetString(nameof (Settings_Run_DataPoint1), Strings.resourceCulture);

    public static string Settings_Run_DataPoint2 => Strings.ResourceManager.GetString(nameof (Settings_Run_DataPoint2), Strings.resourceCulture);

    public static string Settings_Run_DataPoint3 => Strings.ResourceManager.GetString(nameof (Settings_Run_DataPoint3), Strings.resourceCulture);

    public static string Settings_Run_DataPoint4 => Strings.ResourceManager.GetString(nameof (Settings_Run_DataPoint4), Strings.resourceCulture);

    public static string Settings_Run_DataPoint5 => Strings.ResourceManager.GetString(nameof (Settings_Run_DataPoint5), Strings.resourceCulture);

    public static string Settings_Run_DataPoint6 => Strings.ResourceManager.GetString(nameof (Settings_Run_DataPoint6), Strings.resourceCulture);

    public static string Settings_Run_DataPoint7 => Strings.ResourceManager.GetString(nameof (Settings_Run_DataPoint7), Strings.resourceCulture);

    public static string Settings_Run_DataPoint8 => Strings.ResourceManager.GetString(nameof (Settings_Run_DataPoint8), Strings.resourceCulture);

    public static string Settings_Run_DataPoint9 => Strings.ResourceManager.GetString(nameof (Settings_Run_DataPoint9), Strings.resourceCulture);

    public static string Settings_Run_SubHeading1 => Strings.ResourceManager.GetString(nameof (Settings_Run_SubHeading1), Strings.resourceCulture);

    public static string Settings_Run_SubHeading2 => Strings.ResourceManager.GetString(nameof (Settings_Run_SubHeading2), Strings.resourceCulture);

    public static string Settings_Run_SubHeading3 => Strings.ResourceManager.GetString(nameof (Settings_Run_SubHeading3), Strings.resourceCulture);

    public static string Settings_Run_Title => Strings.ResourceManager.GetString(nameof (Settings_Run_Title), Strings.resourceCulture);

    public static string Settings_SecondaryData => Strings.ResourceManager.GetString(nameof (Settings_SecondaryData), Strings.resourceCulture);

    public static string Settings_SMS_SubHeading1 => Strings.ResourceManager.GetString(nameof (Settings_SMS_SubHeading1), Strings.resourceCulture);

    public static string Settings_SMS_SubHeading2 => Strings.ResourceManager.GetString(nameof (Settings_SMS_SubHeading2), Strings.resourceCulture);

    public static string Settings_SMS_Title => Strings.ResourceManager.GetString(nameof (Settings_SMS_Title), Strings.resourceCulture);

    public static string Settings_SplitMarkers => Strings.ResourceManager.GetString(nameof (Settings_SplitMarkers), Strings.resourceCulture);

    public static string Settings_SplitMarkers_Imperial => Strings.ResourceManager.GetString(nameof (Settings_SplitMarkers_Imperial), Strings.resourceCulture);

    public static string Settings_SplitMarkers_Metric => Strings.ResourceManager.GetString(nameof (Settings_SplitMarkers_Metric), Strings.resourceCulture);

    public static string Settings_Starbucks_Title => Strings.ResourceManager.GetString(nameof (Settings_Starbucks_Title), Strings.resourceCulture);

    public static string Settings_TertiaryData => Strings.ResourceManager.GetString(nameof (Settings_TertiaryData), Strings.resourceCulture);

    public static string Settings_Twitter_SubHeading1 => Strings.ResourceManager.GetString(nameof (Settings_Twitter_SubHeading1), Strings.resourceCulture);

    public static string Settings_Twitter_Title => Strings.ResourceManager.GetString(nameof (Settings_Twitter_Title), Strings.resourceCulture);

    public static string Status_AppUpdate_Available => Strings.ResourceManager.GetString(nameof (Status_AppUpdate_Available), Strings.resourceCulture);

    public static string Status_AppUpdate_Checking => Strings.ResourceManager.GetString(nameof (Status_AppUpdate_Checking), Strings.resourceCulture);

    public static string Status_AppUpdate_Unknown => Strings.ResourceManager.GetString(nameof (Status_AppUpdate_Unknown), Strings.resourceCulture);

    public static string Status_AppUpdate_UpToDate => Strings.ResourceManager.GetString(nameof (Status_AppUpdate_UpToDate), Strings.resourceCulture);

    public static string Status_FirmwareUpdate_BootingToUpdate => Strings.ResourceManager.GetString(nameof (Status_FirmwareUpdate_BootingToUpdate), Strings.resourceCulture);

    public static string Status_FirmwareUpdate_Downloading => Strings.ResourceManager.GetString(nameof (Status_FirmwareUpdate_Downloading), Strings.resourceCulture);

    public static string Status_FirmwareUpdate_SyncingLog => Strings.ResourceManager.GetString(nameof (Status_FirmwareUpdate_SyncingLog), Strings.resourceCulture);

    public static string Status_FirmwareUpdate_Uploading => Strings.ResourceManager.GetString(nameof (Status_FirmwareUpdate_Uploading), Strings.resourceCulture);

    public static string Status_FirmwareUpdate_WaitingForReconnect => Strings.ResourceManager.GetString(nameof (Status_FirmwareUpdate_WaitingForReconnect), Strings.resourceCulture);

    public static string Status_FW_Available => Strings.ResourceManager.GetString(nameof (Status_FW_Available), Strings.resourceCulture);

    public static string Status_FW_Checking => Strings.ResourceManager.GetString(nameof (Status_FW_Checking), Strings.resourceCulture);

    public static string Status_FW_Downloading => Strings.ResourceManager.GetString(nameof (Status_FW_Downloading), Strings.resourceCulture);

    public static string Status_FW_ReadyToUpdate => Strings.ResourceManager.GetString(nameof (Status_FW_ReadyToUpdate), Strings.resourceCulture);

    public static string Status_FW_Unknown => Strings.ResourceManager.GetString(nameof (Status_FW_Unknown), Strings.resourceCulture);

    public static string Status_FW_UpToDate => Strings.ResourceManager.GetString(nameof (Status_FW_UpToDate), Strings.resourceCulture);

    public static string Status_Syncing => Strings.ResourceManager.GetString(nameof (Status_Syncing), Strings.resourceCulture);

    public static string Status_Updating => Strings.ResourceManager.GetString(nameof (Status_Updating), Strings.resourceCulture);

    public static string SubHead_Page_Disconnected_Connecting => Strings.ResourceManager.GetString(nameof (SubHead_Page_Disconnected_Connecting), Strings.resourceCulture);

    public static string SubHead_Page_Disconnected_Disconnected => Strings.ResourceManager.GetString(nameof (SubHead_Page_Disconnected_Disconnected), Strings.resourceCulture);

    public static string SubHead_Page_LoginLogoutStatusMessage_Login => Strings.ResourceManager.GetString(nameof (SubHead_Page_LoginLogoutStatusMessage_Login), Strings.resourceCulture);

    public static string SubHead_Page_LoginLogoutStatusMessage_Logout => Strings.ResourceManager.GetString(nameof (SubHead_Page_LoginLogoutStatusMessage_Logout), Strings.resourceCulture);

    public static string SubHead_Page_Sync => Strings.ResourceManager.GetString(nameof (SubHead_Page_Sync), Strings.resourceCulture);

    public static string Text_AboutCopyrightNotice => Strings.ResourceManager.GetString(nameof (Text_AboutCopyrightNotice), Strings.resourceCulture);

    public static string Text_AboutLegalNotice => Strings.ResourceManager.GetString(nameof (Text_AboutLegalNotice), Strings.resourceCulture);

    public static string Text_Error_15Characters => Strings.ResourceManager.GetString(nameof (Text_Error_15Characters), Strings.resourceCulture);

    public static string Text_Error_25Characters => Strings.ResourceManager.GetString(nameof (Text_Error_25Characters), Strings.resourceCulture);

    public static string Text_TrayIconBalloonTip_CloseWindowWarning => Strings.ResourceManager.GetString(nameof (Text_TrayIconBalloonTip_CloseWindowWarning), Strings.resourceCulture);

    public static string Text_TrayIconBalloonTip_DeviceAbsent => Strings.ResourceManager.GetString(nameof (Text_TrayIconBalloonTip_DeviceAbsent), Strings.resourceCulture);

    public static string Text_TrayIconBalloonTip_DevicePresent => Strings.ResourceManager.GetString(nameof (Text_TrayIconBalloonTip_DevicePresent), Strings.resourceCulture);

    public static string Text_TrayIconBalloonTip_Error => Strings.ResourceManager.GetString(nameof (Text_TrayIconBalloonTip_Error), Strings.resourceCulture);

    public static string Text_TrayIconBalloonTip_FirmwateUpdateRequired => Strings.ResourceManager.GetString(nameof (Text_TrayIconBalloonTip_FirmwateUpdateRequired), Strings.resourceCulture);

    public static string Text_TrayIconBalloonTip_LoginToStart => Strings.ResourceManager.GetString(nameof (Text_TrayIconBalloonTip_LoginToStart), Strings.resourceCulture);

    public static string Text_TrayIconBalloonTip_MultipleDevices => Strings.ResourceManager.GetString(nameof (Text_TrayIconBalloonTip_MultipleDevices), Strings.resourceCulture);

    public static string Text_TrayIconBalloonTip_Syncing => Strings.ResourceManager.GetString(nameof (Text_TrayIconBalloonTip_Syncing), Strings.resourceCulture);

    public static string Text_TrayIconBalloonTip_UpdatingFirmware => Strings.ResourceManager.GetString(nameof (Text_TrayIconBalloonTip_UpdatingFirmware), Strings.resourceCulture);

    public static string Title_Button_Cancel => Strings.ResourceManager.GetString(nameof (Title_Button_Cancel), Strings.resourceCulture);

    public static string Title_Button_CheckMark => Strings.ResourceManager.GetString(nameof (Title_Button_CheckMark), Strings.resourceCulture);

    public static string Title_Button_Confirm => Strings.ResourceManager.GetString(nameof (Title_Button_Confirm), Strings.resourceCulture);

    public static string Title_Button_Download => Strings.ResourceManager.GetString(nameof (Title_Button_Download), Strings.resourceCulture);

    public static string Title_Button_Edit => Strings.ResourceManager.GetString(nameof (Title_Button_Edit), Strings.resourceCulture);

    public static string Title_Button_EditProfile => Strings.ResourceManager.GetString(nameof (Title_Button_EditProfile), Strings.resourceCulture);

    public static string Title_Button_ForgetDevice => Strings.ResourceManager.GetString(nameof (Title_Button_ForgetDevice), Strings.resourceCulture);

    public static string Title_Button_Logout => Strings.ResourceManager.GetString(nameof (Title_Button_Logout), Strings.resourceCulture);

    public static string Title_Button_MicrosoftAccount => Strings.ResourceManager.GetString(nameof (Title_Button_MicrosoftAccount), Strings.resourceCulture);

    public static string Title_Button_PairNow => Strings.ResourceManager.GetString(nameof (Title_Button_PairNow), Strings.resourceCulture);

    public static string Title_Button_Save => Strings.ResourceManager.GetString(nameof (Title_Button_Save), Strings.resourceCulture);

    public static string Title_Button_SaveProfile => Strings.ResourceManager.GetString(nameof (Title_Button_SaveProfile), Strings.resourceCulture);

    public static string Title_Button_X_Cancel => Strings.ResourceManager.GetString(nameof (Title_Button_X_Cancel), Strings.resourceCulture);

    public static string Title_Cancel => Strings.ResourceManager.GetString(nameof (Title_Cancel), Strings.resourceCulture);

    public static string Title_CloseAppDialog_Close => Strings.ResourceManager.GetString(nameof (Title_CloseAppDialog_Close), Strings.resourceCulture);

    public static string Title_Command_About => Strings.ResourceManager.GetString(nameof (Title_Command_About), Strings.resourceCulture);

    public static string Title_Command_Back => Strings.ResourceManager.GetString(nameof (Title_Command_Back), Strings.resourceCulture);

    public static string Title_Command_CancelSyncSensorLogs => Strings.ResourceManager.GetString(nameof (Title_Command_CancelSyncSensorLogs), Strings.resourceCulture);

    public static string Title_Command_CheckForAppUpdate => Strings.ResourceManager.GetString(nameof (Title_Command_CheckForAppUpdate), Strings.resourceCulture);

    public static string Title_Command_CheckForFirmwareUpdate => Strings.ResourceManager.GetString(nameof (Title_Command_CheckForFirmwareUpdate), Strings.resourceCulture);

    public static string Title_Command_Close => Strings.ResourceManager.GetString(nameof (Title_Command_Close), Strings.resourceCulture);

    public static string Title_Command_LaunchAppUpdate => Strings.ResourceManager.GetString(nameof (Title_Command_LaunchAppUpdate), Strings.resourceCulture);

    public static string Title_Command_Logout => Strings.ResourceManager.GetString(nameof (Title_Command_Logout), Strings.resourceCulture);

    public static string Title_Command_Open => Strings.ResourceManager.GetString(nameof (Title_Command_Open), Strings.resourceCulture);

    public static string Title_Command_Settings => Strings.ResourceManager.GetString(nameof (Title_Command_Settings), Strings.resourceCulture);

    public static string Title_Command_StopSyncing => Strings.ResourceManager.GetString(nameof (Title_Command_StopSyncing), Strings.resourceCulture);

    public static string Title_Command_SyncSensorLogs => Strings.ResourceManager.GetString(nameof (Title_Command_SyncSensorLogs), Strings.resourceCulture);

    public static string Title_Command_UpdateFirmware => Strings.ResourceManager.GetString(nameof (Title_Command_UpdateFirmware), Strings.resourceCulture);

    public static string Title_CurrentFWVersion => Strings.ResourceManager.GetString(nameof (Title_CurrentFWVersion), Strings.resourceCulture);

    public static string Title_DialogDefaultButton => Strings.ResourceManager.GetString(nameof (Title_DialogDefaultButton), Strings.resourceCulture);

    public static string Title_ErrorMessage => Strings.ResourceManager.GetString(nameof (Title_ErrorMessage), Strings.resourceCulture);

    public static string Title_Label_DeviceName => Strings.ResourceManager.GetString(nameof (Title_Label_DeviceName), Strings.resourceCulture);

    public static string Title_LastFWCheck => Strings.ResourceManager.GetString(nameof (Title_LastFWCheck), Strings.resourceCulture);

    public static string Title_LastSync => Strings.ResourceManager.GetString(nameof (Title_LastSync), Strings.resourceCulture);

    public static string Title_Login => Strings.ResourceManager.GetString(nameof (Title_Login), Strings.resourceCulture);

    public static string Title_LoginDialog_Login => Strings.ResourceManager.GetString(nameof (Title_LoginDialog_Login), Strings.resourceCulture);

    public static string Title_LoginDialog_Logout => Strings.ResourceManager.GetString(nameof (Title_LoginDialog_Logout), Strings.resourceCulture);

    public static string Title_MainWindow => Strings.ResourceManager.GetString(nameof (Title_MainWindow), Strings.resourceCulture);

    public static string Title_NavButton_About => Strings.ResourceManager.GetString(nameof (Title_NavButton_About), Strings.resourceCulture);

    public static string Title_NavButton_Dashboard => Strings.ResourceManager.GetString(nameof (Title_NavButton_Dashboard), Strings.resourceCulture);

    public static string Title_NavButton_ManageTiles => Strings.ResourceManager.GetString(nameof (Title_NavButton_ManageTiles), Strings.resourceCulture);

    public static string Title_NavButton_PersonalizeBand => Strings.ResourceManager.GetString(nameof (Title_NavButton_PersonalizeBand), Strings.resourceCulture);

    public static string Title_NavButton_Profile => Strings.ResourceManager.GetString(nameof (Title_NavButton_Profile), Strings.resourceCulture);

    public static string Title_NavButton_Settings => Strings.ResourceManager.GetString(nameof (Title_NavButton_Settings), Strings.resourceCulture);

    public static string Title_NewFWVersion => Strings.ResourceManager.GetString(nameof (Title_NewFWVersion), Strings.resourceCulture);

    public static string Title_Page_About1 => Strings.ResourceManager.GetString(nameof (Title_Page_About1), Strings.resourceCulture);

    public static string Title_Page_About2 => Strings.ResourceManager.GetString(nameof (Title_Page_About2), Strings.resourceCulture);

    public static string Title_Page_CantPair => Strings.ResourceManager.GetString(nameof (Title_Page_CantPair), Strings.resourceCulture);

    public static string Title_Page_CantPair_2 => Strings.ResourceManager.GetString(nameof (Title_Page_CantPair_2), Strings.resourceCulture);

    public static string Title_Page_Disconnected_1 => Strings.ResourceManager.GetString(nameof (Title_Page_Disconnected_1), Strings.resourceCulture);

    public static string Title_Page_Disconnected_2 => Strings.ResourceManager.GetString(nameof (Title_Page_Disconnected_2), Strings.resourceCulture);

    public static string Title_Page_FirmwareUpdateReady => Strings.ResourceManager.GetString(nameof (Title_Page_FirmwareUpdateReady), Strings.resourceCulture);

    public static string Title_Page_FirmwareUpdating => Strings.ResourceManager.GetString(nameof (Title_Page_FirmwareUpdating), Strings.resourceCulture);

    public static string Title_Page_ForgetDeviceConfirm => Strings.ResourceManager.GetString(nameof (Title_Page_ForgetDeviceConfirm), Strings.resourceCulture);

    public static string Title_Page_InstallSoftware => Strings.ResourceManager.GetString(nameof (Title_Page_InstallSoftware), Strings.resourceCulture);

    public static string Title_Page_LogIn => Strings.ResourceManager.GetString(nameof (Title_Page_LogIn), Strings.resourceCulture);

    public static string Title_Page_LoginLogoutBusy => Strings.ResourceManager.GetString(nameof (Title_Page_LoginLogoutBusy), Strings.resourceCulture);

    public static string Title_Page_LogoutConfirm => Strings.ResourceManager.GetString(nameof (Title_Page_LogoutConfirm), Strings.resourceCulture);

    public static string Title_Page_PairConfirm => Strings.ResourceManager.GetString(nameof (Title_Page_PairConfirm), Strings.resourceCulture);

    public static string Title_Page_PairDevice => Strings.ResourceManager.GetString(nameof (Title_Page_PairDevice), Strings.resourceCulture);

    public static string Title_Page_PersonalizeBand => Strings.ResourceManager.GetString(nameof (Title_Page_PersonalizeBand), Strings.resourceCulture);

    public static string Title_Page_Profile => Strings.ResourceManager.GetString(nameof (Title_Page_Profile), Strings.resourceCulture);

    public static string Title_Page_Settings => Strings.ResourceManager.GetString(nameof (Title_Page_Settings), Strings.resourceCulture);

    public static string Title_Page_TileManagement => Strings.ResourceManager.GetString(nameof (Title_Page_TileManagement), Strings.resourceCulture);

    public static string Title_Page_UpsellPage => Strings.ResourceManager.GetString(nameof (Title_Page_UpsellPage), Strings.resourceCulture);

    public static string Title_PleaseLogIn_1 => Strings.ResourceManager.GetString(nameof (Title_PleaseLogIn_1), Strings.resourceCulture);

    public static string Title_PleaseLogIn_2 => Strings.ResourceManager.GetString(nameof (Title_PleaseLogIn_2), Strings.resourceCulture);

    public static string Title_Setting_AutoStart => Strings.ResourceManager.GetString(nameof (Title_Setting_AutoStart), Strings.resourceCulture);

    public static string Title_Setting_AutoSync => Strings.ResourceManager.GetString(nameof (Title_Setting_AutoSync), Strings.resourceCulture);

    public static string Title_Tiles_ClickIcon => Strings.ResourceManager.GetString(nameof (Title_Tiles_ClickIcon), Strings.resourceCulture);

    public static string Title_Tiles_DragIcon => Strings.ResourceManager.GetString(nameof (Title_Tiles_DragIcon), Strings.resourceCulture);

    public static string Title_TrayIconToolTip_DeviceAbsent => Strings.ResourceManager.GetString(nameof (Title_TrayIconToolTip_DeviceAbsent), Strings.resourceCulture);

    public static string Title_TrayIconToolTip_DevicePresent => Strings.ResourceManager.GetString(nameof (Title_TrayIconToolTip_DevicePresent), Strings.resourceCulture);

    public static string Title_TrayIconToolTip_Error => Strings.ResourceManager.GetString(nameof (Title_TrayIconToolTip_Error), Strings.resourceCulture);

    public static string Title_TrayIconToolTip_FirmwareUpdateRequired => Strings.ResourceManager.GetString(nameof (Title_TrayIconToolTip_FirmwareUpdateRequired), Strings.resourceCulture);

    public static string Title_TrayIconToolTip_LoginToStart => Strings.ResourceManager.GetString(nameof (Title_TrayIconToolTip_LoginToStart), Strings.resourceCulture);

    public static string Title_TrayIconToolTip_MultipleDevices => Strings.ResourceManager.GetString(nameof (Title_TrayIconToolTip_MultipleDevices), Strings.resourceCulture);

    public static string Title_TrayIconToolTip_Syncing => Strings.ResourceManager.GetString(nameof (Title_TrayIconToolTip_Syncing), Strings.resourceCulture);

    public static string Title_TrayIconToolTip_UpdatingFirmware => Strings.ResourceManager.GetString(nameof (Title_TrayIconToolTip_UpdatingFirmware), Strings.resourceCulture);

    public static string ToolTip_CancelSync => Strings.ResourceManager.GetString(nameof (ToolTip_CancelSync), Strings.resourceCulture);

    public static string ToolTip_ChangeToImperialHeight => Strings.ResourceManager.GetString(nameof (ToolTip_ChangeToImperialHeight), Strings.resourceCulture);

    public static string ToolTip_ChangeToImperialWeight => Strings.ResourceManager.GetString(nameof (ToolTip_ChangeToImperialWeight), Strings.resourceCulture);

    public static string ToolTip_ChangeToMetricHeight => Strings.ResourceManager.GetString(nameof (ToolTip_ChangeToMetricHeight), Strings.resourceCulture);

    public static string ToolTip_ChangeToMetricWeight => Strings.ResourceManager.GetString(nameof (ToolTip_ChangeToMetricWeight), Strings.resourceCulture);

    public static string ToolTip_PersonalizeBand => Strings.ResourceManager.GetString(nameof (ToolTip_PersonalizeBand), Strings.resourceCulture);

    public static string ToolTip_Profile_DeviceNameRequiredField => Strings.ResourceManager.GetString(nameof (ToolTip_Profile_DeviceNameRequiredField), Strings.resourceCulture);

    public static string ToolTip_Profile_FirstNameRequiredField => Strings.ResourceManager.GetString(nameof (ToolTip_Profile_FirstNameRequiredField), Strings.resourceCulture);

    public static string UoM_Profile_Centimeters => Strings.ResourceManager.GetString(nameof (UoM_Profile_Centimeters), Strings.resourceCulture);

    public static string UoM_Profile_Feet => Strings.ResourceManager.GetString(nameof (UoM_Profile_Feet), Strings.resourceCulture);

    public static string UoM_Profile_Inches => Strings.ResourceManager.GetString(nameof (UoM_Profile_Inches), Strings.resourceCulture);

    public static string UoM_Profile_Kilograms => Strings.ResourceManager.GetString(nameof (UoM_Profile_Kilograms), Strings.resourceCulture);

    public static string UoM_Profile_Pounds => Strings.ResourceManager.GetString(nameof (UoM_Profile_Pounds), Strings.resourceCulture);
  }
}
