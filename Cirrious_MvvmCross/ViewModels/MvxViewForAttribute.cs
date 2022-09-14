// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxViewForAttribute
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using System;

namespace Cirrious.MvvmCross.ViewModels
{
  [AttributeUsage(AttributeTargets.Class)]
  public class MvxViewForAttribute : Attribute
  {
    public Type ViewModel { get; set; }

    public MvxViewForAttribute(Type viewModel) => this.ViewModel = viewModel;
  }
}
