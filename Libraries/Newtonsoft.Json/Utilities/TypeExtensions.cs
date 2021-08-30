// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.TypeExtensions
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using System;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
  internal static class TypeExtensions
  {
    public static MethodInfo Method(this Delegate d) => d.Method;

    public static MemberTypes MemberType(this MemberInfo memberInfo) => memberInfo.MemberType;

    public static bool ContainsGenericParameters(this Type type) => type.ContainsGenericParameters;

    public static bool IsInterface(this Type type) => type.IsInterface;

    public static bool IsGenericType(this Type type) => type.IsGenericType;

    public static bool IsGenericTypeDefinition(this Type type) => type.IsGenericTypeDefinition;

    public static Type BaseType(this Type type) => type.BaseType;

    public static System.Reflection.Assembly Assembly(this Type type) => type.Assembly;

    public static bool IsEnum(this Type type) => type.IsEnum;

    public static bool IsClass(this Type type) => type.IsClass;

    public static bool IsSealed(this Type type) => type.IsSealed;

    public static bool IsAbstract(this Type type) => type.IsAbstract;

    public static bool IsVisible(this Type type) => type.IsVisible;

    public static bool IsValueType(this Type type) => type.IsValueType;

    public static bool AssignableToTypeName(this Type type, string fullTypeName, out Type match)
    {
      for (Type type1 = type; type1 != (Type) null; type1 = type1.BaseType())
      {
        if (string.Equals(type1.FullName, fullTypeName, StringComparison.Ordinal))
        {
          match = type1;
          return true;
        }
      }
      foreach (MemberInfo memberInfo in type.GetInterfaces())
      {
        if (string.Equals(memberInfo.Name, fullTypeName, StringComparison.Ordinal))
        {
          match = type;
          return true;
        }
      }
      match = (Type) null;
      return false;
    }

    public static bool AssignableToTypeName(this Type type, string fullTypeName) => type.AssignableToTypeName(fullTypeName, out Type _);
  }
}
