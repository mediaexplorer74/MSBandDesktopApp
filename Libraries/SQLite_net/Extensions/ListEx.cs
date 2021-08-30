// Decompiled with JetBrains decompiler
// Type: SQLite.Extensions.ListEx
// Assembly: SQLite-net, Version=1.0.9.0, Culture=neutral, PublicKeyToken=null
// MVID: 3C087C9E-3E7A-4EB5-8100-289515B40443
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\SQLite_net.dll

using System;
using System.Collections.Generic;

namespace SQLite.Extensions
{
  public static class ListEx
  {
    public static bool TryAdd<TKey, TValue>(
      this IDictionary<TKey, TValue> dict,
      TKey key,
      TValue value)
    {
      try
      {
        dict.Add(key, value);
        return true;
      }
      catch (ArgumentException ex)
      {
        return false;
      }
    }
  }
}
