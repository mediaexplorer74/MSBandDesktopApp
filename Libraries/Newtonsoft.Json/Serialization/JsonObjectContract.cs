// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonObjectContract
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security;

namespace Newtonsoft.Json.Serialization
{
  public class JsonObjectContract : JsonContainerContract
  {
    private bool? _hasRequiredOrDefaultValueProperties;
    private ConstructorInfo _parametrizedConstructor;
    private ConstructorInfo _overrideConstructor;
    private ObjectConstructor<object> _overrideCreator;
    private ObjectConstructor<object> _parametrizedCreator;

    public MemberSerialization MemberSerialization { get; set; }

    public Required? ItemRequired { get; set; }

    public JsonPropertyCollection Properties { get; private set; }

    [Obsolete("ConstructorParameters is obsolete. Use CreatorParameters instead.")]
    public JsonPropertyCollection ConstructorParameters => this.CreatorParameters;

    public JsonPropertyCollection CreatorParameters { get; private set; }

    [Obsolete("OverrideConstructor is obsolete. Use OverrideCreator instead.")]
    public ConstructorInfo OverrideConstructor
    {
      get => this._overrideConstructor;
      set
      {
        this._overrideConstructor = value;
        this._overrideCreator = value != (ConstructorInfo) null ? JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor((MethodBase) value) : (ObjectConstructor<object>) null;
      }
    }

    [Obsolete("ParametrizedConstructor is obsolete. Use OverrideCreator instead.")]
    public ConstructorInfo ParametrizedConstructor
    {
      get => this._parametrizedConstructor;
      set
      {
        this._parametrizedConstructor = value;
        this._parametrizedCreator = value != (ConstructorInfo) null ? JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor((MethodBase) value) : (ObjectConstructor<object>) null;
      }
    }

    public ObjectConstructor<object> OverrideCreator
    {
      get => this._overrideCreator;
      set
      {
        this._overrideCreator = value;
        this._overrideConstructor = (ConstructorInfo) null;
      }
    }

    internal ObjectConstructor<object> ParametrizedCreator => this._parametrizedCreator;

    public ExtensionDataSetter ExtensionDataSetter { get; set; }

    public ExtensionDataGetter ExtensionDataGetter { get; set; }

    internal bool HasRequiredOrDefaultValueProperties
    {
      get
      {
        if (!this._hasRequiredOrDefaultValueProperties.HasValue)
        {
          this._hasRequiredOrDefaultValueProperties = new bool?(false);
          if (this.ItemRequired.GetValueOrDefault(Required.Default) != Required.Default)
          {
            this._hasRequiredOrDefaultValueProperties = new bool?(true);
          }
          else
          {
            foreach (JsonProperty property in (Collection<JsonProperty>) this.Properties)
            {
              if (property.Required == Required.Default)
              {
                DefaultValueHandling? defaultValueHandling = property.DefaultValueHandling;
                DefaultValueHandling? nullable = defaultValueHandling.HasValue ? new DefaultValueHandling?(defaultValueHandling.GetValueOrDefault() & DefaultValueHandling.Populate) : new DefaultValueHandling?();
                if ((nullable.GetValueOrDefault() != DefaultValueHandling.Populate ? 0 : (nullable.HasValue ? 1 : 0)) == 0)
                  continue;
              }
              this._hasRequiredOrDefaultValueProperties = new bool?(true);
              break;
            }
          }
        }
        return this._hasRequiredOrDefaultValueProperties.Value;
      }
    }

    public JsonObjectContract(Type underlyingType)
      : base(underlyingType)
    {
      this.ContractType = JsonContractType.Object;
      this.Properties = new JsonPropertyCollection(this.UnderlyingType);
      this.CreatorParameters = new JsonPropertyCollection(this.UnderlyingType);
    }

    [SecuritySafeCritical]
    internal object GetUninitializedObject()
    {
      if (!JsonTypeReflector.FullyTrusted)
        throw new JsonException("Insufficient permissions. Creating an uninitialized '{0}' type requires full trust.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.NonNullableUnderlyingType));
      return FormatterServices.GetUninitializedObject(this.NonNullableUnderlyingType);
    }
  }
}
