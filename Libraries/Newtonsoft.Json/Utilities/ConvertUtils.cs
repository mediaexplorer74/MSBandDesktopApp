// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.ConvertUtils
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Globalization;
using System.Numerics;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
  internal static class ConvertUtils
  {
    private static readonly Dictionary<Type, PrimitiveTypeCode> TypeCodeMap = new Dictionary<Type, PrimitiveTypeCode>()
    {
      {
        typeof (char),
        PrimitiveTypeCode.Char
      },
      {
        typeof (char?),
        PrimitiveTypeCode.CharNullable
      },
      {
        typeof (bool),
        PrimitiveTypeCode.Boolean
      },
      {
        typeof (bool?),
        PrimitiveTypeCode.BooleanNullable
      },
      {
        typeof (sbyte),
        PrimitiveTypeCode.SByte
      },
      {
        typeof (sbyte?),
        PrimitiveTypeCode.SByteNullable
      },
      {
        typeof (short),
        PrimitiveTypeCode.Int16
      },
      {
        typeof (short?),
        PrimitiveTypeCode.Int16Nullable
      },
      {
        typeof (ushort),
        PrimitiveTypeCode.UInt16
      },
      {
        typeof (ushort?),
        PrimitiveTypeCode.UInt16Nullable
      },
      {
        typeof (int),
        PrimitiveTypeCode.Int32
      },
      {
        typeof (int?),
        PrimitiveTypeCode.Int32Nullable
      },
      {
        typeof (byte),
        PrimitiveTypeCode.Byte
      },
      {
        typeof (byte?),
        PrimitiveTypeCode.ByteNullable
      },
      {
        typeof (uint),
        PrimitiveTypeCode.UInt32
      },
      {
        typeof (uint?),
        PrimitiveTypeCode.UInt32Nullable
      },
      {
        typeof (long),
        PrimitiveTypeCode.Int64
      },
      {
        typeof (long?),
        PrimitiveTypeCode.Int64Nullable
      },
      {
        typeof (ulong),
        PrimitiveTypeCode.UInt64
      },
      {
        typeof (ulong?),
        PrimitiveTypeCode.UInt64Nullable
      },
      {
        typeof (float),
        PrimitiveTypeCode.Single
      },
      {
        typeof (float?),
        PrimitiveTypeCode.SingleNullable
      },
      {
        typeof (double),
        PrimitiveTypeCode.Double
      },
      {
        typeof (double?),
        PrimitiveTypeCode.DoubleNullable
      },
      {
        typeof (DateTime),
        PrimitiveTypeCode.DateTime
      },
      {
        typeof (DateTime?),
        PrimitiveTypeCode.DateTimeNullable
      },
      {
        typeof (DateTimeOffset),
        PrimitiveTypeCode.DateTimeOffset
      },
      {
        typeof (DateTimeOffset?),
        PrimitiveTypeCode.DateTimeOffsetNullable
      },
      {
        typeof (Decimal),
        PrimitiveTypeCode.Decimal
      },
      {
        typeof (Decimal?),
        PrimitiveTypeCode.DecimalNullable
      },
      {
        typeof (Guid),
        PrimitiveTypeCode.Guid
      },
      {
        typeof (Guid?),
        PrimitiveTypeCode.GuidNullable
      },
      {
        typeof (TimeSpan),
        PrimitiveTypeCode.TimeSpan
      },
      {
        typeof (TimeSpan?),
        PrimitiveTypeCode.TimeSpanNullable
      },
      {
        typeof (BigInteger),
        PrimitiveTypeCode.BigInteger
      },
      {
        typeof (BigInteger?),
        PrimitiveTypeCode.BigIntegerNullable
      },
      {
        typeof (Uri),
        PrimitiveTypeCode.Uri
      },
      {
        typeof (string),
        PrimitiveTypeCode.String
      },
      {
        typeof (byte[]),
        PrimitiveTypeCode.Bytes
      },
      {
        typeof (DBNull),
        PrimitiveTypeCode.DBNull
      }
    };
    private static readonly TypeInformation[] PrimitiveTypeCodes = new TypeInformation[19]
    {
      new TypeInformation()
      {
        Type = typeof (object),
        TypeCode = PrimitiveTypeCode.Empty
      },
      new TypeInformation()
      {
        Type = typeof (object),
        TypeCode = PrimitiveTypeCode.Object
      },
      new TypeInformation()
      {
        Type = typeof (object),
        TypeCode = PrimitiveTypeCode.DBNull
      },
      new TypeInformation()
      {
        Type = typeof (bool),
        TypeCode = PrimitiveTypeCode.Boolean
      },
      new TypeInformation()
      {
        Type = typeof (char),
        TypeCode = PrimitiveTypeCode.Char
      },
      new TypeInformation()
      {
        Type = typeof (sbyte),
        TypeCode = PrimitiveTypeCode.SByte
      },
      new TypeInformation()
      {
        Type = typeof (byte),
        TypeCode = PrimitiveTypeCode.Byte
      },
      new TypeInformation()
      {
        Type = typeof (short),
        TypeCode = PrimitiveTypeCode.Int16
      },
      new TypeInformation()
      {
        Type = typeof (ushort),
        TypeCode = PrimitiveTypeCode.UInt16
      },
      new TypeInformation()
      {
        Type = typeof (int),
        TypeCode = PrimitiveTypeCode.Int32
      },
      new TypeInformation()
      {
        Type = typeof (uint),
        TypeCode = PrimitiveTypeCode.UInt32
      },
      new TypeInformation()
      {
        Type = typeof (long),
        TypeCode = PrimitiveTypeCode.Int64
      },
      new TypeInformation()
      {
        Type = typeof (ulong),
        TypeCode = PrimitiveTypeCode.UInt64
      },
      new TypeInformation()
      {
        Type = typeof (float),
        TypeCode = PrimitiveTypeCode.Single
      },
      new TypeInformation()
      {
        Type = typeof (double),
        TypeCode = PrimitiveTypeCode.Double
      },
      new TypeInformation()
      {
        Type = typeof (Decimal),
        TypeCode = PrimitiveTypeCode.Decimal
      },
      new TypeInformation()
      {
        Type = typeof (DateTime),
        TypeCode = PrimitiveTypeCode.DateTime
      },
      new TypeInformation()
      {
        Type = typeof (object),
        TypeCode = PrimitiveTypeCode.Empty
      },
      new TypeInformation()
      {
        Type = typeof (string),
        TypeCode = PrimitiveTypeCode.String
      }
    };
    private static readonly ThreadSafeStore<ConvertUtils.TypeConvertKey, Func<object, object>> CastConverters = new ThreadSafeStore<ConvertUtils.TypeConvertKey, Func<object, object>>(new Func<ConvertUtils.TypeConvertKey, Func<object, object>>(ConvertUtils.CreateCastConverter));

    public static PrimitiveTypeCode GetTypeCode(Type t) => ConvertUtils.GetTypeCode(t, out bool _);

    public static PrimitiveTypeCode GetTypeCode(Type t, out bool isEnum)
    {
      PrimitiveTypeCode primitiveTypeCode;
      if (ConvertUtils.TypeCodeMap.TryGetValue(t, out primitiveTypeCode))
      {
        isEnum = false;
        return primitiveTypeCode;
      }
      if (t.IsEnum())
      {
        isEnum = true;
        return ConvertUtils.GetTypeCode(Enum.GetUnderlyingType(t));
      }
      if (ReflectionUtils.IsNullableType(t))
      {
        Type underlyingType = Nullable.GetUnderlyingType(t);
        if (underlyingType.IsEnum())
        {
          Type t1 = typeof (Nullable<>).MakeGenericType(Enum.GetUnderlyingType(underlyingType));
          isEnum = true;
          return ConvertUtils.GetTypeCode(t1);
        }
      }
      isEnum = false;
      return PrimitiveTypeCode.Object;
    }

    public static TypeInformation GetTypeInformation(IConvertible convertable) => ConvertUtils.PrimitiveTypeCodes[(int) convertable.GetTypeCode()];

    public static bool IsConvertible(Type t) => typeof (IConvertible).IsAssignableFrom(t);

    public static TimeSpan ParseTimeSpan(string input) => TimeSpan.Parse(input, (IFormatProvider) CultureInfo.InvariantCulture);

    private static Func<object, object> CreateCastConverter(ConvertUtils.TypeConvertKey t)
    {
      MethodInfo method = t.TargetType.GetMethod("op_Implicit", new Type[1]
      {
        t.InitialType
      });
      if (method == (MethodInfo) null)
        method = t.TargetType.GetMethod("op_Explicit", new Type[1]
        {
          t.InitialType
        });
      if (method == (MethodInfo) null)
        return (Func<object, object>) null;
      MethodCall<object, object> call = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>((MethodBase) method);
      return (Func<object, object>) (o => call((object) null, new object[1]
      {
        o
      }));
    }

    internal static BigInteger ToBigInteger(object value)
    {
      switch (value)
      {
        case BigInteger bigInteger:
          return bigInteger;
        case string _:
          return BigInteger.Parse((string) value, (IFormatProvider) CultureInfo.InvariantCulture);
        case float num1:
          return new BigInteger(num1);
        case double num2:
          return new BigInteger(num2);
        case Decimal num3:
          return new BigInteger(num3);
        case int num4:
          return new BigInteger(num4);
        case long num5:
          return new BigInteger(num5);
        case uint num6:
          return new BigInteger(num6);
        case ulong num7:
          return new BigInteger(num7);
        case byte[] _:
          return new BigInteger((byte[]) value);
        default:
          throw new InvalidCastException("Cannot convert {0} to BigInteger.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) value.GetType()));
      }
    }

    public static object FromBigInteger(BigInteger i, Type targetType)
    {
      if (targetType == typeof (Decimal))
        return (object) (Decimal) i;
      if (targetType == typeof (double))
        return (object) (double) i;
      if (targetType == typeof (float))
        return (object) (float) i;
      if (targetType == typeof (ulong))
        return (object) (ulong) i;
      try
      {
        return System.Convert.ChangeType((object) (long) i, targetType, (IFormatProvider) CultureInfo.InvariantCulture);
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException("Can not convert from BigInteger to {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) targetType), ex);
      }
    }

    public static object Convert(object initialValue, CultureInfo culture, Type targetType)
    {
      object obj;
      switch (ConvertUtils.TryConvertInternal(initialValue, culture, targetType, out obj))
      {
        case ConvertUtils.ConvertResult.Success:
          return obj;
        case ConvertUtils.ConvertResult.CannotConvertNull:
          throw new Exception("Can not convert null {0} into non-nullable {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) initialValue.GetType(), (object) targetType));
        case ConvertUtils.ConvertResult.NotInstantiableType:
          throw new ArgumentException("Target type {0} is not a value type or a non-abstract class.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) targetType), nameof (targetType));
        case ConvertUtils.ConvertResult.NoValidConversion:
          throw new InvalidOperationException("Can not convert from {0} to {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) initialValue.GetType(), (object) targetType));
        default:
          throw new InvalidOperationException("Unexpected conversion result.");
      }
    }

    private static bool TryConvert(
      object initialValue,
      CultureInfo culture,
      Type targetType,
      out object value)
    {
      try
      {
        if (ConvertUtils.TryConvertInternal(initialValue, culture, targetType, out value) == ConvertUtils.ConvertResult.Success)
          return true;
        value = (object) null;
        return false;
      }
      catch
      {
        value = (object) null;
        return false;
      }
    }

    private static ConvertUtils.ConvertResult TryConvertInternal(
      object initialValue,
      CultureInfo culture,
      Type targetType,
      out object value)
    {
      if (initialValue == null)
        throw new ArgumentNullException(nameof (initialValue));
      if (ReflectionUtils.IsNullableType(targetType))
        targetType = Nullable.GetUnderlyingType(targetType);
      Type type = initialValue.GetType();
      if (targetType == type)
      {
        value = initialValue;
        return ConvertUtils.ConvertResult.Success;
      }
      if (ConvertUtils.IsConvertible(initialValue.GetType()) && ConvertUtils.IsConvertible(targetType))
      {
        if (targetType.IsEnum())
        {
          if (initialValue is string)
          {
            value = Enum.Parse(targetType, initialValue.ToString(), true);
            return ConvertUtils.ConvertResult.Success;
          }
          if (ConvertUtils.IsInteger(initialValue))
          {
            value = Enum.ToObject(targetType, initialValue);
            return ConvertUtils.ConvertResult.Success;
          }
        }
        value = System.Convert.ChangeType(initialValue, targetType, (IFormatProvider) culture);
        return ConvertUtils.ConvertResult.Success;
      }
      switch (initialValue)
      {
        case DateTime _ when targetType == typeof (DateTimeOffset):
          value = (object) new DateTimeOffset((DateTime) initialValue);
          return ConvertUtils.ConvertResult.Success;
        case byte[] _ when targetType == typeof (Guid):
          value = (object) new Guid((byte[]) initialValue);
          return ConvertUtils.ConvertResult.Success;
        case Guid _ when targetType == typeof (byte[]):
          value = (object) ((Guid) initialValue).ToByteArray();
          return ConvertUtils.ConvertResult.Success;
        case string _:
          if (targetType == typeof (Guid))
          {
            value = (object) new Guid((string) initialValue);
            return ConvertUtils.ConvertResult.Success;
          }
          if (targetType == typeof (Uri))
          {
            value = (object) new Uri((string) initialValue, UriKind.RelativeOrAbsolute);
            return ConvertUtils.ConvertResult.Success;
          }
          if (targetType == typeof (TimeSpan))
          {
            value = (object) ConvertUtils.ParseTimeSpan((string) initialValue);
            return ConvertUtils.ConvertResult.Success;
          }
          if (targetType == typeof (byte[]))
          {
            value = (object) System.Convert.FromBase64String((string) initialValue);
            return ConvertUtils.ConvertResult.Success;
          }
          if (typeof (Type).IsAssignableFrom(targetType))
          {
            value = (object) Type.GetType((string) initialValue, true);
            return ConvertUtils.ConvertResult.Success;
          }
          break;
      }
      if (targetType == typeof (BigInteger))
      {
        value = (object) ConvertUtils.ToBigInteger(initialValue);
        return ConvertUtils.ConvertResult.Success;
      }
      if (initialValue is BigInteger i)
      {
        value = ConvertUtils.FromBigInteger(i, targetType);
        return ConvertUtils.ConvertResult.Success;
      }
      TypeConverter converter1 = ConvertUtils.GetConverter(type);
      if (converter1 != null && converter1.CanConvertTo(targetType))
      {
        value = converter1.ConvertTo((ITypeDescriptorContext) null, culture, initialValue, targetType);
        return ConvertUtils.ConvertResult.Success;
      }
      TypeConverter converter2 = ConvertUtils.GetConverter(targetType);
      if (converter2 != null && converter2.CanConvertFrom(type))
      {
        value = converter2.ConvertFrom((ITypeDescriptorContext) null, culture, initialValue);
        return ConvertUtils.ConvertResult.Success;
      }
      if (initialValue == DBNull.Value)
      {
        if (ReflectionUtils.IsNullable(targetType))
        {
          value = ConvertUtils.EnsureTypeAssignable((object) null, type, targetType);
          return ConvertUtils.ConvertResult.Success;
        }
        value = (object) null;
        return ConvertUtils.ConvertResult.CannotConvertNull;
      }
      if (initialValue is INullable)
      {
        value = ConvertUtils.EnsureTypeAssignable(ConvertUtils.ToValue((INullable) initialValue), type, targetType);
        return ConvertUtils.ConvertResult.Success;
      }
      if (targetType.IsInterface() || targetType.IsGenericTypeDefinition() || targetType.IsAbstract())
      {
        value = (object) null;
        return ConvertUtils.ConvertResult.NotInstantiableType;
      }
      value = (object) null;
      return ConvertUtils.ConvertResult.NoValidConversion;
    }

    public static object ConvertOrCast(object initialValue, CultureInfo culture, Type targetType)
    {
      if (targetType == typeof (object))
        return initialValue;
      if (initialValue == null && ReflectionUtils.IsNullable(targetType))
        return (object) null;
      object obj;
      return ConvertUtils.TryConvert(initialValue, culture, targetType, out obj) ? obj : ConvertUtils.EnsureTypeAssignable(initialValue, ReflectionUtils.GetObjectType(initialValue), targetType);
    }

    private static object EnsureTypeAssignable(object value, Type initialType, Type targetType)
    {
      Type type = value?.GetType();
      if (value != null)
      {
        if (targetType.IsAssignableFrom(type))
          return value;
        Func<object, object> func = ConvertUtils.CastConverters.Get(new ConvertUtils.TypeConvertKey(type, targetType));
        if (func != null)
          return func(value);
      }
      else if (ReflectionUtils.IsNullable(targetType))
        return (object) null;
      throw new ArgumentException("Could not cast or convert from {0} to {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, initialType != (Type) null ? (object) initialType.ToString() : (object) "{null}", (object) targetType));
    }

    public static object ToValue(INullable nullableValue)
    {
      switch (nullableValue)
      {
        case null:
          return (object) null;
        case SqlInt32 sqlInt32:
          return ConvertUtils.ToValue((INullable) sqlInt32);
        case SqlInt64 sqlInt64:
          return ConvertUtils.ToValue((INullable) sqlInt64);
        case SqlBoolean sqlBoolean:
          return ConvertUtils.ToValue((INullable) sqlBoolean);
        case SqlString sqlString:
          return ConvertUtils.ToValue((INullable) sqlString);
        case SqlDateTime sqlDateTime:
          return ConvertUtils.ToValue((INullable) sqlDateTime);
        default:
          throw new ArgumentException("Unsupported INullable type: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) nullableValue.GetType()));
      }
    }

    internal static TypeConverter GetConverter(Type t) => JsonTypeReflector.GetTypeConverter(t);

    public static bool IsInteger(object value)
    {
      switch (ConvertUtils.GetTypeCode(value.GetType()))
      {
        case PrimitiveTypeCode.SByte:
        case PrimitiveTypeCode.Int16:
        case PrimitiveTypeCode.UInt16:
        case PrimitiveTypeCode.Int32:
        case PrimitiveTypeCode.Byte:
        case PrimitiveTypeCode.UInt32:
        case PrimitiveTypeCode.Int64:
        case PrimitiveTypeCode.UInt64:
          return true;
        default:
          return false;
      }
    }

    public static ParseResult Int32TryParse(
      char[] chars,
      int start,
      int length,
      out int value)
    {
      value = 0;
      if (length == 0)
        return ParseResult.Invalid;
      bool flag = chars[start] == '-';
      if (flag)
      {
        if (length == 1)
          return ParseResult.Invalid;
        ++start;
        --length;
      }
      int num1 = start + length;
      for (int index1 = start; index1 < num1; ++index1)
      {
        int num2 = (int) chars[index1] - 48;
        if (num2 < 0 || num2 > 9)
          return ParseResult.Invalid;
        int num3 = 10 * value - num2;
        if (num3 > value)
        {
          for (int index2 = index1 + 1; index2 < num1; ++index2)
          {
            int num4 = (int) chars[index2] - 48;
            if (num4 < 0 || num4 > 9)
              return ParseResult.Invalid;
          }
          return ParseResult.Overflow;
        }
        value = num3;
      }
      if (!flag)
      {
        if (value == int.MinValue)
          return ParseResult.Overflow;
        value = -value;
      }
      return ParseResult.Success;
    }

    public static ParseResult Int64TryParse(
      char[] chars,
      int start,
      int length,
      out long value)
    {
      value = 0L;
      if (length == 0)
        return ParseResult.Invalid;
      bool flag = chars[start] == '-';
      if (flag)
      {
        if (length == 1)
          return ParseResult.Invalid;
        ++start;
        --length;
      }
      int num1 = start + length;
      for (int index1 = start; index1 < num1; ++index1)
      {
        int num2 = (int) chars[index1] - 48;
        if (num2 < 0 || num2 > 9)
          return ParseResult.Invalid;
        long num3 = 10L * value - (long) num2;
        if (num3 > value)
        {
          for (int index2 = index1 + 1; index2 < num1; ++index2)
          {
            int num4 = (int) chars[index2] - 48;
            if (num4 < 0 || num4 > 9)
              return ParseResult.Invalid;
          }
          return ParseResult.Overflow;
        }
        value = num3;
      }
      if (!flag)
      {
        if (value == long.MinValue)
          return ParseResult.Overflow;
        value = -value;
      }
      return ParseResult.Success;
    }

    public static bool TryConvertGuid(string s, out Guid g) => Guid.TryParseExact(s, "D", out g);

    internal struct TypeConvertKey : IEquatable<ConvertUtils.TypeConvertKey>
    {
      private readonly Type _initialType;
      private readonly Type _targetType;

      public Type InitialType => this._initialType;

      public Type TargetType => this._targetType;

      public TypeConvertKey(Type initialType, Type targetType)
      {
        this._initialType = initialType;
        this._targetType = targetType;
      }

      public override int GetHashCode() => this._initialType.GetHashCode() ^ this._targetType.GetHashCode();

      public override bool Equals(object obj) => obj is ConvertUtils.TypeConvertKey other && this.Equals(other);

      public bool Equals(ConvertUtils.TypeConvertKey other) => this._initialType == other._initialType && this._targetType == other._targetType;
    }

    internal enum ConvertResult
    {
      Success,
      CannotConvertNull,
      NotInstantiableType,
      NoValidConversion,
    }
  }
}
