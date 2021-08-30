// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Diagnostics.IDiagnosticsService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models.Diagnostics;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Diagnostics
{
  public interface IDiagnosticsService
  {
    Task InitializeHttpLoggingAsync();

    Task CaptureCrashInformationAsync(Exception error);

    Task<string> GetLastCrashInformationAsync();

    Task CheckAndReportOnLastCrashAsync();

    Task<IFile> CaptureDiagnosisPackageAsync(
      DiagnosticsUserFeedback userFeedback,
      IEnumerable<IFile> imageFiles,
      bool isPublic,
      bool includeLogs);

    Task SendFeedbackEmailAsync(IFile diagnosticsFile, string body = null);

    Task<IFile> CreateDiagnosisFileAsync(
      string packageFileName,
      bool isEmailAttachment,
      CreationCollisionOption collisionOption);
  }
}
