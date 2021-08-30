// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ServiceLocation.ServiceLocatorImplBase
// Assembly: Microsoft.Practices.ServiceLocation, Version=1.3.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 7D3316BA-C928-4A64-AD5F-824E0C3D6D36
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_ServiceLocation.dll

using Microsoft.Practices.ServiceLocation.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.Practices.ServiceLocation
{
  public abstract class ServiceLocatorImplBase : IServiceLocator, IServiceProvider
  {
    public virtual object GetService(Type serviceType) => this.GetInstance(serviceType, (string) null);

    public virtual object GetInstance(Type serviceType) => this.GetInstance(serviceType, (string) null);

    public virtual object GetInstance(Type serviceType, string key)
    {
      try
      {
        return this.DoGetInstance(serviceType, key);
      }
      catch (Exception ex)
      {
        throw new ActivationException(this.FormatActivationExceptionMessage(ex, serviceType, key), ex);
      }
    }

    public virtual IEnumerable<object> GetAllInstances(Type serviceType)
    {
      try
      {
        return this.DoGetAllInstances(serviceType);
      }
      catch (Exception ex)
      {
        throw new ActivationException(this.FormatActivateAllExceptionMessage(ex, serviceType), ex);
      }
    }

    public virtual TService GetInstance<TService>() => (TService) this.GetInstance(typeof (TService), (string) null);

    public virtual TService GetInstance<TService>(string key) => (TService) this.GetInstance(typeof (TService), key);

    public virtual IEnumerable<TService> GetAllInstances<TService>()
    {
      foreach (object item in this.GetAllInstances(typeof (TService)))
        yield return (TService) item;
    }

    protected abstract object DoGetInstance(Type serviceType, string key);

    protected abstract IEnumerable<object> DoGetAllInstances(Type serviceType);

    protected virtual string FormatActivationExceptionMessage(
      Exception actualException,
      Type serviceType,
      string key)
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentUICulture, Resources.ActivationExceptionMessage, new object[2]
      {
        (object) serviceType.Name,
        (object) key
      });
    }

    protected virtual string FormatActivateAllExceptionMessage(
      Exception actualException,
      Type serviceType)
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentUICulture, Resources.ActivateAllExceptionMessage, new object[1]
      {
        (object) serviceType.Name
      });
    }
  }
}
