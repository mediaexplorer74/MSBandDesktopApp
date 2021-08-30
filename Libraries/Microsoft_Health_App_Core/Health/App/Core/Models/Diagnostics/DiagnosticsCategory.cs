// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.Diagnostics.DiagnosticsCategory
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Health.App.Core.Models.Diagnostics
{
  [JsonConverter(typeof (StringEnumConverter))]
  public enum DiagnosticsCategory
  {
    Golf,
  }
}
