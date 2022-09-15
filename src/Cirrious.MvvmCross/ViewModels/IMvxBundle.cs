// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.IMvxBundle
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cirrious.MvvmCross.ViewModels
{
  public interface IMvxBundle
  {
    IDictionary<string, string> Data { get; }

    void Write(object toStore);

    T Read<T>() where T : new();

    object Read(Type type);

    IEnumerable<object> CreateArgumentList(
      IEnumerable<ParameterInfo> requiredParameters,
      string debugText);
  }
}
