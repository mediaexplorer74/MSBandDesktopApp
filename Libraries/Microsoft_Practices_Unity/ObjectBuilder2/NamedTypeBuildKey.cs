// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.NamedTypeBuildKey
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using System;
using System.Globalization;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class NamedTypeBuildKey
  {
    private readonly Type type;
    private readonly string name;

    public NamedTypeBuildKey(Type type, string name)
    {
      this.type = type;
      this.name = !string.IsNullOrEmpty(name) ? name : (string) null;
    }

    public NamedTypeBuildKey(Type type)
      : this(type, (string) null)
    {
    }

    public static NamedTypeBuildKey Make<T>() => new NamedTypeBuildKey(typeof (T));

    public static NamedTypeBuildKey Make<T>(string name) => new NamedTypeBuildKey(typeof (T), name);

    public Type Type => this.type;

    public string Name => this.name;

    public override bool Equals(object obj)
    {
      NamedTypeBuildKey namedTypeBuildKey = obj as NamedTypeBuildKey;
      return !(namedTypeBuildKey == (NamedTypeBuildKey) null) && this == namedTypeBuildKey;
    }

    public override int GetHashCode() => ((object) this.type == null ? 0 : this.type.GetHashCode()) + 37 ^ (this.name == null ? 0 : this.name.GetHashCode()) + 17;

    public static bool operator ==(NamedTypeBuildKey left, NamedTypeBuildKey right)
    {
      bool flag1 = object.ReferenceEquals((object) left, (object) null);
      bool flag2 = object.ReferenceEquals((object) right, (object) null);
      if (flag1 && flag2)
        return true;
      return !flag1 && !flag2 && (object) left.type == (object) right.type && string.Compare(left.name, right.name, StringComparison.Ordinal) == 0;
    }

    public static bool operator !=(NamedTypeBuildKey left, NamedTypeBuildKey right) => !(left == right);

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Build Key[{0}, {1}]", new object[2]
    {
      (object) this.type,
      (object) (this.name ?? "null")
    });
  }
}
