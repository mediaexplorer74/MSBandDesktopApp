// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Configuration.Dynamic.MapsConfiguration
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Runtime.Serialization;

namespace Microsoft.Health.App.Core.Services.Configuration.Dynamic
{
  [DataContract]
  public class MapsConfiguration : IMapsConfiguration
  {
    public MapsConfiguration()
    {
      this.ServiceUrl = (Uri) null;
      this.Token = (string) null;
      this.RoadUriFormat = (string) null;
    }

    [DataMember(Name = "serviceUrl")]
    public Uri ServiceUrl { get; private set; }

    [DataMember(Name = "token")]
    public string Token { get; private set; }

    [DataMember(Name = "roadUriFormat")]
    public string RoadUriFormat { get; private set; }
  }
}
