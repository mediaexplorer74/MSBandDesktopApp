// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Http.HttpLogging
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client.Http
{
  public static class HttpLogging
  {
    public static event EventHandler<HttpTransactionEventArgs> HttpRequestCompleted;

    public static bool IsLoggingEnabled { get; set; }

    internal static async Task OnHttpRequestBeforeSubmitAsync(
      DefaultHttpTransaction transaction,
      HttpRequestMessage request)
    {
      // ISSUE: unable to decompile the method.
    }

    internal static void OnHttpRequestFailed(
      DefaultHttpTransaction transaction,
      Exception exception)
    {
      if (!HttpLogging.IsLoggingEnabled)
        return;
      transaction.GotResponseHeadersTime = new DateTimeOffset?(DateTimeOffset.Now);
      transaction.DoneResponseTime = new DateTimeOffset?(DateTimeOffset.Now);
      transaction.Error = exception;
      if (HttpLogging.HttpRequestCompleted == null)
        return;
      HttpLogging.HttpRequestCompleted((object) null, new HttpTransactionEventArgs((IHttpTransaction) transaction));
    }

    internal static async Task OnHttpResponseReceivedAsync(
      DefaultHttpTransaction transaction,
      HttpResponseMessage response)
    {
      // ISSUE: unable to decompile the method.
    }
  }
}
