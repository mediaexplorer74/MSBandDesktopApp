// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Caching.ResponseSharer`1
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client.Caching
{
  public class ResponseSharer<T>
  {
    private Dictionary<Uri, Task<T>> activeCalls = new Dictionary<Uri, Task<T>>();
    private object sync = new object();

    public Task<T> GetResponseWithSharingAsync(Uri key, Func<Task<T>> fetcher)
    {
      lock (this.sync)
      {
        if (this.activeCalls.ContainsKey(key))
          return this.activeCalls[key];
        Task<T> responseInternalAsync = this.GetResponseInternalAsync(key, fetcher);
        this.activeCalls[key] = responseInternalAsync;
        return responseInternalAsync;
      }
    }

    private async Task<T> GetResponseInternalAsync(Uri key, Func<Task<T>> fetcher)
    {
      T obj;
      try
      {
        await Task.Yield();
        obj = await fetcher().ConfigureAwait(false);
      }
      finally
      {
        lock (this.sync)
          this.activeCalls.Remove(key);
      }
      return obj;
    }
  }
}
