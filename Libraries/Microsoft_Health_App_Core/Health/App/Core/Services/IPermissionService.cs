// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.IPermissionService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public interface IPermissionService
  {
    Task RequestPermissionsAsync(FeaturePermissions feature);

    Task RequestPermissionsAsync(IList<FeaturePermissions> feature);

    Task<bool> CheckPermissionsAsync(FeaturePermissions feature);

    Task<bool> CheckPermissionsAsync(IList<FeaturePermissions> feature);

    FeaturePermissions? GetFeatureMapForTileGuid(Guid tileGuid);

    string GetPermissionError(FeaturePermissions feature);

    string GetPermissionError(IList<FeaturePermissions> feature);
  }
}
