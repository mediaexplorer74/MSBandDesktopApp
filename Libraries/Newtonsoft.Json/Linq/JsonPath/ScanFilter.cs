// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.ScanFilter
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
  internal class ScanFilter : PathFilter
  {
    public string Name { get; set; }

    public override IEnumerable<JToken> ExecuteFilter(
      IEnumerable<JToken> current,
      bool errorWhenNoMatch)
    {
      using (IEnumerator<JToken> enumerator = current.GetEnumerator())
      {
label_17:
        while (enumerator.MoveNext())
        {
          JToken root = enumerator.Current;
          if (this.Name == null)
            yield return root;
          JToken value = root;
          JToken container = root;
          while (true)
          {
            if (container != null && container.HasValues)
            {
              value = container.First;
            }
            else
            {
              while (value != null && value != root && value == value.Parent.Last)
                value = (JToken) value.Parent;
              if (value != null && value != root)
                value = value.Next;
              else
                goto label_17;
            }
            if (value is JProperty e6)
            {
              if (e6.Name == this.Name)
                yield return e6.Value;
            }
            else if (this.Name == null)
              yield return value;
            container = (JToken) (value as JContainer);
          }
        }
      }
    }
  }
}
