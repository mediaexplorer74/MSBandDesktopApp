// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.CloudEnvironment
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.Utilities
{
  public static class CloudEnvironment
  {
    public const string Custom = "Custom";
    private static readonly StringComparer EnvironmentNameComparer = StringComparer.OrdinalIgnoreCase;
    private static readonly IReadOnlyDictionary<string, string> EnvironmentUrls = (IReadOnlyDictionary<string, string>) new Dictionary<string, string>((IEqualityComparer<string>) CloudEnvironment.EnvironmentNameComparer)
    {
      {
        "KMAIN",
        "https://kmain-kds-eus2-0.cloudapp.net/"
      },
      {
        "Prod",
        "https://prodkds.dns-cargo.com/"
      },
      {
        "Stg",
        "https://stgkds.dns-cargo.com/"
      },
      {
        "Int",
        "https://intkds.dns-cargo.com/"
      }
    };
    public static readonly IList<string> EnvironmentNames = (IList<string>) CloudEnvironment.EnvironmentUrls.Keys.OrderBy<string, string>((Func<string, string>) (k => k)).Union<string>((IEnumerable<string>) new string[1]
    {
      nameof (Custom)
    }).ToArray<string>();

    public static string Default => "Prod";

    public static string GetUrl(string environment)
    {
      string str;
      CloudEnvironment.EnvironmentUrls.TryGetValue(environment, out str);
      return str;
    }

    public static bool IsValid(string environment)
    {
      if (string.IsNullOrEmpty(environment))
        return false;
      return CloudEnvironment.EnvironmentUrls.ContainsKey(environment) || CloudEnvironment.IsCustom(environment);
    }

    public static bool IsCustom(string environment) => CloudEnvironment.EnvironmentNameComparer.Equals(environment, "Custom");
  }
}
