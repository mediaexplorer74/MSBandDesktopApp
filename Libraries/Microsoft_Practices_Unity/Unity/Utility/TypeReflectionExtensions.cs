// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.Utility.TypeReflectionExtensions
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Practices.Unity.Utility
{
  public static class TypeReflectionExtensions
  {
    public static ConstructorInfo GetConstructor(
      this Type type,
      params Type[] constructorParameters)
    {
      return type.GetTypeInfo().DeclaredConstructors.Single<ConstructorInfo>((Func<ConstructorInfo, bool>) (c => !c.IsStatic && TypeReflectionExtensions.ParametersMatch(c.GetParameters(), constructorParameters)));
    }

    public static IEnumerable<MethodInfo> GetMethodsHierarchical(this Type type)
    {
      if ((object) type == null)
        return Enumerable.Empty<MethodInfo>();
      return type.Equals(typeof (object)) ? type.GetTypeInfo().DeclaredMethods.Where<MethodInfo>((Func<MethodInfo, bool>) (m => !m.IsStatic)) : type.GetTypeInfo().DeclaredMethods.Where<MethodInfo>((Func<MethodInfo, bool>) (m => !m.IsStatic)).Concat<MethodInfo>(type.GetTypeInfo().BaseType.GetMethodsHierarchical());
    }

    public static MethodInfo GetMethodHierarchical(
      this Type type,
      string methodName,
      Type[] closedParameters)
    {
      return type.GetMethodsHierarchical().Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name.Equals(methodName) && TypeReflectionExtensions.ParametersMatch(m.GetParameters(), closedParameters)));
    }

    public static IEnumerable<PropertyInfo> GetPropertiesHierarchical(
      this Type type)
    {
      if ((object) type == null)
        return Enumerable.Empty<PropertyInfo>();
      return type.Equals(typeof (object)) ? type.GetTypeInfo().DeclaredProperties : type.GetTypeInfo().DeclaredProperties.Concat<PropertyInfo>(type.GetTypeInfo().BaseType.GetPropertiesHierarchical());
    }

    public static bool ParametersMatch(
      ParameterInfo[] parameters,
      Type[] closedConstructorParameterTypes)
    {
      Guard.ArgumentNotNull((object) parameters, nameof (parameters));
      Guard.ArgumentNotNull((object) closedConstructorParameterTypes, nameof (closedConstructorParameterTypes));
      if (parameters.Length != closedConstructorParameterTypes.Length)
        return false;
      for (int index = 0; index < parameters.Length; ++index)
      {
        if (!parameters[index].ParameterType.Equals(closedConstructorParameterTypes[index]))
          return false;
      }
      return true;
    }
  }
}
