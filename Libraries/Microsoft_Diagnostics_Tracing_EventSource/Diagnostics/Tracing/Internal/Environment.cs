// Decompiled with JetBrains decompiler
// Type: Microsoft.Diagnostics.Tracing.Internal.Environment
// Assembly: Microsoft.Diagnostics.Tracing.EventSource, Version=1.1.16.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 5BB68207-0B7F-412A-8836-4E370F261506
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Diagnostics_Tracing_EventSource.dll

using Microsoft.Reflection;
using System.Resources;

namespace Microsoft.Diagnostics.Tracing.Internal
{
  internal static class Environment
  {
    public static readonly string NewLine = System.Environment.NewLine;
    private static ResourceManager rm = new ResourceManager("Microsoft.Diagnostics.Tracing.Messages", ReflectionExtensions.Assembly(typeof (Environment)));

    public static int TickCount => System.Environment.TickCount;

    public static string GetResourceString(string key, params object[] args)
    {
      string format = Environment.rm.GetString(key);
      if (format != null)
        return string.Format(format, args);
      string empty = string.Empty;
      foreach (object obj in args)
      {
        if (empty != string.Empty)
          empty += ", ";
        empty += obj.ToString();
      }
      return key + " (" + empty + ")";
    }

    public static string GetRuntimeResourceString(string key, params object[] args) => Environment.GetResourceString(key, args);
  }
}
