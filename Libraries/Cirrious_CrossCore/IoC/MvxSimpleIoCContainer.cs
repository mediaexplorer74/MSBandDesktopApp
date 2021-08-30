// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.IoC.MvxSimpleIoCContainer
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cirrious.CrossCore.IoC
{
  public class MvxSimpleIoCContainer : MvxSingleton<IMvxIoCProvider>, IMvxIoCProvider
  {
    private readonly Dictionary<Type, MvxSimpleIoCContainer.IResolver> _resolvers = new Dictionary<Type, MvxSimpleIoCContainer.IResolver>();
    private readonly Dictionary<Type, List<Action>> _waiters = new Dictionary<Type, List<Action>>();
    private readonly Dictionary<Type, bool> _circularTypeDetection = new Dictionary<Type, bool>();
    private readonly object _lockObject = new object();
    private readonly IMvxIocOptions _options;
    private readonly IMvxPropertyInjector _propertyInjector;
    private static readonly MvxSimpleIoCContainer.ResolverType? ResolverTypeNoneSpecified = new MvxSimpleIoCContainer.ResolverType?();

    public static IMvxIoCProvider Initialize(IMvxIocOptions options = null)
    {
      if (MvxSingleton<IMvxIoCProvider>.Instance != null)
        return MvxSingleton<IMvxIoCProvider>.Instance;
      MvxSimpleIoCContainer simpleIoCcontainer = new MvxSimpleIoCContainer(options);
      return MvxSingleton<IMvxIoCProvider>.Instance;
    }

    protected object LockObject => this._lockObject;

    protected IMvxIocOptions Options => this._options;

    protected MvxSimpleIoCContainer(IMvxIocOptions options)
    {
      this._options = options ?? (IMvxIocOptions) new MvxIocOptions();
      if ((object) this._options.PropertyInjectorType != null)
        this._propertyInjector = Activator.CreateInstance(this._options.PropertyInjectorType) as IMvxPropertyInjector;
      if (this._propertyInjector == null)
        return;
      this.RegisterSingleton(typeof (IMvxPropertyInjector), (object) this._propertyInjector);
    }

    public bool CanResolve<T>() where T : class => this.CanResolve(typeof (T));

    public bool CanResolve(Type t)
    {
      lock (this._lockObject)
        return this._resolvers.ContainsKey(t);
    }

    public bool TryResolve<T>(out T resolved) where T : class
    {
      try
      {
        object resolved1;
        bool flag = this.TryResolve(typeof (T), out resolved1);
        resolved = (T) resolved1;
        return flag;
      }
      catch (MvxIoCResolveException ex)
      {
        resolved = (T) typeof (T).CreateDefault();
        return false;
      }
    }

    public bool TryResolve(Type type, out object resolved)
    {
      lock (this._lockObject)
        return this.InternalTryResolve(type, out resolved);
    }

    public T Resolve<T>() where T : class => (T) this.Resolve(typeof (T));

    public object Resolve(Type t)
    {
      lock (this._lockObject)
      {
        object resolved;
        if (!this.InternalTryResolve(t, out resolved))
          throw new MvxIoCResolveException("Failed to resolve type {0}", new object[1]
          {
            (object) t.FullName
          });
        return resolved;
      }
    }

    public T GetSingleton<T>() where T : class => (T) this.GetSingleton(typeof (T));

    public object GetSingleton(Type t)
    {
      lock (this._lockObject)
      {
        object resolved;
        if (!this.InternalTryResolve(t, new MvxSimpleIoCContainer.ResolverType?(MvxSimpleIoCContainer.ResolverType.Singleton), out resolved))
          throw new MvxIoCResolveException("Failed to resolve type {0}", new object[1]
          {
            (object) t.FullName
          });
        return resolved;
      }
    }

    public T Create<T>() where T : class => (T) this.Create(typeof (T));

    public object Create(Type t)
    {
      lock (this._lockObject)
      {
        object resolved;
        if (!this.InternalTryResolve(t, new MvxSimpleIoCContainer.ResolverType?(MvxSimpleIoCContainer.ResolverType.DynamicPerResolve), out resolved))
          throw new MvxIoCResolveException("Failed to resolve type {0}", new object[1]
          {
            (object) t.FullName
          });
        return resolved;
      }
    }

    public void RegisterType<TInterface, TToConstruct>()
      where TInterface : class
      where TToConstruct : class, TInterface
    {
      this.RegisterType(typeof (TInterface), typeof (TToConstruct));
    }

    public void RegisterType<TInterface>(Func<TInterface> constructor) where TInterface : class => this.InternalSetResolver(typeof (TInterface), (MvxSimpleIoCContainer.IResolver) new MvxSimpleIoCContainer.FuncConstructingResolver((Func<object>) constructor));

    public void RegisterType(Type t, Func<object> constructor)
    {
      MvxSimpleIoCContainer.FuncConstructingResolver constructingResolver = new MvxSimpleIoCContainer.FuncConstructingResolver((Func<object>) (() =>
      {
        object obj = constructor();
        return obj == null || ReflectionExtensions.IsInstanceOfType(t, obj) ? obj : throw new MvxIoCResolveException("Constructor failed to return a compatibly object for type {0}", new object[1]
        {
          (object) t.FullName
        });
      }));
      this.InternalSetResolver(t, (MvxSimpleIoCContainer.IResolver) constructingResolver);
    }

    public void RegisterType(Type tInterface, Type tConstruct)
    {
      MvxSimpleIoCContainer.ConstructingResolver constructingResolver = new MvxSimpleIoCContainer.ConstructingResolver(tConstruct, this);
      this.InternalSetResolver(tInterface, (MvxSimpleIoCContainer.IResolver) constructingResolver);
    }

    public void RegisterSingleton<TInterface>(TInterface theObject) where TInterface : class => this.RegisterSingleton(typeof (TInterface), (object) theObject);

    public void RegisterSingleton(Type tInterface, object theObject) => this.InternalSetResolver(tInterface, (MvxSimpleIoCContainer.IResolver) new MvxSimpleIoCContainer.SingletonResolver(theObject));

    public void RegisterSingleton<TInterface>(Func<TInterface> theConstructor) where TInterface : class => this.RegisterSingleton(typeof (TInterface), (Func<object>) (() => (object) theConstructor()));

    public void RegisterSingleton(Type tInterface, Func<object> theConstructor) => this.InternalSetResolver(tInterface, (MvxSimpleIoCContainer.IResolver) new MvxSimpleIoCContainer.ConstructingSingletonResolver(theConstructor));

    public T IoCConstruct<T>() where T : class => (T) this.IoCConstruct(typeof (T));

    public virtual object IoCConstruct(Type type)
    {
      ConstructorInfo firstConstructor = type.GetConstructors(Cirrious.CrossCore.BindingFlags.Instance | Cirrious.CrossCore.BindingFlags.Public).FirstOrDefault<ConstructorInfo>();
      List<object> objectList = (object) firstConstructor != null ? this.GetIoCParameterValues(type, firstConstructor) : throw new MvxIoCResolveException("Failed to find constructor for type {0}", new object[1]
      {
        (object) type.FullName
      });
      object obj;
      try
      {
        obj = firstConstructor.Invoke(objectList.ToArray());
      }
      catch (TargetInvocationException ex)
      {
        throw new MvxIoCResolveException((Exception) ex, "Failed to construct {0}", new object[1]
        {
          (object) type.Name
        });
      }
      try
      {
        this.InjectProperties(obj);
      }
      catch (Exception ex)
      {
        if (!this.Options.CheckDisposeIfPropertyInjectionFails)
        {
          throw;
        }
        else
        {
          obj.DisposeIfDisposable();
          throw;
        }
      }
      return obj;
    }

    public void CallbackWhenRegistered<T>(Action action) => this.CallbackWhenRegistered(typeof (T), action);

    public void CallbackWhenRegistered(Type type, Action action)
    {
      lock (this._lockObject)
      {
        if (!this.CanResolve(type))
        {
          List<Action> actionList;
          if (this._waiters.TryGetValue(type, out actionList))
          {
            actionList.Add(action);
            return;
          }
          actionList = new List<Action>() { action };
          this._waiters[type] = actionList;
          return;
        }
      }
      action();
    }

    private bool Supports(
      MvxSimpleIoCContainer.IResolver resolver,
      MvxSimpleIoCContainer.ResolverType? requiredResolverType)
    {
      return !requiredResolverType.HasValue || resolver.ResolveType == requiredResolverType.Value;
    }

    private bool InternalTryResolve(Type type, out object resolved) => this.InternalTryResolve(type, MvxSimpleIoCContainer.ResolverTypeNoneSpecified, out resolved);

    private bool InternalTryResolve(
      Type type,
      MvxSimpleIoCContainer.ResolverType? requiredResolverType,
      out object resolved)
    {
      MvxSimpleIoCContainer.IResolver resolver;
      if (!this._resolvers.TryGetValue(type, out resolver))
      {
        resolved = type.CreateDefault();
        return false;
      }
      if (this.Supports(resolver, requiredResolverType))
        return this.InternalTryResolve(type, resolver, out resolved);
      resolved = type.CreateDefault();
      return false;
    }

    private bool ShouldDetectCircularReferencesFor(MvxSimpleIoCContainer.IResolver resolver)
    {
      switch (resolver.ResolveType)
      {
        case MvxSimpleIoCContainer.ResolverType.DynamicPerResolve:
          return this.Options.TryToDetectDynamicCircularReferences;
        case MvxSimpleIoCContainer.ResolverType.Singleton:
          return this.Options.TryToDetectSingletonCircularReferences;
        case MvxSimpleIoCContainer.ResolverType.Unknown:
          throw new MvxException("A resolver must have a known type - error in {0}", new object[1]
          {
            (object) resolver.GetType().Name
          });
        default:
          throw new ArgumentOutOfRangeException(nameof (resolver), "unknown resolveType of " + (object) resolver.ResolveType);
      }
    }

    private bool InternalTryResolve(
      Type type,
      MvxSimpleIoCContainer.IResolver resolver,
      out object resolved)
    {
      bool flag = this.ShouldDetectCircularReferencesFor(resolver);
      if (flag)
      {
        try
        {
          this._circularTypeDetection.Add(type, true);
        }
        catch (ArgumentException ex)
        {
          Mvx.Error("IoC circular reference detected - cannot currently resolve {0}", (object) type.Name);
          resolved = type.CreateDefault();
          return false;
        }
      }
      try
      {
        object obj = resolver.Resolve();
        resolved = ReflectionExtensions.IsInstanceOfType(type, obj) ? obj : throw new MvxException("Resolver returned object type {0} which does not support interface {1}", new object[2]
        {
          (object) obj.GetType().FullName,
          (object) type.FullName
        });
        return true;
      }
      finally
      {
        if (flag)
          this._circularTypeDetection.Remove(type);
      }
    }

    private void InternalSetResolver(Type tInterface, MvxSimpleIoCContainer.IResolver resolver)
    {
      List<Action> actionList;
      lock (this._lockObject)
      {
        this._resolvers[tInterface] = resolver;
        if (this._waiters.TryGetValue(tInterface, out actionList))
          this._waiters.Remove(tInterface);
      }
      if (actionList == null)
        return;
      foreach (Action action in actionList)
        action();
    }

    protected virtual void InjectProperties(object toReturn)
    {
      if (this._propertyInjector == null)
        return;
      this._propertyInjector.Inject(toReturn, this._options.PropertyInjectorOptions);
    }

    protected virtual List<object> GetIoCParameterValues(
      Type type,
      ConstructorInfo firstConstructor)
    {
      List<object> objectList = new List<object>();
      foreach (ParameterInfo parameter in firstConstructor.GetParameters())
      {
        object resolved;
        if (!this.TryResolve(parameter.ParameterType, out resolved))
        {
          if (parameter.IsOptional)
            resolved = Type.Missing;
          else
            throw new MvxIoCResolveException("Failed to resolve parameter for parameter {0} of type {1} when creating {2}", new object[3]
            {
              (object) parameter.Name,
              (object) parameter.ParameterType.Name,
              (object) type.FullName
            });
        }
        objectList.Add(resolved);
      }
      return objectList;
    }

    public interface IResolver
    {
      object Resolve();

      MvxSimpleIoCContainer.ResolverType ResolveType { get; }
    }

    public class ConstructingResolver : MvxSimpleIoCContainer.IResolver
    {
      private readonly Type _type;
      private readonly MvxSimpleIoCContainer _parent;

      public ConstructingResolver(Type type, MvxSimpleIoCContainer parent)
      {
        this._type = type;
        this._parent = parent;
      }

      public object Resolve() => this._parent.IoCConstruct(this._type);

      public MvxSimpleIoCContainer.ResolverType ResolveType => MvxSimpleIoCContainer.ResolverType.DynamicPerResolve;
    }

    public class FuncConstructingResolver : MvxSimpleIoCContainer.IResolver
    {
      private readonly Func<object> _constructor;

      public FuncConstructingResolver(Func<object> constructor) => this._constructor = constructor;

      public object Resolve() => this._constructor();

      public MvxSimpleIoCContainer.ResolverType ResolveType => MvxSimpleIoCContainer.ResolverType.DynamicPerResolve;
    }

    public class SingletonResolver : MvxSimpleIoCContainer.IResolver
    {
      private readonly object _theObject;

      public SingletonResolver(object theObject) => this._theObject = theObject;

      public object Resolve() => this._theObject;

      public MvxSimpleIoCContainer.ResolverType ResolveType => MvxSimpleIoCContainer.ResolverType.Singleton;
    }

    public class ConstructingSingletonResolver : MvxSimpleIoCContainer.IResolver
    {
      private readonly object _syncObject = new object();
      private readonly Func<object> _constructor;
      private object _theObject;

      public ConstructingSingletonResolver(Func<object> theConstructor) => this._constructor = theConstructor;

      public object Resolve()
      {
        if (this._theObject != null)
          return this._theObject;
        lock (this._syncObject)
        {
          if (this._theObject == null)
            this._theObject = this._constructor();
        }
        return this._theObject;
      }

      public MvxSimpleIoCContainer.ResolverType ResolveType => MvxSimpleIoCContainer.ResolverType.Singleton;
    }

    public enum ResolverType
    {
      DynamicPerResolve,
      Singleton,
      Unknown,
    }
  }
}
