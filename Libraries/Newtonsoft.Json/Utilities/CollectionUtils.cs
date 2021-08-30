// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.CollectionUtils
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
  internal static class CollectionUtils
  {
    public static bool IsNullOrEmpty<T>(ICollection<T> collection) => collection == null || collection.Count == 0;

    public static void AddRange<T>(this IList<T> initial, IEnumerable<T> collection)
    {
      if (initial == null)
        throw new ArgumentNullException(nameof (initial));
      if (collection == null)
        return;
      foreach (T obj in collection)
        initial.Add(obj);
    }

    public static bool IsDictionaryType(Type type)
    {
      ValidationUtils.ArgumentNotNull((object) type, nameof (type));
      return typeof (IDictionary).IsAssignableFrom(type) || ReflectionUtils.ImplementsGenericDefinition(type, typeof (IDictionary<,>)) || ReflectionUtils.ImplementsGenericDefinition(type, typeof (IReadOnlyDictionary<,>));
    }

    public static ConstructorInfo ResolveEnumerableCollectionConstructor(
      Type collectionType,
      Type collectionItemType)
    {
      Type type = typeof (IEnumerable<>).MakeGenericType(collectionItemType);
      ConstructorInfo constructorInfo = (ConstructorInfo) null;
      foreach (ConstructorInfo constructor in collectionType.GetConstructors(BindingFlags.Instance | BindingFlags.Public))
      {
        IList<ParameterInfo> parameters = (IList<ParameterInfo>) constructor.GetParameters();
        if (parameters.Count == 1)
        {
          if (type == parameters[0].ParameterType)
          {
            constructorInfo = constructor;
            break;
          }
          if (constructorInfo == (ConstructorInfo) null && type.IsAssignableFrom(parameters[0].ParameterType))
            constructorInfo = constructor;
        }
      }
      return constructorInfo;
    }

    public static bool AddDistinct<T>(this IList<T> list, T value) => list.AddDistinct<T>(value, (IEqualityComparer<T>) EqualityComparer<T>.Default);

    public static bool AddDistinct<T>(this IList<T> list, T value, IEqualityComparer<T> comparer)
    {
      if (list.ContainsValue<T>(value, comparer))
        return false;
      list.Add(value);
      return true;
    }

    public static bool ContainsValue<TSource>(
      this IEnumerable<TSource> source,
      TSource value,
      IEqualityComparer<TSource> comparer)
    {
      if (comparer == null)
        comparer = (IEqualityComparer<TSource>) EqualityComparer<TSource>.Default;
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      foreach (TSource x in source)
      {
        if (comparer.Equals(x, value))
          return true;
      }
      return false;
    }

    public static bool AddRangeDistinct<T>(
      this IList<T> list,
      IEnumerable<T> values,
      IEqualityComparer<T> comparer)
    {
      bool flag = true;
      foreach (T obj in values)
      {
        if (!list.AddDistinct<T>(obj, comparer))
          flag = false;
      }
      return flag;
    }

    public static int IndexOf<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
    {
      int num = 0;
      foreach (T obj in collection)
      {
        if (predicate(obj))
          return num;
        ++num;
      }
      return -1;
    }

    public static bool Contains(this IEnumerable list, object value, IEqualityComparer comparer)
    {
      foreach (object x in list)
      {
        if (comparer.Equals(x, value))
          return true;
      }
      return false;
    }

    public static int IndexOf<TSource>(
      this IEnumerable<TSource> list,
      TSource value,
      IEqualityComparer<TSource> comparer)
    {
      int num = 0;
      foreach (TSource x in list)
      {
        if (comparer.Equals(x, value))
          return num;
        ++num;
      }
      return -1;
    }

    private static IList<int> GetDimensions(IList values, int dimensionsCount)
    {
      IList<int> intList = (IList<int>) new List<int>();
      IList list = values;
      while (true)
      {
        intList.Add(list.Count);
        if (intList.Count != dimensionsCount && list.Count != 0)
        {
          object obj = list[0];
          if (obj is IList)
            list = (IList) obj;
          else
            break;
        }
        else
          break;
      }
      return intList;
    }

    private static void CopyFromJaggedToMultidimensionalArray(
      IList values,
      Array multidimensionalArray,
      int[] indices)
    {
      int length1 = indices.Length;
      if (length1 == multidimensionalArray.Rank)
      {
        multidimensionalArray.SetValue(CollectionUtils.JaggedArrayGetValue(values, indices), indices);
      }
      else
      {
        int length2 = multidimensionalArray.GetLength(length1);
        if (((ICollection) CollectionUtils.JaggedArrayGetValue(values, indices)).Count != length2)
          throw new Exception("Cannot deserialize non-cubical array as multidimensional array.");
        int[] indices1 = new int[length1 + 1];
        for (int index = 0; index < length1; ++index)
          indices1[index] = indices[index];
        for (int index = 0; index < multidimensionalArray.GetLength(length1); ++index)
        {
          indices1[length1] = index;
          CollectionUtils.CopyFromJaggedToMultidimensionalArray(values, multidimensionalArray, indices1);
        }
      }
    }

    private static object JaggedArrayGetValue(IList values, int[] indices)
    {
      IList list = values;
      for (int index1 = 0; index1 < indices.Length; ++index1)
      {
        int index2 = indices[index1];
        if (index1 == indices.Length - 1)
          return list[index2];
        list = (IList) list[index2];
      }
      return (object) list;
    }

    public static Array ToMultidimensionalArray(IList values, Type type, int rank)
    {
      IList<int> dimensions = CollectionUtils.GetDimensions(values, rank);
      while (dimensions.Count < rank)
        dimensions.Add(0);
      Array instance = Array.CreateInstance(type, dimensions.ToArray<int>());
      CollectionUtils.CopyFromJaggedToMultidimensionalArray(values, instance, new int[0]);
      return instance;
    }
  }
}
