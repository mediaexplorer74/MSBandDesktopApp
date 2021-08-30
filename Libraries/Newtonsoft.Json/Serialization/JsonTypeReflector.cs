// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonTypeReflector
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace Newtonsoft.Json.Serialization
{
  internal static class JsonTypeReflector
  {
    public const string IdPropertyName = "$id";
    public const string RefPropertyName = "$ref";
    public const string TypePropertyName = "$type";
    public const string ValuePropertyName = "$value";
    public const string ArrayValuesPropertyName = "$values";
    public const string ShouldSerializePrefix = "ShouldSerialize";
    public const string SpecifiedPostfix = "Specified";
    private static bool? _dynamicCodeGeneration;
    private static bool? _fullyTrusted;
    private static readonly ThreadSafeStore<Type, Func<object[], JsonConverter>> JsonConverterCreatorCache = new ThreadSafeStore<Type, Func<object[], JsonConverter>>(new Func<Type, Func<object[], JsonConverter>>(JsonTypeReflector.GetJsonConverterCreator));
    private static readonly ThreadSafeStore<Type, Type> AssociatedMetadataTypesCache = new ThreadSafeStore<Type, Type>(new Func<Type, Type>(JsonTypeReflector.GetAssociateMetadataTypeFromAttribute));
    private static ReflectionObject _metadataTypeAttributeReflectionObject;

    public static T GetCachedAttribute<T>(object attributeProvider) where T : Attribute => CachedAttributeGetter<T>.GetAttribute(attributeProvider);

    public static DataContractAttribute GetDataContractAttribute(Type type)
    {
      for (Type type1 = type; type1 != (Type) null; type1 = type1.BaseType())
      {
        DataContractAttribute attribute = CachedAttributeGetter<DataContractAttribute>.GetAttribute((object) type1);
        if (attribute != null)
          return attribute;
      }
      return (DataContractAttribute) null;
    }

    public static DataMemberAttribute GetDataMemberAttribute(
      MemberInfo memberInfo)
    {
      if (memberInfo.MemberType() == MemberTypes.Field)
        return CachedAttributeGetter<DataMemberAttribute>.GetAttribute((object) memberInfo);
      PropertyInfo propertyInfo = (PropertyInfo) memberInfo;
      DataMemberAttribute attribute = CachedAttributeGetter<DataMemberAttribute>.GetAttribute((object) propertyInfo);
      if (attribute == null && propertyInfo.IsVirtual())
      {
        for (Type type = propertyInfo.DeclaringType; attribute == null && type != (Type) null; type = type.BaseType())
        {
          PropertyInfo memberInfoFromType = (PropertyInfo) ReflectionUtils.GetMemberInfoFromType(type, (MemberInfo) propertyInfo);
          if (memberInfoFromType != (PropertyInfo) null && memberInfoFromType.IsVirtual())
            attribute = CachedAttributeGetter<DataMemberAttribute>.GetAttribute((object) memberInfoFromType);
        }
      }
      return attribute;
    }

    public static MemberSerialization GetObjectMemberSerialization(
      Type objectType,
      bool ignoreSerializableAttribute)
    {
      JsonObjectAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonObjectAttribute>((object) objectType);
      if (cachedAttribute != null)
        return cachedAttribute.MemberSerialization;
      if (JsonTypeReflector.GetDataContractAttribute(objectType) != null)
        return MemberSerialization.OptIn;
      return !ignoreSerializableAttribute && JsonTypeReflector.GetCachedAttribute<SerializableAttribute>((object) objectType) != null ? MemberSerialization.Fields : MemberSerialization.OptOut;
    }

    public static JsonConverter GetJsonConverter(object attributeProvider)
    {
      JsonConverterAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonConverterAttribute>(attributeProvider);
      if (cachedAttribute != null)
      {
        Func<object[], JsonConverter> func = JsonTypeReflector.JsonConverterCreatorCache.Get(cachedAttribute.ConverterType);
        if (func != null)
          return func(cachedAttribute.ConverterParameters);
      }
      return (JsonConverter) null;
    }

    public static JsonConverter CreateJsonConverterInstance(
      Type converterType,
      object[] converterArgs)
    {
      return JsonTypeReflector.JsonConverterCreatorCache.Get(converterType)(converterArgs);
    }

    private static Func<object[], JsonConverter> GetJsonConverterCreator(
      Type converterType)
    {
      Func<object> defaultConstructor = ReflectionUtils.HasDefaultConstructor(converterType, false) ? JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(converterType) : (Func<object>) null;
      return (Func<object[], JsonConverter>) (parameters =>
      {
        try
        {
          if (parameters != null)
          {
            ConstructorInfo constructor = converterType.GetConstructor(((IEnumerable<object>) parameters).Select<object, Type>((Func<object, Type>) (param => param.GetType())).ToArray<Type>());
            if ((ConstructorInfo) null != constructor)
              return (JsonConverter) JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor((MethodBase) constructor)(parameters);
            throw new JsonException("No matching parameterized constructor found for '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) converterType));
          }
          if (defaultConstructor == null)
            throw new JsonException("No parameterless constructor defined for '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) converterType));
          return (JsonConverter) defaultConstructor();
        }
        catch (Exception ex)
        {
          throw new JsonException("Error creating '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) converterType), ex);
        }
      });
    }

    public static TypeConverter GetTypeConverter(Type type) => TypeDescriptor.GetConverter(type);

    private static Type GetAssociatedMetadataType(Type type) => JsonTypeReflector.AssociatedMetadataTypesCache.Get(type);

    private static Type GetAssociateMetadataTypeFromAttribute(Type type)
    {
      foreach (Attribute attribute in ReflectionUtils.GetAttributes((object) type, (Type) null, true))
      {
        Type type1 = attribute.GetType();
        if (string.Equals(type1.FullName, "System.ComponentModel.DataAnnotations.MetadataTypeAttribute", StringComparison.Ordinal))
        {
          if (JsonTypeReflector._metadataTypeAttributeReflectionObject == null)
            JsonTypeReflector._metadataTypeAttributeReflectionObject = ReflectionObject.Create(type1, "MetadataClassType");
          return (Type) JsonTypeReflector._metadataTypeAttributeReflectionObject.GetValue((object) attribute, "MetadataClassType");
        }
      }
      return (Type) null;
    }

    private static T GetAttribute<T>(Type type) where T : Attribute
    {
      Type associatedMetadataType = JsonTypeReflector.GetAssociatedMetadataType(type);
      if (associatedMetadataType != (Type) null)
      {
        T attribute = ReflectionUtils.GetAttribute<T>((object) associatedMetadataType, true);
        if ((object) attribute != null)
          return attribute;
      }
      T attribute1 = ReflectionUtils.GetAttribute<T>((object) type, true);
      if ((object) attribute1 != null)
        return attribute1;
      foreach (object attributeProvider in type.GetInterfaces())
      {
        T attribute2 = ReflectionUtils.GetAttribute<T>(attributeProvider, true);
        if ((object) attribute2 != null)
          return attribute2;
      }
      return default (T);
    }

    private static T GetAttribute<T>(MemberInfo memberInfo) where T : Attribute
    {
      Type associatedMetadataType = JsonTypeReflector.GetAssociatedMetadataType(memberInfo.DeclaringType);
      if (associatedMetadataType != (Type) null)
      {
        MemberInfo memberInfoFromType = ReflectionUtils.GetMemberInfoFromType(associatedMetadataType, memberInfo);
        if (memberInfoFromType != (MemberInfo) null)
        {
          T attribute = ReflectionUtils.GetAttribute<T>((object) memberInfoFromType, true);
          if ((object) attribute != null)
            return attribute;
        }
      }
      T attribute1 = ReflectionUtils.GetAttribute<T>((object) memberInfo, true);
      if ((object) attribute1 != null)
        return attribute1;
      if (memberInfo.DeclaringType != (Type) null)
      {
        foreach (Type targetType in memberInfo.DeclaringType.GetInterfaces())
        {
          MemberInfo memberInfoFromType = ReflectionUtils.GetMemberInfoFromType(targetType, memberInfo);
          if (memberInfoFromType != (MemberInfo) null)
          {
            T attribute2 = ReflectionUtils.GetAttribute<T>((object) memberInfoFromType, true);
            if ((object) attribute2 != null)
              return attribute2;
          }
        }
      }
      return default (T);
    }

    public static T GetAttribute<T>(object provider) where T : Attribute
    {
      Type type = provider as Type;
      if (type != (Type) null)
        return JsonTypeReflector.GetAttribute<T>(type);
      MemberInfo memberInfo = provider as MemberInfo;
      return memberInfo != (MemberInfo) null ? JsonTypeReflector.GetAttribute<T>(memberInfo) : ReflectionUtils.GetAttribute<T>(provider, true);
    }

    public static bool DynamicCodeGeneration
    {
      [SecuritySafeCritical] get
      {
        if (!JsonTypeReflector._dynamicCodeGeneration.HasValue)
        {
          try
          {
            new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Demand();
            new ReflectionPermission(ReflectionPermissionFlag.RestrictedMemberAccess).Demand();
            new SecurityPermission(SecurityPermissionFlag.SkipVerification).Demand();
            new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
            new SecurityPermission(PermissionState.Unrestricted).Demand();
            JsonTypeReflector._dynamicCodeGeneration = new bool?(true);
          }
          catch (Exception ex)
          {
            JsonTypeReflector._dynamicCodeGeneration = new bool?(false);
          }
        }
        return JsonTypeReflector._dynamicCodeGeneration.Value;
      }
    }

    public static bool FullyTrusted
    {
      get
      {
        if (!JsonTypeReflector._fullyTrusted.HasValue)
        {
          AppDomain currentDomain = AppDomain.CurrentDomain;
          JsonTypeReflector._fullyTrusted = new bool?(currentDomain.IsHomogenous && currentDomain.IsFullyTrusted);
        }
        return JsonTypeReflector._fullyTrusted.Value;
      }
    }

    public static ReflectionDelegateFactory ReflectionDelegateFactory => JsonTypeReflector.DynamicCodeGeneration ? (ReflectionDelegateFactory) DynamicReflectionDelegateFactory.Instance : LateBoundReflectionDelegateFactory.Instance;
  }
}
