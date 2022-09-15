// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.ToastNotification.ToastNotificationData
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;

namespace Microsoft.Health.App.Core.Services.ToastNotification
{
  public class ToastNotificationData
  {
    private string summary;

    public ToastNotificationData(string title, string message, ToastNotificationIcon iconType)
    {
      Assert.ParamIsNotNullOrEmpty(title, nameof (title));
      Assert.ParamIsNotNullOrEmpty(message, nameof (message));
      Assert.EnumIsDefined<ToastNotificationIcon>(iconType, nameof (iconType));
      this.Title = title;
      this.Message = message;
      this.IconType = iconType;
    }

    public ToastNotificationData(
      string title,
      string message,
      string summary,
      ToastNotificationIcon iconType)
      : this(title, message, iconType)
    {
      Assert.ParamIsNotNullOrEmpty(summary, nameof (summary));
      this.Summary = summary;
    }

    public string Title { get; set; }

    public string Message { get; set; }

    public string Summary
    {
      get => string.IsNullOrEmpty(this.summary) ? this.Message : this.summary;
      set => this.summary = value;
    }

    public ToastNotificationIcon IconType { get; set; }

    public IToastNotificationNavigation Navigation { get; set; }
  }
}
