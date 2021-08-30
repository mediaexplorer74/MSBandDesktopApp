// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.IoC.MvxTypeExtensions
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using Cirrious.CrossCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cirrious.CrossCore.IoC
{
  public static class MvxTypeExtensions
  {
    public static IEnumerable<Type> ExceptionSafeGetTypes(this Assembly assembly)
    {
      try
      {
        return ReflectionExtensions.GetTypes(assembly);
      }
      catch (ReflectionTypeLoadException ex)
      {
        Mvx.Warning("ReflectionTypeLoadException masked during loading of {0} - error {1}", (object) assembly.FullName, (object) ex.ToLongString());
        return (IEnumerable<Type>) new Type[0];
      }
    }

    public static IEnumerable<Type> CreatableTypes(this Assembly assembly) => assembly.ExceptionSafeGetTypes().Select<Type, TypeInfo>((Func<Type, TypeInfo>) (t => t.GetTypeInfo())).Where<TypeInfo>((Func<TypeInfo, bool>) (t => !t.IsAbstract)).Where<TypeInfo>((Func<TypeInfo, bool>) (t => t.DeclaredConstructors.Any<ConstructorInfo>((Func<ConstructorInfo, bool>) (c => !c.IsStatic && c.IsPublic)))).Select<TypeInfo, Type>((Func<TypeInfo, Type>) (t => t.AsType()));

    public static IEnumerable<Type> EndingWith(
      this IEnumerable<Type> types,
      string endingWith)
    {
      return types.Where<Type>((Func<Type, bool>) (x => x.Name.EndsWith(endingWith)));
    }

    public static IEnumerable<Type> StartingWith(
      this IEnumerable<Type> types,
      string endingWith)
    {
      return types.Where<Type>((Func<Type, bool>) (x => x.Name.StartsWith(endingWith)));
    }

    public static IEnumerable<Type> Containing(
      this IEnumerable<Type> types,
      string containing)
    {
      return types.Where<Type>((Func<Type, bool>) (x => x.Name.Contains(containing)));
    }

    public static IEnumerable<Type> InNamespace(
      this IEnumerable<Type> types,
      string namespaceBase)
    {
      return types.Where<Type>((Func<Type, bool>) (x => x.Namespace != null && x.Namespace.StartsWith(namespaceBase)));
    }

    public static IEnumerable<Type> WithAttribute(
      this IEnumerable<Type> types,
      Type attributeType)
    {
      return types.Where<Type>((Func<Type, bool>) (x => ((IEnumerable<Attribute>) ReflectionExtensions.GetCustomAttributes(x, attributeType, true)).Any<Attribute>()));
    }

    public static IEnumerable<Type> WithAttribute<TAttribute>(
      this IEnumerable<Type> types)
      where TAttribute : Attribute
    {
      return types.WithAttribute(typeof (TAttribute));
    }

    public static IEnumerable<Type> Inherits(
      this IEnumerable<Type> types,
      Type baseType)
    {
      return types.Where<Type>((Func<Type, bool>) (x => ReflectionExtensions.IsAssignableFrom(baseType, x)));
    }

    public static IEnumerable<Type> Inherits<TBase>(this IEnumerable<Type> types) => types.Inherits(typeof (TBase));

    public static IEnumerable<Type> DoesNotInherit(
      this IEnumerable<Type> types,
      Type baseType)
    {
      return types.Where<Type>((Func<Type, bool>) (x => !ReflectionExtensions.IsAssignableFrom(baseType, x)));
    }

    public static IEnumerable<Type> DoesNotInherit<TBase>(
      this IEnumerable<Type> types)
      where TBase : Attribute
    {
      return types.DoesNotInherit(typeof (TBase));
    }

    public static IEnumerable<Type> Except(
      this IEnumerable<Type> types,
      params Type[] except)
    {
      if (except.Length < 3)
        return types.Where<Type>((Func<Type, bool>) (x => !((IEnumerable<Type>) except).Contains<Type>(x)));
      Dictionary<Type, bool> lookup = ((IEnumerable<Type>) except).ToDictionary<Type, Type, bool>((Func<Type, Type>) (x => x), (Func<Type, bool>) (x => true));
      return types.Where<Type>((Func<Type, bool>) (x => !lookup.ContainsKey(x)));
    }

    public static IEnumerable<MvxTypeExtensions.ServiceTypeAndImplementationTypePair> AsTypes(
      this IEnumerable<Type> types)
    {
      return types.Select<Type, MvxTypeExtensions.ServiceTypeAndImplementationTypePair>((Func<Type, MvxTypeExtensions.ServiceTypeAndImplementationTypePair>) (t => new MvxTypeExtensions.ServiceTypeAndImplementationTypePair(new List<Type>()
      {
        t
      }, t)));
    }

    public static IEnumerable<MvxTypeExtensions.ServiceTypeAndImplementationTypePair> AsInterfaces(
      this IEnumerable<Type> types)
    {
      return types.Select<Type, MvxTypeExtensions.ServiceTypeAndImplementationTypePair>((Func<Type, MvxTypeExtensions.ServiceTypeAndImplementationTypePair>) (t => new MvxTypeExtensions.ServiceTypeAndImplementationTypePair(ReflectionExtensions.GetInterfaces(t).ToList<Type>(), t)));
    }

    public static IEnumerable<MvxTypeExtensions.ServiceTypeAndImplementationTypePair> AsInterfaces(
      this IEnumerable<Type> types,
      params Type[] interfaces)
    {
      if (interfaces.Length < 3)
        return types.Select<Type, MvxTypeExtensions.ServiceTypeAndImplementationTypePair>((Func<Type, MvxTypeExtensions.ServiceTypeAndImplementationTypePair>) (t => new MvxTypeExtensions.ServiceTypeAndImplementationTypePair(ReflectionExtensions.GetInterfaces(t).Where<Type>((Func<Type, bool>) (iface => ((IEnumerable<Type>) interfaces).Contains<Type>(iface))).ToList<Type>(), t)));
      Dictionary<Type, bool> lookup = ((IEnumerable<Type>) interfaces).ToDictionary<Type, Type, bool>((Func<Type, Type>) (x => x), (Func<Type, bool>) (x => true));
      return types.Select<Type, MvxTypeExtensions.ServiceTypeAndImplementationTypePair>((Func<Type, MvxTypeExtensions.ServiceTypeAndImplementationTypePair>) (t => new MvxTypeExtensions.ServiceTypeAndImplementationTypePair(ReflectionExtensions.GetInterfaces(t).Where<Type>((Func<Type, bool>) (iface => lookup.ContainsKey(iface))).ToList<Type>(), t)));
    }

    public static IEnumerable<MvxTypeExtensions.ServiceTypeAndImplementationTypePair> ExcludeInterfaces(
      this IEnumerable<MvxTypeExtensions.ServiceTypeAndImplementationTypePair> pairs,
      params Type[] toExclude)
    {
      foreach (MvxTypeExtensions.ServiceTypeAndImplementationTypePair pair in pairs)
      {
        List<Type> excludedList = pair.ServiceTypes.Where<Type>((Func<Type, bool>) (c => !((IEnumerable<Type>) toExclude).Contains<Type>(c))).ToList<Type>();
        if (excludedList.Any<Type>())
        {
          MvxTypeExtensions.ServiceTypeAndImplementationTypePair newPair = new MvxTypeExtensions.ServiceTypeAndImplementationTypePair(excludedList, pair.ImplementationType);
          yield return newPair;
        }
      }
    }

    public static void RegisterAsSingleton(
      this IEnumerable<MvxTypeExtensions.ServiceTypeAndImplementationTypePair> pairs)
    {
      foreach (MvxTypeExtensions.ServiceTypeAndImplementationTypePair pair in pairs)
      {
        if (pair.ServiceTypes.Any<Type>())
        {
          object service = Mvx.IocConstruct(pair.ImplementationType);
          foreach (Type serviceType in pair.ServiceTypes)
            Mvx.RegisterSingleton(serviceType, service);
        }
      }
    }

    public static void RegisterAsLazySingleton(
      this IEnumerable<MvxTypeExtensions.ServiceTypeAndImplementationTypePair> pairs)
    {
      foreach (MvxTypeExtensions.ServiceTypeAndImplementationTypePair pair in pairs)
      {
        if (pair.ServiceTypes.Any<Type>())
        {
          MvxLazySingletonCreator creator = new MvxLazySingletonCreator(pair.ImplementationType);
          Func<object> serviceConstructor = (Func<object>) (() => creator.Instance);
          foreach (Type serviceType in pair.ServiceTypes)
            Mvx.RegisterSingleton(serviceType, serviceConstructor);
        }
      }
    }

    public static void RegisterAsDynamic(
      this IEnumerable<MvxTypeExtensions.ServiceTypeAndImplementationTypePair> pairs)
    {
      foreach (MvxTypeExtensions.ServiceTypeAndImplementationTypePair pair in pairs)
      {
        foreach (Type serviceType in pair.ServiceTypes)
          Mvx.RegisterType(serviceType, pair.ImplementationType);
      }
    }

    public static object CreateDefault(this Type type)
    {
      if ((object) type == null)
        return (object) null;
      if (!type.GetTypeInfo().IsValueType)
        return (object) null;
      return (object) Nullable.GetUnderlyingType(type) != null ? (object) null : Activator.CreateInstance(type);
    }

    public class ServiceTypeAndImplementationTypePair
    {
      public ServiceTypeAndImplementationTypePair(List<Type> serviceTypes, Type implementationType)
      {
        this.ImplementationType = implementationType;
        this.ServiceTypes = serviceTypes;
      }

      public List<Type> ServiceTypes { get; private set; }

      public Type ImplementationType { get; private set; }
    }
  }
}
