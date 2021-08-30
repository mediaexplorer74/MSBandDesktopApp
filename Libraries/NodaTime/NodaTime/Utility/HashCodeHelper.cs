// Decompiled with JetBrains decompiler
// Type: NodaTime.Utility.HashCodeHelper
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

namespace NodaTime.Utility
{
  internal static class HashCodeHelper
  {
    private const int HashcodeMultiplier = 37;
    private const int HashcodeInitializer = 17;

    internal static int Initialize() => 17;

    internal static int Hash<T>(int code, T value)
    {
      int num = 0;
      if ((object) value != null)
        num = value.GetHashCode();
      return HashCodeHelper.MakeHash(code, num);
    }

    private static int MakeHash(int code, int value)
    {
      code = code * 37 + value;
      return code;
    }
  }
}
