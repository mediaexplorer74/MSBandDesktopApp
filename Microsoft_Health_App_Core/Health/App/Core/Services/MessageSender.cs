// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.MessageSender
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using GalaSoft.MvvmLight.Messaging;
using System;

namespace Microsoft.Health.App.Core.Services
{
  public class MessageSender : IMessageSender
  {
    public void Send<T>(T message) => Messenger.Default.Send<T>(message);

    public void Register<TMessage>(object recipient, Action<TMessage> action) => Messenger.Default.Register<TMessage>(recipient, action);

    public void Unregister(object recipient) => Messenger.Default.Unregister(recipient);

    public void Unregister<TMessage>(object recipient, Action<TMessage> action) => Messenger.Default.Unregister<TMessage>(recipient, action);
  }
}
