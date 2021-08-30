// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.InvalidPatternException
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Annotations;
using System;
using System.Globalization;

namespace NodaTime.Text
{
  [Mutable]
  public sealed class InvalidPatternException : FormatException
  {
    public InvalidPatternException()
    {
    }

    public InvalidPatternException(string message)
      : base(message)
    {
    }

    internal InvalidPatternException(string formatString, params object[] parameters)
      : this(string.Format((IFormatProvider) CultureInfo.CurrentCulture, formatString, parameters))
    {
    }
  }
}
