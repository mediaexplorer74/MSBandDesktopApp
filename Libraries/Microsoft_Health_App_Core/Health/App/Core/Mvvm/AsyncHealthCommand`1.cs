// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Mvvm.AsyncHealthCommand`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using System;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Mvvm
{
  public sealed class AsyncHealthCommand<T> : AsyncHealthCommandBase
  {
    private Func<T, Task> onExecute;

    public AsyncHealthCommand(Func<T, Task> onExecute, bool reportCanExecuteChanged = true)
      : base(reportCanExecuteChanged)
    {
      Assert.ParamIsNotNull((object) onExecute, nameof (onExecute));
      this.onExecute = onExecute;
    }

    protected override Task ExecuteAsync(object parameter) => this.onExecute((T) parameter);
  }
}
