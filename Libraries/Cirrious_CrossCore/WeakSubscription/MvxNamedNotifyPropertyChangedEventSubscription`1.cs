// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.WeakSubscription.MvxNamedNotifyPropertyChangedEventSubscription`1
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using Cirrious.CrossCore.Core;
using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Cirrious.CrossCore.WeakSubscription
{
  public class MvxNamedNotifyPropertyChangedEventSubscription<T> : 
    MvxNotifyPropertyChangedEventSubscription
  {
    private readonly string _propertyName;

    public MvxNamedNotifyPropertyChangedEventSubscription(
      INotifyPropertyChanged source,
      Expression<Func<T>> property,
      EventHandler<PropertyChangedEventArgs> targetEventHandler)
      : this(source, source.GetPropertyNameFromExpression<T>(property), targetEventHandler)
    {
    }

    public MvxNamedNotifyPropertyChangedEventSubscription(
      INotifyPropertyChanged source,
      string propertyName,
      EventHandler<PropertyChangedEventArgs> targetEventHandler)
      : base(source, targetEventHandler)
    {
      this._propertyName = propertyName;
    }

    protected override Delegate CreateEventHandler() => (Delegate) ((sender, e) =>
    {
      if (!string.IsNullOrEmpty(e.PropertyName) && !(e.PropertyName == this._propertyName))
        return;
      this.OnSourceEvent(sender, e);
    });
  }
}
