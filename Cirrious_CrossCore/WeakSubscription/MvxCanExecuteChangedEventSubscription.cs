// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.WeakSubscription.MvxCanExecuteChangedEventSubscription
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;
using System.Reflection;
using System.Windows.Input;

namespace Cirrious.CrossCore.WeakSubscription
{
  public class MvxCanExecuteChangedEventSubscription : MvxWeakEventSubscription<ICommand, EventArgs>
  {
    private static readonly EventInfo CanExecuteChangedEventInfo = ReflectionExtensions.GetEvent(typeof (ICommand), "CanExecuteChanged");

    public MvxCanExecuteChangedEventSubscription(
      ICommand source,
      EventHandler<EventArgs> eventHandler)
      : base(source, MvxCanExecuteChangedEventSubscription.CanExecuteChangedEventInfo, eventHandler)
    {
    }

    protected override Delegate CreateEventHandler() => (Delegate) new EventHandler(((MvxWeakEventSubscription<ICommand, EventArgs>) this).OnSourceEvent);
  }
}
