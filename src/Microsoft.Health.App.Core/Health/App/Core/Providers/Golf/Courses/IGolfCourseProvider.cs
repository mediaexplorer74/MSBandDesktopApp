// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.Golf.Courses.IGolfCourseProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Providers.Golf.Courses
{
  public interface IGolfCourseProvider
  {
    Task<IReadOnlyList<GolfCourseLocality>> GetRegionsAsync(
      CancellationToken token);

    Task<IReadOnlyList<GolfCourseLocality>> GetStatesAsync(
      string regionId,
      CancellationToken token);

    Task<IReadOnlyList<GolfCourseSummary>> GetCoursesByLocationAsync(
      double latitude,
      double longitude,
      int pageNumber,
      int resultsPerPage,
      CancellationToken token,
      Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseType? type = null,
      int? numberOfHoles = null);

    Task<IReadOnlyList<GolfCourseSummary>> GetCoursesByStateAsync(
      string stateId,
      CancellationToken token,
      Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseType? type = null,
      int? numberOfHoles = null,
      int? pageNumber = null,
      int? resultsPerPage = null);

    Task<IReadOnlyList<GolfCourseSummary>> GetCoursesByNameAsync(
      string query,
      int pageNumber,
      int resultsPerPage,
      CancellationToken token,
      Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseType? type = null,
      int? numberOfHoles = null);

    Task<IReadOnlyList<GolfCourseSummary>> GetRecentCoursesAsync(
      CancellationToken token);

    Task<GolfCourseDetails> GetCourseDetailsAsync(
      string courseId,
      CancellationToken token);

    Task<Stream> GetGolfCourseFileAsync(
      string courseId,
      string teeId,
      CancellationToken token);
  }
}
