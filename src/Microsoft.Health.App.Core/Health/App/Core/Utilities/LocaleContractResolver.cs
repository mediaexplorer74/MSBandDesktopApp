// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.LocaleContractResolver
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;

namespace Microsoft.Health.App.Core.Utilities
{
  public class LocaleContractResolver : DefaultContractResolver
  {
    private static readonly JsonConverter Converter = (JsonConverter) new LocaleConverter();
    private static readonly TypeInfo DisplayTimeFormatTypeInfo = typeof (DisplayTimeFormat).GetTypeInfo();

    protected override JsonConverter ResolveContractConverter(Type objectType) => (object) objectType == null || !LocaleContractResolver.DisplayTimeFormatTypeInfo.IsAssignableFrom(objectType.GetTypeInfo()) ? base.ResolveContractConverter(objectType) : LocaleContractResolver.Converter;
  }
}
