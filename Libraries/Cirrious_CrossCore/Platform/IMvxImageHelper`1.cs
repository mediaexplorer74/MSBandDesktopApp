// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Platform.IMvxImageHelper`1
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using Cirrious.CrossCore.Core;
using System;

namespace Cirrious.CrossCore.Platform
{
  public interface IMvxImageHelper<T> : IDisposable where T : class
  {
    string DefaultImagePath { get; set; }

    string ErrorImagePath { get; set; }

    string ImageUrl { get; set; }

    event EventHandler<MvxValueEventArgs<T>> ImageChanged;
  }
}
