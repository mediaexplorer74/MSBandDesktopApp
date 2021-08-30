// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Messaging.NotificationMessageAction`1
// Assembly: GalaSoft.MvvmLight, Version=5.0.2.32240, Culture=neutral, PublicKeyToken=e7570ab207bcb616
// MVID: 672AD33A-61F0-448A-AE1B-56983EAB4C33
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\GalaSoft_MvvmLight.dll

using System;

namespace GalaSoft.MvvmLight.Messaging
{
  public class NotificationMessageAction<TCallbackParameter> : NotificationMessageWithCallback
  {
    public NotificationMessageAction(string notification, Action<TCallbackParameter> callback)
      : base(notification, (Delegate) callback)
    {
    }

    public NotificationMessageAction(
      object sender,
      string notification,
      Action<TCallbackParameter> callback)
      : base(sender, notification, (Delegate) callback)
    {
    }

    public NotificationMessageAction(
      object sender,
      object target,
      string notification,
      Action<TCallbackParameter> callback)
      : base(sender, target, notification, (Delegate) callback)
    {
    }

    public void Execute(TCallbackParameter parameter) => this.Execute((object) parameter);
  }
}
