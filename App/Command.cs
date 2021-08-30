// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.Command
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Windows.Input;

namespace DesktopSyncApp
{
  public class Command : ICommand
  {
    public event ExecuteHandler OnExecute;

    public event EventHandler CanExecuteChanged;

    public Command()
    {
    }

    public Command(ExecuteHandler onExecute) => this.OnExecute = onExecute;

    public void Execute(object parameter)
    {
      if (this.OnExecute == null)
        return;
      this.OnExecute(parameter, Globals.DefaultEventArgs);
    }

    public virtual bool CanExecute(object parameter) => true;

    protected void OnCanExecuteChanged()
    {
      if (this.CanExecuteChanged == null)
        return;
      this.CanExecuteChanged((object) this, new EventArgs());
    }
  }
}
