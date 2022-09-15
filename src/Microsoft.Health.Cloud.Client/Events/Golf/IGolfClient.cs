// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Events.Golf.IGolfClient
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client.Events.Golf
{
  public interface IGolfClient
  {
    Task<IReadOnlyList<GolfEvent>> GetTopEventsAsync(
      int count,
      CancellationToken token,
      bool expandSequences = false);

    Task<GolfEvent> GetEventAsync(
      string eventId,
      CancellationToken token,
      bool expandSequences = false,
      bool expandInfo = false,
      bool expandEvidences = false);

    Task RenameGolfEventAsync(string eventId, string name, CancellationToken token);

    Task<Stream> GetGolfCourseFileAsync(
      string courseId,
      string teeId,
      CancellationToken token);
  }
}
