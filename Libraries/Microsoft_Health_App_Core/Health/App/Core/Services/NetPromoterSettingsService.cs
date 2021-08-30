// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.NetPromoterSettingsService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

namespace Microsoft.Health.App.Core.Services
{
  public class NetPromoterSettingsService : INetPromoterSettingsService
  {
    private const string SurveyNumber = "1";
    private const bool PromptUserForNpsSurvey = true;
    private const bool PromptAllUsersForNpsSurvey = false;
    private const string ResetTag = "1";
    private const int ApplicationLaunchThreshold = 10;
    private const int DaysOfUseThreshold = 10;

    public string NetPromoterSurveyNumber => "1";

    public bool NetPromoterPromptNewUsers => true;

    public bool NetPromoterPromptAllUsersForSurvey => false;

    public string NetPromoterResetTag => "1";

    public int ApplicationUseThreshold => 10;

    public int UniqueDaysOfUseThreshold => 10;
  }
}
