// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxViewModelViewLookupBuilder
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cirrious.MvvmCross.ViewModels
{
  public class MvxViewModelViewLookupBuilder : IMvxTypeToTypeLookupBuilder
  {
    public virtual IDictionary<Type, Type> Build(Assembly[] sourceAssemblies)
    {
      IMvxViewModelTypeFinder associatedTypeFinder = Mvx.Resolve<IMvxViewModelTypeFinder>();
      IEnumerable<KeyValuePair<Type, Type>> keyValuePairs = ((IEnumerable<Assembly>) sourceAssemblies).SelectMany((Func<Assembly, IEnumerable<Type>>) (assembly => assembly.ExceptionSafeGetTypes()), (assembly, candidateViewType) => new
      {
        assembly = assembly,
        candidateViewType = candidateViewType
      }).Select(_param1 => new
      {
        _TransparentIdentifier0 = _param1,
        viewModelType = associatedTypeFinder.FindTypeOrNull(_param1.candidateViewType)
      }).Where(_param0 => (object) _param0.viewModelType != null).Select(_param0 => new KeyValuePair<Type, Type>(_param0.viewModelType, _param0._TransparentIdentifier0.candidateViewType));
      try
      {
        return (IDictionary<Type, Type>) keyValuePairs.ToDictionary<KeyValuePair<Type, Type>, Type, Type>((Func<KeyValuePair<Type, Type>, Type>) (x => x.Key), (Func<KeyValuePair<Type, Type>, Type>) (x => x.Value));
      }
      catch (ArgumentException ex)
      {
        throw MvxViewModelViewLookupBuilder.ReportBuildProblem(keyValuePairs, ex);
      }
    }

    private static Exception ReportBuildProblem(
      IEnumerable<KeyValuePair<Type, Type>> views,
      ArgumentException exception)
    {
      string[] array = views.GroupBy<KeyValuePair<Type, Type>, Type>((Func<KeyValuePair<Type, Type>, Type>) (x => x.Key)).Select(x => new
      {
        Name = x.Key.Name,
        Count = x.Count<KeyValuePair<Type, Type>>(),
        ViewNames = x.Select<KeyValuePair<Type, Type>, string>((Func<KeyValuePair<Type, Type>, string>) (v => v.Value.Name)).ToList<string>()
      }).Where(x => x.Count > 1).Select(x => string.Format("{0}*{1} ({2})", new object[3]
      {
        (object) x.Count,
        (object) x.Name,
        (object) string.Join(",", (IEnumerable<string>) x.ViewNames)
      })).ToArray<string>();
      if (array.Length == 0)
        return exception.MvxWrap("Unknown problem in ViewModelViewLookup construction");
      string str = string.Join(";", array);
      return exception.MvxWrap("Problem seen creating View-ViewModel lookup table - you have more than one View registered for the ViewModels: {0}", (object) str);
    }
  }
}
