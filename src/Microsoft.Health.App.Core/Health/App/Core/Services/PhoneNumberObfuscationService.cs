// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.PhoneNumberObfuscationService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Storage;
using Microsoft.Health.App.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class PhoneNumberObfuscationService : IPhoneNumberObfuscationService
  {
    private const string FileName = "PhoneNumberObfuscationMap.json";
    private const int ScavengeHighThreshold = 40;
    private const int ScavengeLowThreshold = 20;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\PhoneNumberObfuscationService.cs");
    private readonly IFileObjectStorageService fileObjectStorageService;
    private readonly IDateTimeService dateTimeService;
    private AsyncLock fileLock = new AsyncLock();
    private Random randomGenerator = new Random();

    public PhoneNumberObfuscationService(
      IFileObjectStorageService fileObjectStorageService,
      IDateTimeService dateTimeService)
    {
      this.fileObjectStorageService = fileObjectStorageService;
      this.dateTimeService = dateTimeService;
    }

    public Task<uint> StoreAsync(string phoneNumber) => this.StoreInternalAsync(new uint?(), phoneNumber);

    public async Task StoreAsync(uint id, string phoneNumber)
    {
      int num = (int) await this.StoreInternalAsync(new uint?(id), phoneNumber).ConfigureAwait(false);
    }

    private async Task<uint> StoreInternalAsync(uint? id, string phoneNumber)
    {
      Assert.ParamIsNotNullOrEmpty(phoneNumber, nameof (phoneNumber));
      uint key = (uint) (id ?? (uint) this.GenerateKey());
      AsyncLock.Releaser releaser = await this.fileLock.LockAsync().ConfigureAwait(false);
      try
      {
        Dictionary<uint, PhoneNumberObfuscationEntry> map = await this.ReadMapAsync().ConfigureAwait(false);
        PhoneNumberObfuscationEntry obfuscationEntry;
        if (map.TryGetValue(key, out obfuscationEntry) && phoneNumber != obfuscationEntry.PhoneNumber)
          PhoneNumberObfuscationService.Logger.Warn((object) string.Format("Phone number mapping for id {0} changed from {1} to {2}", new object[3]
          {
            (object) key,
            (object) obfuscationEntry.PhoneNumber,
            (object) phoneNumber
          }));
        map[key] = new PhoneNumberObfuscationEntry()
        {
          Timestamp = this.dateTimeService.Now,
          PhoneNumber = phoneNumber
        };
        PhoneNumberObfuscationService.ScavengeIfNeeded(map);
        await this.fileObjectStorageService.WriteObjectAsync((object) map, "PhoneNumberObfuscationMap.json", CancellationToken.None, useMutex: false).ConfigureAwait(false);
      }
      finally
      {
        releaser.Dispose();
      }
      releaser = new AsyncLock.Releaser();
      return key;
    }

    public async Task<string> RetrieveAsync(uint id)
    {
      string str;
      using (await this.fileLock.LockAsync().ConfigureAwait(false))
      {
        PhoneNumberObfuscationEntry obfuscationEntry;
        str = !(await this.ReadMapAsync().ConfigureAwait(false)).TryGetValue(id, out obfuscationEntry) ? (string) null : obfuscationEntry.PhoneNumber;
      }
      return str;
    }

    private async Task<Dictionary<uint, PhoneNumberObfuscationEntry>> ReadMapAsync()
    {
      Dictionary<uint, PhoneNumberObfuscationEntry> dictionary = await this.fileObjectStorageService.ReadObjectAsync<Dictionary<uint, PhoneNumberObfuscationEntry>>("PhoneNumberObfuscationMap.json", CancellationToken.None, useMutex: false).ConfigureAwait(false);
      return dictionary == null ? new Dictionary<uint, PhoneNumberObfuscationEntry>() : dictionary;
    }

    private uint GenerateKey() => (uint) this.randomGenerator.Next(int.MinValue, int.MaxValue);

    private static void ScavengeIfNeeded(Dictionary<uint, PhoneNumberObfuscationEntry> map)
    {
      if (map.Count < 40)
        return;
      List<uint> list = map.OrderBy<KeyValuePair<uint, PhoneNumberObfuscationEntry>, DateTimeOffset>((Func<KeyValuePair<uint, PhoneNumberObfuscationEntry>, DateTimeOffset>) (pair => pair.Value.Timestamp)).Select<KeyValuePair<uint, PhoneNumberObfuscationEntry>, uint>((Func<KeyValuePair<uint, PhoneNumberObfuscationEntry>, uint>) (pair => pair.Key)).ToList<uint>();
      for (int index = 0; index < map.Count - 20; ++index)
        map.Remove(list[index]);
    }
  }
}
