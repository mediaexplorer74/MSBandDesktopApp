// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Mvvm.AsyncHealthCommandBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.Mvvm
{
  public abstract class AsyncHealthCommandBase : HealthObservableObject, ICommand
  {
    private readonly bool reportCanExecuteChanged;
    private bool isEnabled;
    private bool isExecuting;

    public event EventHandler CanExecuteChanged;

    public AsyncHealthCommandBase(bool reportCanExecute)
    {
      this.isEnabled = true;
      this.reportCanExecuteChanged = reportCanExecute;
    }

    public bool IsEnabled
    {
      get => this.isEnabled && !this.isExecuting;
      set
      {
        if (!this.SetProperty<bool>(ref this.isEnabled, value, nameof (IsEnabled)) || !this.reportCanExecuteChanged)
          return;
        this.RaiseChangeEvent();
      }
    }

    public bool CanExecute(object parameter) => this.IsEnabled;

    public async void Execute(object parameter)
    {
      if (!this.IsEnabled)
        return;
      this.isExecuting = true;
      this.RaiseChangeEvent();
      try
      {
        await this.ExecuteAsync(parameter);
      }
      finally
      {
        this.isExecuting = false;
        this.RaiseChangeEvent();
      }
    }

    protected abstract Task ExecuteAsync(object parameter);

    private void RaiseChangeEvent()
    {
      if (!this.reportCanExecuteChanged)
        return;
      EventHandler canExecuteChanged = this.CanExecuteChanged;
      if (canExecuteChanged == null)
        return;
      canExecuteChanged((object) this, EventArgs.Empty);
    }
  }
}
