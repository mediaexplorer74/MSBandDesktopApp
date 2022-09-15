// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Json.JsonAssert
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;

namespace Microsoft.Health.Cloud.Client.Json
{
  public static class JsonAssert
  {
    public static void PropertyIsNotNullOrWhiteSpace(string propertyName, string value) => JsonAssert.IsTrue((!string.IsNullOrWhiteSpace(value) ? 1 : 0) != 0, "Property '{0}' should not be null", (object) propertyName);

    public static void IsTrue(bool value, string messageFormat, params object[] args)
    {
      if (!value)
        throw new InvalidJsonException(args == null || args.Length == 0 ? messageFormat : string.Format(messageFormat, args));
    }

    public static void PropertyIsGreaterOrEqualToZero(string propertyName, int value) => JsonAssert.IsTrue((value >= 0 ? 1 : 0) != 0, "Property '{0}' cannot have a negative value: {1}", (object) propertyName, (object) value);

    public static void PropertyIsGreaterOrEqualToZero(string propertyName, double value) => JsonAssert.IsTrue((value >= 0.0 ? 1 : 0) != 0, "Property '{0}' cannot have a negative value: {1}", (object) propertyName, (object) value);

    public static void PropertyIsGreaterOrEqualToZero(string propertyName, Length value) => JsonAssert.IsTrue((value.TotalMillimeters >= 0.0 ? 1 : 0) != 0, "Property '{0}' cannot have a negative value: {1}", (object) propertyName, (object) value);

    public static void PropertyIsGreaterOrEqualToZero(string propertyName, Weight value) => JsonAssert.IsTrue((value.TotalGrams >= 0.0 ? 1 : 0) != 0, "Property '{0}' cannot have a negative value: {1}", (object) propertyName, (object) value);

    public static void PropertyIsGreaterOrEqualToZero(string propertyName, TimeSpan value) => JsonAssert.IsTrue((value.TotalMilliseconds >= 0.0 ? 1 : 0) != 0, "Property '{0}' cannot have a negative value: {1}", (object) propertyName, (object) value);

    public static void PropertyIsGreaterOrEqualToZero(string propertyName, Speed value) => JsonAssert.IsTrue((value.MillisecondsPerKilometer >= 0.0 ? 1 : 0) != 0, "Property '{0}' cannot have a negative value: {1}", (object) propertyName, (object) value);

    public static void PropertyIsWithinRange(
      string propertyName,
      double value,
      double minValue,
      double maxValue)
    {
      JsonAssert.IsTrue((minValue > value ? 0 : (value <= maxValue ? 1 : 0)) != 0, "Property '{0}' is outside of valid bounds [{2}, {3}]: {1}", (object) propertyName, (object) value, (object) minValue, (object) maxValue);
    }

    public static void PropertyIsWithinRange(
      string propertyName,
      int value,
      int minValue,
      int maxValue)
    {
      JsonAssert.IsTrue((minValue > value ? 0 : (value <= maxValue ? 1 : 0)) != 0, "Property '{0}' is outside of valid bounds [{2}, {3}]: {1}", (object) propertyName, (object) value, (object) minValue, (object) maxValue);
    }

    public static void PropertyIsGreaterThanZero(string propertyName, long value) => JsonAssert.IsTrue((value > 0L ? 1 : 0) != 0, "Property '{0}' should be greater than zero: {1}", (object) propertyName, (object) value);

    public static void PropertyIsNotNull(string propertyName, object value) => JsonAssert.IsTrue((value != null ? 1 : 0) != 0, "Property '{0}' should not be null", (object) propertyName, value);

    public static void DateTimePropertyIsSet(string propertyName, DateTimeOffset value) => JsonAssert.IsTrue((value != DateTimeOffset.MinValue ? 1 : 0) != 0, "Property '{0}' was not set to a valid value: {1}", (object) propertyName, (object) value);
  }
}
