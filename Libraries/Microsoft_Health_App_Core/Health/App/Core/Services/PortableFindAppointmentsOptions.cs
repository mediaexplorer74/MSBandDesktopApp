// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.PortableFindAppointmentsOptions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Services
{
  public class PortableFindAppointmentsOptions
  {
    private IList<string> calendarIds;
    private IList<string> fetchProperties;

    public PortableFindAppointmentsOptions()
    {
      this.CalendarIds = (IList<string>) new List<string>();
      this.FetchProperties = (IList<string>) new List<string>();
    }

    public IList<string> CalendarIds
    {
      get => this.calendarIds;
      set => this.calendarIds = value != null ? value : throw new ArgumentNullException(nameof (value));
    }

    public IList<string> FetchProperties
    {
      get => this.fetchProperties;
      set => this.fetchProperties = value != null ? value : throw new ArgumentNullException(nameof (value));
    }

    public bool IncludeHidden { get; set; }

    public uint MaxCount { get; set; }
  }
}
