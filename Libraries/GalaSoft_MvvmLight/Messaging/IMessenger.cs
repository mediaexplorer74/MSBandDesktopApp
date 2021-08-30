// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Messaging.IMessenger
// Assembly: GalaSoft.MvvmLight, Version=5.0.2.32240, Culture=neutral, PublicKeyToken=e7570ab207bcb616
// MVID: 672AD33A-61F0-448A-AE1B-56983EAB4C33
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\GalaSoft_MvvmLight.dll

using System;

namespace GalaSoft.MvvmLight.Messaging
{
  public interface IMessenger
  {
    void Register<TMessage>(object recipient, Action<TMessage> action);

    void Register<TMessage>(object recipient, object token, Action<TMessage> action);

    void Register<TMessage>(
      object recipient,
      object token,
      bool receiveDerivedMessagesToo,
      Action<TMessage> action);

    void Register<TMessage>(
      object recipient,
      bool receiveDerivedMessagesToo,
      Action<TMessage> action);

    void Send<TMessage>(TMessage message);

    void Send<TMessage, TTarget>(TMessage message);

    void Send<TMessage>(TMessage message, object token);

    void Unregister(object recipient);

    void Unregister<TMessage>(object recipient);

    void Unregister<TMessage>(object recipient, object token);

    void Unregister<TMessage>(object recipient, Action<TMessage> action);

    void Unregister<TMessage>(object recipient, object token, Action<TMessage> action);
  }
}
