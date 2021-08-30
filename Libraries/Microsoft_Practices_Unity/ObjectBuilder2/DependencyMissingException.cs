// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.DependencyMissingException
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Properties;
using System;
using System.Globalization;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class DependencyMissingException : Exception
  {
    public DependencyMissingException()
    {
    }

    public DependencyMissingException(string message)
      : base(message)
    {
    }

    public DependencyMissingException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public DependencyMissingException(object buildKey)
      : base(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.MissingDependency, new object[1]
      {
        buildKey
      }))
    {
    }
  }
}
