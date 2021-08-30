// Decompiled with JetBrains decompiler
// Type: SQLite.IndexedAttribute
// Assembly: SQLite-net, Version=1.0.9.0, Culture=neutral, PublicKeyToken=null
// MVID: 3C087C9E-3E7A-4EB5-8100-289515B40443
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\SQLite_net.dll

using System;

namespace SQLite
{
  [AttributeUsage(AttributeTargets.Property)]
  public class IndexedAttribute : Attribute
  {
    public IndexedAttribute()
    {
    }

    public IndexedAttribute(string name, int order)
    {
      this.Name = name;
      this.Order = order;
    }

    public string Name { get; set; }

    public int Order { get; set; }

    public virtual bool Unique { get; set; }
  }
}
