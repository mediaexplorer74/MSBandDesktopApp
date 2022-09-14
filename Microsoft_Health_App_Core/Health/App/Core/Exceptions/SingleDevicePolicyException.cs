// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Exceptions.SingleDevicePolicyException
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using System;

namespace Microsoft.Health.App.Core.Exceptions
{
  public sealed class SingleDevicePolicyException : Exception
  {
    private readonly SingleDevicePolicyResult result;

    public SingleDevicePolicyException(SingleDevicePolicyResult result)
      : base(AppResources.SingleDevicePolicyExceptionMessage)
    {
      this.result = result;
    }

    public SingleDevicePolicyException(SingleDevicePolicyResult result, Exception innerException)
      : base(AppResources.SingleDevicePolicyExceptionMessage, innerException)
    {
      this.result = result;
    }

    public SingleDevicePolicyResult Result => this.result;
  }
}
