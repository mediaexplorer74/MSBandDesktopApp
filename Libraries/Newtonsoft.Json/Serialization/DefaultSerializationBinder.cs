// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.DefaultSerializationBinder
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
  public class DefaultSerializationBinder : SerializationBinder
  {
    internal static readonly DefaultSerializationBinder Instance = new DefaultSerializationBinder();
    private readonly ThreadSafeStore<DefaultSerializationBinder.TypeNameKey, Type> _typeCache = new ThreadSafeStore<DefaultSerializationBinder.TypeNameKey, Type>(new Func<DefaultSerializationBinder.TypeNameKey, Type>(DefaultSerializationBinder.GetTypeFromTypeNameKey));

    private static Type GetTypeFromTypeNameKey(
      DefaultSerializationBinder.TypeNameKey typeNameKey)
    {
      string assemblyName = typeNameKey.AssemblyName;
      string typeName = typeNameKey.TypeName;
      if (assemblyName == null)
        return Type.GetType(typeName);
      Assembly assembly1 = Assembly.LoadWithPartialName(assemblyName);
      if (assembly1 == (Assembly) null)
      {
        foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
        {
          if (assembly2.FullName == assemblyName)
          {
            assembly1 = assembly2;
            break;
          }
        }
      }
      Type type = !(assembly1 == (Assembly) null) ? assembly1.GetType(typeName) : throw new JsonSerializationException("Could not load assembly '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) assemblyName));
      return !(type == (Type) null) ? type : throw new JsonSerializationException("Could not find type '{0}' in assembly '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) typeName, (object) assembly1.FullName));
    }

    public override Type BindToType(string assemblyName, string typeName) => this._typeCache.Get(new DefaultSerializationBinder.TypeNameKey(assemblyName, typeName));

    public override void BindToName(
      Type serializedType,
      out string assemblyName,
      out string typeName)
    {
      assemblyName = serializedType.Assembly.FullName;
      typeName = serializedType.FullName;
    }

    internal struct TypeNameKey : IEquatable<DefaultSerializationBinder.TypeNameKey>
    {
      internal readonly string AssemblyName;
      internal readonly string TypeName;

      public TypeNameKey(string assemblyName, string typeName)
      {
        this.AssemblyName = assemblyName;
        this.TypeName = typeName;
      }

      public override int GetHashCode() => (this.AssemblyName != null ? this.AssemblyName.GetHashCode() : 0) ^ (this.TypeName != null ? this.TypeName.GetHashCode() : 0);

      public override bool Equals(object obj) => obj is DefaultSerializationBinder.TypeNameKey other && this.Equals(other);

      public bool Equals(DefaultSerializationBinder.TypeNameKey other) => this.AssemblyName == other.AssemblyName && this.TypeName == other.TypeName;
    }
  }
}
