// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.FacebookUserProfileFragment
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Runtime.Serialization;

namespace Microsoft.Health.App.Core.Models
{
  [DataContract]
  public class FacebookUserProfileFragment
  {
    [DataMember(Name = "id")]
    public string UserId { get; set; }

    [DataMember(Name = "name")]
    public string FullName { get; set; }

    [DataMember(Name = "first_name")]
    public string FirstName { get; set; }

    [DataMember(Name = "last_name")]
    public string LastName { get; set; }
  }
}
