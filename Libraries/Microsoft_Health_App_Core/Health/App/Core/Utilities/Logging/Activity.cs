// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.Logging.Activity
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;

namespace Microsoft.Health.App.Core.Utilities.Logging
{
  public class Activity
  {
    public string Name { get; private set; }

    public Level LogLevel { get; private set; }

    public Activity(string name, Level logLevel)
    {
      this.Name = name != null ? name : throw new ArgumentNullException(nameof (name));
      this.LogLevel = logLevel;
    }
  }
}
