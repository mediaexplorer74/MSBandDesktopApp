// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.ILauncherService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public interface ILauncherService
  {
    Task<IList<EmailAddress>> PromptEmailAsync();

    void ShowUserWebBrowser(Uri uri);

    void CallPhoneNumber(string phoneNumber);

    void MapAddress(params string[] addressLines);

    void MapAddress(IEnumerable<string> addressLines);

    void MapGeoposition(double latitude, double longitude, string poiTitle, int zoomLevel = 15);
  }
}
