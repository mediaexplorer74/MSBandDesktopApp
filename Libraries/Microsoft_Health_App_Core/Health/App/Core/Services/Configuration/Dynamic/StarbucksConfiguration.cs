// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Configuration.Dynamic.StarbucksConfiguration
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Runtime.Serialization;

namespace Microsoft.Health.App.Core.Services.Configuration.Dynamic
{
  [DataContract]
  public class StarbucksConfiguration : IStarbucksConfiguration
  {
    public StarbucksConfiguration()
    {
      this.IsEnabled = true;
      this.DisplayUrl = (Uri) null;
      this.DisplayUrlString = string.Empty;
      this.CardFrontUrl = (Uri) null;
      this.CardBackUrl = (Uri) null;
    }

    [DataMember(Name = "enabled")]
    public bool IsEnabled { get; private set; }

    [DataMember(Name = "displayUrl")]
    public Uri DisplayUrl { get; private set; }

    [DataMember(Name = "displayUrlString")]
    public string DisplayUrlString { get; private set; }

    [DataMember(Name = "cardFrontUrl")]
    public Uri CardFrontUrl { get; private set; }

    [DataMember(Name = "cardBackUrl")]
    public Uri CardBackUrl { get; private set; }
  }
}
