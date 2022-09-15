// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Devices.GetDevicesResult
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Services.Devices
{
  public class GetDevicesResult : IGetDevicesResult, IEnumerable<IDevice>, IEnumerable
  {
    private readonly IEnumerable<IDevice> devices;
    private readonly AggregateException exception;

    public GetDevicesResult(IEnumerable<IDevice> devices, AggregateException exception)
    {
      this.devices = devices ?? (IEnumerable<IDevice>) new List<IDevice>();
      this.exception = exception;
    }

    public IEnumerator<IDevice> GetEnumerator() => this.devices.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public AggregateException Exception => this.exception;
  }
}
