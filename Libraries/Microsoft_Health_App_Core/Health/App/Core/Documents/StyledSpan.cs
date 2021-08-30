// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Documents.StyledSpan
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Health.App.Core.Documents
{
  public class StyledSpan : IEnumerable<StyledRun>, IEnumerable, IMarkupString
  {
    private readonly IEnumerable<StyledRun> fullRuns;

    public StyledSpan(string format, params object[] args)
    {
      Assert.ParamIsNotNull((object) format, nameof (format));
      this.fullRuns = new StyledSpanParser(format, args).Parse();
    }

    public StyledSpan(IEnumerable<StyledRun> fullRuns)
    {
      Assert.ParamIsNotNull((object) fullRuns, nameof (fullRuns));
      this.fullRuns = fullRuns;
    }

    public StyledSpan(params StyledRun[] fullRuns) => this.fullRuns = (IEnumerable<StyledRun>) fullRuns;

    public static StyledSpan FromSerializedString(string serializedString)
    {
      Assert.ParamIsNotNullOrEmpty(serializedString, nameof (serializedString));
      return new StyledSpan(JsonConvert.DeserializeObject<IEnumerable<StyledRun>>(serializedString));
    }

    public static implicit operator string(StyledSpan styledSpan) => styledSpan?.ToString();

    public string ToSerializedString() => JsonConvert.SerializeObject((object) this.fullRuns);

    public IEnumerator<StyledRun> GetEnumerator() => this.fullRuns.GetEnumerator();

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (StyledRun fullRun in this.fullRuns)
        stringBuilder.Append(fullRun.Text);
      return stringBuilder.ToString();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.fullRuns.GetEnumerator();

    public string ToMarkupString() => this.fullRuns.Aggregate<StyledRun, string>(string.Empty, (Func<string, StyledRun, string>) ((sum, current) => sum + current.ToMarkupString()));

    string IMarkupString.ToMarkupString() => this.ToMarkupString();

    IEnumerable<StyledRun> IMarkupString.Runs => (IEnumerable<StyledRun>) this;
  }
}
