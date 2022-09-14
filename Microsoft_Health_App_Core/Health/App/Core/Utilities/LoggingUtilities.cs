// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.LoggingUtilities
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.Utilities
{
  public static class LoggingUtilities
  {
    public static string DictionaryToString(IDictionary<string, string> dictionary) => dictionary != null ? string.Join(", ", dictionary.Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (m => m.Key + ":" + m.Value)).ToArray<string>()) : string.Empty;
  }
}
