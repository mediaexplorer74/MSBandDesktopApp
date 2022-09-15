// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.WeakSubscription.MvxNotifyPropertyChangedEventSubscription
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;
using System.ComponentModel;
using System.Reflection;

namespace Cirrious.CrossCore.WeakSubscription
{
  public class MvxNotifyPropertyChangedEventSubscription : 
    MvxWeakEventSubscription<INotifyPropertyChanged, PropertyChangedEventArgs>
  {
    private static readonly EventInfo PropertyChangedEventInfo = ReflectionExtensions.GetEvent(typeof (INotifyPropertyChanged), "PropertyChanged");

    public static void LinkerPleaseInclude(INotifyPropertyChanged iNotifyPropertyChanged) => iNotifyPropertyChanged.PropertyChanged += (PropertyChangedEventHandler) ((sender, e) => { });

    public MvxNotifyPropertyChangedEventSubscription(
      INotifyPropertyChanged source,
      EventHandler<PropertyChangedEventArgs> targetEventHandler)
      : base(source, MvxNotifyPropertyChangedEventSubscription.PropertyChangedEventInfo, targetEventHandler)
    {
    }

    protected override Delegate CreateEventHandler() => (Delegate) new PropertyChangedEventHandler(((MvxWeakEventSubscription<INotifyPropertyChanged, PropertyChangedEventArgs>) this).OnSourceEvent);
  }
}
