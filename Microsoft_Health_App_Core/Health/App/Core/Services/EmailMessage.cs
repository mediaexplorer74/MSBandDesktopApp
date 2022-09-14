// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.EmailMessage
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using PCLStorage;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Services
{
  public class EmailMessage
  {
    public ICollection<string> Recipients { get; } = (ICollection<string>) new List<string>();

    public ICollection<IFile> Attachments { get; } = (ICollection<IFile>) new List<IFile>();

    public string Subject { get; set; }

    public string Body { get; set; }
  }
}
