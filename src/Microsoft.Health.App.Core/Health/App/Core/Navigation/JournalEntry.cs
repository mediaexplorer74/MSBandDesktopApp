// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Navigation.JournalEntry
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.App.Core.Navigation
{
  [DataContract]
  public class JournalEntry
  {
    public JournalEntry() => this.Id = Guid.NewGuid();

    public Guid Id { get; }

    public object ViewModel { get; set; }

    public object Page { get; set; }

    public Type ViewModelType { get; set; }

    public bool IsPageCached { get; set; }

    public IDictionary<string, string> Arguments { get; set; }

    public void ClearCachedData()
    {
      this.ViewModel = (object) null;
      this.Page = (object) null;
      this.IsPageCached = false;
    }
  }
}
