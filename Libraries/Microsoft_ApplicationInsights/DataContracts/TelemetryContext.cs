// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.DataContracts.TelemetryContext
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.Extensibility.Implementation;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.ApplicationInsights.DataContracts
{
  public sealed class TelemetryContext : IJsonSerializable
  {
    private readonly IDictionary<string, string> properties;
    private readonly IDictionary<string, string> tags;
    private string instrumentationKey;
    private ComponentContext component;
    private DeviceContext device;
    private SessionContext session;
    private UserContext user;
    private OperationContext operation;
    private LocationContext location;
    private InternalContext internalContext;

    public TelemetryContext()
      : this((IDictionary<string, string>) new SnapshottingDictionary<string, string>(), (IDictionary<string, string>) new SnapshottingDictionary<string, string>())
    {
    }

    internal TelemetryContext(
      IDictionary<string, string> properties,
      IDictionary<string, string> tags)
    {
      this.properties = properties;
      this.tags = tags;
    }

    public string InstrumentationKey
    {
      get => this.instrumentationKey ?? string.Empty;
      set => Property.Set<string>(ref this.instrumentationKey, value);
    }

    public ComponentContext Component => LazyInitializer.EnsureInitialized<ComponentContext>(ref this.component, (Func<ComponentContext>) (() => new ComponentContext(this.Tags)));

    public DeviceContext Device => LazyInitializer.EnsureInitialized<DeviceContext>(ref this.device, (Func<DeviceContext>) (() => new DeviceContext(this.Tags)));

    public SessionContext Session => LazyInitializer.EnsureInitialized<SessionContext>(ref this.session, (Func<SessionContext>) (() => new SessionContext(this.Tags)));

    public UserContext User => LazyInitializer.EnsureInitialized<UserContext>(ref this.user, (Func<UserContext>) (() => new UserContext(this.Tags)));

    public OperationContext Operation => LazyInitializer.EnsureInitialized<OperationContext>(ref this.operation, (Func<OperationContext>) (() => new OperationContext(this.Tags)));

    public LocationContext Location => LazyInitializer.EnsureInitialized<LocationContext>(ref this.location, (Func<LocationContext>) (() => new LocationContext(this.Tags)));

    public IDictionary<string, string> Properties => this.properties;

    internal InternalContext Internal => LazyInitializer.EnsureInitialized<InternalContext>(ref this.internalContext, (Func<InternalContext>) (() => new InternalContext(this.Tags)));

    internal IDictionary<string, string> Tags => this.tags;

    void IJsonSerializable.Serialize(IJsonWriter writer)
    {
      writer.WriteProperty("iKey", this.InstrumentationKey);
      writer.WriteProperty("tags", this.Tags);
    }

    internal void Initialize(TelemetryContext source, string instrumentationKey)
    {
      Property.Initialize(ref this.instrumentationKey, instrumentationKey);
      if (source.tags == null || source.tags.Count <= 0)
        return;
      Utils.CopyDictionary<string>(source.tags, this.Tags);
    }
  }
}
