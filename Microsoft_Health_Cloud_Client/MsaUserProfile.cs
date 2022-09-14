// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.MsaUserProfile
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using System;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class MsaUserProfile : DeserializableObjectBase
  {
    [DataMember]
    public string AuthenticationType { get; set; }

    [DataMember]
    public bool IsAuthenticated { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string PuidHex { get; set; }

    [DataMember]
    public string FirstName { get; set; }

    [DataMember]
    public string LastName { get; set; }

    [DataMember]
    public string EmailAddress { get; set; }

    [DataMember]
    public DateTime? BirthDate { get; set; }

    [DataMember]
    public string ZipCode { get; set; }

    [DataMember]
    public string Country { get; set; }

    [DataMember]
    public string Region { get; set; }

    [DataMember]
    public string Occupation { get; set; }

    [DataMember]
    public string LocaleId { get; set; }

    [DataMember]
    public int Gender { get; set; }

    [DataMember]
    public int Age { get; set; }

    protected override void Validate()
    {
      base.Validate();
      JsonAssert.PropertyIsNotNull("FirstName", (object) this.FirstName);
      JsonAssert.IsTrue(this.Gender == 0 || this.Gender == 1, "Gender value is not recognized.");
    }
  }
}
