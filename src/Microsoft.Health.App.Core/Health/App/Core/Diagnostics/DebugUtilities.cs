// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Diagnostics.DebugUtilities
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Diagnostics;

namespace Microsoft.Health.App.Core.Diagnostics
{
  public static class DebugUtilities
  {
    public static void Fail(string message, params object[] args) => DebugUtilities.Assert(false, message, args);

    public static void Assert(bool condition, string message, params object[] args)
    {
      if (condition || !Debugger.IsAttached)
        return;
      Debugger.Break();
    }

    private static string Format(string message, object[] args) => args == null || args.Length == 0 ? message : string.Format(message, args);
  }
}
