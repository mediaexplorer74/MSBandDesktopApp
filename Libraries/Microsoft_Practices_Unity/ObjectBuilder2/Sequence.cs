// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.Sequence
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Utility;
using System.Collections.Generic;

namespace Microsoft.Practices.ObjectBuilder2
{
  public static class Sequence
  {
    public static T[] Collect<T>(params T[] arguments) => arguments;

    public static IEnumerable<Pair<TFirstSequenceElement, TSecondSequenceElement>> Zip<TFirstSequenceElement, TSecondSequenceElement>(
      IEnumerable<TFirstSequenceElement> sequence1,
      IEnumerable<TSecondSequenceElement> sequence2)
    {
      IEnumerator<TFirstSequenceElement> enum1 = sequence1.GetEnumerator();
      IEnumerator<TSecondSequenceElement> enum2 = sequence2.GetEnumerator();
      while (enum1.MoveNext() && enum2.MoveNext())
        yield return new Pair<TFirstSequenceElement, TSecondSequenceElement>(enum1.Current, enum2.Current);
    }
  }
}
