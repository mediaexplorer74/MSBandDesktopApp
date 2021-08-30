// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonArrayContract
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Newtonsoft.Json.Serialization
{
  public class JsonArrayContract : JsonContainerContract
  {
    private readonly Type _genericCollectionDefinitionType;
    private Type _genericWrapperType;
    private ObjectConstructor<object> _genericWrapperCreator;
    private Func<object> _genericTemporaryCollectionCreator;
    private readonly ConstructorInfo _parametrizedConstructor;
    private ObjectConstructor<object> _parametrizedCreator;

    public Type CollectionItemType { get; private set; }

    public bool IsMultidimensionalArray { get; private set; }

    internal bool IsArray { get; private set; }

    internal bool ShouldCreateWrapper { get; private set; }

    internal bool CanDeserialize { get; private set; }

    internal ObjectConstructor<object> ParametrizedCreator
    {
      get
      {
        if (this._parametrizedCreator == null)
          this._parametrizedCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor((MethodBase) this._parametrizedConstructor);
        return this._parametrizedCreator;
      }
    }

    internal bool HasParametrizedCreator => this._parametrizedCreator != null || this._parametrizedConstructor != (ConstructorInfo) null;

    public JsonArrayContract(Type underlyingType)
      : base(underlyingType)
    {
      this.ContractType = JsonContractType.Array;
      this.IsArray = this.CreatedType.IsArray;
      bool flag;
      if (this.IsArray)
      {
        this.CollectionItemType = ReflectionUtils.GetCollectionItemType(this.UnderlyingType);
        this.IsReadOnlyOrFixedSize = true;
        this._genericCollectionDefinitionType = typeof (List<>).MakeGenericType(this.CollectionItemType);
        flag = true;
        this.IsMultidimensionalArray = this.IsArray && this.UnderlyingType.GetArrayRank() > 1;
      }
      else if (typeof (IList).IsAssignableFrom(underlyingType))
      {
        this.CollectionItemType = !ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof (ICollection<>), out this._genericCollectionDefinitionType) ? ReflectionUtils.GetCollectionItemType(underlyingType) : this._genericCollectionDefinitionType.GetGenericArguments()[0];
        if (underlyingType == typeof (IList))
          this.CreatedType = typeof (List<object>);
        if (this.CollectionItemType != (Type) null)
          this._parametrizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(underlyingType, this.CollectionItemType);
        this.IsReadOnlyOrFixedSize = ReflectionUtils.InheritsGenericDefinition(underlyingType, typeof (ReadOnlyCollection<>));
        flag = true;
      }
      else if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof (ICollection<>), out this._genericCollectionDefinitionType))
      {
        this.CollectionItemType = this._genericCollectionDefinitionType.GetGenericArguments()[0];
        if (ReflectionUtils.IsGenericDefinition(underlyingType, typeof (ICollection<>)) || ReflectionUtils.IsGenericDefinition(underlyingType, typeof (IList<>)))
          this.CreatedType = typeof (List<>).MakeGenericType(this.CollectionItemType);
        if (ReflectionUtils.IsGenericDefinition(underlyingType, typeof (ISet<>)))
          this.CreatedType = typeof (HashSet<>).MakeGenericType(this.CollectionItemType);
        this._parametrizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(underlyingType, this.CollectionItemType);
        flag = true;
        this.ShouldCreateWrapper = true;
      }
      else
      {
        Type implementingType;
        if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof (IReadOnlyCollection<>), out implementingType))
        {
          this.CollectionItemType = implementingType.GetGenericArguments()[0];
          if (ReflectionUtils.IsGenericDefinition(underlyingType, typeof (IReadOnlyCollection<>)) || ReflectionUtils.IsGenericDefinition(underlyingType, typeof (IReadOnlyList<>)))
            this.CreatedType = typeof (ReadOnlyCollection<>).MakeGenericType(this.CollectionItemType);
          this._genericCollectionDefinitionType = typeof (List<>).MakeGenericType(this.CollectionItemType);
          this._parametrizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(this.CreatedType, this.CollectionItemType);
          this.IsReadOnlyOrFixedSize = true;
          flag = this.HasParametrizedCreator;
        }
        else if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof (IEnumerable<>), out implementingType))
        {
          this.CollectionItemType = implementingType.GetGenericArguments()[0];
          if (ReflectionUtils.IsGenericDefinition(this.UnderlyingType, typeof (IEnumerable<>)))
            this.CreatedType = typeof (List<>).MakeGenericType(this.CollectionItemType);
          this._parametrizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(underlyingType, this.CollectionItemType);
          if (!this.HasParametrizedCreator && underlyingType.Name == "FSharpList`1")
          {
            FSharpUtils.EnsureInitialized(underlyingType.Assembly());
            this._parametrizedCreator = FSharpUtils.CreateSeq(this.CollectionItemType);
          }
          if (underlyingType.IsGenericType() && underlyingType.GetGenericTypeDefinition() == typeof (IEnumerable<>))
          {
            this._genericCollectionDefinitionType = implementingType;
            this.IsReadOnlyOrFixedSize = false;
            this.ShouldCreateWrapper = false;
            flag = true;
          }
          else
          {
            this._genericCollectionDefinitionType = typeof (List<>).MakeGenericType(this.CollectionItemType);
            this.IsReadOnlyOrFixedSize = true;
            this.ShouldCreateWrapper = true;
            flag = this.HasParametrizedCreator;
          }
        }
        else
        {
          flag = false;
          this.ShouldCreateWrapper = true;
        }
      }
      this.CanDeserialize = flag;
      Type createdType;
      ObjectConstructor<object> parameterizedCreator;
      if (!ImmutableCollectionsUtils.TryBuildImmutableForArrayContract(underlyingType, this.CollectionItemType, out createdType, out parameterizedCreator))
        return;
      this.CreatedType = createdType;
      this._parametrizedCreator = parameterizedCreator;
      this.IsReadOnlyOrFixedSize = true;
      this.CanDeserialize = true;
    }

    internal IWrappedCollection CreateWrapper(object list)
    {
      if (this._genericWrapperCreator == null)
      {
        this._genericWrapperType = typeof (CollectionWrapper<>).MakeGenericType(this.CollectionItemType);
        Type type;
        if (ReflectionUtils.InheritsGenericDefinition(this._genericCollectionDefinitionType, typeof (List<>)) || this._genericCollectionDefinitionType.GetGenericTypeDefinition() == typeof (IEnumerable<>))
          type = typeof (ICollection<>).MakeGenericType(this.CollectionItemType);
        else
          type = this._genericCollectionDefinitionType;
        this._genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor((MethodBase) this._genericWrapperType.GetConstructor(new Type[1]
        {
          type
        }));
      }
      return (IWrappedCollection) this._genericWrapperCreator(new object[1]
      {
        list
      });
    }

    internal IList CreateTemporaryCollection()
    {
      if (this._genericTemporaryCollectionCreator == null)
        this._genericTemporaryCollectionCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(typeof (List<>).MakeGenericType(this.IsMultidimensionalArray ? typeof (object) : this.CollectionItemType));
      return (IList) this._genericTemporaryCollectionCreator();
    }
  }
}
