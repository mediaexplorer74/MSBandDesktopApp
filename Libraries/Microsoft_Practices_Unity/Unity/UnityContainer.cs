// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.UnityContainer
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.ObjectBuilder;
using Microsoft.Practices.Unity.Properties;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Microsoft.Practices.Unity
{
  public class UnityContainer : IUnityContainer, IDisposable
  {
    private readonly UnityContainer parent;
    private LifetimeContainer lifetimeContainer;
    private StagedStrategyChain<UnityBuildStage> strategies;
    private StagedStrategyChain<UnityBuildStage> buildPlanStrategies;
    private PolicyList policies;
    private NamedTypesRegistry registeredNames;
    private List<UnityContainerExtension> extensions;
    private IStrategyChain cachedStrategies;
    private object cachedStrategiesLock;

    private event EventHandler<RegisterEventArgs> Registering;

    private event EventHandler<RegisterInstanceEventArgs> RegisteringInstance;

    private event EventHandler<ChildContainerCreatedEventArgs> ChildContainerCreated;

    public UnityContainer()
      : this((UnityContainer) null)
    {
      this.AddExtension((UnityContainerExtension) new UnityDefaultStrategiesExtension());
    }

    private UnityContainer(UnityContainer parent)
    {
      this.parent = parent;
      parent?.lifetimeContainer.Add((object) this);
      this.InitializeBuilderState();
      this.Registering += (EventHandler<RegisterEventArgs>) ((param0, param1) => { });
      this.RegisteringInstance += (EventHandler<RegisterInstanceEventArgs>) ((param0, param1) => { });
      this.ChildContainerCreated += (EventHandler<ChildContainerCreatedEventArgs>) ((param0, param1) => { });
      this.AddExtension((UnityContainerExtension) new UnityDefaultBehaviorExtension());
      this.AddExtension((UnityContainerExtension) new InjectedMembers());
    }

    public IUnityContainer RegisterType(
      Type from,
      Type to,
      string name,
      LifetimeManager lifetimeManager,
      InjectionMember[] injectionMembers)
    {
      Guard.ArgumentNotNull((object) to, nameof (to));
      Guard.ArgumentNotNull((object) injectionMembers, nameof (injectionMembers));
      if (string.IsNullOrEmpty(name))
        name = (string) null;
      if ((object) from != null && !from.GetTypeInfo().IsGenericType && !to.GetTypeInfo().IsGenericType)
        Guard.TypeIsAssignable(from, to, nameof (from));
      this.Registering((object) this, new RegisterEventArgs(from, to, name, lifetimeManager));
      if (injectionMembers.Length > 0)
      {
        this.ClearExistingBuildPlan(to, name);
        foreach (InjectionMember injectionMember in injectionMembers)
          injectionMember.AddPolicies(from, to, name, (IPolicyList) this.policies);
      }
      return (IUnityContainer) this;
    }

    public IUnityContainer RegisterInstance(
      Type t,
      string name,
      object instance,
      LifetimeManager lifetime)
    {
      Guard.ArgumentNotNull(instance, nameof (instance));
      Guard.ArgumentNotNull((object) lifetime, nameof (lifetime));
      Guard.InstanceIsAssignable(t, instance, nameof (instance));
      this.RegisteringInstance((object) this, new RegisterInstanceEventArgs(t, instance, name, lifetime));
      return (IUnityContainer) this;
    }

    public object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides) => this.DoBuildUp(t, name, (IEnumerable<ResolverOverride>) resolverOverrides);

    public IEnumerable<object> ResolveAll(
      Type t,
      params ResolverOverride[] resolverOverrides)
    {
      Guard.ArgumentNotNull((object) t, nameof (t));
      return (IEnumerable<object>) this.Resolve(t.MakeArrayType(), resolverOverrides);
    }

    public object BuildUp(
      Type t,
      object existing,
      string name,
      params ResolverOverride[] resolverOverrides)
    {
      Guard.ArgumentNotNull(existing, nameof (existing));
      Guard.InstanceIsAssignable(t, existing, nameof (existing));
      return this.DoBuildUp(t, existing, name, (IEnumerable<ResolverOverride>) resolverOverrides);
    }

    public void Teardown(object o)
    {
      IBuilderContext context = (IBuilderContext) null;
      try
      {
        Guard.ArgumentNotNull(o, nameof (o));
        context = (IBuilderContext) new BuilderContext(this.GetStrategies().Reverse(), (ILifetimeContainer) this.lifetimeContainer, (IPolicyList) this.policies, (NamedTypeBuildKey) null, o);
        context.Strategies.ExecuteTearDown(context);
      }
      catch (Exception ex)
      {
        throw new ResolutionFailedException(o.GetType(), (string) null, ex, context);
      }
    }

    public IUnityContainer AddExtension(UnityContainerExtension extension)
    {
      Guard.ArgumentNotNull((object) this.extensions, "extensions");
      this.extensions.Add(extension);
      extension.InitializeExtension((ExtensionContext) new UnityContainer.ExtensionContextImpl(this));
      lock (this.cachedStrategiesLock)
        this.cachedStrategies = (IStrategyChain) null;
      return (IUnityContainer) this;
    }

    public object Configure(Type configurationInterface) => (object) this.extensions.Where<UnityContainerExtension>((Func<UnityContainerExtension, bool>) (ex => configurationInterface.GetTypeInfo().IsAssignableFrom(ex.GetType().GetTypeInfo()))).FirstOrDefault<UnityContainerExtension>();

    public IUnityContainer RemoveAllExtensions()
    {
      List<UnityContainerExtension> containerExtensionList = new List<UnityContainerExtension>((IEnumerable<UnityContainerExtension>) this.extensions);
      containerExtensionList.Reverse();
      foreach (UnityContainerExtension containerExtension in containerExtensionList)
      {
        containerExtension.Remove();
        if (containerExtension is IDisposable disposable2)
          disposable2.Dispose();
      }
      this.extensions.Clear();
      this.strategies.Clear();
      this.policies.ClearAll();
      this.registeredNames.Clear();
      return (IUnityContainer) this;
    }

    public IUnityContainer CreateChildContainer()
    {
      UnityContainer container = new UnityContainer(this);
      this.ChildContainerCreated((object) this, new ChildContainerCreatedEventArgs((ExtensionContext) new UnityContainer.ExtensionContextImpl(container)));
      return (IUnityContainer) container;
    }

    public IUnityContainer Parent => (IUnityContainer) this.parent;

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      if (this.lifetimeContainer != null)
      {
        this.lifetimeContainer.Dispose();
        this.lifetimeContainer = (LifetimeContainer) null;
        if (this.parent != null && this.parent.lifetimeContainer != null)
          this.parent.lifetimeContainer.Remove((object) this);
      }
      this.extensions.OfType<IDisposable>().ForEach<IDisposable>((Action<IDisposable>) (ex => ex.Dispose()));
      this.extensions.Clear();
    }

    private object DoBuildUp(Type t, string name, IEnumerable<ResolverOverride> resolverOverrides) => this.DoBuildUp(t, (object) null, name, resolverOverrides);

    private object DoBuildUp(
      Type t,
      object existing,
      string name,
      IEnumerable<ResolverOverride> resolverOverrides)
    {
      IBuilderContext context = (IBuilderContext) null;
      try
      {
        context = (IBuilderContext) new BuilderContext(this.GetStrategies(), (ILifetimeContainer) this.lifetimeContainer, (IPolicyList) this.policies, new NamedTypeBuildKey(t, name), existing);
        context.AddResolverOverrides(resolverOverrides);
        if (t.GetTypeInfo().IsGenericTypeDefinition)
          throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.CannotResolveOpenGenericType, new object[1]
          {
            (object) t.FullName
          }), nameof (t));
        return context.Strategies.ExecuteBuildUp(context);
      }
      catch (Exception ex)
      {
        throw new ResolutionFailedException(t, name, ex, context);
      }
    }

    private IStrategyChain GetStrategies()
    {
      IStrategyChain strategyChain = this.cachedStrategies;
      if (strategyChain == null)
      {
        lock (this.cachedStrategiesLock)
        {
          if (this.cachedStrategies == null)
          {
            strategyChain = this.strategies.MakeStrategyChain();
            this.cachedStrategies = strategyChain;
          }
          else
            strategyChain = this.cachedStrategies;
        }
      }
      return strategyChain;
    }

    private void InitializeBuilderState()
    {
      this.registeredNames = new NamedTypesRegistry(this.ParentNameRegistry);
      this.extensions = new List<UnityContainerExtension>();
      this.lifetimeContainer = new LifetimeContainer();
      this.strategies = new StagedStrategyChain<UnityBuildStage>(this.ParentStrategies);
      this.buildPlanStrategies = new StagedStrategyChain<UnityBuildStage>(this.ParentBuildPlanStrategies);
      this.policies = new PolicyList((IPolicyList) this.ParentPolicies);
      this.policies.Set<IRegisteredNamesPolicy>((IRegisteredNamesPolicy) new RegisteredNamesPolicy(this.registeredNames), (object) null);
      this.cachedStrategies = (IStrategyChain) null;
      this.cachedStrategiesLock = new object();
    }

    private StagedStrategyChain<UnityBuildStage> ParentStrategies => this.parent != null ? this.parent.strategies : (StagedStrategyChain<UnityBuildStage>) null;

    private StagedStrategyChain<UnityBuildStage> ParentBuildPlanStrategies => this.parent != null ? this.parent.buildPlanStrategies : (StagedStrategyChain<UnityBuildStage>) null;

    private PolicyList ParentPolicies => this.parent != null ? this.parent.policies : (PolicyList) null;

    private NamedTypesRegistry ParentNameRegistry => this.parent != null ? this.parent.registeredNames : (NamedTypesRegistry) null;

    public IEnumerable<ContainerRegistration> Registrations
    {
      get
      {
        Dictionary<Type, List<string>> allRegisteredNames = new Dictionary<Type, List<string>>();
        this.FillTypeRegistrationDictionary((IDictionary<Type, List<string>>) allRegisteredNames);
        return allRegisteredNames.Keys.SelectMany<Type, string, ContainerRegistration>((Func<Type, IEnumerable<string>>) (type => (IEnumerable<string>) allRegisteredNames[type]), (Func<Type, string, ContainerRegistration>) ((type, name) => new ContainerRegistration(type, name, (IPolicyList) this.policies)));
      }
    }

    private void ClearExistingBuildPlan(Type typeToInject, string name)
    {
      NamedTypeBuildKey namedTypeBuildKey = new NamedTypeBuildKey(typeToInject, name);
      DependencyResolverTrackerPolicy.RemoveResolvers((IPolicyList) this.policies, (object) namedTypeBuildKey);
      this.policies.Set<IBuildPlanPolicy>((IBuildPlanPolicy) new OverriddenBuildPlanMarkerPolicy(), (object) namedTypeBuildKey);
    }

    private void FillTypeRegistrationDictionary(IDictionary<Type, List<string>> typeRegistrations)
    {
      if (this.parent != null)
        this.parent.FillTypeRegistrationDictionary(typeRegistrations);
      foreach (Type registeredType in this.registeredNames.RegisteredTypes)
      {
        if (!typeRegistrations.ContainsKey(registeredType))
          typeRegistrations[registeredType] = new List<string>();
        typeRegistrations[registeredType] = typeRegistrations[registeredType].Concat<string>(this.registeredNames.GetKeys(registeredType)).Distinct<string>().ToList<string>();
      }
    }

    private class ExtensionContextImpl : ExtensionContext
    {
      private readonly UnityContainer container;

      public ExtensionContextImpl(UnityContainer container) => this.container = container;

      public override IUnityContainer Container => (IUnityContainer) this.container;

      public override StagedStrategyChain<UnityBuildStage> Strategies => this.container.strategies;

      public override StagedStrategyChain<UnityBuildStage> BuildPlanStrategies => this.container.buildPlanStrategies;

      public override IPolicyList Policies => (IPolicyList) this.container.policies;

      public override ILifetimeContainer Lifetime => (ILifetimeContainer) this.container.lifetimeContainer;

      public override void RegisterNamedType(Type t, string name) => this.container.registeredNames.RegisterType(t, name);

      public override event EventHandler<RegisterEventArgs> Registering
      {
        add => this.container.Registering += value;
        remove => this.container.Registering -= value;
      }

      public override event EventHandler<RegisterInstanceEventArgs> RegisteringInstance
      {
        add => this.container.RegisteringInstance += value;
        remove => this.container.RegisteringInstance -= value;
      }

      public override event EventHandler<ChildContainerCreatedEventArgs> ChildContainerCreated
      {
        add => this.container.ChildContainerCreated += value;
        remove => this.container.ChildContainerCreated -= value;
      }
    }
  }
}
