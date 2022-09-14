// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.LengthExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class LengthExtensions
  {
    public static Length Sum(this IEnumerable<Length> lengths)
    {
      Assert.ParamIsNotNull((object) lengths, nameof (lengths));
      return lengths.Aggregate<Length, Length>(Length.Zero, (Func<Length, Length, Length>) ((x, y) => x + y));
    }

    public static Length Sum<T>(this IEnumerable<T> items, Func<T, Length> selector)
    {
      Assert.ParamIsNotNull((object) items, nameof (items));
      return items.Select<T, Length>(selector).Sum();
    }
  }
}
