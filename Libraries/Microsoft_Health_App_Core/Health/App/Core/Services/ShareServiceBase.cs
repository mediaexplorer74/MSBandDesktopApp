// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.ShareServiceBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Sharing;
using Microsoft.Health.App.Core.Services.Storage;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Models;
using Microsoft.Practices.ServiceLocation;
using PCLStorage;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public abstract class ShareServiceBase : IShareService
  {
    protected static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\ShareServiceBase.cs");
    private readonly IHealthCloudClient cloudClient;

    protected ShareServiceBase(IHealthCloudClient cloudClient)
    {
      Assert.ParamIsNotNull((object) cloudClient, nameof (cloudClient));
      this.ShareType = ShareType.Degraded;
      this.cloudClient = cloudClient;
    }

    protected IHealthCloudClient CloudClient => this.cloudClient;

    public ShareRequest Request { get; private set; }

    public async Task PrepareForSharingAsync(ShareRequest request)
    {
      this.Reset();
      this.Request = request;
      try
      {
        ShareRequest shareRequest = this.Request;
        ShareUrlsResponseData urlsResponseData = await this.EnableEventSharingAsync(CancellationToken.None);
        shareRequest.ShareUrls = urlsResponseData;
        shareRequest = (ShareRequest) null;
        request.Message = string.Format("{0} {1} {2}", new object[3]
        {
          (object) request.Message,
          (object) AppResources.ShareDashboardCallToAction,
          (object) this.Request.ShareUrls.ShareLinkUrl
        });
        this.FinalizeSharingPreparation();
      }
      catch (Exception ex)
      {
        ShareServiceBase.Logger.Error((object) "Error while preparing share service for sharing", ex);
        ApplicationTelemetry.LogShareFailure(this.Request.EventType, ShareType.Enhanced, this.Request.ButtonType, ShareFailureType.General, ex.ToString());
      }
    }

    public virtual void Reset() => this.Request = (ShareRequest) null;

    protected virtual void FinalizeSharingPreparation()
    {
    }

    public virtual void Share() => this.ShareViaOsFacility();

    public abstract void ShareViaOsFacility();

    private async Task<ShareUrlsResponseData> EnableEventSharingAsync(
      CancellationToken cancellationToken)
    {
      IMultiPartAttachment thumbnail = (IMultiPartAttachment) null;
      if (this.Request.ShareImageThumbnail != null)
      {
        string extension = ShareServiceBase.ExtractExtension(this.Request.ShareImageThumbnail.Name);
        string mimeType = ShareServiceBase.ExtractMimeType(extension);
        thumbnail = await this.GetMultiPartAttachmentAsync(extension, mimeType, cancellationToken);
      }
      ShareUrlsResponseData urlsResponseData;
      using (thumbnail)
        urlsResponseData = await this.cloudClient.EnableEventSharingAsync(this.Request.ShareUrlsRequestFormData, thumbnail, cancellationToken);
      return urlsResponseData;
    }

    public virtual async Task<IMultiPartAttachment> GetMultiPartAttachmentAsync(
      string extension,
      string mimeType,
      CancellationToken cancellationToken)
    {
      MultiPartStreamAttachment streamAttachment1 = new MultiPartStreamAttachment();
      MultiPartStreamAttachment streamAttachment2 = streamAttachment1;
      Stream stream = await this.Request.ShareImageThumbnail.OpenAsync(FileAccess.Read);
      streamAttachment2.Data = stream;
      streamAttachment1.FileName = string.Format("Thumbnail.{0}", new object[1]
      {
        (object) extension
      });
      streamAttachment1.MimeType = mimeType;
      MultiPartStreamAttachment streamAttachment = streamAttachment1;
      streamAttachment2 = (MultiPartStreamAttachment) null;
      streamAttachment1 = (MultiPartStreamAttachment) null;
      return (IMultiPartAttachment) streamAttachment;
    }

    public static string ExtractExtension(string fileName) => Path.GetExtension(fileName).TrimStart('.');

    public static string ExtractMimeType(string extension)
    {
      if (extension == "jpg" || extension == "jpeg")
        return "image/jpeg";
      if (extension == "png")
        return "image/png";
      throw new InvalidDataException("Share image thumbnail file extension not supported");
    }

    protected async Task<IFile> DownloadShareImageToFileAsync(
      CancellationToken cancellationToken)
    {
      IFile imageFile = await (await ServiceLocator.Current.GetInstance<IFileSystemService>().GetSocialSharingFolderAsync()).CreateFileAsync("EnhancedShare.png", CreationCollisionOption.ReplaceExisting, cancellationToken);
      using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, this.Request.ShareUrls.ShareImageUrl))
      {
        using (HttpResponseMessage response = await new HttpClient().SendAsync(request, cancellationToken).ConfigureAwait(false))
        {
          using (Stream outputStream = await imageFile.OpenAsync(FileAccess.ReadAndWrite, cancellationToken))
          {
            using (Stream stream = await response.Content.ReadAsStreamAsync())
              await stream.CopyToAsync(outputStream);
          }
        }
      }
      return imageFile;
    }

    protected ShareType ShareType { get; set; }
  }
}
