// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Command.RelayCommand`1
// Assembly: GalaSoft.MvvmLight, Version=5.0.2.32240, Culture=neutral, PublicKeyToken=e7570ab207bcb616
// MVID: 672AD33A-61F0-448A-AE1B-56983EAB4C33
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\GalaSoft_MvvmLight.dll

using GalaSoft.MvvmLight.Helpers;
using System;
using System.Reflection;
using System.Windows.Input;

namespace GalaSoft.MvvmLight.Command
{
  public class RelayCommand<T> : ICommand
  {
    private readonly WeakAction<T> _execute;
    private readonly WeakFunc<T, bool> _canExecute;

    public RelayCommand(Action<T> execute)
      : this(execute, (Func<T, bool>) null)
    {
    }

    public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
    {
      this._execute = execute != null ? new WeakAction<T>(execute) : throw new ArgumentNullException(nameof (execute));
      if (canExecute == null)
        return;
      this._canExecute = new WeakFunc<T, bool>(canExecute);
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
      if (this._canExecute.IsStatic || this._canExecute.IsAlive)
      {
        if (parameter == null && typeof (T).GetTypeInfo().IsValueType)
          return this._canExecute.Execute(default (T));
        if (parameter is T parameter3)
          return this._canExecute.Execute(parameter3);
      }
      return false;
    }

    public virtual void Execute(object parameter)
    {
      object parameter1 = parameter;
      if (!this.CanExecute(parameter1) || this._execute == null || !this._execute.IsStatic && !this._execute.IsAlive)
        return;
      if (parameter1 == null)
      {
        if (typeof (T).GetTypeInfo().IsValueType)
          this._execute.Execute(default (T));
        else
          this._execute.Execute((T) parameter1);
      }
      else
        this._execute.Execute((T) parameter1);
    }
  }
}
