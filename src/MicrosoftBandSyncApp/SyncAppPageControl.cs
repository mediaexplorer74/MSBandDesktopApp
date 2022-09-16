// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.SyncAppPageControl
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Windows.Controls;

namespace DesktopSyncApp
{
    // abstract
    public partial class SyncAppPageControl : UserControl
  {
    protected AppMainWindow parent;
    protected ViewModel model;
    protected SyncAppPageControl container;
    protected bool transparent;

    public SyncAppPageControl(AppMainWindow parent, bool transparent)
    {
      this.parent = parent;
      this.model = parent.Model;
      this.transparent = transparent;
      this.container = (SyncAppPageControl) null;
    }

    public SyncAppPageControl(AppMainWindow parent, SyncAppPageControl container, bool transparent)
    {
      this.parent = parent;
      this.model = parent.Model;
      this.container = container;
      this.transparent = transparent;
    }

    public AnimatedPageControl pageControl { get; set; }

    protected override void OnInitialized(EventArgs e)
    {
      base.OnInitialized(e);
      int num = this.transparent ? 1 : 0;
      this.Width = double.NaN;
      this.Height = double.NaN;
    }

    public virtual void OnShow()
    {
    }

    public virtual void OnHide()
    {
    }
  }
}
