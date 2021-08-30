// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.BackgroundSyncInvalidator
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class BackgroundSyncInvalidator : ISyncInvalidator
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\BackgroundSyncInvalidator.cs");
    private IConfig config;

    public BackgroundSyncInvalidator(IConfig config) => this.config = config;

    public Task InvalidateAsync(int deviceCount, bool anySucceeded)
    {
      if (deviceCount > 0 & anySucceeded)
      {
        BackgroundSyncInvalidator.Logger.Debug((object) "Marked background refresh pending.");
        this.config.IsBackgroundRefreshPending = true;
      }
      return (Task) Task.FromResult<bool>(true);
    }
  }
}
