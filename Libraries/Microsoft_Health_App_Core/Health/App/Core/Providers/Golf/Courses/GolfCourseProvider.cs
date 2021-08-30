// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.Golf.Courses.GolfCourseProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Events.Golf;
using Microsoft.Health.Cloud.Client.Events.Golf.Courses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Providers.Golf.Courses
{
  internal sealed class GolfCourseProvider : IGolfCourseProvider
  {
    private readonly IGolfCourseClient golfCourseClient;
    private readonly IGolfClient golfClient;
    private readonly IEnvironmentService environmentService;

    public GolfCourseProvider(
      IGolfCourseClient golfCourseClient,
      IGolfClient golfClient,
      IEnvironmentService environmentService)
    {
      Assert.ParamIsNotNull((object) golfCourseClient, nameof (golfCourseClient));
      Assert.ParamIsNotNull((object) golfClient, nameof (golfClient));
      Assert.ParamIsNotNull((object) environmentService, nameof (environmentService));
      this.golfCourseClient = golfCourseClient;
      this.golfClient = golfClient;
      this.environmentService = environmentService;
    }

    public async Task<IReadOnlyList<GolfCourseLocality>> GetRegionsAsync(
      CancellationToken token)
    {
      return (IReadOnlyList<GolfCourseLocality>) (await this.golfCourseClient.GetRegionsAsync(token).ConfigureAwait(false)).Regions.Select<Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseLocality, GolfCourseLocality>((Func<Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseLocality, GolfCourseLocality>) (region => new GolfCourseLocality(region.Id.ToString((IFormatProvider) CultureInfo.InvariantCulture), region.Name, region.NumberOfSubregions, region.NumberOfCourses))).ToList<GolfCourseLocality>();
    }

    public async Task<IReadOnlyList<GolfCourseLocality>> GetStatesAsync(
      string regionId,
      CancellationToken token)
    {
      return (IReadOnlyList<GolfCourseLocality>) (await this.golfCourseClient.GetStatesAsync(long.Parse(regionId, (IFormatProvider) CultureInfo.InvariantCulture), token).ConfigureAwait(false)).States.Select<Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseLocality, GolfCourseLocality>((Func<Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseLocality, GolfCourseLocality>) (state => new GolfCourseLocality(state.Id.ToString((IFormatProvider) CultureInfo.InvariantCulture), state.Name, state.NumberOfSubregions, state.NumberOfCourses))).ToList<GolfCourseLocality>();
    }

    public async Task<IReadOnlyList<GolfCourseSummary>> GetCoursesByLocationAsync(
      double latitude,
      double longitude,
      int pageNumber,
      int resultsPerPage,
      CancellationToken token,
      Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseType? type = null,
      int? numberOfHoles = null)
    {
      return (IReadOnlyList<GolfCourseSummary>) (await this.golfCourseClient.GetCoursesByLocationAsync(latitude, longitude, token, new GolfCourseSearchPagingOptions(pageNumber, resultsPerPage), new GolfCourseSearchFilterOptions(type, numberOfHoles), !this.environmentService.IsPublicRelease).ConfigureAwait(false)).Facilities.Select<Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseSummary, GolfCourseSummary>((Func<Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseSummary, GolfCourseSummary>) (facility => GolfCourseProvider.CreateSummary(facility))).ToList<GolfCourseSummary>();
    }

    public async Task<IReadOnlyList<GolfCourseSummary>> GetCoursesByStateAsync(
      string stateId,
      CancellationToken token,
      Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseType? type = null,
      int? numberOfHoles = null,
      int? pageNumber = null,
      int? resultsPerPage = null)
    {
      GolfCourseSearchPagingOptions pagingOptions = (GolfCourseSearchPagingOptions) null;
      if (pageNumber.HasValue || resultsPerPage.HasValue)
      {
        if (!pageNumber.HasValue || !resultsPerPage.HasValue)
          throw new ArgumentException("Page number and results per page are required.");
        pagingOptions = new GolfCourseSearchPagingOptions(pageNumber.Value, resultsPerPage.Value);
      }
      return (IReadOnlyList<GolfCourseSummary>) (await this.golfCourseClient.GetCoursesByStateAsync(long.Parse(stateId, (IFormatProvider) CultureInfo.InvariantCulture), token, pagingOptions, new GolfCourseSearchFilterOptions(type, numberOfHoles)).ConfigureAwait(false)).Facilities.Select<Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseSummary, GolfCourseSummary>((Func<Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseSummary, GolfCourseSummary>) (facility => GolfCourseProvider.CreateSummary(facility))).ToList<GolfCourseSummary>();
    }

    public async Task<IReadOnlyList<GolfCourseSummary>> GetCoursesByNameAsync(
      string query,
      int pageNumber,
      int resultsPerPage,
      CancellationToken token,
      Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseType? type = null,
      int? numberOfHoles = null)
    {
      return (IReadOnlyList<GolfCourseSummary>) (await this.golfCourseClient.GetCoursesByNameAsync(query, token, new GolfCourseSearchPagingOptions(pageNumber, resultsPerPage), new GolfCourseSearchFilterOptions(type, numberOfHoles)).ConfigureAwait(false)).Facilities.Select<Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseSummary, GolfCourseSummary>((Func<Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseSummary, GolfCourseSummary>) (facility => GolfCourseProvider.CreateSummary(facility))).ToList<GolfCourseSummary>();
    }

    public async Task<IReadOnlyList<GolfCourseSummary>> GetRecentCoursesAsync(
      CancellationToken token)
    {
      return (IReadOnlyList<GolfCourseSummary>) (await this.golfCourseClient.GetRecentCoursesAsync(token).ConfigureAwait(false)).Facilities.Select<Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseSummary, GolfCourseSummary>((Func<Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseSummary, GolfCourseSummary>) (facility => GolfCourseProvider.CreateSummary(facility))).ToList<GolfCourseSummary>();
    }

    public async Task<GolfCourseDetails> GetCourseDetailsAsync(
      string courseId,
      CancellationToken token)
    {
      Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseDetails courseDetailsAsync = await this.golfCourseClient.GetCourseDetailsAsync(long.Parse(courseId, (IFormatProvider) CultureInfo.InvariantCulture), token);
      return courseDetailsAsync != null ? GolfCourseProvider.CreateDetails(courseDetailsAsync) : (GolfCourseDetails) null;
    }

    public Task<Stream> GetGolfCourseFileAsync(
      string courseId,
      string teeId,
      CancellationToken token)
    {
      return this.golfClient.GetGolfCourseFileAsync(courseId, teeId, token);
    }

    private static GolfCourseSummary CreateSummary(Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseSummary facility)
    {
      Uri result = (Uri) null;
      if (!Uri.TryCreate(facility.Website, UriKind.Absolute, out result))
        result = (Uri) null;
      string courseId = facility.CourseId.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      string name = facility.Name;
      int courseType = (int) GolfCourseProvider.ToCourseType(facility.CourseType);
      int numberOfHoles = facility.NumberOfHoles;
      string city = facility.City;
      string state = facility.State;
      string country = facility.Country;
      string displayAddress = facility.DisplayAddress;
      string displayAddress2 = facility.DisplayAddress2;
      string displayAddress3 = facility.DisplayAddress3;
      string phoneNumber = facility.PhoneNumber;
      Uri websiteUrl = result;
      double? distance1 = facility.Distance;
      Length? distance2;
      if (!distance1.HasValue)
      {
        distance2 = new Length?();
      }
      else
      {
        distance1 = facility.Distance;
        distance2 = new Length?(Length.FromCentimeters(distance1.Value));
      }
      return new GolfCourseSummary(courseId, name, (GolfCourseType) courseType, numberOfHoles, city, state, country, displayAddress, displayAddress2, displayAddress3, phoneNumber, websiteUrl, distance2);
    }

    private static GolfCourseDetails CreateDetails(Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseDetails details)
    {
      Uri result = (Uri) null;
      if (!Uri.TryCreate(details.Website, UriKind.Absolute, out result))
        result = (Uri) null;
      return new GolfCourseDetails(details.CourseId.ToString((IFormatProvider) CultureInfo.InvariantCulture), details.Name, GolfCourseProvider.ToCourseType(details.CourseType), details.NumberOfHoles, details.City, details.State, details.Country, details.DisplayAddress, details.DisplayAddress2, details.DisplayAddress3, details.PhoneNumber, result, details.Holes.Select<Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseHole, GolfCourseHole>((Func<Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseHole, GolfCourseHole>) (hole => GolfCourseProvider.CreateHole(hole))), details.Tees.Select<Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseTee, GolfCourseTee>((Func<Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseTee, GolfCourseTee>) (tee => GolfCourseProvider.CreateTee(tee))));
    }

    private static GolfCourseHole CreateHole(Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseHole hole) => new GolfCourseHole(hole.Id.ToString((IFormatProvider) CultureInfo.InvariantCulture), hole.Name, hole.Tees.Select<Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseHoleTee, GolfCourseHoleTee>((Func<Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseHoleTee, GolfCourseHoleTee>) (tee => GolfCourseProvider.CreateHoleTee(tee))));

    private static GolfCourseHoleTee CreateHoleTee(Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseHoleTee tee) => new GolfCourseHoleTee(tee.Id.ToString((IFormatProvider) CultureInfo.InvariantCulture), tee.Name, tee.Par, Length.FromYards(tee.Yards), tee.StrokeIndex, tee.Default);

    private static GolfCourseTee CreateTee(Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseTee tee) => new GolfCourseTee(tee.Id.ToString((IFormatProvider) CultureInfo.InvariantCulture), tee.Name, tee.Par, Length.FromYards(tee.Yards), tee.Rating, tee.Slope, tee.Default);

    private static GolfCourseType ToCourseType(Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseType courseType)
    {
      switch (courseType)
      {
        case Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseType.Public:
          return GolfCourseType.Public;
        case Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseType.Private:
          return GolfCourseType.Private;
        default:
          return GolfCourseType.Unknown;
      }
    }
  }
}
