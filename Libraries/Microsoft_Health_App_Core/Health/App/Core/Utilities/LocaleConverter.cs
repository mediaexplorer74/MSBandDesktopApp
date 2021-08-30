// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.LocaleConverter
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Newtonsoft.Json;
using System;

namespace Microsoft.Health.App.Core.Utilities
{
  public class LocaleConverter : JsonConverter
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Utilities\\LocaleConverter.cs");

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      DisplayTimeFormat displayTimeFormat = DisplayTimeFormat.Undefined;
      if (value == null)
      {
        writer.WriteValue((object) displayTimeFormat);
      }
      else
      {
        try
        {
          displayTimeFormat = (DisplayTimeFormat) value;
        }
        catch (Exception ex)
        {
          LocaleConverter.Logger.Error(ex, "Error casting value to DisplayTimeFormat while writing");
        }
        writer.WriteValue((object) displayTimeFormat);
      }
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      DisplayTimeFormat displayTimeFormat = DisplayTimeFormat.Undefined;
      if (reader.TokenType == JsonToken.Null)
        return (object) displayTimeFormat;
      try
      {
        displayTimeFormat = (DisplayTimeFormat) existingValue;
      }
      catch (Exception ex)
      {
        LocaleConverter.Logger.Error(ex, "Error casting value to DisplayTimeFormat while reading");
      }
      return (object) displayTimeFormat;
    }

    public override bool CanConvert(Type objectType) => (object) objectType == (object) typeof (DisplayTimeFormat);
  }
}
