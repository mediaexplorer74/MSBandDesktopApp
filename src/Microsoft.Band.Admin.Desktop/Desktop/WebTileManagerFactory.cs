// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.Desktop.WebTileManagerFactory
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using Microsoft.Band.Admin.Desktop.WebTiles;
using Microsoft.Band.Admin.WebTiles;

namespace Microsoft.Band.Admin.Desktop
{
  public class WebTileManagerFactory : IWebTileManagerFactory
  {
    private static IWebTileManager instance;
    private static object lockingObject = new object();

    public static IWebTileManager Instance
    {
      get
      {
        if (WebTileManagerFactory.instance == null)
        {
          lock (WebTileManagerFactory.lockingObject)
          {
            if (WebTileManagerFactory.instance == null)
            {
              StorageProvider storageProvider = StorageProvider.Create();
              WebTileManagerFactory.instance = (IWebTileManager) new WebTileManager((IStorageProvider) storageProvider, (IImageProvider) new ImageProvider(storageProvider));
            }
          }
        }
        return WebTileManagerFactory.instance;
      }
    }
  }
}
