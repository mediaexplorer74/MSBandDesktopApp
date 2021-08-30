// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.FormattedMetricViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Utilities;

namespace Microsoft.Health.App.Core.ViewModels
{
  public sealed class FormattedMetricViewModel
  {
    private readonly FormattedMetric metric;

    public FormattedMetricViewModel(FormattedMetric metric)
    {
      Assert.ParamIsNotNull((object) metric, nameof (metric));
      this.metric = metric;
    }

    public string MetricValue => this.metric.MetricValue;

    public string UnitValue => this.metric.UnitValue;
  }
}
