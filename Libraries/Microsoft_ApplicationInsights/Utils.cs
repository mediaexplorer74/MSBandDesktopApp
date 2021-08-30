// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Utils
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Microsoft.ApplicationInsights
{
  internal static class Utils
  {
    private static readonly string[] RelativeFolderPath = new string[3]
    {
      "Microsoft",
      "ApplicationInsights",
      "Cache"
    };

    public static string GetHashedId(string input, bool isCaseSensitive = false)
    {
      if (input == null)
        return string.Empty;
      using (SHA256 shA256 = (SHA256) new SHA256Cng())
      {
        string s = input;
        if (!isCaseSensitive)
          s = input.ToUpperInvariant();
        return new SoapBase64Binary(shA256.ComputeHash(Encoding.UTF8.GetBytes(s))).ToString();
      }
    }

    public static TType ReadSerializedContext<TType>(string fileName) where TType : IFallbackContext, new()
    {
      string tempPath = Path.GetTempPath();
      string str = ((IEnumerable<string>) Utils.RelativeFolderPath).Aggregate<string, string>(tempPath, new Func<string, string, string>(Path.Combine));
      if (!Directory.Exists(str))
      {
        try
        {
          Directory.CreateDirectory(str);
        }
        catch (IOException ex)
        {
        }
        catch (UnauthorizedAccessException ex)
        {
        }
        catch (NotSupportedException ex)
        {
        }
      }
      bool flag = true;
      string path = Path.Combine(str, fileName);
      if (File.Exists(path))
      {
        try
        {
          using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
          {
            using (XmlReader reader = XmlReader.Create((Stream) fileStream))
            {
              XDocument xdocument = XDocument.Load(reader);
              TType type = new TType();
              if (type.Deserialize(xdocument.Root))
              {
                flag = false;
                return type;
              }
            }
          }
        }
        catch (IOException ex)
        {
        }
        catch (SecurityException ex)
        {
        }
        catch (UnauthorizedAccessException ex)
        {
        }
        catch (XmlException ex)
        {
        }
        catch (NotSupportedException ex)
        {
        }
      }
      if (flag)
      {
        XDocument xdocument = new XDocument();
        xdocument.Add((object) new XElement(XName.Get(typeof (TType).Name)));
        TType type = new TType();
        type.Initialize();
        type.Serialize(xdocument.Root);
        try
        {
          using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
          {
            fileStream.SetLength(0L);
            using (XmlWriter writer = XmlWriter.Create((Stream) fileStream))
            {
              xdocument.Save(writer);
              writer.Flush();
              fileStream.Flush();
            }
            return type;
          }
        }
        catch (IOException ex)
        {
        }
        catch (SecurityException ex)
        {
        }
        catch (UnauthorizedAccessException ex)
        {
        }
        catch (XmlException ex)
        {
        }
        catch (NotSupportedException ex)
        {
        }
      }
      TType type1 = new TType();
      type1.Initialize();
      return type1;
    }

    public static bool IsNullOrWhiteSpace(this string value) => value == null || value.All<char>(new Func<char, bool>(char.IsWhiteSpace));

    public static void CopyDictionary<TValue>(
      IDictionary<string, TValue> source,
      IDictionary<string, TValue> target)
    {
      foreach (KeyValuePair<string, TValue> keyValuePair in (IEnumerable<KeyValuePair<string, TValue>>) source)
      {
        if (!string.IsNullOrEmpty(keyValuePair.Key) && !target.ContainsKey(keyValuePair.Key))
          target[keyValuePair.Key] = keyValuePair.Value;
      }
    }

    public static string PopulateRequiredStringValue(
      string value,
      string parameterName,
      string telemetryType)
    {
      if (!string.IsNullOrEmpty(value))
        return value;
      CoreEventSource.Log.PopulateRequiredStringWithValue(parameterName, telemetryType);
      return parameterName + " is a required field for " + telemetryType;
    }

    public static TimeSpan ValidateDuration(string value)
    {
      TimeSpan output;
      if (TimeSpanEx.TryParse(value, CultureInfo.InvariantCulture, out output))
        return output;
      CoreEventSource.Log.RequestTelemetryIncorrectDuration();
      return TimeSpan.Zero;
    }
  }
}
