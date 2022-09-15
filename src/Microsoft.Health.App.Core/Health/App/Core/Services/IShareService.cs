// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.IShareService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.Cloud.Client.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public interface IShareService
  {
    ShareRequest Request { get; }

    Task PrepareForSharingAsync(ShareRequest request);

    void Reset();

    void Share();

    void ShareViaOsFacility();

    Task<IMultiPartAttachment> GetMultiPartAttachmentAsync(
      string extension,
      string mimeType,
      CancellationToken cancellationToken);
  }
}
