// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.IExerciseProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Providers
{
  public interface IExerciseProvider
  {
    Task<IList<ExerciseEvent>> GetExerciseEventsAsync(
      DateTimeOffset startDayId,
      DateTimeOffset endDayId);

    Task<IList<ExerciseEvent>> GetTopExerciseEventsAsync(
      int count,
      bool expand = false);

    Task<ExerciseEvent> GetExerciseEventAsync(string eventId);

    Task<ExerciseEvent> GetLastExerciseEventAsync(bool expand = false);

    Task DeleteExerciseEventAsync(string eventId);

    Task PatchExerciseEventAsync(string eventId, string name);
  }
}
