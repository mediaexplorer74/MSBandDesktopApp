// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Pedometer.ISensorUploader
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Pedometer
{
  public interface ISensorUploader : IConfigurationState
  {
    Task<DateTimeOffset> GetLastCompleteReadingEndTimeAsync(
      TimeSpan duration,
      CancellationToken cancellationToken);

    Task UploadReadingsAsync(
      IEnumerable<SensorReading> readings,
      TimeSpan duration,
      IDictionary<string, string> uploadMetadata,
      CancellationToken cancellationToken);
  }
}
