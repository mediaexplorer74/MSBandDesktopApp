// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.DynamicGlobalizationConfiguration
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Services.Configuration.Dynamic;
using Microsoft.Health.Cloud.Client.DynamicConfiguration;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Threading.Tasks;

namespace DesktopSyncApp
{
  public class DynamicGlobalizationConfiguration
  {
    private const string metaDataApi = "//api/AppsConfig";
    private Dictionary<string, CurrentDynamicConfigurationFile> dynamicConfigurations = new Dictionary<string, CurrentDynamicConfigurationFile>();
    private string regionName;
    private string baseFileRegion = "defaultconfig";
    private string pathAssetsLocal = "Assets-Local";

    public DynamicGlobalizationConfiguration(string regionName)
    {
      this.regionName = regionName.ToLower();
      Assembly executingAssembly = Assembly.GetExecutingAssembly();
      foreach (string manifestResourceName in executingAssembly.GetManifestResourceNames())
      {
        if (manifestResourceName.StartsWith("DesktopSyncApp.Assets.Configurations.") && manifestResourceName.EndsWith(".json"))
        {
          DynamicConfigurationFile baseFileLocally = this.GetBaseFileLocally(executingAssembly.GetManifestResourceStream(manifestResourceName));
          this.dynamicConfigurations.Add(baseFileLocally.Name.ToLower(), new CurrentDynamicConfigurationFile(DateTimeOffset.Now, (ConfigurationFileSource) 1, baseFileLocally.Name, baseFileLocally));
        }
      }
      IsolatedStorageFile storeForAssembly = IsolatedStorageFile.GetMachineStoreForAssembly();
      if (!storeForAssembly.DirectoryExists(this.pathAssetsLocal))
        return;
      foreach (string fileName in storeForAssembly.GetFileNames(Path.Combine(this.pathAssetsLocal, "dynamicconfig.*.json")))
      {
        CurrentDynamicConfigurationFile fileLocally = this.GetFileLocally((Stream) storeForAssembly.OpenFile(Path.Combine(this.pathAssetsLocal, fileName), FileMode.Open));
        this.dynamicConfigurations[fileLocally.RegionName.ToLower()] = fileLocally;
      }
    }

    public Microsoft.Health.App.Core.Services.Configuration.Dynamic.DynamicConfiguration CurrentDynamicGlobalizationConfig
    {
      get
      {
        Microsoft.Health.App.Core.Services.Configuration.Dynamic.DynamicConfiguration dynamicConfiguration = (Microsoft.Health.App.Core.Services.Configuration.Dynamic.DynamicConfiguration) null;
        if (this.dynamicConfigurations.ContainsKey(this.regionName))
          dynamicConfiguration = this.dynamicConfigurations[this.regionName].File.Configuration;
        if (dynamicConfiguration == null && this.dynamicConfigurations.ContainsKey(this.baseFileRegion))
          dynamicConfiguration = this.dynamicConfigurations[this.baseFileRegion].File.Configuration;
        return dynamicConfiguration;
      }
    }

    public async Task UpdateDynamicConfiguration(ServiceInfo serviceInfo) => await this.UpdateDynamicConfiguration(serviceInfo, this.regionName);

    public async Task UpdateDynamicConfiguration(ServiceInfo serviceInfo, string region)
    {
      try
      {
        region = region.ToLower();
        DynamicConfigurationFile dynamicdata = (DynamicConfigurationFile) null;
        DynamicConfigurationFileMetadata metadata = (DynamicConfigurationFileMetadata) null;
        Uri downloadFileUrl = new Uri(string.Format("{0}{1}", (object) serviceInfo.FileUpdateServiceAddress, (object) "//api/AppsConfig"));
        Dictionary<string, string> dictionary1 = new Dictionary<string, string>()
        {
          {
            "Authorization",
            string.Format("WRAP access_token=\"{0}\"", (object) serviceInfo.AccessToken)
          },
          {
            "Region",
            region
          }
        };
        Dictionary<string, string> dynamicheaders = new Dictionary<string, string>()
        {
          {
            "Region",
            region
          }
        };
        Dictionary<string, string> dictionary2 = dictionary1;
        TimeSpan timeout = TimeSpan.FromSeconds(30.0);
        metadata = await (await WebUtil.DownloadFile(downloadFileUrl, (IDictionary<string, string>) dictionary2, timeout)).ReadJsonAsync<DynamicConfigurationFileMetadata>();
        if (metadata != null)
        {
          if (!this.dynamicConfigurations.ContainsKey(region) || this.dynamicConfigurations[region].File.Version < metadata.Version)
          {
            try
            {
              if (metadata != null)
              {
                if (metadata.PrimaryUrl != (Uri) null)
                  dynamicdata = await (await WebUtil.DownloadFile(metadata.PrimaryUrl, (IDictionary<string, string>) dynamicheaders, new TimeSpan(0, 0, 30))).ReadJsonAsync<DynamicConfigurationFile>();
              }
            }
            catch
            {
            }
            try
            {
              if (dynamicdata == null)
              {
                if (metadata != null)
                {
                  if (metadata.MirrorUrl != (Uri) null)
                    dynamicdata = await (await WebUtil.DownloadFile(metadata.MirrorUrl, (IDictionary<string, string>) dynamicheaders, new TimeSpan(0, 0, 30))).ReadJsonAsync<DynamicConfigurationFile>();
                }
              }
            }
            catch
            {
            }
          }
          if (dynamicdata != null)
          {
            CurrentDynamicConfigurationFile file = new CurrentDynamicConfigurationFile(DateTimeOffset.Now, (ConfigurationFileSource) 2, dynamicdata.Name, dynamicdata);
            this.dynamicConfigurations[region] = file;
            this.SaveFileLocally(string.Format("DynamicConfig.{0}.json", (object) file.RegionName), file);
          }
        }
        dynamicdata = (DynamicConfigurationFile) null;
        metadata = (DynamicConfigurationFileMetadata) null;
        dynamicheaders = (Dictionary<string, string>) null;
      }
      catch
      {
      }
    }

    private void SaveFileLocally(string fileRelativePath, CurrentDynamicConfigurationFile file)
    {
      IsolatedStorageFile storeForAssembly = IsolatedStorageFile.GetMachineStoreForAssembly();
      if (!storeForAssembly.DirectoryExists(this.pathAssetsLocal))
        storeForAssembly.CreateDirectory(this.pathAssetsLocal);
      using (StreamWriter streamWriter = new StreamWriter((Stream) storeForAssembly.OpenFile(Path.Combine(this.pathAssetsLocal, fileRelativePath), FileMode.Create)))
        JsonUtilities.SerializeObject((object) file, (TextWriter) streamWriter);
    }

    private CurrentDynamicConfigurationFile GetFileLocally(
      Stream fileStream)
    {
      using (StreamReader streamReader = new StreamReader(fileStream))
        return JsonUtilities.DeserializeObject<CurrentDynamicConfigurationFile>((TextReader) streamReader);
    }

    private DynamicConfigurationFile GetBaseFileLocally(Stream fileStream)
    {
      using (StreamReader streamReader = new StreamReader(fileStream))
        return JsonUtilities.DeserializeObject<DynamicConfigurationFile>((TextReader) streamReader);
    }
  }
}
