// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonSerializerSettings
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;

namespace Newtonsoft.Json
{
  public class JsonSerializerSettings
  {
    internal const ReferenceLoopHandling DefaultReferenceLoopHandling = ReferenceLoopHandling.Error;
    internal const MissingMemberHandling DefaultMissingMemberHandling = MissingMemberHandling.Ignore;
    internal const NullValueHandling DefaultNullValueHandling = NullValueHandling.Include;
    internal const DefaultValueHandling DefaultDefaultValueHandling = DefaultValueHandling.Include;
    internal const ObjectCreationHandling DefaultObjectCreationHandling = ObjectCreationHandling.Auto;
    internal const PreserveReferencesHandling DefaultPreserveReferencesHandling = PreserveReferencesHandling.None;
    internal const ConstructorHandling DefaultConstructorHandling = ConstructorHandling.Default;
    internal const TypeNameHandling DefaultTypeNameHandling = TypeNameHandling.None;
    internal const MetadataPropertyHandling DefaultMetadataPropertyHandling = MetadataPropertyHandling.Default;
    internal const FormatterAssemblyStyle DefaultTypeNameAssemblyFormat = FormatterAssemblyStyle.Simple;
    internal const Formatting DefaultFormatting = Formatting.None;
    internal const DateFormatHandling DefaultDateFormatHandling = DateFormatHandling.IsoDateFormat;
    internal const DateTimeZoneHandling DefaultDateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
    internal const DateParseHandling DefaultDateParseHandling = DateParseHandling.DateTime;
    internal const FloatParseHandling DefaultFloatParseHandling = FloatParseHandling.Double;
    internal const FloatFormatHandling DefaultFloatFormatHandling = FloatFormatHandling.String;
    internal const StringEscapeHandling DefaultStringEscapeHandling = StringEscapeHandling.Default;
    internal const FormatterAssemblyStyle DefaultFormatterAssemblyStyle = FormatterAssemblyStyle.Simple;
    internal const bool DefaultCheckAdditionalContent = false;
    internal const string DefaultDateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
    internal static readonly StreamingContext DefaultContext = new StreamingContext();
    internal static readonly CultureInfo DefaultCulture = CultureInfo.InvariantCulture;
    internal Formatting? _formatting;
    internal DateFormatHandling? _dateFormatHandling;
    internal DateTimeZoneHandling? _dateTimeZoneHandling;
    internal DateParseHandling? _dateParseHandling;
    internal FloatFormatHandling? _floatFormatHandling;
    internal FloatParseHandling? _floatParseHandling;
    internal StringEscapeHandling? _stringEscapeHandling;
    internal CultureInfo _culture;
    internal bool? _checkAdditionalContent;
    internal int? _maxDepth;
    internal bool _maxDepthSet;
    internal string _dateFormatString;
    internal bool _dateFormatStringSet;
    internal FormatterAssemblyStyle? _typeNameAssemblyFormat;
    internal DefaultValueHandling? _defaultValueHandling;
    internal PreserveReferencesHandling? _preserveReferencesHandling;
    internal NullValueHandling? _nullValueHandling;
    internal ObjectCreationHandling? _objectCreationHandling;
    internal MissingMemberHandling? _missingMemberHandling;
    internal ReferenceLoopHandling? _referenceLoopHandling;
    internal StreamingContext? _context;
    internal ConstructorHandling? _constructorHandling;
    internal TypeNameHandling? _typeNameHandling;
    internal MetadataPropertyHandling? _metadataPropertyHandling;

    public ReferenceLoopHandling ReferenceLoopHandling
    {
      get => this._referenceLoopHandling ?? ReferenceLoopHandling.Error;
      set => this._referenceLoopHandling = new ReferenceLoopHandling?(value);
    }

    public MissingMemberHandling MissingMemberHandling
    {
      get => this._missingMemberHandling ?? MissingMemberHandling.Ignore;
      set => this._missingMemberHandling = new MissingMemberHandling?(value);
    }

    public ObjectCreationHandling ObjectCreationHandling
    {
      get => this._objectCreationHandling ?? ObjectCreationHandling.Auto;
      set => this._objectCreationHandling = new ObjectCreationHandling?(value);
    }

    public NullValueHandling NullValueHandling
    {
      get => this._nullValueHandling ?? NullValueHandling.Include;
      set => this._nullValueHandling = new NullValueHandling?(value);
    }

    public DefaultValueHandling DefaultValueHandling
    {
      get => this._defaultValueHandling ?? DefaultValueHandling.Include;
      set => this._defaultValueHandling = new DefaultValueHandling?(value);
    }

