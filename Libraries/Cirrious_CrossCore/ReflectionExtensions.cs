// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.ReflectionExtensions
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cirrious.CrossCore
{
  public static class ReflectionExtensions
  {
    public static IEnumerable<Type> GetTypes(this Assembly assembly) => assembly.DefinedTypes.Select<TypeInfo, Type>((Func<TypeInfo, Type>) (t => t.AsType()));

    public static EventInfo GetEvent(this Type type, string name) => type.GetRuntimeEvent(name);

    public static IEnumerable<Type> GetInterfaces(this Type type) => type.GetTypeInfo().ImplementedInterfaces;

    public static bool IsAssignableFrom(this Type type, Type otherType) => type.GetTypeInfo().IsAssignableFrom(otherType.GetTypeInfo());

    public static Attribute[] GetCustomAttributes(
      this Type type,
      Type attributeType,
      bool inherit)
    {
      return CustomAttributeExtensions.GetCustomAttributes(type.GetTypeInfo(), attributeType, inherit).ToArray<Attribute>();
    }

    public static IEnumerable<ConstructorInfo> GetConstructors(
      this Type type)
    {
      return type.GetTypeInfo().DeclaredConstructors.Where<ConstructorInfo>((Func<ConstructorInfo, bool>) (c => c.IsPublic));
    }

    public static bool IsInstanceOfType(this Type type, object obj) => ReflectionExtensions.IsAssignableFrom(type, obj.GetType());

    public static MethodInfo GetAddMethod(this EventInfo eventInfo, bool nonPublic = false) => (object) eventInfo.AddMethod == null || !nonPublic && !eventInfo.AddMethod.IsPublic ? (MethodInfo) null : eventInfo.AddMethod;

    public static MethodInfo GetRemoveMethod(this EventInfo eventInfo, bool nonPublic = false) => (object) eventInfo.RemoveMethod == null || !nonPublic && !eventInfo.RemoveMethod.IsPublic ? (MethodInfo) null : eventInfo.RemoveMethod;

    public static MethodInfo GetGetMethod(this PropertyInfo property, bool nonPublic = false) => (object) property.GetMethod == null || !nonPublic && !property.GetMethod.IsPublic ? (MethodInfo) null : property.GetMethod;

    public static MethodInfo GetSetMethod(this PropertyInfo property, bool nonPublic = false) => (object) property.SetMethod == null || !nonPublic && !property.SetMethod.IsPublic ? (MethodInfo) null : property.SetMethod;

    public static IEnumerable<PropertyInfo> GetProperties(this Type type) => type.GetProperties(BindingFlags.Public | BindingFlags.FlattenHierarchy);

    private static bool NullSafeIsPublic(this MethodInfo info) => (object) info != null && info.IsPublic;

    private static bool NullSafeIsStatic(this MethodInfo info) => (object) info != null && info.IsStatic;

    public static IEnumerable<PropertyInfo> GetProperties(
      this Type type,
      BindingFlags flags)
    {
      IEnumerable<PropertyInfo> source = type.GetTypeInfo().DeclaredProperties;
      if ((flags & BindingFlags.FlattenHierarchy) == BindingFlags.FlattenHierarchy)
        source = type.GetRuntimeProperties();
      return source.Select(property => new
      {
        property = property,
        getMethod = property.GetMethod
      }).Select(_param0 => new
      {
        \u003C\u003Eh__TransparentIdentifier4 = _param0,
        setMethod = _param0.property.SetMethod
      }).Where(_param0 => (object) _param0.\u003C\u003Eh__TransparentIdentifier4.getMethod != null || (object) _param0.setMethod != null).Where(_param1 => (flags & BindingFlags.Public) != BindingFlags.Public || _param1.\u003C\u003Eh__TransparentIdentifier4.getMethod.NullSafeIsPublic() || _param1.setMethod.NullSafeIsPublic()).Where(_param1 => (flags & BindingFlags.Instance) != BindingFlags.Instance || !_param1.\u003C\u003Eh__TransparentIdentifier4.getMethod.NullSafeIsStatic() || !_param1.setMethod.NullSafeIsStatic()).Where(_param1 => (flags & BindingFlags.Static) != BindingFlags.Static || _param1.\u003C\u003Eh__TransparentIdentifier4.getMethod.NullSafeIsStatic() || _param1.setMethod.NullSafeIsStatic()).Select(_param0 => _param0.\u003C\u003Eh__TransparentIdentifier4.property);
    }

    public static PropertyInfo GetProperty(
      this Type type,
      string name,
      BindingFlags flags)
    {
      return type.GetProperties(flags).FirstOrDefault<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.Name == name));
    }

    public static PropertyInfo GetProperty(this Type type, string name) => type.GetProperties(BindingFlags.Public | BindingFlags.FlattenHierarchy).FirstOrDefault<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.Name == name));

    public static IEnumerable<MethodInfo> GetMethods(this Type type) => type.GetMethods(BindingFlags.Public | BindingFlags.FlattenHierarchy);

    public static IEnumerable<MethodInfo> GetMethods(
      this Type type,
      BindingFlags flags)
    {
      IEnumerable<MethodInfo> source = type.GetTypeInfo().DeclaredMethods;
      if ((flags & BindingFlags.FlattenHierarchy) == BindingFlags.FlattenHierarchy)
        source = type.GetRuntimeMethods();
      return source.Where<MethodInfo>((Func<MethodInfo, bool>) (m => (flags & BindingFlags.Public) != BindingFlags.Public || m.IsPublic)).Where<MethodInfo>((Func<MethodInfo, bool>) (m => (flags & BindingFlags.Instance) != BindingFlags.Instance || !m.IsStatic)).Where<MethodInfo>((Func<MethodInfo, bool>) (m => (flags & BindingFlags.Static) != BindingFlags.Static || m.IsStatic));
    }

    public static MethodInfo GetMethod(this Type type, string name, BindingFlags flags) => type.GetMethods(flags).FirstOrDefault<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == name));

    public static MethodInfo GetMethod(this Type type, string name) => type.GetMethods(BindingFlags.Public | BindingFlags.FlattenHierarchy).FirstOrDefault<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == name));

    public static IEnumerable<ConstructorInfo> GetConstructors(
      this Type type,
      BindingFlags flags)
    {
      return ReflectionExtensions.GetConstructors(type).Where<ConstructorInfo>((Func<ConstructorInfo, bool>) (m => (flags & BindingFlags.Public) != BindingFlags.Public || m.IsPublic)).Where<ConstructorInfo>((Func<ConstructorInfo, bool>) (m => (flags & BindingFlags.Instance) != BindingFlags.Instance || !m.IsStatic)).Where<ConstructorInfo>((Func<ConstructorInfo, bool>) (m => (flags & BindingFlags.Static) != BindingFlags.Static || m.IsStatic));
    }

    public static IEnumerable<FieldInfo> GetFields(this Type type) => type.GetFields(BindingFlags.Public | BindingFlags.FlattenHierarchy);

    public static IEnumerable<FieldInfo> GetFields(
      this Type type,
      BindingFlags flags)
    {
      IEnumerable<FieldInfo> source = type.GetTypeInfo().DeclaredFields;
      if ((flags & BindingFlags.FlattenHierarchy) == BindingFlags.FlattenHierarchy)
        source = type.GetRuntimeFields();
      return source.Where<FieldInfo>((Func<FieldInfo, bool>) (f => (flags & BindingFlags.Public) != BindingFlags.Public || f.IsPublic)).Where<FieldInfo>((Func<FieldInfo, bool>) (f => (flags & BindingFlags.Instance) != BindingFlags.Instance || !f.IsStatic)).Where<FieldInfo>((Func<FieldInfo, bool>) (f => (flags & BindingFlags.Static) != BindingFlags.Static || f.IsStatic));
    }

    public static FieldInfo GetField(this Type type, string name, BindingFlags flags) => type.GetFields(flags).FirstOrDefault<FieldInfo>((Func<FieldInfo, bool>) (p => p.Name == name));

    public static FieldInfo GetField(this Type type, string name) => type.GetFields(BindingFlags.Public | BindingFlags.FlattenHierarchy).FirstOrDefault<FieldInfo>((Func<FieldInfo, bool>) (p => p.Name == name));

    public static Type[] GetGenericArguments(this Type type) => type.GenericTypeArguments;
  }
}
