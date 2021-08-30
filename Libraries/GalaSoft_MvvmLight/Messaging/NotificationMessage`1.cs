// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Messaging.NotificationMessage`1
// Assembly: GalaSoft.MvvmLight, Version=5.0.2.32240, Culture=neutral, PublicKeyToken=e7570ab207bcb616
// MVID: 672AD33A-61F0-448A-AE1B-56983EAB4C33
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\GalaSoft_MvvmLight.dll

namespace GalaSoft.MvvmLight.Messaging
{
  public class NotificationMessage<T> : GenericMessage<T>
  {
    public NotificationMessage(T content, string notification)
      : base(content)
    {
      this.Notification = notification;
    }

    public NotificationMessage(object sender, T content, string notification)
      : base(sender, content)
    {
      this.Notification = notification;
    }

    public NotificationMessage(object sender, object target, T content, string notification)
      : base(sender, target, content)
    {
      this.Notification = notification;
    }

    public string Notification { get; private set; }
  }
}
