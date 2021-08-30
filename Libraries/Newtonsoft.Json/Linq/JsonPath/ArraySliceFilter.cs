// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.ArraySliceFilter
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Newtonsoft.Json.Linq.JsonPath
{
  internal class ArraySliceFilter : PathFilter
  {
    public int? Start { get; set; }

    public int? End { get; set; }

    public int? Step { get; set; }

    public override IEnumerable<JToken> ExecuteFilter(
      IEnumerable<JToken> current,
      bool errorWhenNoMatch)
    {
      int? step = this.Step;
      if ((step.GetValueOrDefault() != 0 ? 0 : (step.HasValue ? 1 : 0)) != 0)
        throw new JsonException("Step cannot be zero.");
      foreach (JToken t in current)
      {
        if (t is JArray a1)
        {
          int stepCount = this.Step ?? 1;
          int startIndex = this.Start ?? (stepCount > 0 ? 0 : a1.Count - 1);
          int stopIndex = this.End ?? (stepCount > 0 ? a1.Count : -1);
          int? start = this.Start;
          if ((start.GetValueOrDefault() >= 0 ? 0 : (start.HasValue ? 1 : 0)) != 0)
            startIndex = a1.Count + startIndex;
          int? end = this.End;
          if ((end.GetValueOrDefault() >= 0 ? 0 : (end.HasValue ? 1 : 0)) != 0)
            stopIndex = a1.Count + stopIndex;
          startIndex = Math.Max(startIndex, stepCount > 0 ? 0 : int.MinValue);
          startIndex = Math.Min(startIndex, stepCount > 0 ? a1.Count : a1.Count - 1);
          stopIndex = Math.Max(stopIndex, -1);
          stopIndex = Math.Min(stopIndex, a1.Count);
          bool positiveStep = stepCount > 0;
          if (this.IsValid(startIndex, stopIndex, positiveStep))
          {
            for (int i = startIndex; this.IsValid(i, stopIndex, positiveStep); i += stepCount)
              yield return a1[i];
          }
          else if (errorWhenNoMatch)
            throw new JsonException("Array slice of {0} to {1} returned no results.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, this.Start.HasValue ? (object) this.Start.Value.ToString((IFormatProvider) CultureInfo.InvariantCulture) : (object) "*", this.End.HasValue ? (object) this.End.Value.ToString((IFormatProvider) CultureInfo.InvariantCulture) : (object) "*"));
        }
        else if (errorWhenNoMatch)
          throw new JsonException("Array slice is not valid on {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) t.GetType().Name));
      }
    }

    private bool IsValid(int index, int stopIndex, bool positiveStep) => positiveStep ? index < stopIndex : index > stopIndex;
  }
}
