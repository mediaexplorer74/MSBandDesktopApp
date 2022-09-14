// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxViewModelByNameLookup
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cirrious.MvvmCross.ViewModels
{
  public class MvxViewModelByNameLookup : IMvxViewModelByNameLookup, IMvxViewModelByNameRegistry
  {
    private readonly Dictionary<string, Type> _availableViewModelsByName;
    private readonly Dictionary<string, Type> _availableViewModelsByFullName;

    public MvxViewModelByNameLookup()
    {
      this._availableViewModelsByName = new Dictionary<string, Type>();
      this._availableViewModelsByFullName = new Dictionary<string, Type>();
    }

    public bool TryLookupByName(string name, out Type viewModelType) => this._availableViewModelsByName.TryGetValue(name, out viewModelType);

    public bool TryLookupByFullName(string name, out Type viewModelType) => this._availableViewModelsByFullName.TryGetValue(name, out viewModelType);

    public void Add(Type viewModelType)
    {
      this._availableViewModelsByName[viewModelType.Name] = viewModelType;
      this._availableViewModelsByFullName[viewModelType.FullName] = viewModelType;
    }

    public void Add<TViewModel>() where TViewModel : IMvxViewModel => this.Add(typeof (TViewModel));

    public void AddAll(Assembly assembly)
    {
      foreach (Type viewModelType in assembly.ExceptionSafeGetTypes().Where<Type>((Func<Type, bool>) (type => !type.GetTypeInfo().IsAbstract)).Where<Type>((Func<Type, bool>) (type => !type.GetTypeInfo().IsInterface)).Where<Type>((Func<Type, bool>) (type => ReflectionExtensions.IsAssignableFrom(typeof (IMvxViewModel), type))))
        this.Add(viewModelType);
    }
  }
}
