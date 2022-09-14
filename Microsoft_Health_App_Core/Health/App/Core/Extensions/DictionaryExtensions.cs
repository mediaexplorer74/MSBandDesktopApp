// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.DictionaryExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class DictionaryExtensions
  {
    public static string ToDebugString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
    {
      StringBuilder stringBuilder = new StringBuilder("{ ");
      stringBuilder.Append(string.Join(", ", dictionary.Select<KeyValuePair<TKey, TValue>, string>((Func<KeyValuePair<TKey, TValue>, string>) (kv => kv.Key.ToString() + " = " + ((object) kv.Value).ToDebugString())).ToArray<string>()));
      stringBuilder.Append(" }");
      return stringBuilder.ToString();
    }

    public static TValue GetValueOrDefault<TKey, TValue>(
      this IDictionary<TKey, TValue> dictionary,
      TKey key,
      TValue defaultValue = null)
    {
      Assert.ParamIsNotNull((object) dictionary, nameof (dictionary));
      Assert.ParamIsNotNull((object) key, nameof (key));
      TValue obj;
      return !dictionary.TryGetValue(key, out obj) ? defaultValue : obj;
    }
  }
}
