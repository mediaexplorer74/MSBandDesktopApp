﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Configuration.Dynamic.IStarbucksConfiguration
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Services.Configuration.Dynamic
{
  public interface IStarbucksConfiguration
  {
    bool IsEnabled { get; }

    Uri DisplayUrl { get; }

    string DisplayUrlString { get; }

    Uri CardFrontUrl { get; }

    Uri CardBackUrl { get; }
  }
}