// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.LocaleSettingsProviderBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Providers
{
  public abstract class LocaleSettingsProviderBase : ILocaleSettingsProvider
  {
    private List<ILocaleSettings> localeSettings;

    public async Task<IList<ILocaleSettings>> LoadLocaleSettingsAsync(
      CancellationToken token)
    {
      if (this.localeSettings == null)
      {
        using (StreamReader streamReader = new StreamReader(await this.GetLocaleSettingsStreamAsync(token)))
          this.localeSettings = JsonConvert.DeserializeObject<LocaleSettingsProviderBase.LocaleSettingsContainer>(streamReader.ReadToEnd()).Locales.Select<LocaleSettings, ILocaleSettings>((Func<LocaleSettings, ILocaleSettings>) (p => (ILocaleSettings) p)).ToList<ILocaleSettings>();
      }
      return (IList<ILocaleSettings>) this.localeSettings;
    }

    protected abstract Task<Stream> GetLocaleSettingsStreamAsync(CancellationToken token);

    private class LocaleSettingsContainer
    {
      public IList<LocaleSettings> Locales { get; set; }
    }
  }
}
