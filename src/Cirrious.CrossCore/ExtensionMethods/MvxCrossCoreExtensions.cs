// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.ExtensionMethods.MvxCrossCoreExtensions
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using Cirrious.CrossCore.IoC;
using System;
using System.Globalization;
using System.Reflection;

namespace Cirrious.CrossCore.ExtensionMethods
{
  public static class MvxCrossCoreExtensions
  {
    public static bool ConvertToBooleanCore(this object result)
    {
      switch (result)
      {
        case null:
          return false;
        case string _:
          return !string.IsNullOrEmpty((string) result);
        case bool flag:
          return flag;
        default:
          Type type1 = result.GetType();
          if (!type1.GetTypeInfo().IsValueType)
            return true;
          Type type2 = Nullable.GetUnderlyingType(type1);
          if ((object) type2 == null)
            type2 = type1;
          Type type3 = type2;
          return !result.Equals(type3.CreateDefault());
      }
    }

    public static object MakeSafeValueCore(this Type propertyType, object value)
    {
      if (value == null)
        return propertyType.CreateDefault();
      object obj = value;
      if (!ReflectionExtensions.IsInstanceOfType(propertyType, value))
      {
        if ((object) propertyType == (object) typeof (string))
          obj = (object) value.ToString();
        else if (propertyType.GetTypeInfo().IsEnum)
          obj = !(value is string) ? Enum.ToObject(propertyType, value) : Enum.Parse(propertyType, (string) value, true);
        else if (propertyType.GetTypeInfo().IsValueType)
        {
          Type type1 = Nullable.GetUnderlyingType(propertyType);
          if ((object) type1 == null)
            type1 = propertyType;
          Type type2 = type1;
          obj = (object) type2 != (object) typeof (bool) ? MvxCrossCoreExtensions.ErrorMaskedConvert(value, type2, CultureInfo.CurrentUICulture) : (object) value.ConvertToBooleanCore();
        }
        else
          obj = MvxCrossCoreExtensions.ErrorMaskedConvert(value, propertyType, CultureInfo.CurrentUICulture);
      }
      return obj;
    }

    private static object ErrorMaskedConvert(object value, Type type, CultureInfo cultureInfo)
    {
      try
      {
        return Convert.ChangeType(value, type, (IFormatProvider) cultureInfo);
      }
      catch (Exception ex)
      {
        return value;
      }
    }
  }
}
