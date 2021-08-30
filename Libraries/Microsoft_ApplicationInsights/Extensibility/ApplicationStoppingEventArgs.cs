// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.ApplicationStoppingEventArgs
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Microsoft.ApplicationInsights.Extensibility
{
  public class ApplicationStoppingEventArgs : EventArgs
  {
    internal static readonly ApplicationStoppingEventArgs Empty = new ApplicationStoppingEventArgs((Func<Func<Task>, Task>) (asyncMethod => asyncMethod()));
    private readonly Func<Func<Task>, Task> asyncMethodRunner;

    public ApplicationStoppingEventArgs(Func<Func<Task>, Task> asyncMethodRunner) => this.asyncMethodRunner = asyncMethodRunner != null ? asyncMethodRunner : throw new ArgumentNullException(nameof (asyncMethodRunner));

    public async void Run(Func<Task> asyncMethod)
    {
      try
      {
        await this.asyncMethodRunner(asyncMethod);
      }
      catch (Exception ex)
      {
        string msg = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Unexpected excption when handling IApplicationLifecycle.Stopping event:{0}", (object) ex.ToString());
        CoreEventSource.Log.LogError(msg);
      }
    }
  }
}
