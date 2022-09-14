// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxCommand
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using System;
using System.Windows.Input;

namespace Cirrious.MvvmCross.ViewModels
{
  public class MvxCommand : MvxCommandBase, IMvxCommand, ICommand
  {
    private readonly Func<bool> _canExecute;
    private readonly Action _execute;

    public MvxCommand(Action execute)
      : this(execute, (Func<bool>) null)
    {
    }

    public MvxCommand(Action execute, Func<bool> canExecute)
    {
      this._execute = execute;
      this._canExecute = canExecute;
    }

    public bool CanExecute(object parameter) => this._canExecute == null || this._canExecute();

    public bool CanExecute() => this.CanExecute((object) null);

    public void Execute(object parameter)
    {
      if (!this.CanExecute(parameter))
        return;
      this._execute();
    }

    public void Execute() => this.Execute((object) null);
  }
}
