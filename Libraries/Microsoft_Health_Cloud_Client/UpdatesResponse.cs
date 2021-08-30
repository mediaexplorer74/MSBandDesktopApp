// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.UpdatesResponse
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class UpdatesResponse : DeserializableObjectBase
  {
    public UpdatesResponse() => this.Updates = (IList<Update>) new List<Update>();

    [DataMember(Name = "ver")]
    public string Version { get; set; }

    [DataMember(Name = "created")]
    public string Created { get; set; }

    [DataMember(Name = "updates")]
    public IList<Update> Updates { get; set; }
  }
}
