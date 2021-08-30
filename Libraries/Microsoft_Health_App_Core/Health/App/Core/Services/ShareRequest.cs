// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.ShareRequest
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Sharing;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Models;
using PCLStorage;

namespace Microsoft.Health.App.Core.Services
{
  public class ShareRequest
  {
    public EventType EventType { get; set; }

    public string Title { get; set; }

    public string Message { get; set; }

    public string ChooserTitle { get; set; }

    public IFile ShareImageThumbnail { get; set; }

    public ShareUrlsRequestFormData ShareUrlsRequestFormData { get; set; }

    public ShareUrlsResponseData ShareUrls { get; set; }

    public string SharePreviewImageFileName { get; set; }

    public ShareButtonType ButtonType { get; set; }
  }
}
