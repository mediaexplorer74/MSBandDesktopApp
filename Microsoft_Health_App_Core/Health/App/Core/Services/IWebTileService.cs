// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.IWebTileService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Band.Admin.WebTiles;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public interface IWebTileService
  {
    string WebTileFileName { get; set; }

    Uri WebTileUri { get; set; }

    IWebTile CurrentWebTile { get; }

    IList<IWebTileResource> ResourcesRequiringAuthentication { get; }

    IWebTileManager GetWebTileManager { get; }

    Task LoadWebTileFromCurrentPackageAsync(CancellationToken cancellationToken);

    Task ClearWebTileAsync(CancellationToken cancellationToken);

    Task LoadResourcesRequiringAuthenticationAsync(CancellationToken cancellationToken);

    Task InstallWebTileAsync(CancellationToken cancellationToken);

    Task<AdminBandTile> GetAdminBandTileFromWebTileAsync(
      CancellationToken cancellationToken,
      BandClass bandClass);

    Task DeleteAllStoredResourceCredentialsAsync();
  }
}
