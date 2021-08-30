// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.ValidationUtils
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using System;
using System.Globalization;

namespace Newtonsoft.Json.Utilities
{
  internal static class ValidationUtils
  {
    public static void ArgumentNotNullOrEmpty(string value, string parameterName)
    {
      switch (value)
      {
        case "":
          throw new ArgumentException("'{0}' cannot be empty.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) parameterName), parameterName);
        case null:
          throw new ArgumentNullException(parameterName);
      }
    }

    public static void ArgumentTypeIsEnum(Type enumType, string parameterName)
    {
      ValidationUtils.ArgumentNotNull((object) enumType, nameof (enumType));
      if (!enumType.IsEnum())
        throw new ArgumentException("Type {0} is not an Enum.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) enumType), parameterName);
    }

    public static void ArgumentNotNull(object value, string parameterName)
    {
      if (value == null)
        throw new ArgumentNullException(parameterName);
    }
  }
}
