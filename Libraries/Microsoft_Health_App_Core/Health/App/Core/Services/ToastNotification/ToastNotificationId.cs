// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.ToastNotification.ToastNotificationId
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;

namespace Microsoft.Health.App.Core.Services.ToastNotification
{
  public class ToastNotificationId
  {
    public ToastNotificationId(string tag, int id)
    {
      Assert.ParamIsNotNullOrEmpty(tag, nameof (tag));
      this.Tag = tag;
      this.Id = id;
    }

    public string Tag { get; }

    public int Id { get; }
  }
}
