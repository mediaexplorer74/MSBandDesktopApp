// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.TypeExtensions
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
  internal static class TypeExtensions
  {
    public static IEnumerable<MethodInfo> GetRuntimeMethods(this Type type) => (IEnumerable<MethodInfo>) type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

    public static bool IsAbstract(this Type type) => type.IsAbstract;

    public static bool IsGenericType(this Type type) => type.IsGenericType;
  }
}
