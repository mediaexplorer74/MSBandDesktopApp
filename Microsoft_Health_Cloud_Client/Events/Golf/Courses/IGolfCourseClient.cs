// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Events.Golf.Courses.IGolfCourseClient
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client.Events.Golf.Courses
{
  public interface IGolfCourseClient
  {
    Task<GolfCourseRegionSearchResults> GetRegionsAsync(
      CancellationToken token);

    Task<GolfCourseStateSearchResults> GetStatesAsync(
      long regionId,
      CancellationToken token);

    Task<GolfCourseSearchResults> GetRecentCoursesAsync(
      CancellationToken token);

    Task<GolfCourseSearchResults> GetCoursesByLocationAsync(
      double latitude,
      double longitude,
      CancellationToken token,
      GolfCourseSearchPagingOptions pagingOptions = null,
      GolfCourseSearchFilterOptions filterOptions = null,
      bool logFullUrl = true);

    Task<GolfCourseSearchResults> GetCoursesByStateAsync(
      long stateId,
      CancellationToken token,
      GolfCourseSearchPagingOptions pagingOptions = null,
      GolfCourseSearchFilterOptions filterOptions = null);

    Task<GolfCourseSearchResults> GetCoursesByNameAsync(
      string query,
      CancellationToken token,
      GolfCourseSearchPagingOptions pagingOptions = null,
      GolfCourseSearchFilterOptions filterOptions = null);

    Task<GolfCourseDetails> GetCourseDetailsAsync(
      long courseId,
      CancellationToken token);
  }
}
