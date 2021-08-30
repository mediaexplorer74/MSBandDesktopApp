// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Pedometer.IPedometerManager
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Pedometer
{
  public interface IPedometerManager
  {
    event EventHandler IsEnabledChanged;

    Task<bool> IsAvailableAsync(CancellationToken token);

    Task<bool> IsEnabledAsync(CancellationToken token);

    Task StartBatchReadAsync(CancellationToken token);

    Task<IEnumerable<SensorReading>> GetSensorReadingsAsync(
      IList<DateTimeOffset> range,
      TimeSpan duration,
      CancellationToken token);

    Task EndBatchReadAsync(CancellationToken token);

    Task LaunchSettingsAsync(CancellationToken token);

    Task<bool> CanSetIsEnabledAsync(CancellationToken token);

    Task SetIsEnabledAsync(bool enable, CancellationToken token);

    Task<int?> GetVersionAsync(CancellationToken token);

    string GetSource(CancellationToken token);
  }
}
