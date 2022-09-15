// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.WebTileIconViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Tiles;
using Microsoft.Health.App.Core.Resources;
using System;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.ViewModels
{
  public class WebTileIconViewModel
  {
    public string Name { get; private set; }

    public BandIcon Icon { get; private set; }

    public string Description { get; private set; }

    public string Author { get; private set; }

    public string Organization { get; private set; }

    public string Version { get; private set; }

    public IEnumerable<string> DataSource { get; private set; }

    public string DataSourceString { get; private set; }

    public WebTileIconViewModel(string name, BandIcon icon, string description)
    {
      this.Name = name;
      this.Icon = icon;
      this.Description = description;
    }

    public WebTileIconViewModel(
      string name,
      BandIcon icon,
      string description,
      string author,
      string organization,
      string version,
      IEnumerable<string> dataSource)
      : this(name, icon, description)
    {
      this.Author = !string.IsNullOrWhiteSpace(author) ? author : AppResources.NotAvailable;
      this.Organization = !string.IsNullOrWhiteSpace(organization) ? organization : AppResources.NotAvailable;
      this.Version = version;
      this.DataSource = dataSource;
      this.DataSourceString = string.Join(Environment.NewLine, dataSource);
    }
  }
}
