// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxPropertyChangedListener
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.WeakSubscription;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Cirrious.MvvmCross.ViewModels
{
  public class MvxPropertyChangedListener : IDisposable
  {
    private readonly Dictionary<string, List<PropertyChangedEventHandler>> _handlersLookup = new Dictionary<string, List<PropertyChangedEventHandler>>();
    private readonly INotifyPropertyChanged _notificationObject;
    private readonly MvxNotifyPropertyChangedEventSubscription _token;

    public MvxPropertyChangedListener(INotifyPropertyChanged notificationObject)
    {
      this._notificationObject = notificationObject != null ? notificationObject : throw new ArgumentNullException(nameof (notificationObject));
      this._token = this._notificationObject.WeakSubscribe(new EventHandler<PropertyChangedEventArgs>(this.NotificationObjectOnPropertyChanged));
    }

    public void NotificationObjectOnPropertyChanged(
      object sender,
      PropertyChangedEventArgs propertyChangedEventArgs)
    {
      string propertyName = propertyChangedEventArgs.PropertyName;
      List<PropertyChangedEventHandler> changedEventHandlerList = (List<PropertyChangedEventHandler>) null;
      if (string.IsNullOrEmpty(propertyName))
        changedEventHandlerList = this._handlersLookup.Values.SelectMany<List<PropertyChangedEventHandler>, PropertyChangedEventHandler>((Func<List<PropertyChangedEventHandler>, IEnumerable<PropertyChangedEventHandler>>) (x => (IEnumerable<PropertyChangedEventHandler>) x)).ToList<PropertyChangedEventHandler>();
      else if (!this._handlersLookup.TryGetValue(propertyName, out changedEventHandlerList))
        return;
      foreach (PropertyChangedEventHandler changedEventHandler in changedEventHandlerList)
        changedEventHandler(sender, propertyChangedEventArgs);
    }

    ~MvxPropertyChangedListener() => this.Dispose(false);

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool isDisposing)
    {
      if (!isDisposing)
        return;
      this._token.Dispose();
      this.Clear();
    }

    public void Clear() => this._handlersLookup.Clear();

    public MvxPropertyChangedListener Listen<TProperty>(
      Expression<Func<TProperty>> property,
      Action handler)
    {
      return this.Listen<TProperty>(property, (PropertyChangedEventHandler) ((s, e) => handler()));
    }

    public MvxPropertyChangedListener Listen<TProperty>(
      Expression<Func<TProperty>> propertyExpression,
      PropertyChangedEventHandler handler)
    {
      return this.Listen(this._notificationObject.GetPropertyNameFromExpression<TProperty>(propertyExpression), handler);
    }

    public MvxPropertyChangedListener Listen(
      string propertyName,
      Action handler)
    {
      return this.Listen(propertyName, (PropertyChangedEventHandler) ((s, e) => handler()));
    }

    public MvxPropertyChangedListener Listen(
      string propertyName,
      PropertyChangedEventHandler handler)
    {
      List<PropertyChangedEventHandler> changedEventHandlerList = (List<PropertyChangedEventHandler>) null;
      if (!this._handlersLookup.TryGetValue(propertyName, out changedEventHandlerList))
      {
        changedEventHandlerList = new List<PropertyChangedEventHandler>();
        this._handlersLookup.Add(propertyName, changedEventHandlerList);
      }
      changedEventHandlerList.Add(handler);
      return this;
    }
  }
}
