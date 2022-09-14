// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.Globals
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace DesktopSyncApp
{
  internal class Globals
  {
    public static readonly string ApplicationName;
    public static readonly string ApplicationFilePath;
    public static readonly string ApplicationFileName;
    public static Version applicationVersion;
    public const string HostOS = "Windows Desktop";
    public static readonly Version HostOSVersion = Environment.OSVersion.Version;
    public static readonly Encoding UTF8Encoding;
    public const string RegistrySoftwareRootPath = "Software\\Microsoft";
    public static readonly string RegistrySoftwareAppRootPath;
    public const string RegistryWindowsRunPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
    public const string RegistryWebBrowserControlCompatibilityPath = "Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION";
    public const uint RegistryWebBrowserControlCompatibilityIE8DWORDValue = 8000;
    public static readonly DateTime UnixEpoc = new DateTime(1970, 1, 1);
    public static readonly EventArgs DefaultEventArgs = new EventArgs();
    public static readonly string MonthNameJanuary = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[0];
    public static readonly string MonthNameFebruary = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[1];
    public static readonly string MonthNameMarch = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[2];
    public static readonly string MonthNameApril = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[3];
    public static readonly string MonthNameMay = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[4];
    public static readonly string MonthNameJune = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[5];
    public static readonly string MonthNameJuly = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[6];
    public static readonly string MonthNameAugust = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[7];
    public static readonly string MonthNameSeptember = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[8];
    public static readonly string MonthNameOctober = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[9];
    public static readonly string MonthNameNovember = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[10];
    public static readonly string MonthNameDecember = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[11];
    public static readonly string DefaultUserAgent;
    public const string LiveIDUrlHost = "https://login.live.com";
    public const string AuthClientID = "000000004811DB42";
    public const string AuthStopUrl = "https://login.live.com/oauth20_desktop.srf";
    public static readonly string LoginUrlFormat = string.Format("{0}/oauth20_authorize.srf?client_id={1}&scope=service::{{0}}::MBI_SSL&response_type=token&redirect_uri={2}", (object) "https://login.live.com", (object) "000000004811DB42", (object) "https://login.live.com/oauth20_desktop.srf");
    public static readonly string TokenRefreshUrlFormat = string.Format("{0}/oauth20_token.srf?client_id={1}&scope=service::{{0}}::MBI_SSL&grant_type=refresh_token&refresh_token={{1}}", (object) "https://login.live.com", (object) "000000004811DB42");
    public static readonly string LogoutUrl = string.Format("{0}/oauth20_logout.srf?client_id={1}&redirect_uri={2}", (object) "https://login.live.com", (object) "000000004811DB42", (object) "https://login.live.com/oauth20_desktop.srf");
    public static readonly TimeSpan RefreshTokenTimeToLive = TimeSpan.FromDays(14.0);
    public static readonly TimeSpan KATTokenTimeToLive = TimeSpan.FromDays(1.0);
    public static readonly TimeSpan AppCheckUpdateSchedule = TimeSpan.FromHours(24.0);
    public static TimeSpan FWCheckUpdateSchedule = TimeSpan.FromHours(24.0);
    public static TimeSpan FWCheckUpdateMinimum = TimeSpan.FromHours(12.0);
    public const int MinProfileAge = 18;
    public const int MaxProfileAge = 109;
    public const int MaxUserNameLength = 25;
    public const int MaxDeviceNameLength = 15;
    public const int MaxUSPostalCodeLength = 14;
    public const int MaxQuickResponseLength = 160;
    public const string SingletonMutexName = "Local\\MicrosoftBandSyncSingleton";
    public const string IPCPipeNameFormat = "\\\\.\\Pipe\\MicrosoftBandSyncIPC_{0:0000000000}";
    public const string TermsOfUseUrl = "https://go.microsoft.com/fwlink/?LinkID=507589";
    public const string PrivacyPolicyUrl = "https://go.microsoft.com/fwlink/?LinkID=521839";
    public const string SupportUrl = "https://go.microsoft.com/fwlink/?LinkID=506763";
    public const string ThirdPartyNoticesUrl = "https://go.microsoft.com/fwlink/?LinkID=513024";
    public const string DownloadUlr = "https://www.microsoft.com/en-us/store/apps/microsoft-health/9wzdncrfjbcx";
    public const string StrappWorkouts = "2af008a7-cd03-a04d-bb33-be904e6a2924";
    public const string StrappRun = "65bd93db-4293-46af-9a28-bdd6513b4677";
    public const string StrappBike = "96430fcb-0060-41cb-9de2-e00cac97f85d";
    public const string StrappSleep = "23e7bc94-f90d-44e0-843f-250910fdf74e";
    public const string StrappExercise = "a708f02a-03cd-4da0-bb33-be904e6a2924";
    public const string StrappAlarmStopwatch = "d36a92ea-3e85-4aed-a726-2898a6f2769b";
    public const string StrappUV = "59976cf5-15c8-4799-9e31-f34c765a6bd1";
    public const string StrappWeather = "69a39b4e-084b-4b53-9a1b-581826df9e36";
    public const string StrappWeatherLastUpdatedPage = "76150e97-94cd-4d55-847f-ba7e9fc408e6";
    public const string StrappFinance = "5992928a-bd79-4bb5-9678-f08246d03e68";
    public const string StrappFinanceLastUpdatedPage = "f375f75e-05b7-42e0-a5ff-9a5c409e9dd9";
    public const string StrappStarbucks = "64a29f65-70bb-4f32-99a2-0f250a05d427";
    public const string StrappStarbucksLastUpdatedPage = "5fe2048d-7336-684f-901d-dda85118c509";
    public const string StrappGuidedWorkouts = "0281c878-afa8-40ff-acfd-bca06c5c4922";

    static Globals()
    {
      Globals.UTF8Encoding = Encoding.UTF8;
      Globals.ApplicationName = Assembly.GetEntryAssembly().GetName().Name;
      Globals.ApplicationFilePath = Process.GetCurrentProcess().MainModule.FileName;
      Globals.ApplicationFileName = Path.GetFileName(Globals.ApplicationFilePath);
      Globals.applicationVersion = Assembly.GetEntryAssembly().GetName().Version;
      Globals.RegistrySoftwareAppRootPath = Path.Combine("Software\\Microsoft", Globals.ApplicationName);
      Version version = Globals.ApplicationVersion;
      if (version == new Version(1, 0, 0, 0))
        version = new Version(9, 9, 9, 9);
      Globals.DefaultUserAgent = string.Format("KSync/{0} (.NET CLR/{1}; {2}/{3}; {4})", (object) version, (object) Environment.Version, (object) "Windows Desktop", (object) Globals.HostOSVersion, (object) CultureInfo.CurrentCulture.Name);
    }

    public static Version ApplicationVersion => Globals.applicationVersion;
  }
}
