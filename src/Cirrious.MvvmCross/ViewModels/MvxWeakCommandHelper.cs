// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxWeakCommandHelper
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.ViewModels
{
  public class MvxWeakCommandHelper : IMvxCommandHelper
  {
    private readonly List<WeakReference> _eventHandlers = new List<WeakReference>();

    public event EventHandler CanExecuteChanged
    {
      add => this._eventHandlers.Add(new WeakReference((object) value));
      remove
      {
        foreach (WeakReference eventHandler in this._eventHandlers)
        {
          if (eventHandler.IsAlive && (Delegate) eventHandler.Target == (Delegate) value)
          {
            this._eventHandlers.Remove(eventHandler);
            break;
          }
        }
      }
    }

    private IEnumerable<EventHandler> SafeCopyEventHandlerList()
    {
      List<EventHandler> eventHandlerList = new List<EventHandler>();
      List<WeakReference> weakReferenceList = new List<WeakReference>();
      foreach (WeakReference eventHandler in this._eventHandlers)
      {
        if (!eventHandler.IsAlive)
        {
          weakReferenceList.Add(eventHandler);
        }
        else
        {
          EventHandler target = (EventHandler) eventHandler.Target;
          if (target != null)
            eventHandlerList.Add(target);
        }
      }
      foreach (WeakReference weakReference in weakReferenceList)
        this._eventHandlers.Remove(weakReference);
      return (IEnumerable<EventHandler>) eventHandlerList;
    }

    public void RaiseCanExecuteChanged(object sender)
    {
      foreach (EventHandler copyEventHandler in this.SafeCopyEventHandlerList())
        copyEventHandler(sender, EventArgs.Empty);
    }
  }
}
