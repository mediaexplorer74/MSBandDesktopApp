// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxBundle
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.MvvmCross.Platform;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cirrious.MvvmCross.ViewModels
{
  public class MvxBundle : IMvxBundle
  {
    public MvxBundle()
      : this((IDictionary<string, string>) new Dictionary<string, string>())
    {
    }

    public MvxBundle(IDictionary<string, string> data) => this.Data = data ?? (IDictionary<string, string>) new Dictionary<string, string>();

    public IDictionary<string, string> Data { get; private set; }

    public void Write(object toStore) => this.Data.Write(toStore);

    public T Read<T>() where T : new() => this.Data.Read<T>();

    public object Read(Type type) => this.Data.Read(type);

    public IEnumerable<object> CreateArgumentList(
      IEnumerable<ParameterInfo> requiredParameters,
      string debugText)
    {
      return this.Data.CreateArgumentList(requiredParameters, debugText);
    }
  }
}
