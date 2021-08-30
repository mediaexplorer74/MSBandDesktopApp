// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.Utility.Pair
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

namespace Microsoft.Practices.Unity.Utility
{
  public static class Pair
  {
    public static Pair<TFirstParameter, TSecondParameter> Make<TFirstParameter, TSecondParameter>(
      TFirstParameter first,
      TSecondParameter second)
    {
      return new Pair<TFirstParameter, TSecondParameter>(first, second);
    }
  }
}
