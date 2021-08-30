// Decompiled with JetBrains decompiler
// Type: SQLite.Orm
// Assembly: SQLite-net, Version=1.0.9.0, Culture=neutral, PublicKeyToken=null
// MVID: 3C087C9E-3E7A-4EB5-8100-289515B40443
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\SQLite_net.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SQLite
{
  public static class Orm
  {
    public const int DefaultMaxStringLength = 140;
    public const string ImplicitPkName = "Id";
    public const string ImplicitIndexSuffix = "Id";

    public static string SqlDecl(TableMapping.Column p, bool storeDateTimeAsTicks)
    {
      string str = "\"" + p.Name + "\" " + Orm.SqlType(p, storeDateTimeAsTicks) + " ";
      if (p.IsPK)
        str += "primary key ";
      if (p.IsAutoInc)
        str += "autoincrement ";
      if (!p.IsNullable)
        str += "not null ";
      if (!string.IsNullOrEmpty(p.Collation))
        str = str + "collate " + p.Collation + " ";
      return str;
    }

    public static string SqlType(TableMapping.Column p, bool storeDateTimeAsTicks)
    {
      Type columnType = p.ColumnType;
      if ((object) columnType == (object) typeof (bool) || (object) columnType == (object) typeof (byte) || (object) columnType == (object) typeof (ushort) || (object) columnType == (object) typeof (sbyte) || (object) columnType == (object) typeof (short) || (object) columnType == (object) typeof (int))
        return "integer";
      if ((object) columnType == (object) typeof (uint) || (object) columnType == (object) typeof (long))
        return "bigint";
      if ((object) columnType == (object) typeof (float) || (object) columnType == (object) typeof (double) || (object) columnType == (object) typeof (Decimal))
        return "float";
      if ((object) columnType == (object) typeof (string))
      {
        int? maxStringLength = p.MaxStringLength;
        return maxStringLength.HasValue ? "varchar(" + (object) maxStringLength.Value + ")" : "varchar";
      }
      if ((object) columnType == (object) typeof (TimeSpan))
        return "bigint";
      if ((object) columnType == (object) typeof (DateTime))
        return storeDateTimeAsTicks ? "bigint" : "datetime";
      if ((object) columnType == (object) typeof (DateTimeOffset))
        return "bigint";
      if (columnType.GetTypeInfo().IsEnum)
        return "integer";
      if ((object) columnType == (object) typeof (byte[]))
        return "blob";
      if ((object) columnType == (object) typeof (Guid))
        return "varchar(36)";
      throw new NotSupportedException("Don't know about " + (object) columnType);
    }

    public static bool IsPK(MemberInfo p) => CustomAttributeExtensions.GetCustomAttributes(p, typeof (PrimaryKeyAttribute), true).Count<Attribute>() > 0;

    public static string Collation(MemberInfo p)
    {
      IEnumerable<Attribute> customAttributes = CustomAttributeExtensions.GetCustomAttributes(p, typeof (CollationAttribute), true);
      return customAttributes.Count<Attribute>() > 0 ? ((CollationAttribute) customAttributes.First<Attribute>()).Value : string.Empty;
    }

    public static bool IsAutoInc(MemberInfo p) => CustomAttributeExtensions.GetCustomAttributes(p, typeof (AutoIncrementAttribute), true).Count<Attribute>() > 0;

    public static IEnumerable<IndexedAttribute> GetIndices(MemberInfo p) => CustomAttributeExtensions.GetCustomAttributes(p, typeof (IndexedAttribute), true).Cast<IndexedAttribute>();

    public static int? MaxStringLength(PropertyInfo p)
    {
      IEnumerable<Attribute> customAttributes = CustomAttributeExtensions.GetCustomAttributes(p, typeof (MaxLengthAttribute), true);
      return customAttributes.Count<Attribute>() > 0 ? new int?(((MaxLengthAttribute) customAttributes.First<Attribute>()).Value) : new int?();
    }

    public static bool IsMarkedNotNull(MemberInfo p) => CustomAttributeExtensions.GetCustomAttributes(p, typeof (NotNullAttribute), true).Count<Attribute>() > 0;
  }
}
