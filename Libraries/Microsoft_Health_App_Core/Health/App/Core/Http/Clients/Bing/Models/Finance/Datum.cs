// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Http.Clients.Bing.Models.Finance.Datum
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Runtime.Serialization;

namespace Microsoft.Health.App.Core.Http.Clients.Bing.Models.Finance
{
  [DataContract]
  public class Datum
  {
    [DataMember]
    public string OS001 { get; set; }

    [DataMember]
    public string OS010 { get; set; }

    [DataMember]
    public string OS01W { get; set; }

    [DataMember]
    public string AC040 { get; set; }

    [DataMember]
    public string AC036 { get; set; }

    [DataMember]
    public string FriendlyName { get; set; }

    [DataMember]
    public string FullInstrument { get; set; }
  }
}
