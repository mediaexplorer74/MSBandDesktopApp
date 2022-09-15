// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Configuration.Dynamic.FinanceConfiguration
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Band;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.App.Core.Services.Configuration.Dynamic
{
  [DataContract]
  public class FinanceConfiguration : IFinanceConfiguration
  {
    public FinanceConfiguration()
    {
      this.IsEnabled = true;
      this.ServiceUrl = (Uri) null;
      this.DefaultStockList = (IReadOnlyList<Stock>) new List<Stock>();
    }

    [DataMember(Name = "enabled")]
    public bool IsEnabled { get; private set; }

    [DataMember(Name = "serviceUrl")]
    public Uri ServiceUrl { get; private set; }

    [DataMember(Name = "defaultStockList")]
    public IReadOnlyList<Stock> DefaultStockList { get; private set; }
  }
}
