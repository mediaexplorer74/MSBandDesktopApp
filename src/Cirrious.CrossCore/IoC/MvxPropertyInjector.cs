// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.IoC.MvxPropertyInjector
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
  public class MvxPropertyInjector : IMvxPropertyInjector
  {
    public virtual void Inject(object target, IMvxPropertyInjectorOptions options = null)
    {
      options = options ?? MvxPropertyInjectorOptions.All;
      if (options.InjectIntoProperties == MvxPropertyInjection.None)
        return;
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      foreach (PropertyInfo injectableProperty in this.FindInjectableProperties(target.GetType(), options))
        this.InjectProperty(target, injectableProperty, options);
    }

    protected virtual void InjectProperty(
      object toReturn,
      PropertyInfo injectableProperty,
      IMvxPropertyInjectorOptions options)
    {
      object service;
      if (Mvx.TryResolve(injectableProperty.PropertyType, out service))
      {
        try
        {
          injectableProperty.SetValue(toReturn, service, (object[]) null);
        }
        catch (TargetInvocationException ex)
        {
          throw new MvxIoCResolveException((Exception) ex, "Failed to inject into {0} on {1}", new object[2]
          {
            (object) injectableProperty.Name,
            (object) toReturn.GetType().Name
          });
        }
      }
      else
      {
        if (options.ThrowIfPropertyInjectionFails)
          throw new MvxIoCResolveException("IoC property injection failed for {0} on {1}", new object[2]
          {
            (object) injectableProperty.Name,
            (object) toReturn.GetType().Name
          });
        Mvx.Warning("IoC property injection skipped for {0} on {1}", (object) injectableProperty.Name, (object) toReturn.GetType().Name);
      }
    }

    protected virtual IEnumerable<PropertyInfo> FindInjectableProperties(
      Type type,
      IMvxPropertyInjectorOptions options)
    {
      IEnumerable<PropertyInfo> source = type.GetProperties(Cirrious.CrossCore.BindingFlags.Instance | Cirrious.CrossCore.BindingFlags.Public | Cirrious.CrossCore.BindingFlags.FlattenHierarchy).Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.PropertyType.GetTypeInfo().IsInterface)).Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.IsConventional())).Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.CanWrite));
      switch (options.InjectIntoProperties)
      {
        case MvxPropertyInjection.None:
          Mvx.Error("Internal error - should not call FindInjectableProperties with MvxPropertyInjection.None");
          source = (IEnumerable<PropertyInfo>) new PropertyInfo[0];
          goto case MvxPropertyInjection.AllInterfaceProperties;
        case MvxPropertyInjection.MvxInjectInterfaceProperties:
          source = source.Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => CustomAttributeExtensions.GetCustomAttributes(p, typeof (MvxInjectAttribute), false).Any<Attribute>()));
          goto case MvxPropertyInjection.AllInterfaceProperties;
        case MvxPropertyInjection.AllInterfaceProperties:
          return source;
        default:
          throw new MvxException("unknown option for InjectIntoProperties {0}", new object[1]
          {
            (object) options.InjectIntoProperties
          });
      }
    }
  }
}
