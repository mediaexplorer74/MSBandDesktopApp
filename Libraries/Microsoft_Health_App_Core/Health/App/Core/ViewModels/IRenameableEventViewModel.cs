// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.IRenameableEventViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  public interface IRenameableEventViewModel
  {
    event EventHandler RenameRequested;

    bool DisplayNamingTextBox { get; }

    ICommand AssignNameCommand { get; }

    string Name { get; set; }

    bool IsBeingEdited { get; }
  }
}
