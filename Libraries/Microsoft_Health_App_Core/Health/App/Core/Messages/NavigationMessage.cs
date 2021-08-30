// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Messages.NavigationMessage
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Navigation;
using System;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Messages
{
  public class NavigationMessage
  {
    public Type ToViewModelType { get; set; }

    public Type FromViewModelType { get; set; }

    public IDictionary<string, string> Arguments { get; set; }

    public bool NavigationCanceledForDuplicate { get; set; }

    public NavigationStackAction StackAction { get; set; }

    public bool IsBack { get; set; }

    public int BackAmount { get; set; }
  }
}
