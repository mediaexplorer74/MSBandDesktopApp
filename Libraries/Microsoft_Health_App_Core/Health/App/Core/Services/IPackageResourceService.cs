// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.IPackageResourceService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Band.Personalization;
using Microsoft.Band.Tiles;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Themes;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public interface IPackageResourceService
  {
    Task<TileLayout> LoadLayoutAsync(
      string layoutName,
      BandClass bandClass,
      CancellationToken token = default (CancellationToken));

    Task<BandIcon> LoadIconAsync(
      AppBandIcon icon,
      BandClass bandClass,
      CancellationToken token = default (CancellationToken));

    ResourceIdentifier GetWallpaperResourceId(AppBandTheme theme);

    Task<BandImage> LoadWallpaperAsync(AppBandTheme theme, CancellationToken token = default (CancellationToken));

    Task<Stream> OpenDynamicConfigAsync(string configurationName, CancellationToken token);
  }
}
