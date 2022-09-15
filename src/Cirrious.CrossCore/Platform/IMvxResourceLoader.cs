// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Platform.IMvxResourceLoader
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;
using System.IO;

namespace Cirrious.CrossCore.Platform
{
  public interface IMvxResourceLoader
  {
    bool ResourceExists(string resourcePath);

    string GetTextResource(string resourcePath);

    void GetResourceStream(string resourcePath, Action<Stream> streamAction);
  }
}
