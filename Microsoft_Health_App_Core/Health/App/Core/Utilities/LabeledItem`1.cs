// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.LabeledItem`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.Utilities
{
  public sealed class LabeledItem<T>
  {
    public LabeledItem(T value, string label)
    {
      this.Value = value;
      this.Label = label;
    }

    public T Value { get; private set; }

    public string Label { get; private set; }

    public static LabeledItem<T> Find(T value, IEnumerable<LabeledItem<T>> list)
    {
      using (IEnumerator<LabeledItem<T>> enumerator = list.Where<LabeledItem<T>>((Func<LabeledItem<T>, bool>) (selectableItem => value.Equals((object) selectableItem.Value))).GetEnumerator())
      {
        if (enumerator.MoveNext())
          return enumerator.Current;
      }
      DebugUtilities.Fail("value does not exist in list");
      return (LabeledItem<T>) null;
    }

    public static IList<LabeledItem<T>> FromEnumValues() => (IList<LabeledItem<T>>) ((IEnumerable<T>) Enum.GetValues(typeof (T))).Select<T, LabeledItem<T>>(new Func<T, LabeledItem<T>>(LabeledItem<T>.FromEnumValue)).ToList<LabeledItem<T>>();

    public static LabeledItem<T> FromEnumValue(T value)
    {
      string resource = EnumUtilities.GetResource<T>(value);
      return new LabeledItem<T>(value, resource);
    }

    public override string ToString() => this.Label;
  }
}
