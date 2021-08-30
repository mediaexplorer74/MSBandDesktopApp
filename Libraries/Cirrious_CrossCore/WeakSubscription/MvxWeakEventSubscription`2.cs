// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.WeakSubscription.MvxWeakEventSubscription`2
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using Cirrious.CrossCore.Exceptions;
using System;
using System.Reflection;

namespace Cirrious.CrossCore.WeakSubscription
{
  public class MvxWeakEventSubscription<TSource, TEventArgs> : IDisposable
    where TSource : class
    where TEventArgs : EventArgs
  {
    private readonly WeakReference _targetReference;
    private readonly WeakReference _sourceReference;
    private readonly MethodInfo _eventHandlerMethodInfo;
    private readonly EventInfo _sourceEventInfo;
    private readonly Delegate _ourEventHandler;
    private bool _subscribed;

    public MvxWeakEventSubscription(
      TSource source,
      string sourceEventName,
      EventHandler<TEventArgs> targetEventHandler)
      : this(source, ReflectionExtensions.GetEvent(typeof (TSource), sourceEventName), targetEventHandler)
    {
    }

    public MvxWeakEventSubscription(
      TSource source,
      EventInfo sourceEventInfo,
      EventHandler<TEventArgs> targetEventHandler)
    {
      if ((object) source == null)
        throw new ArgumentNullException();
      if ((object) sourceEventInfo == null)
        throw new ArgumentNullException(nameof (sourceEventInfo), "missing source event info in MvxWeakEventSubscription");
      this._eventHandlerMethodInfo = targetEventHandler.GetMethodInfo();
      this._targetReference = new WeakReference(targetEventHandler.Target);
      this._sourceReference = new WeakReference((object) source);
      this._sourceEventInfo = sourceEventInfo;
      this._ourEventHandler = this.CreateEventHandler();
      this.AddEventHandler();
    }

    protected virtual Delegate CreateEventHandler() => (Delegate) new EventHandler<TEventArgs>(this.OnSourceEvent);

    protected void OnSourceEvent(object sender, TEventArgs e)
    {
      object target = this._targetReference.Target;
      if (target != null)
        this._eventHandlerMethodInfo.Invoke(target, new object[2]
        {
          sender,
          (object) e
        });
      else
        this.RemoveEventHandler();
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      this.RemoveEventHandler();
    }

    private void RemoveEventHandler()
    {
      if (!this._subscribed)
        return;
      TSource target = (TSource) this._sourceReference.Target;
      if ((object) target == null)
        return;
      ReflectionExtensions.GetRemoveMethod(this._sourceEventInfo).Invoke((object) target, new object[1]
      {
        (object) this._ourEventHandler
      });
      this._subscribed = false;
    }

    private void AddEventHandler()
    {
      if (this._subscribed)
        throw new MvxException("Should not call _subscribed twice");
      TSource target = (TSource) this._sourceReference.Target;
      if ((object) target == null)
        return;
      ReflectionExtensions.GetAddMethod(this._sourceEventInfo).Invoke((object) target, new object[1]
      {
        (object) this._ourEventHandler
      });
      this._subscribed = true;
    }
  }
}
