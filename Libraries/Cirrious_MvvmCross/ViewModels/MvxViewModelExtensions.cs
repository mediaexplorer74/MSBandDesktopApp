// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxViewModelExtensions
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cirrious.MvvmCross.ViewModels
{
  public static class MvxViewModelExtensions
  {
    public static void CallBundleMethods(
      this IMvxViewModel viewModel,
      string methodName,
      IMvxBundle bundle)
    {
      foreach (MethodInfo methodInfo in viewModel.GetType().GetMethods(Cirrious.CrossCore.BindingFlags.Instance | Cirrious.CrossCore.BindingFlags.Public | Cirrious.CrossCore.BindingFlags.FlattenHierarchy).Where<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == methodName)).Where<MethodInfo>((Func<MethodInfo, bool>) (m => !m.IsAbstract)).ToList<MethodInfo>())
        viewModel.CallBundleMethod(methodInfo, bundle);
    }

    public static void CallBundleMethod(
      this IMvxViewModel viewModel,
      MethodInfo methodInfo,
      IMvxBundle bundle)
    {
      ParameterInfo[] array1 = ((IEnumerable<ParameterInfo>) methodInfo.GetParameters()).ToArray<ParameterInfo>();
      if (((IEnumerable<ParameterInfo>) array1).Count<ParameterInfo>() == 1 && (object) array1[0].ParameterType == (object) typeof (IMvxBundle))
        methodInfo.Invoke((object) viewModel, new object[1]
        {
          (object) bundle
        });
      else if (((IEnumerable<ParameterInfo>) array1).Count<ParameterInfo>() == 1 && !MvxSingleton<IMvxSingletonCache>.Instance.Parser.TypeSupported(array1[0].ParameterType))
      {
        object obj = bundle.Read(array1[0].ParameterType);
        methodInfo.Invoke((object) viewModel, new object[1]
        {
          obj
        });
      }
      else
      {
        object[] array2 = bundle.CreateArgumentList((IEnumerable<ParameterInfo>) array1, viewModel.GetType().Name).ToArray<object>();
        methodInfo.Invoke((object) viewModel, array2);
      }
    }

    public static IMvxBundle SaveStateBundle(this IMvxViewModel viewModel)
    {
      MvxBundle mvxBundle = new MvxBundle();
      foreach (MethodBase methodBase in ReflectionExtensions.GetMethods(viewModel.GetType()).Where<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "SaveState")).Where<MethodInfo>((Func<MethodInfo, bool>) (m => (object) m.ReturnType != (object) typeof (void))).Where<MethodInfo>((Func<MethodInfo, bool>) (m => !((IEnumerable<ParameterInfo>) m.GetParameters()).Any<ParameterInfo>())))
      {
        object toStore = methodBase.Invoke((object) viewModel, new object[0]);
        if (toStore != null)
          mvxBundle.Write(toStore);
      }
      viewModel.SaveState((IMvxBundle) mvxBundle);
      return (IMvxBundle) mvxBundle;
    }
  }
}
