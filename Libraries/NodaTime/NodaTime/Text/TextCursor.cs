// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.TextCursor
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using System.Diagnostics;

namespace NodaTime.Text
{
  [DebuggerStepThrough]
  internal abstract class TextCursor
  {
    internal const char Nul = '\0';
    private readonly string value;
    private readonly int length;

    protected TextCursor(string value)
    {
      this.value = value;
      this.length = value.Length;
      this.Move(-1);
    }

    internal char Current { get; private set; }

    internal bool HasMoreCharacters => this.Index + 1 < this.Length;

    internal int Index { get; private set; }

    internal int Length => this.length;

    internal string Value => this.value;

    internal string Remainder => this.Value.Substring(this.Index);

    public override string ToString()
    {
      if (this.Index <= 0)
        return "^" + this.Value;
      return this.Index < this.Length ? this.Value.Insert(this.Index, "^") : this.Value + "^";
    }

    internal char PeekNext() => !this.HasMoreCharacters ? char.MinValue : this.Value[this.Index + 1];

    internal bool Move(int targetIndex)
    {
      if (targetIndex >= 0)
      {
        if (targetIndex < this.Length)
        {
          this.Index = targetIndex;
          this.Current = this.Value[this.Index];
          return true;
        }
        this.Current = char.MinValue;
        this.Index = this.Length;
        return false;
      }
      this.Current = char.MinValue;
      this.Index = -1;
      return false;
    }

    internal bool MoveNext()
    {
      int num = this.Index + 1;
      if (num < this.Length)
      {
        this.Index = num;
        this.Current = this.Value[this.Index];
        return true;
      }
      this.Current = char.MinValue;
      this.Index = this.Length;
      return false;
    }

    internal bool MovePrevious()
    {
      if (this.Index > 0)
      {
        --this.Index;
        this.Current = this.Value[this.Index];
        return true;
      }
      this.Current = char.MinValue;
      this.Index = -1;
      return false;
    }
  }
}
