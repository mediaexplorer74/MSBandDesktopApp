// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.WeakSubscription.MvxWeakSubscriptionExtensionMethods
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using Cirrious.CrossCore.Core;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;

namespace Cirrious.CrossCore.WeakSubscription
{
  public static class MvxWeakSubscriptionExtensionMethods
  {
    public static MvxNotifyPropertyChangedEventSubscription WeakSubscribe(
      this INotifyPropertyChanged source,
      EventHandler<PropertyChangedEventArgs> eventHandler)
    {
      return new MvxNotifyPropertyChangedEventSubscription(source, eventHandler);
    }

    public static MvxNamedNotifyPropertyChangedEventSubscription<T> WeakSubscribe<T>(
      this INotifyPropertyChanged source,
      Expression<Func<T>> property,
      EventHandler<PropertyChangedEventArgs> eventHandler)
    {
      return new MvxNamedNotifyPropertyChangedEventSubscription<T>(source, property, eventHandler);
    }

    public static MvxNamedNotifyPropertyChangedEventSubscription<T> WeakSubscribe<T>(
      this INotifyPropertyChanged source,
      string property,
      EventHandler<PropertyChangedEventArgs> eventHandler)
    {
      return new MvxNamedNotifyPropertyChangedEventSubscription<T>(source, property, eventHandler);
    }

    public static MvxNotifyCollectionChangedEventSubscription WeakSubscribe(
      this INotifyCollectionChanged source,
      EventHandler<NotifyCollectionChangedEventArgs> eventHandler)
    {
      return new MvxNotifyCollectionChangedEventSubscription(source, eventHandler);
    }

    public static MvxGeneralEventSubscription WeakSubscribe(
      this EventInfo eventInfo,
      object source,
      EventHandler<EventArgs> eventHandler)
    {
      return new MvxGeneralEventSubscription(source, eventInfo, eventHandler);
    }

    public static MvxValueEventSubscription<T> WeakSubscribe<T>(
      this EventInfo eventInfo,
      object source,
      EventHandler<MvxValueEventArgs<T>> eventHandler)
    {
      return new MvxValueEventSubscription<T>(source, eventInfo, eventHandler);
    }

    public static MvxCanExecuteChangedEventSubscription WeakSubscribe(
      this ICommand source,
      EventHandler<EventArgs> eventHandler)
    {
      return new MvxCanExecuteChangedEventSubscription(source, eventHandler);
    }
  }
}
