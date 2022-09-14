// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.Validation.IModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Models.Validation
{
  public interface IModel
  {
    IList<string> Errors { get; }

    Action<IModel> Validator { get; set; }

    bool IsValid { get; }

    bool IsDirty { get; }

    bool Validate();

    void Revert();
  }
}
