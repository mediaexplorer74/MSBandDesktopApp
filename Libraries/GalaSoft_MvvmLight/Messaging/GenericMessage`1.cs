// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Messaging.GenericMessage`1
// Assembly: GalaSoft.MvvmLight, Version=5.0.2.32240, Culture=neutral, PublicKeyToken=e7570ab207bcb616
// MVID: 672AD33A-61F0-448A-AE1B-56983EAB4C33
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\GalaSoft_MvvmLight.dll

namespace GalaSoft.MvvmLight.Messaging
{
  public class GenericMessage<T> : MessageBase
  {
    public GenericMessage(T content) => this.Content = content;

    public GenericMessage(object sender, T content)
      : base(sender)
    {
      this.Content = content;
    }

    public GenericMessage(object sender, object target, T content)
      : base(sender, target)
    {
      this.Content = content;
    }

    public T Content { get; protected set; }
  }
}
