// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.LocaleSettings
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Resources;
using System;
using System.Runtime.Serialization;

namespace Microsoft.Health.App.Core.Models
{
  [DataContract]
  public class LocaleSettings : ILocaleSettings
  {
    [DataMember(Name = "locale")]
    private string locale;
    [DataMember(Name = "configuration")]
    private LocaleConfiguration configuration;

    public Locale Locale
    {
      get
      {
        switch (this.locale)
        {
          case "AT":
            return Locale.Austria;
          case "AU":
            return Locale.Australia;
          case "BE":
            return Locale.Belgium;
          case "CA":
            return Locale.Canada;
          case "CH":
            return Locale.Switzerland;
          case "CN":
            return Locale.China;
          case "DE":
            return Locale.Germany;
          case "DK":
            return Locale.Denmark;
          case "ES":
            return Locale.Spain;
          case "FI":
            return Locale.Finland;
          case "FR":
            return Locale.France;
          case "GB":
            return Locale.GreatBritian;
          case "HK":
            return Locale.HongKong;
          case "IE":
            return Locale.Ireland;
          case "IT":
            return Locale.Italy;
          case "NL":
            return Locale.Netherlands;
          case "NO":
            return Locale.Norway;
          case "NZ":
            return Locale.NewZealand;
          case "PT":
            return Locale.Portugal;
          case "SE":
            return Locale.Sweden;
          case "SG":
            return Locale.Singapore;
          case "US":
            return Locale.UnitedStates;
          default:
            throw new FormatException(string.Format("Unsupported Locale '{0}'.", new object[1]
            {
              (object) this.locale
            }));
        }
      }
    }

    public string TwoLetterLocale => this.locale;

    public string Country
    {
      get
      {
        switch (this.Locale)
        {
          case Locale.UnitedStates:
            return AppResources.RegionUnitedStates;
          case Locale.GreatBritian:
            return AppResources.RegionGreatBritain;
          case Locale.Canada:
            return AppResources.RegionCanada;
          case Locale.France:
            return AppResources.RegionFrance;
          case Locale.Germany:
            return AppResources.RegionGermany;
          case Locale.Italy:
            return AppResources.RegionItaly;
          case Locale.Mexico:
            return AppResources.RegionMexico;
          case Locale.Spain:
            return AppResources.RegionSpain;
          case Locale.Australia:
            return AppResources.RegionAustralia;
          case Locale.NewZealand:
            return AppResources.RegionNewZealand;
          case Locale.Denmark:
            return AppResources.RegionDenmark;
          case Locale.Finland:
            return AppResources.RegionFinland;
          case Locale.Norway:
            return AppResources.RegionNorway;
          case Locale.Netherlands:
            return AppResources.RegionNetherlands;
          case Locale.Portugal:
            return AppResources.RegionPortugal;
          case Locale.Sweden:
            return AppResources.RegionSweden;
          case Locale.Poland:
            return AppResources.RegionPoland;
          case Locale.China:
            return AppResources.RegionChina;
          case Locale.Taiwan:
            return AppResources.RegionTaiwan;
          case Locale.Japan:
            return AppResources.RegionJapan;
          case Locale.Korea:
            return AppResources.RegionKorea;
          case Locale.Austria:
            return AppResources.RegionAustria;
          case Locale.Belgium:
            return AppResources.RegionBelgium;
          case Locale.HongKong:
            return AppResources.RegionHongKong;
          case Locale.Ireland:
            return AppResources.RegionIreland;
          case Locale.Singapore:
            return AppResources.RegionSingapore;
          case Locale.Switzerland:
            return AppResources.RegionSwitzerland;
          case Locale.SouthAfrica:
            return AppResources.RegionSouthAfrica;
          case Locale.SaudiArabia:
            return AppResources.RegionSaudiArabia;
          case Locale.UnitedArabEmirates:
            return AppResources.RegionUnitedArabEmirates;
          default:
            return string.Empty;
        }
      }
    }

    ILocaleConfiguration ILocaleSettings.Configuration => (ILocaleConfiguration) this.configuration;
  }
}
