// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Command.RelayCommand
// Assembly: GalaSoft.MvvmLight, Version=5.0.2.32240, Culture=neutral, PublicKeyToken=e7570ab207bcb616
// MVID: 672AD33A-61F0-448A-AE1B-56983EAB4C33
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\GalaSoft_MvvmLight.dll

using GalaSoft.MvvmLight.Helpers;
using System;
using System.Windows.Input;

namespace GalaSoft.MvvmLight.Command
{
  public class RelayCommand : ICommand
  {
    private readonly WeakAction _execute;
    private readonly WeakFunc<bool> _canExecute;

    public RelayCommand(Action execute)
      : this(execute, (Func<bool>) null)
    {
    }

    public RelayCommand(Action execute, Func<bool> canExecute)
    {
      this._execute = execute != null ? new WeakAction(execute) : throw new ArgumentNullException(nameof (execute));
      if (canExecute == null)
        return;
      this._canExecute = new WeakFunc<bool>(canExecute);
    }

    public event EventHandler CanExecuteChanged;

    public void RaiseCanExecuteChanged()
    {
      EventHandler canExecuteChanged = this.CanExecuteChanged;
      if (canExecuteChanged == null)
        return;
      canExecuteChanged((object) this, EventArgs.Empty);
    }

    public bool CanExecute(object parameter)
    {
      if (this._canExecute == null)
        return true;
      return (this._canExecute.IsStatic || this._canExecute.IsAlive) && this._canExecute.Execute();
    }

    public virtual void Execute(object parameter)
    {
      if (!this.CanExecute(parameter) || this._execute == null || !this._execute.IsStatic && !this._execute.IsAlive)
        return;
      this._execute.Execute();
    }
  }
}
