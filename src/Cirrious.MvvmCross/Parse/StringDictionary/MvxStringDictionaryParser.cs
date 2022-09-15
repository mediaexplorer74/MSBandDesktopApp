// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.Parse.StringDictionary.MvxStringDictionaryParser
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Parse;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Parse.StringDictionary
{
  public class MvxStringDictionaryParser : MvxParser, IMvxStringDictionaryParser
  {
    protected Dictionary<string, string> CurrentEntries { get; private set; }

    public IDictionary<string, string> Parse(string textToParse)
    {
      this.Reset(textToParse);
      while (!this.IsComplete)
      {
        this.ParseNextKeyValuePair();
        this.SkipWhitespaceAndCharacters(';');
      }
      return (IDictionary<string, string>) this.CurrentEntries;
    }

    protected override void Reset(string textToParse)
    {
      this.CurrentEntries = new Dictionary<string, string>();
      base.Reset(textToParse);
    }

    private void ParseNextKeyValuePair()
    {
      this.SkipWhitespace();
      if (this.IsComplete)
        return;
      object obj1 = this.ReadValue();
      if (!(obj1 is string))
        throw new MvxException("Unexpected object in key for keyvalue pair {0} at position {1}", new object[2]
        {
          (object) obj1.GetType().Name,
          (object) this.CurrentIndex
        });
      this.SkipWhitespace();
      if (this.CurrentChar != '=')
        throw new MvxException("Unexpected character in keyvalue pair {0} at position {1}", new object[2]
        {
          (object) this.CurrentChar,
          (object) this.CurrentIndex
        });
      this.MoveNext();
      this.SkipWhitespace();
      object obj2 = this.ReadValue();
      switch (obj2)
      {
        case null:
        case string _:
          this.CurrentEntries[(string) obj1] = (string) obj2;
          break;
        default:
          throw new MvxException("Unexpected object in value for keyvalue pair {0} for key {1} at position {2}", new object[3]
          {
            (object) obj2.GetType().Name,
            obj1,
            (object) this.CurrentIndex
          });
      }
    }
  }
}
