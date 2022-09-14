// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.BandSettings
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class BandSettings
  {
    public BandSettings() => this.CommandSettings = (IDictionary<string, IDictionary<string, string>>) new Dictionary<string, IDictionary<string, string>>();

    [DataMember]
    public IDictionary<string, IDictionary<string, string>> CommandSettings { get; private set; }
  }
}
