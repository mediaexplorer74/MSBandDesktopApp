// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.LinqExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class LinqExtensions
  {
    public static IEnumerable<IList<T>> SplitByCondition<T>(
      this IEnumerable<T> enumerable,
      Func<T, bool> condition,
      bool includeConditionBreakingItem = false)
    {
      IEnumerator<T> enumerator = enumerable.GetEnumerator();
      while (enumerator.MoveNext())
      {
        if (condition(enumerator.Current))
        {
          List<T> objList = new List<T>()
          {
            enumerator.Current
          };
          while (enumerator.MoveNext())
          {
            if (condition(enumerator.Current))
            {
              objList.Add(enumerator.Current);
            }
            else
            {
              if (includeConditionBreakingItem)
              {
                objList.Add(enumerator.Current);
                break;
              }
              break;
            }
          }
          yield return (IList<T>) objList;
        }
      }
    }

    public static IEnumerable<IList<TU>> SplitByCondition<T, TU>(
      this IEnumerable<T> enumerable,
      Func<T, bool> condition,
      Func<T, TU> selection,
      bool includeConditionBreakingItem = false)
    {
      IEnumerator<T> enumerator = enumerable.GetEnumerator();
      while (enumerator.MoveNext())
      {
        if (condition(enumerator.Current))
        {
          List<TU> uList = new List<TU>()
          {
            selection(enumerator.Current)
          };
          while (enumerator.MoveNext())
          {
            if (condition(enumerator.Current))
            {
              uList.Add(selection(enumerator.Current));
            }
            else
            {
              if (includeConditionBreakingItem)
              {
                uList.Add(selection(enumerator.Current));
                break;
              }
              break;
            }
          }
          yield return (IList<TU>) uList;
        }
      }
    }

    public static IEnumerable<ICollection<T>> Partition<T>(
      this IEnumerable<T> enumerable,
      int amountOfCollections,
      bool duplicateEdgeItems = false)
    {
      IList<T> list = !(enumerable is IList<T>) ? (IList<T>) enumerable.ToList<T>() : enumerable as IList<T>;
      List<T> next = new List<T>();
      if (duplicateEdgeItems)
      {
        int amountPerCollection = (int) Math.Ceiling((double) ((list.Count + amountOfCollections - 1) / amountOfCollections));
        for (int i = 0; i < list.Count; ++i)
        {
          next.Add(list[i]);
          if (next.Count == amountPerCollection)
          {
            if (list.Count - i < amountPerCollection)
            {
              while (i < list.Count)
                next.Add(list[i++]);
              yield return (ICollection<T>) next;
              break;
            }
            yield return (ICollection<T>) next;
            next = new List<T>() { list[i] };
          }
        }
      }
      else
      {
        int amountPerCollection = (int) Math.Ceiling((double) (list.Count / amountOfCollections));
        for (int i = 0; i < list.Count; ++i)
        {
          next.Add(list[i]);
          if (next.Count == amountPerCollection)
          {
            if (list.Count - i < amountPerCollection)
            {
              while (i < list.Count)
                next.Add(list[i++]);
              yield return (ICollection<T>) next;
              break;
            }
            yield return (ICollection<T>) next;
            next = new List<T>();
          }
        }
      }
    }
  }
}
