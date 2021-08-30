// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.BooleanQueryExpression
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
  internal class BooleanQueryExpression : QueryExpression
  {
    public List<PathFilter> Path { get; set; }

    public JValue Value { get; set; }

    public override bool IsMatch(JToken t)
    {
      foreach (JToken jtoken in JPath.Evaluate(this.Path, t, false))
      {
        JValue jvalue = jtoken as JValue;
        switch (this.Operator)
        {
          case QueryOperator.Equals:
            if (jvalue != null && jvalue.Equals(this.Value))
              return true;
            continue;
          case QueryOperator.NotEquals:
            if (jvalue != null && !jvalue.Equals(this.Value))
              return true;
            continue;
          case QueryOperator.Exists:
            return true;
          case QueryOperator.LessThan:
            if (jvalue != null && jvalue.CompareTo(this.Value) < 0)
              return true;
            continue;
          case QueryOperator.LessThanOrEquals:
            if (jvalue != null && jvalue.CompareTo(this.Value) <= 0)
              return true;
            continue;
          case QueryOperator.GreaterThan:
            if (jvalue != null && jvalue.CompareTo(this.Value) > 0)
              return true;
            continue;
          case QueryOperator.GreaterThanOrEquals:
            if (jvalue != null && jvalue.CompareTo(this.Value) >= 0)
              return true;
            continue;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      return false;
    }
  }
}
