// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Band.BandConnectionFactory
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Configuration;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Band
{
  public class BandConnectionFactory : IBandConnectionFactory
  {
    public const string MockCargoConnectionName = "MockCargoConnection";
    public const string CargoConnectionName = "CargoConnection";
    public static readonly ConfigurationValue<bool> IsMocked = ConfigurationValue.CreateBoolean("CargoConnectionFactory", nameof (IsMocked), (Func<bool>) (() => ServiceLocator.Current.GetInstance<IEnvironmentService>().IsEmulated));
    private readonly IConfigurationService configurationService;
    private readonly IServiceLocator serviceLocator;
    private readonly IApplicationLifecycleService applicationLifecycleService;
    private readonly List<IBandConnection> cargoConnections = new List<IBandConnection>();
    private readonly object connectionLock = new object();

    public BandConnectionFactory(
      IConfigurationService configurationService,
      IServiceLocator serviceLocator,
      IApplicationLifecycleService applicationLifecycleService)
    {
      Assert.ParamIsNotNull((object) configurationService, nameof (configurationService));
      Assert.ParamIsNotNull((object) serviceLocator, nameof (serviceLocator));
      Assert.ParamIsNotNull((object) applicationLifecycleService, nameof (applicationLifecycleService));
      this.configurationService = configurationService;
      this.serviceLocator = serviceLocator;
      this.applicationLifecycleService = applicationLifecycleService;
      this.applicationLifecycleService.RegisterSuspending(new Func<CancellationToken, Task>(this.OnSuspendingAsync));
      this.applicationLifecycleService.Resuming += new EventHandler<object>(this.OnResuming);
    }

    public Task<IBandConnection> CreateConnectionAsync(
      CancellationToken cancellationToken)
    {
      IBandConnection instance = this.serviceLocator.GetInstance<IBandConnection>();
      lock (this.connectionLock)
      {
        this.RemoveDisposedConnections();
        this.cargoConnections.Add(instance);
      }
      return Task.FromResult<IBandConnection>(instance);
    }

    private Task OnSuspendingAsync(CancellationToken cancellationToken)
    {
      IList<IBandConnection> source;
      lock (this.connectionLock)
      {
        this.RemoveDisposedConnections();
        source = (IList<IBandConnection>) new List<IBandConnection>((IEnumerable<IBandConnection>) this.cargoConnections);
      }
      return Task.WhenAll((IEnumerable<Task>) source.Select<IBandConnection, Task>((Func<IBandConnection, Task>) (p => p.NotifyOfSuspendAsync(cancellationToken))).ToList<Task>());
    }

    private void OnResuming(object sender, object e)
    {
      lock (this.connectionLock)
      {
        this.RemoveDisposedConnections();
        foreach (IBandConnection cargoConnection in this.cargoConnections)
          cargoConnection.NotifyOfResume();
      }
    }

    private void RemoveDisposedConnections() => this.cargoConnections.RemoveAll((Predicate<IBandConnection>) (p => p.IsDisposed));
  }
}
