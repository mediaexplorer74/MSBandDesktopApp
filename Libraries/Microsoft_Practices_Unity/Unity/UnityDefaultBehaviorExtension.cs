// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.UnityDefaultBehaviorExtension
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.Properties;
using System;
using System.Reflection;

namespace Microsoft.Practices.Unity
{
  public class UnityDefaultBehaviorExtension : UnityContainerExtension
  {
    protected override void Initialize()
    {
      this.Context.Registering += new EventHandler<RegisterEventArgs>(this.OnRegister);
      this.Context.RegisteringInstance += new EventHandler<RegisterInstanceEventArgs>(this.OnRegisterInstance);
      this.Container.RegisterInstance<IUnityContainer>(this.Container, (LifetimeManager) new UnityDefaultBehaviorExtension.ContainerLifetimeManager());
    }

    public override void Remove()
    {
      this.Context.Registering -= new EventHandler<RegisterEventArgs>(this.OnRegister);
      this.Context.RegisteringInstance -= new EventHandler<RegisterInstanceEventArgs>(this.OnRegisterInstance);
    }

    private void OnRegister(object sender, RegisterEventArgs e)
    {
      ExtensionContext context = this.Context;
      Type t = e.TypeFrom;
      if ((object) t == null)
        t = e.TypeTo;
      string name = e.Name;
      context.RegisterNamedType(t, name);
      if ((object) e.TypeFrom != null)
      {
        if (e.TypeFrom.GetTypeInfo().IsGenericTypeDefinition && e.TypeTo.GetTypeInfo().IsGenericTypeDefinition)
          this.Context.Policies.Set<IBuildKeyMappingPolicy>((IBuildKeyMappingPolicy) new GenericTypeBuildKeyMappingPolicy(new NamedTypeBuildKey(e.TypeTo, e.Name)), (object) new NamedTypeBuildKey(e.TypeFrom, e.Name));
        else
          this.Context.Policies.Set<IBuildKeyMappingPolicy>((IBuildKeyMappingPolicy) new BuildKeyMappingPolicy(new NamedTypeBuildKey(e.TypeTo, e.Name)), (object) new NamedTypeBuildKey(e.TypeFrom, e.Name));
      }
      if (e.LifetimeManager == null)
        return;
      this.SetLifetimeManager(e.TypeTo, e.Name, e.LifetimeManager);
    }

    private void OnRegisterInstance(object sender, RegisterInstanceEventArgs e)
    {
      this.Context.RegisterNamedType(e.RegisteredType, e.Name);
      this.SetLifetimeManager(e.RegisteredType, e.Name, e.LifetimeManager);
      NamedTypeBuildKey newBuildKey = new NamedTypeBuildKey(e.RegisteredType, e.Name);
      this.Context.Policies.Set<IBuildKeyMappingPolicy>((IBuildKeyMappingPolicy) new BuildKeyMappingPolicy(newBuildKey), (object) newBuildKey);
      e.LifetimeManager.SetValue(e.Instance);
    }

    private void SetLifetimeManager(
      Type lifetimeType,
      string name,
      LifetimeManager lifetimeManager)
    {
      if (lifetimeManager.InUse)
        throw new InvalidOperationException(Resources.LifetimeManagerInUse);
      if (lifetimeType.GetTypeInfo().IsGenericTypeDefinition)
      {
        this.Context.Policies.Set<ILifetimeFactoryPolicy>((ILifetimeFactoryPolicy) new LifetimeManagerFactory(this.Context, lifetimeManager.GetType()), (object) new NamedTypeBuildKey(lifetimeType, name));
      }
      else
      {
        lifetimeManager.InUse = true;
        this.Context.Policies.Set<ILifetimePolicy>((ILifetimePolicy) lifetimeManager, (object) new NamedTypeBuildKey(lifetimeType, name));
        if (!(lifetimeManager is IDisposable))
          return;
        this.Context.Lifetime.Add((object) lifetimeManager);
      }
    }

    private class ContainerLifetimeManager : LifetimeManager
    {
      private object value;

      public override object GetValue() => this.value;

      public override void SetValue(object newValue) => this.value = newValue;

      public override void RemoveValue()
      {
      }
    }
  }
}
