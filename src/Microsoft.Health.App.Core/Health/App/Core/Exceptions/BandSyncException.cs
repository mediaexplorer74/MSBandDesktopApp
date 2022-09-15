// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Exceptions.BandSyncException
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Exceptions
{
  public class BandSyncException : Exception, IIgnorableException
  {
    private readonly bool suppress;

    public BandSyncException(bool suppress = false) => this.suppress = suppress;

    public BandSyncException(string message, bool suppress = false)
      : base(message)
    {
      this.suppress = suppress;
    }

    public BandSyncException(string message, Exception inner, bool suppress = false)
      : base(message, inner)
    {
      this.suppress = suppress;
    }

    public bool Suppress => this.suppress;
  }
}
