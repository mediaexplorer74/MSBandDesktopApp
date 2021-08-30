// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.Platform.MvxSimplePropertyDictionaryExtensionMethods
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cirrious.MvvmCross.Platform
{
  public static class MvxSimplePropertyDictionaryExtensionMethods
  {
    public static IDictionary<string, string> ToSimpleStringPropertyDictionary(
      this IDictionary<string, object> input)
    {
      return input == null ? (IDictionary<string, string>) new Dictionary<string, string>() : (IDictionary<string, string>) input.ToDictionary<KeyValuePair<string, object>, string, string>((Func<KeyValuePair<string, object>, string>) (x => x.Key), (Func<KeyValuePair<string, object>, string>) (x => x.Value != null ? x.Value.ToString() : (string) null));
    }

    public static IDictionary<string, string> SafeGetData(this IMvxBundle bundle) => bundle?.Data;

    public static void Write(this IDictionary<string, string> data, object toStore)
    {
      if (toStore == null)
        return;
      foreach (KeyValuePair<string, string> simpleProperty in (IEnumerable<KeyValuePair<string, string>>) toStore.ToSimplePropertyDictionary())
        data[simpleProperty.Key] = simpleProperty.Value;
    }

    public static T Read<T>(this IDictionary<string, string> data) where T : new() => (T) data.Read(typeof (T));

    public static object Read(this IDictionary<string, string> data, Type type)
    {
      object instance = Activator.CreateInstance(type);
      foreach (PropertyInfo propertyInfo in type.GetProperties(Cirrious.CrossCore.BindingFlags.Instance | Cirrious.CrossCore.BindingFlags.Public | Cirrious.CrossCore.BindingFlags.FlattenHierarchy).Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.CanWrite)))
      {
        string rawValue;
        if (data.TryGetValue(propertyInfo.Name, out rawValue))
        {
          object obj = MvxSingleton<IMvxSingletonCache>.Instance.Parser.ReadValue(rawValue, propertyInfo.PropertyType, propertyInfo.Name);
          propertyInfo.SetValue(instance, obj, new object[0]);
        }
      }
      return instance;
    }

    public static IEnumerable<object> CreateArgumentList(
      this IDictionary<string, string> data,
      IEnumerable<ParameterInfo> requiredParameters,
      string debugText)
    {
      List<object> objectList = new List<object>();
      foreach (ParameterInfo requiredParameter in requiredParameters)
      {
        object argumentValue = data.GetArgumentValue(requiredParameter, debugText);
        objectList.Add(argumentValue);
      }
      return (IEnumerable<object>) objectList;
    }

    public static object GetArgumentValue(
      this IDictionary<string, string> data,
      ParameterInfo requiredParameter,
      string debugText)
    {
      string rawValue;
      if (data == null || !data.TryGetValue(requiredParameter.Name, out rawValue))
      {
        if (requiredParameter.IsOptional)
          return Type.Missing;
        MvxTrace.Trace("Missing parameter for call to {0} - missing parameter {1} - asssuming null - this may fail for value types!", (object) debugText, (object) requiredParameter.Name);
        rawValue = (string) null;
      }
      return MvxSingleton<IMvxSingletonCache>.Instance.Parser.ReadValue(rawValue, requiredParameter.ParameterType, requiredParameter.Name);
    }

    public static IDictionary<string, string> ToSimplePropertyDictionary(
      this object input)
    {
      if (input == null)
        return (IDictionary<string, string>) new Dictionary<string, string>();
      if (input is IDictionary<string, string>)
        return (IDictionary<string, string>) input;
      IEnumerable<\u003C\u003Ef__AnonymousType6<bool, PropertyInfo>> datas = input.GetType().GetProperties(Cirrious.CrossCore.BindingFlags.Instance | Cirrious.CrossCore.BindingFlags.Public | Cirrious.CrossCore.BindingFlags.FlattenHierarchy).Where<PropertyInfo>((Func<PropertyInfo, bool>) (property => property.CanRead)).Select(property => new
      {
        CanSerialize = MvxSingleton<IMvxSingletonCache>.Instance.Parser.TypeSupported(property.PropertyType),
        Property = property
      });
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      foreach (var data in datas)
      {
        if (data.CanSerialize)
          dictionary[data.Property.Name] = input.GetPropertyValueAsString(data.Property);
        else
          Mvx.Trace("Skipping serialization of property {0} - don't know how to serialize type {1} - some answers on http://stackoverflow.com/questions/16524236/custom-types-in-navigation-parameters-in-v3", (object) data.Property.Name, (object) data.Property.PropertyType.Name);
      }
      return (IDictionary<string, string>) dictionary;
    }

    public static string GetPropertyValueAsString(this object input, PropertyInfo propertyInfo)
    {
      try
      {
        return propertyInfo.GetValue(input, new object[0])?.ToString();
      }
      catch (Exception ex)
      {
        throw ex.MvxWrap("Problem accessing object - most likely this is caused by an anonymous object being generated as Internal - please see http://stackoverflow.com/questions/8273399/anonymous-types-and-get-accessors-on-wp7-1");
      }
    }
  }
}
