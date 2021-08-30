// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Helpers.Empty
// Assembly: GalaSoft.MvvmLight, Version=5.0.2.32240, Culture=neutral, PublicKeyToken=e7570ab207bcb616
// MVID: 672AD33A-61F0-448A-AE1B-56983EAB4C33
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\GalaSoft_MvvmLight.dll

using System;
using System.Threading.Tasks;

namespace GalaSoft.MvvmLight.Helpers
{
  public static class Empty
  {
    private static readonly Task ConcreteTask = new Task((Action) (() => { }));

    public static Task Task => Empty.ConcreteTask;
  }
}
