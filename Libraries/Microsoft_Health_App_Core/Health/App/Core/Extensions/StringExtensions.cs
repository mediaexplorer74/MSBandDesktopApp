// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.StringExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

namespace Microsoft.Health.App.Core.Extensions
{
  public static class StringExtensions
  {
    public static string ForceLengthPadLeft(this string s, int characters)
    {
      if (s.Length < characters)
        return s.PadLeft(characters);
      return s.Length > characters ? s.Substring(0, characters) : s;
    }

    public static string Truncate(this string s, int length) => s.Length <= length ? s : s.Substring(0, length);
  }
}
