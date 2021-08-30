// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Messaging.NotificationMessageWithCallback
// Assembly: GalaSoft.MvvmLight, Version=5.0.2.32240, Culture=neutral, PublicKeyToken=e7570ab207bcb616
// MVID: 672AD33A-61F0-448A-AE1B-56983EAB4C33
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\GalaSoft_MvvmLight.dll

using System;

namespace GalaSoft.MvvmLight.Messaging
{
  public class NotificationMessageWithCallback : NotificationMessage
  {
    private readonly Delegate _callback;

    public NotificationMessageWithCallback(string notification, Delegate callback)
      : base(notification)
    {
      NotificationMessageWithCallback.CheckCallback(callback);
      this._callback = callback;
    }

    public NotificationMessageWithCallback(object sender, string notification, Delegate callback)
      : base(sender, notification)
    {
      NotificationMessageWithCallback.CheckCallback(callback);
      this._callback = callback;
    }

    public NotificationMessageWithCallback(
      object sender,
      object target,
      string notification,
      Delegate callback)
      : base(sender, target, notification)
    {
      NotificationMessageWithCallback.CheckCallback(callback);
      this._callback = callback;
    }

    public virtual object Execute(params object[] arguments) => this._callback.DynamicInvoke(arguments);

    private static void CheckCallback(Delegate callback)
    {
      if ((object) callback == null)
        throw new ArgumentNullException(nameof (callback), "Callback may not be null");
    }
  }
}
