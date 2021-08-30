// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ServiceLocation.ActivationException
// Assembly: Microsoft.Practices.ServiceLocation, Version=1.3.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 7D3316BA-C928-4A64-AD5F-824E0C3D6D36
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_ServiceLocation.dll

using System;

namespace Microsoft.Practices.ServiceLocation
{
  public class ActivationException : Exception
  {
    public ActivationException()
    {
    }

    public ActivationException(string message)
      : base(message)
    {
    }

    public ActivationException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
