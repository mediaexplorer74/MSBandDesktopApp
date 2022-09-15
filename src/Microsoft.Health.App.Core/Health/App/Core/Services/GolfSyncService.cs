// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.GolfSyncService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Providers.Golf.Courses;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class GolfSyncService : IGolfSyncService
  {
    private const string LastSyncedCourseKey = "GolfSyncService.LastSyncedCourse";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\GolfSyncService.cs");
    private readonly IGolfCourseProvider golfCourseProvider;
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly IConfigProvider configProvider;
    private readonly IDateTimeService dateTimeService;

    public GolfSyncService(
      IGolfCourseProvider golfCourseProvider,
      IBandConnectionFactory cargoConnectionFactory,
      IConfigProvider configProvider,
      IDateTimeService dateTimeService)
    {
      this.golfCourseProvider = golfCourseProvider;
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.configProvider = configProvider;
      this.dateTimeService = dateTimeService;
    }

    public bool IsCourseOnBand(string courseId)
    {
      SyncedGolfCourse lastSyncedCourse = this.LastSyncedCourse;
      return lastSyncedCourse != null && lastSyncedCourse.CourseId == courseId;
    }

    public async Task SyncCourseToBandAsync(
      string courseId,
      string teeId,
      CancellationToken token)
    {
      GolfSyncService.Logger.Debug((object) "Starting sync.");
      using (ApplicationTelemetry.TimeGolfCourseSync(courseId, teeId))
      {
        using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(token))
        {
          using (Stream courseFile = await this.golfCourseProvider.GetGolfCourseFileAsync(courseId, teeId, token))
          {
            GolfSyncService.Logger.Debug((object) "Finished getting course bin file from cloud, starting to push to device.");
            await cargoConnection.SendGolfCourseToBandAsync(courseFile, token).ConfigureAwait(false);
            GolfSyncService.Logger.Debug((object) "Successfully pushed course bin file to device.");
          }
          await cargoConnection.UpdateGpsEphemerisDataAsync(token, false).ConfigureAwait(false);
        }
        GolfSyncService.Logger.Debug((object) "Successfully updated ephemeris file.");
      }
      this.LastSyncedCourse = new SyncedGolfCourse()
      {
        CourseId = courseId,
        TeeId = teeId,
        Timestamp = this.dateTimeService.Now
      };
    }

    public SyncedGolfCourse LastSyncedCourse
    {
      get => this.configProvider.GetComplexValue<SyncedGolfCourse>("GolfSyncService.LastSyncedCourse", (SyncedGolfCourse) null);
      private set => this.configProvider.SetComplexValue<SyncedGolfCourse>("GolfSyncService.LastSyncedCourse", value);
    }

    public void ClearLastSyncedCourse() => this.LastSyncedCourse = (SyncedGolfCourse) null;
  }
}
