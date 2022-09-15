// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Configuration.Dynamic.DynamicConfigurationFeatures
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Runtime.Serialization;

namespace Microsoft.Health.App.Core.Services.Configuration.Dynamic
{
  [DataContract]
  public sealed class DynamicConfigurationFeatures : IDynamicConfigurationFeatures
  {
    private readonly StarbucksConfiguration starbucks = new StarbucksConfiguration();
    private readonly FacebookConfiguration facebook = new FacebookConfiguration();
    private readonly FacebookMessengerConfiguration facebookMessenger = new FacebookMessengerConfiguration();
    private readonly TwitterConfiguration twitter = new TwitterConfiguration();
    private readonly FinanceConfiguration finance = new FinanceConfiguration();
    private readonly HikeConfiguration hike = new HikeConfiguration();
    private readonly SummaryEmailConfiguration summaryEmail = new SummaryEmailConfiguration();

    [DataMember(Name = "starbucks")]
    public StarbucksConfiguration Starbucks => this.starbucks;

    [DataMember(Name = "facebook")]
    public FacebookConfiguration Facebook => this.facebook;

    [DataMember(Name = "facebookMessenger")]
    public FacebookMessengerConfiguration FacebookMessenger => this.facebookMessenger;

    [DataMember(Name = "twitter")]
    public TwitterConfiguration Twitter => this.twitter;

    [DataMember(Name = "finance")]
    public FinanceConfiguration Finance => this.finance;

    [DataMember(Name = "hike")]
    public HikeConfiguration Hike => this.hike;

    [DataMember(Name = "summaryEmail")]
    public SummaryEmailConfiguration SummaryEmail => this.summaryEmail;

    IStarbucksConfiguration IDynamicConfigurationFeatures.Starbucks => (IStarbucksConfiguration) this.Starbucks;

    IFacebookConfiguration IDynamicConfigurationFeatures.Facebook => (IFacebookConfiguration) this.Facebook;

    IFacebookMessengerConfiguration IDynamicConfigurationFeatures.FacebookMessenger => (IFacebookMessengerConfiguration) this.FacebookMessenger;

    ITwitterConfiguration IDynamicConfigurationFeatures.Twitter => (ITwitterConfiguration) this.Twitter;

    IFinanceConfiguration IDynamicConfigurationFeatures.Finance => (IFinanceConfiguration) this.Finance;

    IHikeConfiguration IDynamicConfigurationFeatures.Hike => (IHikeConfiguration) this.Hike;

    ISummaryEmailConfiguration IDynamicConfigurationFeatures.SummaryEmail => (ISummaryEmailConfiguration) this.SummaryEmail;
  }
}
