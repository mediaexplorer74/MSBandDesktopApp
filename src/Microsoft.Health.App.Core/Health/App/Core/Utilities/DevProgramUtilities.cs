// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.DevProgramUtilities
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Utilities
{
  public static class DevProgramUtilities
  {
    private static readonly Version MinVersion = new Version("8.10.14157");

    public static bool NeedsUpdate(Version version) => version < DevProgramUtilities.MinVersion;
  }
}
