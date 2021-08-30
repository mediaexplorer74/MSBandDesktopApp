// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.NotificationCenterConfiguration
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Models
{
  public class NotificationCenterConfiguration
  {
    public NotificationCenterConfiguration()
    {
      this.EnabledApps = (IList<string>) new List<string>();
      this.DisabledApps = (IList<string>) new List<string>();
    }

    public IList<string> EnabledApps { get; }

    public IList<string> DisabledApps { get; }

    public bool IsAppEnabled(string appId) => !this.DisabledApps.Contains(appId);
  }
}
