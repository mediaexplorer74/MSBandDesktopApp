// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Messaging.PropertyChangedMessage`1
// Assembly: GalaSoft.MvvmLight, Version=5.0.2.32240, Culture=neutral, PublicKeyToken=e7570ab207bcb616
// MVID: 672AD33A-61F0-448A-AE1B-56983EAB4C33
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\GalaSoft_MvvmLight.dll

namespace GalaSoft.MvvmLight.Messaging
{
  public class PropertyChangedMessage<T> : PropertyChangedMessageBase
  {
    public PropertyChangedMessage(object sender, T oldValue, T newValue, string propertyName)
      : base(sender, propertyName)
    {
      this.OldValue = oldValue;
      this.NewValue = newValue;
    }

    public PropertyChangedMessage(T oldValue, T newValue, string propertyName)
      : base(propertyName)
    {
      this.OldValue = oldValue;
      this.NewValue = newValue;
    }

    public PropertyChangedMessage(
      object sender,
      object target,
      T oldValue,
      T newValue,
      string propertyName)
      : base(sender, target, propertyName)
    {
      this.OldValue = oldValue;
      this.NewValue = newValue;
    }

    public T NewValue { get; private set; }

    public T OldValue { get; private set; }
  }
}
