// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.ObjectExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Collections;
using System.Text;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class ObjectExtensions
  {
    private const string KeyValueFormat = "{0} = {1}";
    private const string EnumerableFormat = "Enumerable{{ {0}{1} }}";
    private const string EnumerableWithNotShownItemsFormat = "Enumerable{{ {0}{1}, {2} more enumerable items... }}";
    private const string EnumerableWithOneItemFormat = "Enumerable{{ {0} }}";
    private const string EnumerableWithNoItems = "Enumerable{{}}";
    private const string Format = "{0}{{ {1} }}";
    private const string Null = "null";

    public static string ToDebugString(this object o, int amountOfEnumerableItemsToShowIfPresent = 10) => string.Empty;

    public static string ToDebugString(
      this IEnumerable enumerable,
      int amountOfItemsToShowIfPresent = 10)
    {
      IEnumerator enumerator = enumerable.GetEnumerator();
      int num1 = 0;
      int num2 = 0;
      if (!enumerator.MoveNext())
        return string.Format("Enumerable{{}}");
      object current = enumerator.Current;
      int num3 = num1 + 1;
      if (enumerator.MoveNext())
      {
        StringBuilder stringBuilder = new StringBuilder();
        do
        {
          if (num3 <= amountOfItemsToShowIfPresent)
          {
            stringBuilder.AppendFormat(", {0}", new object[1]
            {
              (object) enumerator.Current.ToDebugString(amountOfItemsToShowIfPresent)
            });
            ++num3;
          }
          else
            ++num2;
        }
        while (enumerator.MoveNext());
        return num2 > 0 ? string.Format("Enumerable{{ {0}{1}, {2} more enumerable items... }}", new object[3]
        {
          (object) current.ToDebugString(),
          (object) stringBuilder,
          (object) num2
        }) : string.Format("Enumerable{{ {0}{1} }}", new object[2]
        {
          (object) current.ToDebugString(),
          (object) stringBuilder
        });
      }
      return string.Format("Enumerable{{ {0} }}", new object[1]
      {
        (object) current.ToDebugString(amountOfItemsToShowIfPresent)
      });
    }
  }
}
