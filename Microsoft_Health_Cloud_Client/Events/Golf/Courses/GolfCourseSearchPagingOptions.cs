// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Events.Golf.Courses.GolfCourseSearchPagingOptions
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;

namespace Microsoft.Health.Cloud.Client.Events.Golf.Courses
{
  public sealed class GolfCourseSearchPagingOptions
  {
    private readonly int pageNumber;
    private readonly int resultsPerPage;

    public GolfCourseSearchPagingOptions(int pageNumber, int resultsPerPage)
    {
      if (pageNumber < 1)
        throw new ArgumentOutOfRangeException(nameof (pageNumber));
      if (resultsPerPage < 1)
        throw new ArgumentOutOfRangeException(nameof (resultsPerPage));
      this.pageNumber = pageNumber;
      this.resultsPerPage = resultsPerPage;
    }

    public int PageNumber => this.pageNumber;

    public int ResultsPerPage => this.resultsPerPage;
  }
}
