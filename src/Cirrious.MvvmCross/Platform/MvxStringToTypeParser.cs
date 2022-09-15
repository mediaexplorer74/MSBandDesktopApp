// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.Platform.MvxStringToTypeParser
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.CrossCore.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cirrious.MvvmCross.Platform
{
  public class MvxStringToTypeParser : IMvxStringToTypeParser, IMvxFillableStringToTypeParser
  {
    public IDictionary<Type, MvxStringToTypeParser.IParser> TypeParsers { get; private set; }

    public IList<MvxStringToTypeParser.IExtraParser> ExtraParsers { get; private set; }

    public MvxStringToTypeParser()
    {
      this.TypeParsers = (IDictionary<Type, MvxStringToTypeParser.IParser>) new Dictionary<Type, MvxStringToTypeParser.IParser>()
      {
        {
          typeof (string),
          (MvxStringToTypeParser.IParser) new MvxStringToTypeParser.StringParser()
        },
        {
          typeof (short),
          (MvxStringToTypeParser.IParser) new MvxStringToTypeParser.ShortParser()
        },
        {
          typeof (int),
          (MvxStringToTypeParser.IParser) new MvxStringToTypeParser.IntParser()
        },
        {
          typeof (long),
          (MvxStringToTypeParser.IParser) new MvxStringToTypeParser.LongParser()
        },
        {
          typeof (ushort),
          (MvxStringToTypeParser.IParser) new MvxStringToTypeParser.UshortParser()
        },
        {
          typeof (uint),
          (MvxStringToTypeParser.IParser) new MvxStringToTypeParser.UintParser()
        },
        {
          typeof (ulong),
          (MvxStringToTypeParser.IParser) new MvxStringToTypeParser.UlongParser()
        },
        {
          typeof (double),
          (MvxStringToTypeParser.IParser) new MvxStringToTypeParser.DoubleParser()
        },
        {
          typeof (float),
          (MvxStringToTypeParser.IParser) new MvxStringToTypeParser.FloatParser()
        },
        {
          typeof (bool),
          (MvxStringToTypeParser.IParser) new MvxStringToTypeParser.BoolParser()
        },
        {
          typeof (Guid),
          (MvxStringToTypeParser.IParser) new MvxStringToTypeParser.GuidParser()
        },
        {
          typeof (DateTime),
          (MvxStringToTypeParser.IParser) new MvxStringToTypeParser.DateTimeParser()
        }
      };
      this.ExtraParsers = (IList<MvxStringToTypeParser.IExtraParser>) new List<MvxStringToTypeParser.IExtraParser>()
      {
        (MvxStringToTypeParser.IExtraParser) new MvxStringToTypeParser.EnumParser()
      };
    }

    public bool TypeSupported(Type targetType) => this.TypeParsers.ContainsKey(targetType) || this.ExtraParsers.Any<MvxStringToTypeParser.IExtraParser>((Func<MvxStringToTypeParser.IExtraParser, bool>) (x => x.Parses(targetType)));

    public object ReadValue(string rawValue, Type targetType, string fieldOrParameterName)
    {
      MvxStringToTypeParser.IParser parser;
      if (this.TypeParsers.TryGetValue(targetType, out parser))
        return parser.ReadValue(rawValue, fieldOrParameterName);
      MvxStringToTypeParser.IExtraParser extraParser = this.ExtraParsers.FirstOrDefault<MvxStringToTypeParser.IExtraParser>((Func<MvxStringToTypeParser.IExtraParser, bool>) (x => x.Parses(targetType)));
      if (extraParser != null)
        return extraParser.ReadValue(targetType, rawValue, fieldOrParameterName);
      MvxTrace.Error("Parameter {0} is invalid targetType {1}", (object) fieldOrParameterName, (object) targetType.Name);
      return (object) null;
    }

    public interface IParser
    {
      object ReadValue(string input, string fieldOrParameterName);
    }

    public interface IExtraParser
    {
      bool Parses(Type t);

      object ReadValue(Type t, string input, string fieldOrParameterName);
    }

    public class EnumParser : MvxStringToTypeParser.IExtraParser
    {
      public bool Parses(Type t) => t.GetTypeInfo().IsEnum;

      public object ReadValue(Type t, string input, string fieldOrParameterName)
      {
        object obj = (object) null;
        try
        {
          obj = Enum.Parse(t, input, true);
        }
        catch (Exception ex)
        {
          MvxTrace.Error("Failed to parse enum parameter {0} from string {1}", (object) fieldOrParameterName, (object) input);
        }
        if (obj == null)
        {
          try
          {
            obj = Enum.ToObject(t, (object) 0);
          }
          catch (Exception ex)
          {
            MvxTrace.Error("Failed to create default enum value for {0} - will return null", (object) fieldOrParameterName);
          }
        }
        return obj;
      }
    }

    public class StringParser : MvxStringToTypeParser.IParser
    {
      public object ReadValue(string input, string fieldOrParameterName) => (object) input;
    }

    public abstract class ValueParser : MvxStringToTypeParser.IParser
    {
      protected abstract bool TryParse(string input, out object result);

      public object ReadValue(string input, string fieldOrParameterName)
      {
        object result;
        if (!this.TryParse(input, out result))
          MvxTrace.Error("Failed to parse {0} parameter {1} from string {2}", (object) this.GetType().Name, (object) fieldOrParameterName, (object) input);
        return result;
      }
    }

    public class BoolParser : MvxStringToTypeParser.ValueParser
    {
      protected override bool TryParse(string input, out object result)
      {
        bool result1;
        bool flag = bool.TryParse(input, out result1);
        result = (object) result1;
        return flag;
      }
    }

    public class ShortParser : MvxStringToTypeParser.ValueParser
    {
      protected override bool TryParse(string input, out object result)
      {
        short result1;
        bool flag = short.TryParse(input, out result1);
        result = (object) result1;
        return flag;
      }
    }

    public class IntParser : MvxStringToTypeParser.ValueParser
    {
      protected override bool TryParse(string input, out object result)
      {
        int result1;
        bool flag = int.TryParse(input, out result1);
        result = (object) result1;
        return flag;
      }
    }

    public class LongParser : MvxStringToTypeParser.ValueParser
    {
      protected override bool TryParse(string input, out object result)
      {
        long result1;
        bool flag = long.TryParse(input, out result1);
        result = (object) result1;
        return flag;
      }
    }

    public class UshortParser : MvxStringToTypeParser.ValueParser
    {
      protected override bool TryParse(string input, out object result)
      {
        ushort result1;
        bool flag = ushort.TryParse(input, out result1);
        result = (object) result1;
        return flag;
      }
    }

    public class UintParser : MvxStringToTypeParser.ValueParser
    {
      protected override bool TryParse(string input, out object result)
      {
        uint result1;
        bool flag = uint.TryParse(input, out result1);
        result = (object) result1;
        return flag;
      }
    }

    public class UlongParser : MvxStringToTypeParser.ValueParser
    {
      protected override bool TryParse(string input, out object result)
      {
        ulong result1;
        bool flag = ulong.TryParse(input, out result1);
        result = (object) result1;
        return flag;
      }
    }

    public class FloatParser : MvxStringToTypeParser.ValueParser
    {
      protected override bool TryParse(string input, out object result)
      {
        float result1;
        bool flag = float.TryParse(input, out result1);
        result = (object) result1;
        return flag;
      }
    }

    public class DoubleParser : MvxStringToTypeParser.ValueParser
    {
      protected override bool TryParse(string input, out object result)
      {
        double result1;
        bool flag = double.TryParse(input, out result1);
        result = (object) result1;
        return flag;
      }
    }

    public class GuidParser : MvxStringToTypeParser.ValueParser
    {
      protected override bool TryParse(string input, out object result)
      {
        Guid result1;
        bool flag = Guid.TryParse(input, out result1);
        result = (object) result1;
        return flag;
      }
    }

    public class DateTimeParser : MvxStringToTypeParser.ValueParser
    {
      protected override bool TryParse(string input, out object result)
      {
        DateTime result1;
        bool flag = DateTime.TryParse(input, out result1);
        result = (object) result1;
        return flag;
      }
    }
  }
}
