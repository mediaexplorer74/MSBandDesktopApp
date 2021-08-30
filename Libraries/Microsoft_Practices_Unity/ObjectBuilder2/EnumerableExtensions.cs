// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.EnumerableExtensions
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.ObjectBuilder2
{
  public static class EnumerableExtensions
  {
    public static void ForEach<TItem>(this IEnumerable<TItem> sequence, Action<TItem> action)
    {
      Guard.ArgumentNotNull((object) sequence, nameof (sequence));
      foreach (TItem obj in sequence)
        action(obj);
    }

    public static string JoinStrings<TItem>(
      this IEnumerable<TItem> sequence,
      string separator,
      Func<TItem, string> converter)
    {
      StringBuilder seed = new StringBuilder();
      sequence.Aggregate<TItem, StringBuilder>(seed, (Func<StringBuilder, TItem, StringBuilder>) ((builder, item) =>
      {
        if (builder.Length > 0)
          builder.Append(separator);
        builder.Append(converter(item));
        return builder;
      }));
      return seed.ToString();
    }

    public static string JoinStrings<TItem>(this IEnumerable<TItem> sequence, string separator) => sequence.JoinStrings<TItem>(separator, (Func<TItem, string>) (item => item.ToString()));
  }
}
