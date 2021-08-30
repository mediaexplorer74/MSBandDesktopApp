// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Diagnostics.PerfLogger
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Logging.Framework;
using System.Text;

namespace Microsoft.Health.App.Core.Diagnostics
{
  public class PerfLogger : IPerfLogger
  {
    private const string Delimiter = "|";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Diagnostics\\PerfLogger.cs");

    public void Mark(string perfEvent, string attribute, params string[] values)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(perfEvent);
      stringBuilder.Append("|");
      stringBuilder.Append(attribute);
      foreach (string str in values)
      {
        stringBuilder.Append("|");
        stringBuilder.Append(str.Replace("\\", "\\\\").Replace("|", "\\|"));
      }
      PerfLogger.Logger.Debug((object) stringBuilder.ToString());
    }
  }
}
