// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.Utility.ParameterMatcher
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Practices.Unity.Utility
{
  public class ParameterMatcher
  {
    private readonly List<InjectionParameterValue> parametersToMatch;

    public ParameterMatcher(
      IEnumerable<InjectionParameterValue> parametersToMatch)
    {
      this.parametersToMatch = new List<InjectionParameterValue>(parametersToMatch);
    }

    public virtual bool Matches(IEnumerable<Type> candidate)
    {
      List<Type> typeList = new List<Type>(candidate);
      if (this.parametersToMatch.Count != typeList.Count)
        return false;
      for (int index = 0; index < this.parametersToMatch.Count; ++index)
      {
        if (!this.parametersToMatch[index].MatchesType(typeList[index]))
          return false;
      }
      return true;
    }

    public virtual bool Matches(IEnumerable<ParameterInfo> candidate) => this.Matches(candidate.Select<ParameterInfo, Type>((Func<ParameterInfo, Type>) (pi => pi.ParameterType)));
  }
}
