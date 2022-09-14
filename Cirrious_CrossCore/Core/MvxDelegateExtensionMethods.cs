// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Core.MvxDelegateExtensionMethods
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;

namespace Cirrious.CrossCore.Core
{
  public static class MvxDelegateExtensionMethods
  {
    public static void Raise(this EventHandler eventHandler, object sender)
    {
      if (eventHandler == null)
        return;
      eventHandler(sender, EventArgs.Empty);
    }

    public static void Raise<T>(
      this EventHandler<MvxValueEventArgs<T>> eventHandler,
      object sender,
      T value)
    {
      if (eventHandler == null)
        return;
      eventHandler(sender, new MvxValueEventArgs<T>(value));
    }
  }
}
