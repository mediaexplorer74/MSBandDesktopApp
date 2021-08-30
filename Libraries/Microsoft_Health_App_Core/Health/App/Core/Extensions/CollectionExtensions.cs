// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.CollectionExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class CollectionExtensions
  {
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) => enumerable == null || !enumerable.Any<T>();
  }
}
