// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.ProfileFieldsViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Models.Validation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Configuration.Dynamic;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels
{
  public class ProfileFieldsViewModel : 
    ModelBase<ProfileFieldsViewModel>,
    IProfileFieldsViewModel,
    INotifyPropertyChanged
  {
    private const int MinNameLength = 1;
    private const int MaxNameLength = 25;
    private const int MinCm = 100;
    private const int MaxCm = 250;
    private const int MinFt = 3;
    private const int MaxFt = 8;
    private const int MinIn = 4;
    private const int MaxIn = 2;
    private const int MinKg = 35;
    private const int MaxKg = 250;
    private const int MinLb = 78;
    private const int MaxLb = 551;
    private const int MaxAge = 110;
    private const int MaxCharsForBand = 8;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\ProfileFieldsViewModel.cs");
    private readonly IHealthDiscoveryClient discoveryClient;
    private readonly IConfig config;
    private readonly IDynamicConfigurationService dynamicConfigurationService;
    private readonly IUserProfileService userProfileService;
    private Property<string> firstName = new Property<string>();
    private IList<GenderViewModel> genders;
    private Property<int?> heightCentimeters = new Property<int?>();
    private Property<int?> heightFeet = new Property<int?>();
    private Property<int?> heightInches = new Property<int?>();
    private bool isTelemetryEnabled = true;
    private Property<bool> isMarketingEnabled = new Property<bool>();
    private Property<bool> isSummaryEmailEnabled = new Property<bool>();
    private int maxYear;
    private int minYear;
    private IList<string> months;
    private IList<int> years;
    private Property<string> postalCode = new Property<string>();
    private Property<int> selectedGender = new Property<int>();
    private Property<int> selectedMonth = new Property<int>();
    private Property<int> selectedYear = new Property<int>();
    private ProfileStatus status;
    private Property<double?> weightPounds = new Property<double?>();
    private Property<double?> weightKilograms = new Property<double?>();
    private string preferredLocale;
    private string preferredRegion;

    public BandUserProfile Profile
    {
      get
      {
        Length length;
        if (this.IsDistanceMetric)
        {
          length = Length.FromCentimeters((double) this.HeightCentimeters.Value.Value);
        }
        else
        {
          double feet = (double) this.HeightFeet.Value.Value;
          int? nullable = this.HeightInches.Value;
          int num;
          if (!nullable.HasValue)
          {
            num = 0;
          }
          else
          {
            nullable = this.HeightInches.Value;
            num = nullable.Value;
          }
          double inches = (double) num;
          length = Length.FromFeetAndInches(feet, inches);
        }
        Weight weight = !this.IsMassMetric ? (this.WeightPounds.IsDirty ? Weight.FromPounds(this.WeightPounds.Value.Value) : Weight.FromPounds(this.WeightPounds.Original.Value)) : (this.WeightKilograms.IsDirty ? Weight.FromKilograms(this.WeightKilograms.Value.Value) : Weight.FromKilograms(this.WeightKilograms.Original.Value));
        if (this.userProfileService.CurrentUserProfile != null)
        {
          this.SetBandName();
          this.SetLocaleSettings();
        }
        else
        {
          this.BandName = string.Format(AppResources.OobeBandNameString, new object[1]
          {
            (object) ProfileFieldsViewModel.TrimString(this.FirstName.Value, 8)
          });
          this.LocaleSettings = LocaleUtilities.GetLocaleSettings(this.dynamicConfigurationService.Configuration.Oobe.Defaults);
        }
        return new BandUserProfile()
        {
          FirstName = this.FirstName.Value,
          HeightMM = (ushort) Math.Round(length.TotalMillimeters),
          Weight = (uint) Math.Round(weight.TotalGrams),
          Birthdate = this.GetBirthday(),
          Gender = this.SelectedGender.Value == 0 ? Gender.Female : Gender.Male,
          ZipCode = this.PostalCode.Value,
          BandName = this.BandName,
          LocaleSettings = this.LocaleSettings,
          TelemetryEnabled = this.IsTelemetryEnabled,
          MarketingEnabled = this.IsMarketingEnabled.Value,
          SummaryEmailEnabled = this.IsSummaryEmailEnabled.Value,
          IsOobeCompleted = this.config.OobeStatus == OobeStatus.Shown,
          PreferredLocale = this.preferredLocale,
          PreferredRegion = this.preferredRegion
        };
      }
    }

    public bool IsDistanceMetric => this.userProfileService.DistanceUnitType == DistanceUnitType.Metric;

    public bool IsMassMetric => this.userProfileService.MassUnitType == MassUnitType.Metric;

    public IList<string> Months
    {
      get => this.months;
      set => this.SetProperty<IList<string>>(ref this.months, value, nameof (Months));
    }

    public IList<int> Years
    {
      get => this.years;
      set => this.SetProperty<IList<int>>(ref this.years, value, nameof (Years));
    }

    public IList<GenderViewModel> Genders
    {
      get => this.genders;
      set => this.SetProperty<IList<GenderViewModel>>(ref this.genders, value, nameof (Genders));
    }

    public Property<string> FirstName => this.firstName;

    public Property<int> SelectedMonth => this.selectedMonth;

    public Property<int> SelectedYear => this.selectedYear;

    public Property<int> SelectedGender => this.selectedGender;

    public Property<double?> WeightPounds => this.weightPounds;

    public Property<double?> WeightKilograms => this.weightKilograms;

    public Property<int?> HeightFeet => this.heightFeet;

    public Property<int?> HeightInches => this.heightInches;

    public Property<int?> HeightCentimeters => this.heightCentimeters;

    public Property<string> PostalCode => this.postalCode;

    public string BandName { get; set; }

    public CargoLocaleSettings LocaleSettings { get; set; }

    public string StatusText => this.Status == ProfileStatus.None ? AppResources.ProfileWelcomeMessage : string.Empty;

    public bool StatusVisible => this.Status == ProfileStatus.None && this.ShowWelcomeMessage || this.Status == ProfileStatus.Failure;

    public Property<bool> IsMarketingEnabled => this.isMarketingEnabled;

    public Property<bool> IsSummaryEmailEnabled => this.isSummaryEmailEnabled;

    public bool IsSummaryEmailEnabledDefault => this.dynamicConfigurationService.Configuration.Features.SummaryEmail.IsEnabled;

    public bool IsTelemetryEnabled
    {
      get => this.isTelemetryEnabled;
      set => this.SetProperty<bool>(ref this.isTelemetryEnabled, value, nameof (IsTelemetryEnabled));
    }

    public bool ShowWelcomeMessage { get; set; }

    public ProfileStatus Status
    {
      get => this.status;
      set
      {
        this.SetProperty<ProfileStatus>(ref this.status, value, nameof (Status));
        this.RaisePropertyChanged("StatusText");
      }
    }

    public ProfileFieldsViewModel(
      IHealthDiscoveryClient discoveryClient,
      IUserProfileService userProfileService,
      IConfig config,
      IDynamicConfigurationService dynamicConfigurationService)
    {
      this.discoveryClient = discoveryClient;
      this.userProfileService = userProfileService;
      this.config = config;
      this.dynamicConfigurationService = dynamicConfigurationService;
    }

    public async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      try
      {
        this.LoadSelectionLists();
        BandUserProfile userProfile = await this.userProfileService.GetCloudUserProfileAsync(CancellationToken.None);
        if (userProfile == null || string.IsNullOrEmpty(userProfile.FirstName))
        {
          if (userProfile != null)
          {
            userProfile.LocaleSettings = LocaleUtilities.GetLocaleSettings(this.dynamicConfigurationService.Configuration.Oobe.Defaults);
            await this.userProfileService.SetUserProfileAsync(userProfile, CancellationToken.None);
          }
          MsaUserProfile userProfileAsync = await this.discoveryClient.GetMsaUserProfileAsync(CancellationToken.None);
          DateTime dateOfBirth = userProfileAsync.BirthDate.HasValue ? userProfileAsync.BirthDate.Value : new DateTime(this.maxYear, 1, 1);
          Gender gender = Gender.Female;
          if (userProfileAsync.Gender == 0)
            gender = Gender.Male;
          bool isSummaryEmailEnabled = false;
          if (this.IsSummaryEmailEnabledDefault)
            isSummaryEmailEnabled = this.IsMarketingEnabledDefault;
          this.HydrateObject(userProfileAsync.FirstName, (ushort) 0, 0U, dateOfBirth, userProfileAsync.ZipCode, gender, true, this.IsMarketingEnabledDefault, isSummaryEmailEnabled, (string) null, (string) null);
        }
        else
          this.HydrateObject(userProfile.FirstName, userProfile.HeightMM, userProfile.Weight, userProfile.Birthdate, userProfile.ZipCode, userProfile.Gender, userProfile.TelemetryEnabled, userProfile.MarketingEnabled, userProfile.SummaryEmailEnabled, userProfile.PreferredLocale, userProfile.PreferredRegion);
        this.CreateValidator();
        userProfile = (BandUserProfile) null;
      }
      catch (Exception ex)
      {
        ProfileFieldsViewModel.Logger.Error(ex, "Could not get profile.");
        throw;
      }
    }

    public void LoadSelectionLists()
    {
      this.Months = (IList<string>) new List<string>();
      foreach (string monthName in CultureInfo.CurrentCulture.DateTimeFormat.MonthNames)
      {
        if (!string.IsNullOrEmpty(monthName))
          this.Months.Add(monthName);
      }
      DateTimeOffset now = DateTimeOffset.Now;
      this.minYear = now.Year - 110;
      this.maxYear = now.Year;
      this.Years = (IList<int>) new List<int>();
      for (int maxYear = this.maxYear; maxYear >= this.minYear; --maxYear)
        this.Years.Add(maxYear);
      this.Genders = (IList<GenderViewModel>) new List<GenderViewModel>()
      {
        new GenderViewModel(Gender.Female),
        new GenderViewModel(Gender.Male)
      };
    }

    public void HydrateObject(
      string firstName,
      ushort heightInMM,
      uint weightInGrams,
      DateTime dateOfBirth,
      string zipCode,
      Gender gender,
      bool isTelemetryEnabled,
      bool isMarketingEnabled,
      bool isSummaryEmailEnabled,
      string preferredLocale,
      string preferredRegion)
    {
      this.FirstName.Value = firstName;
      this.SetBandName();
      this.SetLocaleSettings();
      this.SetHeight(heightInMM);
      this.SetWeight(weightInGrams);
      this.SetBirthDate(dateOfBirth);
      this.SetZipCode(zipCode);
      this.SelectedGender.Value = gender == Gender.Male ? 1 : 0;
      this.IsTelemetryEnabled = isTelemetryEnabled;
      this.IsMarketingEnabled.Value = isMarketingEnabled;
      this.IsSummaryEmailEnabled.Value = isSummaryEmailEnabled;
      this.preferredLocale = preferredLocale;
      this.preferredRegion = preferredRegion;
    }

    private void SetBandName()
    {
      if (this.userProfileService.CurrentUserProfile != null && !string.IsNullOrWhiteSpace(this.userProfileService.CurrentUserProfile.BandName))
        this.BandName = this.userProfileService.CurrentUserProfile.BandName;
      else
        this.BandName = string.Format(AppResources.OobeBandNameString, new object[1]
        {
          (object) ProfileFieldsViewModel.TrimString(this.FirstName.Value, 8)
        });
    }

    private void SetLocaleSettings()
    {
      this.LocaleSettings = this.userProfileService.CurrentUserProfile == null || this.userProfileService.CurrentUserProfile.LocaleSettings == null ? LocaleUtilities.GetLocaleSettings(this.dynamicConfigurationService.Configuration.Oobe.Defaults) : this.userProfileService.CurrentUserProfile.LocaleSettings;
      this.RaisePropertyChanged("IsDistanceMetric");
      this.RaisePropertyChanged("IsMassMetric");
    }

    private void SetHeight(ushort heightInMM)
    {
      Length length = Length.FromMillimeters((double) heightInMM);
      if (this.IsDistanceMetric)
      {
        if (heightInMM <= (ushort) 0)
          return;
        this.HeightCentimeters.Value = new int?((int) Math.Round(length.TotalCentimeters));
      }
      else
      {
        if (heightInMM <= (ushort) 0)
          return;
        this.HeightFeet.Value = new int?(length.Feet);
        this.HeightInches.Value = new int?(length.Inches);
      }
    }

    private void SetWeight(uint weightInGrams)
    {
      Weight weight = Weight.FromGrams((double) weightInGrams);
      if (this.IsMassMetric)
      {
        if (weightInGrams <= 0U)
          return;
        this.WeightKilograms.Value = new double?(weight.TotalKilograms);
      }
      else
      {
        if (weightInGrams <= 0U)
          return;
        this.WeightPounds.Value = new double?(weight.TotalPounds);
      }
    }

    private void SetBirthDate(DateTime dateOfBirth)
    {
      this.SelectedYear.Value = dateOfBirth.Year < this.minYear || dateOfBirth.Year > this.maxYear ? this.maxYear : dateOfBirth.Year;
      if (dateOfBirth.Month < 1 || dateOfBirth.Month > 12)
        this.SelectedMonth.Value = 0;
      else
        this.SelectedMonth.Value = dateOfBirth.Month - 1;
    }

    private void SetZipCode(string zipCode) => this.PostalCode.Value = zipCode ?? string.Empty;

    private static string TrimString(string str, int maxLength)
    {
      if (str == null || maxLength <= 0)
        return string.Empty;
      str = str.Trim();
      return str.Substring(0, Math.Min(maxLength, str.Length));
    }

    public void RefreshProfileStatus()
    {
      this.Status = !this.IsValid ? ProfileStatus.Errors : ProfileStatus.None;
      this.RaisePropertyChanged("HasErrors");
    }

    public bool HasErrors => this.Status == ProfileStatus.Errors;

    private bool IsInchesValid
    {
      get
      {
        if (!this.HeightInches.Value.HasValue)
          return true;
        int? nullable = this.HeightInches.Value;
        int num1 = 0;
        if ((nullable.GetValueOrDefault() < num1 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
        {
          nullable = this.HeightInches.Value;
          int num2 = 11;
          if ((nullable.GetValueOrDefault() > num2 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
          {
            nullable = this.HeightInches.Value;
            int num3 = 4;
            if ((nullable.GetValueOrDefault() < num3 ? (nullable.HasValue ? 1 : 0) : 0) != 0)
            {
              nullable = this.HeightFeet.Value;
              int num4 = 3;
              if ((nullable.GetValueOrDefault() <= num4 ? (nullable.HasValue ? 1 : 0) : 0) != 0)
                goto label_8;
            }
            nullable = this.HeightInches.Value;
            int num5 = 2;
            if ((nullable.GetValueOrDefault() > num5 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
              return true;
            nullable = this.HeightFeet.Value;
            int num6 = 8;
            return (nullable.GetValueOrDefault() >= num6 ? (nullable.HasValue ? 1 : 0) : 0) == 0;
          }
        }
label_8:
        return false;
      }
    }

    private bool IsFeetValid
    {
      get
      {
        if (this.HeightFeet.Value.HasValue)
        {
          int? nullable = this.HeightFeet.Value;
          int num1 = 3;
          if ((nullable.GetValueOrDefault() < num1 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
          {
            nullable = this.HeightFeet.Value;
            int num2 = 8;
            if ((nullable.GetValueOrDefault() > num2 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
            {
              nullable = this.HeightFeet.Value;
              int num3 = 3;
              if ((nullable.GetValueOrDefault() <= num3 ? (nullable.HasValue ? 1 : 0) : 0) != 0)
              {
                nullable = this.HeightInches.Value;
                int num4 = 4;
                if ((nullable.GetValueOrDefault() < num4 ? (nullable.HasValue ? 1 : 0) : 0) != 0)
                  goto label_8;
              }
              nullable = this.HeightFeet.Value;
              int num5 = 8;
              if ((nullable.GetValueOrDefault() >= num5 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
                return true;
              nullable = this.HeightInches.Value;
              int num6 = 2;
              return (nullable.GetValueOrDefault() > num6 ? (nullable.HasValue ? 1 : 0) : 0) == 0;
            }
          }
        }
label_8:
        return false;
      }
    }

    private bool IsCentimetersValid
    {
      get
      {
        if (this.HeightCentimeters.Value.HasValue)
        {
          int? nullable = this.HeightCentimeters.Value;
          int num1 = 100;
          if ((nullable.GetValueOrDefault() < num1 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
          {
            nullable = this.HeightCentimeters.Value;
            int num2 = 250;
            return (nullable.GetValueOrDefault() > num2 ? (nullable.HasValue ? 1 : 0) : 0) == 0;
          }
        }
        return false;
      }
    }

    private void ValidateName()
    {
      if (!string.IsNullOrWhiteSpace(this.FirstName.Value) && this.FirstName.Value.Length >= 1 && this.FirstName.Value.Length <= 25)
        return;
      this.FirstName.Errors.Add(string.Format(AppResources.ProfileFirstNameErrorMessage, new object[2]
      {
        (object) 1,
        (object) 25
      }));
    }

    private void ValidateBirthday()
    {
      int age = AgeUtilities.GetAge(this.GetBirthday());
      int minimumAge = this.dynamicConfigurationService.Configuration.Oobe.Defaults.MinimumAge;
      if (age >= minimumAge && age <= 110)
        return;
      string str = string.Format(AppResources.ProfileOutOfRangeAgeErrorMessage, new object[2]
      {
        (object) minimumAge,
        (object) 110
      });
      this.SelectedYear.Errors.Add(str);
      this.SelectedMonth.Errors.Add(str);
    }

    private void ValidateWeight()
    {
      if (!this.IsMassMetric)
      {
        if (this.WeightPounds.Value.HasValue)
        {
          double? nullable = this.WeightPounds.Value;
          double num1 = 78.0;
          if ((nullable.GetValueOrDefault() < num1 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
          {
            nullable = this.WeightPounds.Value;
            double num2 = 551.0;
            if ((nullable.GetValueOrDefault() > num2 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
              return;
          }
        }
        this.WeightPounds.Errors.Add(string.Format(AppResources.ProfileOutOfRangeErrorMessage, new object[3]
        {
          (object) 78,
          (object) 551,
          (object) string.Format(" {0}", new object[1]
          {
            (object) AppResources.PoundsPluralAbbreviation
          })
        }));
      }
      else
      {
        if (this.WeightKilograms.Value.HasValue)
        {
          double? nullable = this.WeightKilograms.Value;
          double num3 = 35.0;
          if ((nullable.GetValueOrDefault() < num3 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
          {
            nullable = this.WeightKilograms.Value;
            double num4 = 250.0;
            if ((nullable.GetValueOrDefault() > num4 ? (nullable.HasValue ? 1 : 0) : 0) == 0)
              return;
          }
        }
        this.WeightKilograms.Errors.Add(string.Format(AppResources.ProfileOutOfRangeErrorMessage, new object[3]
        {
          (object) 35,
          (object) 250,
          (object) string.Format(" {0}", new object[1]
          {
            (object) AppResources.KilogramsAbbreviation
          })
        }));
      }
    }

    private void ValidateHeight()
    {
      if (!this.IsDistanceMetric)
      {
        if (this.IsFeetValid && this.IsInchesValid)
          return;
        string str = string.Format(AppResources.ProfileImperialOutOfRangeErrorMessage, (object) 3, (object) 4, (object) 8, (object) 2);
        this.HeightFeet.Errors.Add(str);
        this.HeightInches.Errors.Add(str);
      }
      else
      {
        if (this.IsCentimetersValid)
          return;
        this.HeightCentimeters.Errors.Add(string.Format(AppResources.ProfileOutOfRangeErrorMessage, new object[3]
        {
          (object) 100,
          (object) 250,
          (object) AppResources.CentimetersAbbreviation
        }));
      }
    }

    private DateTime GetBirthday() => new DateTime(this.SelectedYear.Value, this.SelectedMonth.Value + 1, 1);

    private void CreateValidator() => this.Validator = (Action<IModel>) (e =>
    {
      this.ValidateName();
      this.ValidateBirthday();
      this.ValidateWeight();
      this.ValidateHeight();
    });

    protected override void OnValidationFinished() => this.RefreshProfileStatus();

    public static string CurrentRegion => RegionInfo.CurrentRegion.TwoLetterISORegionName;

    private bool IsMarketingEnabledDefault => this.dynamicConfigurationService.Configuration.Oobe.Defaults.MarketingOptIn;
  }
}
