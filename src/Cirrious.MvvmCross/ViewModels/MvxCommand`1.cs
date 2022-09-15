// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxCommand`1
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.CrossCore.ExtensionMethods;
using System;
using System.Windows.Input;

namespace Cirrious.MvvmCross.ViewModels
{
  public class MvxCommand<T> : MvxCommandBase, IMvxCommand, ICommand
  {
    private readonly Func<T, bool> _canExecute;
    private readonly Action<T> _execute;

    public MvxCommand(Action<T> execute)
      : this(execute, (Func<T, bool>) null)
    {
    }

    public MvxCommand(Action<T> execute, Func<T, bool> canExecute)
    {
      this._execute = execute;
      this._canExecute = canExecute;
    }

    public bool CanExecute(object parameter) => this._canExecute == null || this._canExecute((T) typeof (T).MakeSafeValueCore(parameter));

    public bool CanExecute() => this.CanExecute((object) null);

    public void Execute(object parameter)
    {
      if (!this.CanExecute(parameter))
        return;
      this._execute((T) typeof (T).MakeSafeValueCore(parameter));
    }

    public void Execute() => this.Execute((object) null);
  }
}
