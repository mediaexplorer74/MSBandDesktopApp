// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.ReportProblemStore
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models.Diagnostics;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using PCLStorage;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Services
{
  public class ReportProblemStore : IReportProblemStore
  {
    private static readonly ILog Log = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\ReportProblemStore.cs");

    public ReportProblemStore() => this.ImageFiles = (IList<IFile>) new List<IFile>();

    public string Title { get; set; }

    public DiagnosticsUserFeedback UserFeedback { get; set; }

    public IList<IFile> ImageFiles { get; private set; }

    public void Clear()
    {
      this.Title = (string) null;
      this.UserFeedback = (DiagnosticsUserFeedback) null;
      this.ImageFiles.Clear();
    }
  }
}
