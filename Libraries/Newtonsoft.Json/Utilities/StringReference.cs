// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.StringReference
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

namespace Newtonsoft.Json.Utilities
{
  internal struct StringReference
  {
    private readonly char[] _chars;
    private readonly int _startIndex;
    private readonly int _length;

    public char[] Chars => this._chars;

    public int StartIndex => this._startIndex;

    public int Length => this._length;

    public StringReference(char[] chars, int startIndex, int length)
    {
      this._chars = chars;
      this._startIndex = startIndex;
      this._length = length;
    }

    public override string ToString() => new string(this._chars, this._startIndex, this._length);
  }
}
