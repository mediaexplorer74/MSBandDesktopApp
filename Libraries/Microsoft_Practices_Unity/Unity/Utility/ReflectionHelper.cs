// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.Utility.ReflectionHelper
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Practices.Unity.Utility
{
  public class ReflectionHelper
  {
    private readonly Type t;

    public ReflectionHelper(Type typeToReflect) => this.t = typeToReflect;

    public Type Type => this.t;

    public bool IsGenericType => this.t.GetTypeInfo().IsGenericType;

    public bool IsOpenGeneric => this.t.GetTypeInfo().IsGenericType && this.t.GetTypeInfo().ContainsGenericParameters;

    public bool IsArray => this.t.IsArray;

    public bool IsGenericArray => this.IsArray && this.ArrayElementType.GetTypeInfo().IsGenericParameter;

    public Type ArrayElementType => this.t.GetElementType();

    public static bool MethodHasOpenGenericParameters(MethodBase method)
    {
      Guard.ArgumentNotNull((object) method, nameof (method));
      foreach (ParameterInfo parameter in method.GetParameters())
      {
        if (new ReflectionHelper(parameter.ParameterType).IsOpenGeneric)
          return true;
      }
      return false;
    }

    public Type GetClosedParameterType(Type[] genericArguments)
    {
      Guard.ArgumentNotNull((object) genericArguments, nameof (genericArguments));
      if (this.IsOpenGeneric)
      {
        TypeInfo typeInfo = this.Type.GetTypeInfo();
        Type[] typeArray = typeInfo.IsGenericTypeDefinition ? typeInfo.GenericTypeParameters : typeInfo.GenericTypeArguments;
        for (int index = 0; index < typeArray.Length; ++index)
          typeArray[index] = genericArguments[typeArray[index].GenericParameterPosition];
        return this.Type.GetGenericTypeDefinition().MakeGenericType(typeArray);
      }
      if (this.Type.GetTypeInfo().IsGenericParameter)
        return genericArguments[this.Type.GenericParameterPosition];
      if (!this.IsArray || !this.ArrayElementType.GetTypeInfo().IsGenericParameter)
        return this.Type;
      int arrayRank;
      return (arrayRank = this.Type.GetArrayRank()) == 1 ? genericArguments[this.Type.GetElementType().GenericParameterPosition].MakeArrayType() : genericArguments[this.Type.GetElementType().GenericParameterPosition].MakeArrayType(arrayRank);
    }

    public Type GetNamedGenericParameter(string parameterName)
    {
      TypeInfo typeInfo = this.Type.GetGenericTypeDefinition().GetTypeInfo();
      Type type = (Type) null;
      int index = -1;
      foreach (Type genericTypeParameter in typeInfo.GenericTypeParameters)
      {
        if (genericTypeParameter.GetTypeInfo().Name == parameterName)
        {
          index = genericTypeParameter.GenericParameterPosition;
          break;
        }
      }
      if (index != -1)
        type = this.Type.GenericTypeArguments[index];
      return type;
    }

    public IEnumerable<ConstructorInfo> InstanceConstructors => this.Type.GetTypeInfo().DeclaredConstructors.Where<ConstructorInfo>((Func<ConstructorInfo, bool>) (c => !c.IsStatic && c.IsPublic));
  }
}
