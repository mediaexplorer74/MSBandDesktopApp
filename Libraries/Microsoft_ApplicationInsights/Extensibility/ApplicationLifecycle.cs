// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.ApplicationLifecycle
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.Extensibility.Implementation.Platform;
using System;

namespace Microsoft.ApplicationInsights.Extensibility
{
  public class ApplicationLifecycle : IApplicationLifecycle
  {
    private static readonly object SyncRoot = new object();
    private static ApplicationLifecycle service;
    private IApplicationLifecycle provider;

    protected ApplicationLifecycle()
    {
    }

    public event Action<object, object> Started;

    public event EventHandler<ApplicationStoppingEventArgs> Stopping;

    public static IApplicationLifecycle Service
    {
      get
      {
        if (ApplicationLifecycle.service == null)
        {
          lock (ApplicationLifecycle.SyncRoot)
          {
            if (ApplicationLifecycle.service == null)
              ApplicationLifecycle.service = new ApplicationLifecycle()
              {
                Provider = PlatformApplicationLifecycle.Provider
              };
          }
        }
        return (IApplicationLifecycle) ApplicationLifecycle.service;
      }
      internal set => ApplicationLifecycle.service = (ApplicationLifecycle) value;
    }

    private IApplicationLifecycle Provider
    {
      set
      {
        if (this.provider != null)
        {
          this.provider.Started -= new Action<object, object>(this.OnStarted);
          this.provider.Stopping -= new EventHandler<ApplicationStoppingEventArgs>(this.OnStopping);
        }
        this.provider = value;
        if (this.provider == null)
          return;
        this.provider.Started += new Action<object, object>(this.OnStarted);
        this.provider.Stopping += new EventHandler<ApplicationStoppingEventArgs>(this.OnStopping);
      }
    }

    public static void SetProvider(IApplicationLifecycle provider) => ((ApplicationLifecycle) ApplicationLifecycle.Service).Provider = provider != null ? provider : throw new ArgumentNullException("applicationLifecycleProvider");

    private void OnStarted(object sender, object eventArgs)
    {
      Action<object, object> started = this.Started;
      if (started == null)
        return;
      started((object) this, eventArgs);
    }

    private void OnStopping(object sender, ApplicationStoppingEventArgs eventArgs)
    {
      EventHandler<ApplicationStoppingEventArgs> stopping = this.Stopping;
      if (stopping == null)
        return;
      stopping((object) this, eventArgs);
    }
  }
}
