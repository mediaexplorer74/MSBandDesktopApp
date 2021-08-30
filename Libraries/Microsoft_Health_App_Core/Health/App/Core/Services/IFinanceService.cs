// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.IFinanceService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Band;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public interface IFinanceService
  {
    Task<IList<Stock>> GetStockInformationAsync(
      IList<string> stockIds,
      CancellationToken cancellationToken);

    Task<IList<Stock>> SearchStocksAsync(
      string query,
      CancellationToken cancellationToken);
  }
}
