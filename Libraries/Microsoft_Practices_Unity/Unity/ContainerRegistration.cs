// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.ContainerRegistration
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using System;
using System.Reflection;

namespace Microsoft.Practices.Unity
{
  public class ContainerRegistration
  {
    private readonly NamedTypeBuildKey buildKey;

    internal ContainerRegistration(Type registeredType, string name, IPolicyList policies)
    {
      this.buildKey = new NamedTypeBuildKey(registeredType, name);
      this.MappedToType = this.GetMappedType(policies);
      this.LifetimeManagerType = this.GetLifetimeManagerType(policies);
      this.LifetimeManager = this.GetLifetimeManager(policies);
    }

    public Type RegisteredType => this.buildKey.Type;

    public Type MappedToType { get; private set; }

    public string Name => this.buildKey.Name;

    public Type LifetimeManagerType { get; private set; }

    public LifetimeManager LifetimeManager { get; private set; }

    private Type GetMappedType(IPolicyList policies)
    {
      IBuildKeyMappingPolicy keyMappingPolicy = policies.Get<IBuildKeyMappingPolicy>((object) this.buildKey);
      return keyMappingPolicy != null ? keyMappingPolicy.Map(this.buildKey, (IBuilderContext) null).Type : this.buildKey.Type;
    }

    private Type GetLifetimeManagerType(IPolicyList policies)
    {
      NamedTypeBuildKey namedTypeBuildKey1 = new NamedTypeBuildKey(this.MappedToType, this.Name);
      ILifetimePolicy lifetimePolicy = policies.Get<ILifetimePolicy>((object) namedTypeBuildKey1);
      if (lifetimePolicy != null)
        return lifetimePolicy.GetType();
      if (this.MappedToType.GetTypeInfo().IsGenericType)
      {
        NamedTypeBuildKey namedTypeBuildKey2 = new NamedTypeBuildKey(this.MappedToType.GetGenericTypeDefinition(), this.Name);
        ILifetimeFactoryPolicy lifetimeFactoryPolicy = policies.Get<ILifetimeFactoryPolicy>((object) namedTypeBuildKey2);
        if (lifetimeFactoryPolicy != null)
          return lifetimeFactoryPolicy.LifetimeType;
      }
      return typeof (TransientLifetimeManager);
    }

    private LifetimeManager GetLifetimeManager(IPolicyList policies)
    {
      NamedTypeBuildKey namedTypeBuildKey = new NamedTypeBuildKey(this.MappedToType, this.Name);
      return (LifetimeManager) policies.Get<ILifetimePolicy>((object) namedTypeBuildKey);
    }
  }
}
