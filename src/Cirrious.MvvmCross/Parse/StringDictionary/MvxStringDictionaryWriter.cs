// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.Parse.StringDictionary.MvxStringDictionaryWriter
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using System.Collections.Generic;
using System.Text;

namespace Cirrious.MvvmCross.Parse.StringDictionary
{
  public class MvxStringDictionaryWriter : IMvxStringDictionaryWriter
  {
    public string Write(IDictionary<string, string> dictionary)
    {
      if (dictionary == null || dictionary.Count == 0)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) dictionary)
      {
        if (stringBuilder.Length > 0)
          stringBuilder.Append(";");
        stringBuilder.AppendFormat("{0}={1}", new object[2]
        {
          (object) this.Quote(keyValuePair.Key),
          (object) this.Quote(keyValuePair.Value)
        });
      }
      return stringBuilder.ToString();
    }

    private string Quote(string input)
    {
      if (input == null)
        return "null";
      StringBuilder stringBuilder = new StringBuilder(input.Length + 32);
      stringBuilder.Append('\'');
      foreach (char ch in input)
      {
        switch (ch)
        {
          case '\'':
            stringBuilder.Append("\\'");
            break;
          case '\\':
            stringBuilder.Append("\\\\");
            break;
          default:
            stringBuilder.Append(ch);
            break;
        }
      }
      stringBuilder.Append('\'');
      return stringBuilder.ToString();
    }
  }
}
