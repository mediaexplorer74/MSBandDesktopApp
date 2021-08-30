// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.FacebookPermissions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Microsoft.Health.App.Core.Models
{
  [DataContract]
  public class FacebookPermissions
  {
    private const string FriendPermissionKey = "user_friends";
    private const string PermissionGranted = "granted";

    [DataMember(Name = "data")]
    public FacebookPermission[] Permissions { get; set; }

    public bool CanViewFriends => this.IsPermissionGranted("user_friends");

    private bool IsPermissionGranted(string permission) => (this.Permissions != null || this.Permissions.Length != 0) && ((IEnumerable<FacebookPermission>) this.Permissions).Any<FacebookPermission>((Func<FacebookPermission, bool>) (p => p.Permission == permission && p.Status == "granted"));
  }
}
