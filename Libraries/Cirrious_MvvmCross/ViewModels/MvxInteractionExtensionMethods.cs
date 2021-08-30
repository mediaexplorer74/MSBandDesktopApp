// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxInteractionExtensionMethods
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.WeakSubscription;
using System;

namespace Cirrious.MvvmCross.ViewModels
{
  public static class MvxInteractionExtensionMethods
  {
    public static IDisposable WeakSubscribe(
      this IMvxInteraction interaction,
      EventHandler<EventArgs> action)
    {
      return (IDisposable) ReflectionExtensions.GetEvent(interaction.GetType(), "Requested").WeakSubscribe((object) interaction, action);
    }

    public static MvxValueEventSubscription<T> WeakSubscribe<T>(
      this IMvxInteraction<T> interaction,
      EventHandler<MvxValueEventArgs<T>> action)
    {
      return ReflectionExtensions.GetEvent(interaction.GetType(), "Requested").WeakSubscribe<T>((object) interaction, action);
    }

    public static MvxValueEventSubscription<T> WeakSubscribe<T>(
      this IMvxInteraction<T> interaction,
      Action<T> action)
    {
      EventHandler<MvxValueEventArgs<T>> action1 = (EventHandler<MvxValueEventArgs<T>>) ((sender, args) => action(args.Value));
      return interaction.WeakSubscribe<T>(action1);
    }
  }
}
