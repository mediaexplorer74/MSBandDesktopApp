// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.Platform.PlatformApplicationLifecycle
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Platform
{
  internal class PlatformApplicationLifecycle : IApplicationLifecycle
  {
    private static IApplicationLifecycle provider;

    internal void Initialize() => AppDomain.CurrentDomain.DomainUnload += (EventHandler) ((sender, e) => this.OnStopping(new ApplicationStoppingEventArgs((Func<Func<Task>, Task>) (function => function()))));

    public event Action<object, object> Started;

    public event EventHandler<ApplicationStoppingEventArgs> Stopping;

    public static IApplicationLifecycle Provider
    {
      get => LazyInitializer.EnsureInitialized<IApplicationLifecycle>(ref PlatformApplicationLifecycle.provider, new Func<IApplicationLifecycle>(PlatformApplicationLifecycle.CreateDefaultProvider));
      set => PlatformApplicationLifecycle.provider = value;
    }

    private static IApplicationLifecycle CreateDefaultProvider()
    {
      PlatformApplicationLifecycle applicationLifecycle = new PlatformApplicationLifecycle();
      applicationLifecycle.Initialize();
      return (IApplicationLifecycle) applicationLifecycle;
    }

    private void OnStarted(object eventArgs)
    {
      Action<object, object> started = this.Started;
      if (started == null)
        return;
      started((object) this, eventArgs);
    }

    private void OnStopping(ApplicationStoppingEventArgs eventArgs)
    {
      EventHandler<ApplicationStoppingEventArgs> stopping = this.Stopping;
      if (stopping == null)
        return;
      stopping((object) this, eventArgs);
    }
  }
}
