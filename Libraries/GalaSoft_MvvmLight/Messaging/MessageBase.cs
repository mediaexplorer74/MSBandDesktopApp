// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Messaging.MessageBase
// Assembly: GalaSoft.MvvmLight, Version=5.0.2.32240, Culture=neutral, PublicKeyToken=e7570ab207bcb616
// MVID: 672AD33A-61F0-448A-AE1B-56983EAB4C33
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\GalaSoft_MvvmLight.dll

namespace GalaSoft.MvvmLight.Messaging
{
  public class MessageBase
  {
    public MessageBase()
    {
    }

    public MessageBase(object sender) => this.Sender = sender;

    public MessageBase(object sender, object target)
      : this(sender)
    {
      this.Target = target;
    }

    public object Sender { get; protected set; }

    public object Target { get; protected set; }
  }
}
