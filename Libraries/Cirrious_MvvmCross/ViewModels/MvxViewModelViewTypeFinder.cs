// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxViewModelViewTypeFinder
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cirrious.MvvmCross.ViewModels
{
  public class MvxViewModelViewTypeFinder : IMvxViewModelTypeFinder, IMvxTypeFinder
  {
    private readonly IMvxViewModelByNameLookup _viewModelByNameLookup;
    private readonly IMvxNameMapping _viewToViewModelNameMapping;

    public MvxViewModelViewTypeFinder(
      IMvxViewModelByNameLookup viewModelByNameLookup,
      IMvxNameMapping viewToViewModelNameMapping)
    {
      this._viewModelByNameLookup = viewModelByNameLookup;
      this._viewToViewModelNameMapping = viewToViewModelNameMapping;
    }

    public virtual Type FindTypeOrNull(Type candidateType)
    {
      if (!this.CheckCandidateTypeIsAView(candidateType))
        return (Type) null;
      if (!candidateType.IsConventional())
        return (Type) null;
      Type type1 = this.LookupAttributedViewModelType(candidateType);
      if ((object) type1 != null)
        return type1;
      Type type2 = this.LookupAssociatedConcreteViewModelType(candidateType);
      if ((object) type2 != null)
        return type2;
      Type type3 = this.LookupNamedViewModelType(candidateType);
      if ((object) type3 != null)
        return type3;
      MvxTrace.Trace("No view model association found for candidate view {0}", (object) candidateType.Name);
      return (Type) null;
    }

    protected virtual Type LookupAttributedViewModelType(Type candidateType) => !(((IEnumerable<Attribute>) ReflectionExtensions.GetCustomAttributes(candidateType, typeof (MvxViewForAttribute), false)).FirstOrDefault<Attribute>() is MvxViewForAttribute viewForAttribute) ? (Type) null : viewForAttribute.ViewModel;

    protected virtual Type LookupNamedViewModelType(Type candidateType)
    {
      Type viewModelType;
      this._viewModelByNameLookup.TryLookupByName(this._viewToViewModelNameMapping.Map(candidateType.Name), out viewModelType);
      return viewModelType;
    }

    protected virtual Type LookupAssociatedConcreteViewModelType(Type candidateType) => ReflectionExtensions.GetProperties(candidateType).FirstOrDefault<PropertyInfo>((Func<PropertyInfo, bool>) (x => x.Name == "ViewModel" && !x.PropertyType.GetTypeInfo().IsInterface && !x.PropertyType.GetTypeInfo().IsAbstract))?.PropertyType;

    protected virtual bool CheckCandidateTypeIsAView(Type candidateType) => (object) candidateType != null && !candidateType.GetTypeInfo().IsAbstract && ReflectionExtensions.IsAssignableFrom(typeof (IMvxView), candidateType);
  }
}