    public IList<JsonConverter> Converters { get; set; }

    public PreserveReferencesHandling PreserveReferencesHandling
    {
      get => this._preserveReferencesHandling ?? PreserveReferencesHandling.None;
      set => this._preserveReferencesHandling = new PreserveReferencesHandling?(value);
    }

    public TypeNameHandling TypeNameHandling
    {
      get => this._typeNameHandling ?? TypeNameHandling.None;
      set => this._typeNameHandling = new TypeNameHandling?(value);
    }

    public MetadataPropertyHandling MetadataPropertyHandling
    {
      get => this._metadataPropertyHandling ?? MetadataPropertyHandling.Default;
      set => this._metadataPropertyHandling = new MetadataPropertyHandling?(value);
    }

    public FormatterAssemblyStyle TypeNameAssemblyFormat
    {
      get => this._typeNameAssemblyFormat ?? FormatterAssemblyStyle.Simple;
      set => this._typeNameAssemblyFormat = new FormatterAssemblyStyle?(value);
    }

    public ConstructorHandling ConstructorHandling
    {
      get => this._constructorHandling ?? ConstructorHandling.Default;
      set => this._constructorHandling = new ConstructorHandling?(value);
    }

    public IContractResolver ContractResolver { get; set; }

    public IEqualityComparer EqualityComparer { get; set; }

    [Obsolete("ReferenceResolver property is obsolete. Use the ReferenceResolverProvider property to set the IReferenceResolver: settings.ReferenceResolverProvider = () => resolver")]
    public IReferenceResolver ReferenceResolver
    {
      get => this.ReferenceResolverProvider == null ? (IReferenceResolver) null : this.ReferenceResolverProvider();
      set => this.ReferenceResolverProvider = (Func<IReferenceResolver>) (() => value);
    }

    public Func<IReferenceResolver> ReferenceResolverProvider { get; set; }

    public ITraceWriter TraceWriter { get; set; }

    public SerializationBinder Binder { get; set; }

    public EventHandler<ErrorEventArgs> Error { get; set; }

    public StreamingContext Context
    {
      get => this._context ?? JsonSerializerSettings.DefaultContext;
      set => this._context = new StreamingContext?(value);
    }

    public string DateFormatString
    {
      get => this._dateFormatString ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
      set
      {
        this._dateFormatString = value;
        this._dateFormatStringSet = true;
      }
    }

    public int? MaxDepth
    {
      get => this._maxDepth;
      set
      {
        int? nullable = value;
        if ((nullable.GetValueOrDefault() > 0 ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
          throw new ArgumentException("Value must be positive.", nameof (value));
        this._maxDepth = value;
        this._maxDepthSet = true;
      }
    }

    public Formatting Formatting
    {
      get => this._formatting ?? Formatting.None;
      set => this._formatting = new Formatting?(value);
    }

    public DateFormatHandling DateFormatHandling
    {
      get => this._dateFormatHandling ?? DateFormatHandling.IsoDateFormat;
      set => this._dateFormatHandling = new DateFormatHandling?(value);
    }

    public DateTimeZoneHandling DateTimeZoneHandling
    {
      get => this._dateTimeZoneHandling ?? DateTimeZoneHandling.RoundtripKind;
      set => this._dateTimeZoneHandling = new DateTimeZoneHandling?(value);
    }

    public DateParseHandling DateParseHandling
    {
      get => this._dateParseHandling ?? DateParseHandling.DateTime;
      set => this._dateParseHandling = new DateParseHandling?(value);
    }

    public FloatFormatHandling FloatFormatHandling
    {
      get => this._floatFormatHandling ?? FloatFormatHandling.String;
      set => this._floatFormatHandling = new FloatFormatHandling?(value);
    }

    public FloatParseHandling FloatParseHandling
    {
      get => this._floatParseHandling ?? FloatParseHandling.Double;
      set => this._floatParseHandling = new FloatParseHandling?(value);
    }

    public StringEscapeHandling StringEscapeHandling
    {
      get => this._stringEscapeHandling ?? StringEscapeHandling.Default;
      set => this._stringEscapeHandling = new StringEscapeHandling?(value);
    }

    public CultureInfo Culture
    {
      get => this._culture ?? JsonSerializerSettings.DefaultCulture;
      set => this._culture = value;
    }

    public bool CheckAdditionalContent
    {
      get => this._checkAdditionalContent ?? false;
      set => this._checkAdditionalContent = new bool?(value);
    }

    public JsonSerializerSettings() => this.Converters = (IList<JsonConverter>) new List<JsonConverter>();
  }
}
