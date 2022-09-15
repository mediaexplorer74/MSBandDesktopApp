// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Diagnostics.DiagnosticsServiceBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Authentication;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Models.Diagnostics;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services.Logging.Appenders;
using Microsoft.Health.App.Core.Services.Logging.Configurations;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Storage;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Http;
using Microsoft.Health.Cloud.Client.Services;
using Newtonsoft.Json;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Diagnostics
{
  public abstract class DiagnosticsServiceBase : IDiagnosticsService
  {
    private const string PackageVersion = "1.0";
    private const string MetadataFile = "metadata.json";
    private const string ContextFile = "context.json";
    private const string UserFeedbackFile = "userFeedback.json";
    private const string FiddlerArchiveFile = "http\\app.saz";
    private const string PackageExtension = ".diagz";
    private const string ArchiveImagesFolder = "images";
    private const string LastCrashFileName = "LastCrash.txt";
    private const string LogsArchiveFolder = "logs";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\Diagnostics\\DiagnosticsServiceBase.cs");
    private readonly AsyncLock lastCrashFileLock = new AsyncLock();
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly IConnectionInfoStore connectionInfoStore;
    private readonly IEnvironmentService environmentService;
    private readonly ICultureService cultureService;
    private readonly IRegionService regionService;
    private readonly ISmoothNavService smoothNavService;
    private readonly IFiddlerLogService fiddlerLogService;
    private readonly IFileSystemService fileSystemService;
    private readonly IUserProfileService userProfileService;
    private readonly ILoggerConfiguration loggerConfiguration;
    private readonly IDispatchService dispatchService;
    private readonly IDiagnosticsDataProvider[] dataProviders;
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IMessageBoxService messageBoxService;
    private readonly IEmailService emailService;
    private readonly IDateTimeService dateTimeService;
    private readonly JsonSerializer serializer;
    private readonly IPermissionService permissionService;
    private bool httpLoggerErrorWritten;

    protected DiagnosticsServiceBase(
      IBandConnectionFactory cargoConnectionFactory,
      IConnectionInfoStore connectionInfoStore,
      IEnvironmentService environmentService,
      ICultureService cultureService,
      IRegionService regionService,
      ISmoothNavService smoothNavService,
      IFiddlerLogService fiddlerLogService,
      IFileSystemService fileSystemService,
      IUserProfileService userProfileService,
      ILoggerConfiguration loggerConfiguration,
      IDispatchService dispatchService,
      IDiagnosticsDataProvider[] dataProviders,
      IErrorHandlingService errorHandlingService,
      IMessageBoxService messageBoxService,
      IEmailService emailService,
      IDateTimeService dateTimeService,
      IPermissionService permissionService)
    {
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.connectionInfoStore = connectionInfoStore;
      this.environmentService = environmentService;
      this.cultureService = cultureService;
      this.regionService = regionService;
      this.smoothNavService = smoothNavService;
      this.fiddlerLogService = fiddlerLogService;
      this.fileSystemService = fileSystemService;
      this.userProfileService = userProfileService;
      this.loggerConfiguration = loggerConfiguration;
      this.dispatchService = dispatchService;
      this.dataProviders = dataProviders;
      this.errorHandlingService = errorHandlingService;
      this.messageBoxService = messageBoxService;
      this.emailService = emailService;
      this.dateTimeService = dateTimeService;
      this.permissionService = permissionService;
      this.serializer = JsonSerializer.Create(new JsonSerializerSettings()
      {
        NullValueHandling = NullValueHandling.Ignore,
        Formatting = Formatting.Indented
      });
    }

    public async Task InitializeHttpLoggingAsync()
    {
      HttpLogging.IsLoggingEnabled = !this.environmentService.IsPublicRelease;
      if (!HttpLogging.IsLoggingEnabled)
        return;
      try
      {
        await this.fiddlerLogService.StartAsync(CancellationToken.None);
        HttpLogging.HttpRequestCompleted += new EventHandler<HttpTransactionEventArgs>(this.OnHttpRequestCompleted);
      }
      catch (Exception ex)
      {
        DiagnosticsServiceBase.Logger.Error((object) "Failed to start the fiddler log service.", ex);
      }
    }

    public async Task<IFile> CaptureDiagnosisPackageAsync(
      DiagnosticsUserFeedback userFeedback,
      IEnumerable<IFile> imageFiles,
      bool isPublic,
      bool includeLogs)
    {
      string packageId = Guid.NewGuid().ToString().ToLowerInvariant();
      IFile diagnosisZipFile = await this.CreateDiagnosisFileAsync(packageId + ".diagz", !isPublic, CreationCollisionOption.ReplaceExisting);
      using (Stream zipStream = await diagnosisZipFile.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
      {
        using (ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create))
        {
          this.IncludeJsonDiagnosticsFile((object) this.CaptureDiagnosticsMetadata(packageId), zipArchive, "metadata.json");
          this.IncludeJsonDiagnosticsFile((object) await this.CaptureDiagnosticsContextAsync(isPublic).ConfigureAwait(false), zipArchive, "context.json");
          this.IncludeJsonDiagnosticsFile((object) userFeedback, zipArchive, "userFeedback.json");
          if (includeLogs)
          {
            await this.IncludeLogFilesAsync(zipArchive);
            if (!isPublic)
              await this.IncludeFiddlerFileAsync(zipArchive);
          }
          if (imageFiles != null)
          {
            foreach (IFile imageFile in imageFiles)
            {
              try
              {
                await this.IncludeImageAsync(imageFile, zipArchive).ConfigureAwait(false);
              }
              catch (Exception ex)
              {
                DiagnosticsServiceBase.Logger.Error((object) "Failed to attach the image to the diagnostics package.", ex);
              }
            }
          }
          await this.CaptureDiagnosticsDataFromProvidersAsync(zipArchive, CancellationToken.None);
        }
      }
      return diagnosisZipFile;
    }

    public async Task<IFile> CreateDiagnosisFileAsync(
      string packageFileName,
      bool isEmailAttachment,
      CreationCollisionOption collisionOption)
    {
      IFolder folder;
      if (isEmailAttachment)
        folder = await this.fileSystemService.GetSharingFolderAsync().ConfigureAwait(false);
      else
        folder = await this.fileSystemService.GetLogsFolderAsync().ConfigureAwait(false);
      return await folder.CreateFileAsync(packageFileName, collisionOption);
    }

    private void IncludeJsonDiagnosticsFile(object model, ZipArchive zipArchive, string fileName)
    {
      using (StreamWriter streamWriter = new StreamWriter(zipArchive.CreateEntry(fileName).Open()))
        this.serializer.Serialize((TextWriter) streamWriter, model);
    }

    private async Task IncludeImageAsync(IFile imageFile, ZipArchive zipArchive)
    {
      string[] strArray = new string[2]
      {
        "images",
        imageFile.Name
      };
      using (Stream outputStream = zipArchive.CreateEntry(Path.Combine(strArray)).Open())
      {
        using (Stream inputStream = await imageFile.OpenAsync(PCLStorage.FileAccess.Read))
          await inputStream.CopyToAsync(outputStream);
      }
    }

    private async Task IncludeFiddlerFileAsync(ZipArchive zipArchive, CancellationToken token = default (CancellationToken))
    {
      if ((this.fiddlerLogService == null ? 0 : (this.fiddlerLogService.HasEntries ? 1 : 0)) == 0)
        return;
      using (Stream outputStream = zipArchive.CreateEntry("http\\app.saz").Open())
        await this.fiddlerLogService.PauseCaptureAndCopyToAsync(outputStream, token);
    }

    private async Task CaptureDiagnosticsDataFromProvidersAsync(
      ZipArchive archive,
      CancellationToken token = default (CancellationToken))
    {
      if (this.dataProviders == null)
        return;
      IDiagnosticsDataProvider[] diagnosticsDataProviderArray = this.dataProviders;
      for (int index = 0; index < diagnosticsDataProviderArray.Length; ++index)
      {
        IDiagnosticsDataProvider diagnosticsDataProvider = diagnosticsDataProviderArray[index];
        try
        {
          await diagnosticsDataProvider.CaptureDiagnosticsDataAsync(archive, token).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
          DiagnosticsServiceBase.Logger.Error(ex, "An error occurred capturing diagnostics data from a provider.");
        }
      }
      diagnosticsDataProviderArray = (IDiagnosticsDataProvider[]) null;
    }

    public async Task IncludeLogFilesAsync(ZipArchive zipArchive)
    {
      await this.IncludeLogFileAsync(zipArchive, "App");
      await this.IncludeLogFileAsync(zipArchive, "Background");
    }

    private async Task IncludeLogFileAsync(ZipArchive zipArchive, string logName)
    {
      IFolder folder = await (await this.fileSystemService.GetLogsFolderAsync().ConfigureAwait(true)).TryGetFolderAsync(logName, CancellationToken.None).ConfigureAwait(false);
      if (folder == null)
        return;
      IList<IFile> logFiles = await folder.GetFilesAsync().ConfigureAwait(false);
      if (logFiles.Count == 0)
        return;
      string[] strArray = new string[2]
      {
        "logs",
        logName.ToLowerInvariant() + ".jsonl"
      };
      using (Stream outputStream = zipArchive.CreateEntry(Path.Combine(strArray)).Open())
      {
        for (int i = 0; i < logFiles.Count; ++i)
        {
          IFile originalFile = logFiles[i];
          IFile tempFile = (IFile) null;
          try
          {
            IFile file;
            if (i == logFiles.Count - 1)
            {
              IStorageFileAppender storageFileAppender = this.loggerConfiguration.Appenders.OfType<IStorageFileAppender>().FirstOrDefault<IStorageFileAppender>((Func<IStorageFileAppender, bool>) (a => a.LogName == logName));
              if (storageFileAppender == null)
              {
                file = originalFile;
              }
              else
              {
                tempFile = await storageFileAppender.CopyCurrentFileToTempFileAsync().ConfigureAwait(false);
                file = !(tempFile.Name == originalFile.Name) ? originalFile : tempFile;
              }
            }
            else
              file = originalFile;
            using (Stream sourceStream = await file.OpenAsync(PCLStorage.FileAccess.Read).ConfigureAwait(false))
              await sourceStream.CopyToAsync(outputStream).ConfigureAwait(false);
          }
          catch (Exception ex)
          {
            DiagnosticsServiceBase.Logger.Warn((object) ("Could not include " + originalFile.Name + " in diagnostics package."), ex);
          }
          finally
          {
            if (tempFile != null)
              DiagnosticsServiceBase.StartDelete(tempFile);
          }
          originalFile = (IFile) null;
          tempFile = (IFile) null;
        }
      }
    }

    private static async void StartDelete(IFile file)
    {
      try
      {
        await file.DeleteAsync().ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        DiagnosticsServiceBase.Logger.Error((object) ("Could not delete temporary file " + file.Name));
      }
    }

    private async void OnHttpRequestCompleted(object sender, HttpTransactionEventArgs e)
    {
      try
      {
        if (this.fiddlerLogService == null)
          return;
        await Task.Run((Func<Task>) (async () =>
        {
          if (this.fiddlerLogService == null)
            return;
          await this.fiddlerLogService.WriteTransactionAsync(e.Transaction, CancellationToken.None);
        }));
      }
      catch (Exception ex)
      {
        if (this.httpLoggerErrorWritten)
          return;
        this.httpLoggerErrorWritten = true;
        DiagnosticsServiceBase.Logger.Error(ex, "An error occurred writing to the HTTP logger");
      }
    }

    public async Task CaptureCrashInformationAsync(Exception error)
    {
      AsyncLock.Releaser releaser = await this.lastCrashFileLock.LockAsync().ConfigureAwait(false);
      try
      {
        StringBuilder crashInfo = new StringBuilder();
        crashInfo.AppendFormat("Crash happened on: {0}{1}", new object[2]
        {
          (object) DateTime.Now,
          (object) Environment.NewLine
        });
        crashInfo.AppendFormat("Crashed app version: {0}{1}", new object[2]
        {
          (object) this.environmentService.ApplicationVersion,
          (object) Environment.NewLine
        });
        crashInfo.Append(error.ToString());
        using (StreamWriter writer = new StreamWriter(await (await (await this.fileSystemService.GetCrashesFolderAsync().ConfigureAwait(false)).CreateFileAsync("LastCrash.txt", CreationCollisionOption.ReplaceExisting).ConfigureAwait(false)).OpenAsync(PCLStorage.FileAccess.ReadAndWrite).ConfigureAwait(false)))
          await writer.WriteAsync(crashInfo.ToString()).ConfigureAwait(false);
        crashInfo = (StringBuilder) null;
      }
      catch (Exception ex)
      {
        DiagnosticsServiceBase.Logger.Error(ex, "Could not write last crash information to isolated storage");
      }
      finally
      {
        releaser.Dispose();
      }
      releaser = new AsyncLock.Releaser();
    }

    public async Task<string> GetLastCrashInformationAsync()
    {
      using (await this.lastCrashFileLock.LockAsync().ConfigureAwait(false))
      {
        try
        {
          IFile file = await (await this.fileSystemService.GetCrashesFolderAsync().ConfigureAwait(false)).TryGetFileAsync("LastCrash.txt").ConfigureAwait(false);
          if (file != null)
          {
            string lastCrashString;
            using (StreamReader streamReader = new StreamReader(await file.OpenAsync(PCLStorage.FileAccess.Read).ConfigureAwait(false)))
              lastCrashString = await streamReader.ReadToEndAsync().ConfigureAwait(false);
            await file.DeleteAsync().ConfigureAwait(false);
            return lastCrashString;
          }
          file = (IFile) null;
        }
        catch (Exception ex)
        {
          DiagnosticsServiceBase.Logger.Error(ex, "Could not read last crash information from isolated storage");
        }
        return (string) null;
      }
    }

    public async Task CheckAndReportOnLastCrashAsync()
    {
      if (this.environmentService.IsPublicRelease)
        return;
      string crashInfo = await this.GetLastCrashInformationAsync().ConfigureAwait(false);
      if (crashInfo != null)
      {
        if (await this.messageBoxService.ShowAsync(AppResources.SendErrorReportMessage, AppResources.SendErrorReportTitle, PortableMessageBoxButton.OKCancel) == PortableMessageBoxResult.OK)
          await this.ReportCrashAsync(crashInfo);
      }
      crashInfo = (string) null;
    }

    private async Task<DiagnosticsContext> CaptureDiagnosticsContextAsync(
      bool isPublic)
    {
      JournalEntry currentJournalEntry = this.smoothNavService.CurrentJournalEntry;
      string currentPage = currentJournalEntry != null ? currentJournalEntry.ViewModelType.ToString() : "(none)";
      DiagnosticsContext diagnosticsContext1 = new DiagnosticsContext();
      diagnosticsContext1.Application = this.CaptureApplicationSection();
      diagnosticsContext1.OperatingSystem = this.CaptureOperatingSystemSection();
      DiagnosticsContext diagnosticsContext2 = diagnosticsContext1;
      DiagnosticsDeviceSection diagnosticsDeviceSection = await this.CaptureDeviceSectionAsync().ConfigureAwait(false);
      diagnosticsContext2.Device = diagnosticsDeviceSection;
      diagnosticsContext1.CurrentContext = new DiagnosticsCurrentContext()
      {
        Page = currentPage
      };
      DiagnosticsContext diagnosticsContext3 = diagnosticsContext1;
      DiagnosticsHealth diagnosticsHealth = await this.CaptureHealthSectionAsync(isPublic).ConfigureAwait(false);
      diagnosticsContext3.MicrosoftHealth = diagnosticsHealth;
      DiagnosticsContext diagnosticsContext = diagnosticsContext1;
      diagnosticsContext2 = (DiagnosticsContext) null;
      diagnosticsContext3 = (DiagnosticsContext) null;
      diagnosticsContext1 = (DiagnosticsContext) null;
      return diagnosticsContext;
    }

    private async Task<DiagnosticsHealth> CaptureHealthSectionAsync(
      bool isPublic)
    {
      DiagnosticsHealth healthSection = new DiagnosticsHealth();
      try
      {
        HealthCloudConnectionInfo async = await this.connectionInfoStore.TryGetAsync();
        if (async != null)
        {
          DiagnosticsHealthCloud diagnosticsHealthCloud = new DiagnosticsHealthCloud()
          {
            OdsUserId = async.UserId,
            KdsAddress = async.BaseUri.AbsoluteUri,
            PodAddress = async.PodEndpoint.AbsoluteUri
          };
          if (!isPublic)
          {
            diagnosticsHealthCloud.KAccessToken = async.PodSecurityToken;
            diagnosticsHealthCloud.MsaToken = async.SecurityToken;
          }
          healthSection.Cloud = diagnosticsHealthCloud;
        }
      }
      catch (Exception ex)
      {
        DiagnosticsServiceBase.Logger.ErrorAndDebug(ex);
      }
      if (this.userProfileService.IsRegisteredBandPaired)
      {
        try
        {
          CancellationTokenSource tokenSource = default;
          int num = 0;

          if (num != 1 && num != 2)
            tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30.0));
          
          try
          {
            using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(tokenSource.Token))
            {
              DiagnosticsBandDevice diagnosticsBandDevice = await cargoConnection.GetDiagnosticsInfoAsync(tokenSource.Token).ConfigureAwait(false);
              if (isPublic)
                diagnosticsBandDevice.SerialNumber = (string) null;
              healthSection.PairedDevices.Add((DiagnosticsDevice) diagnosticsBandDevice);
            }
          }
          finally
          {
            tokenSource?.Dispose();
          }
          tokenSource = (CancellationTokenSource) null;
        }
        catch (Exception ex)
        {
          DiagnosticsServiceBase.Logger.Warn((object) "Failed to capture the band information.", ex);
        }
      }
      await this.AddPlatformSpecificDevicesAsync(healthSection);
      return healthSection;
    }

    protected abstract Task AddPlatformSpecificDevicesAsync(DiagnosticsHealth healthSection);

    private DiagnosticsApplication CaptureApplicationSection() => new DiagnosticsApplication()
    {
      Id = "19100286-bd8d-4e5b-bc77-5ffd7a54da39",
      Name = "Microsoft Health",
      Version = this.environmentService.ApplicationVersion.ToString(),
      BuildFlavor = this.environmentService.BuildFlavor
    };

    private DiagnosticsOperatingSystem CaptureOperatingSystemSection() => new DiagnosticsOperatingSystem()
    {
      Name = this.environmentService.OperatingSystemName,
      Version = this.environmentService.OperatingSystemVersion.ToString()
    };

    protected virtual Task<DiagnosticsDeviceSection> CaptureDeviceSectionAsync() => this.dispatchService.RunOnUIThreadAsync<DiagnosticsDeviceSection>((Func<DiagnosticsDeviceSection>) (() =>
    {
      PixelScreenSize pixelScreenSize = this.environmentService.PixelScreenSize;
      TimeSpan offset = this.dateTimeService.Now.Offset;
      string str = (offset >= TimeSpan.Zero ? "+" : "-") + offset.ToString("hh\\:mm");
      return new DiagnosticsDeviceSection()
      {
        Type = "Phone",
        Manufacturer = this.environmentService.Manufacturer,
        Model = this.environmentService.ProductName,
        Version = this.environmentService.HardwareVersion,
        ScreenResolution = new DiagnosticsScreenSize()
        {
          Width = pixelScreenSize.Width,
          Height = pixelScreenSize.Height
        },
        Language = this.cultureService.CurrentUICulture.Name,
        Region = this.regionService.CurrentRegion.Name,
        TimeZone = new DiagnosticsTimeZone()
        {
          Name = this.dateTimeService.TimeZone.ToString(),
          UtcOffset = str
        }
      };
    }));

    private DiagnosticsMetadata CaptureDiagnosticsMetadata(string id)
    {
      string str = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Microsoft Health v{0} ({1})", new object[2]
      {
        (object) this.environmentService.ApplicationVersion,
        (object) this.environmentService.OperatingSystemName
      });
      return new DiagnosticsMetadata()
      {
        Version = "1.0",
        Id = id,
        Date = DateTimeOffset.Now,
        GeneratedBy = str
      };
    }

    public async Task SendFeedbackEmailAsync(IFile diagnosisFile, string body = null)
    {
      this.smoothNavService.DisableNavPanel(typeof (IDiagnosticsService));
      EmailMessage message = this.CreateFeedbackEmail(diagnosisFile, body);
      await this.dispatchService.RunOnUIThreadAsync((Func<Task>) (async () => await this.emailService.ComposeNewEmailAsync(message)));
      this.smoothNavService.EnableNavPanel(typeof (IDiagnosticsService));
    }

    private async Task ReportCrashAsync(string error) => await this.dispatchService.RunOnUIThreadAsync((Func<Task>) (async () =>
    {
      this.smoothNavService.DisableNavPanel(typeof (IDiagnosticsService));
      await this.emailService.ComposeNewEmailAsync(await this.CreateCrashReportEmailAsync(error));
      this.smoothNavService.EnableNavPanel(typeof (IDiagnosticsService));
    }));

    private EmailMessage CreateFeedbackEmail(IFile diagnosisFile, string body = null)
    {
      EmailMessage emailMessage1 = this.CreateEmailMessage("Feedback");
      emailMessage1.Body = this.GetFeedbackEmailBody(body);
      try
      {
        emailMessage1.Attachments.Add(diagnosisFile);
      }
      catch (Exception ex)
      {
        DiagnosticsServiceBase.Logger.Error(ex, "Failed to capture diagnosis package");
        EmailMessage emailMessage2 = emailMessage1;
        emailMessage2.Body = emailMessage2.Body + Environment.NewLine + (object) ex;
      }
      return emailMessage1;
    }

    private EmailMessage CreateEmailMessage(string emailType) => new EmailMessage()
    {
      Recipients = {
        "cargodfsupport@microsoft.com"
      },
      Subject = this.GetSubject(emailType)
    };

    private string GetSubject(string emailType)
    {
      string applicationNameQualifier = this.environmentService.ApplicationNameQualifier;
      if (!string.IsNullOrEmpty(applicationNameQualifier))
        applicationNameQualifier += " ";
      return string.Format("{0}: {1}{2} {3} App v{4}", (object) emailType, (object) applicationNameQualifier, (object) this.environmentService.OperatingSystemName, (object) AppResources.ApplicationTitle, (object) this.environmentService.ApplicationVersion);
    }

    private string GetFeedbackEmailBody(string body = null)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (body != null)
        stringBuilder.Append(body);
      stringBuilder.AppendLine();
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("IMPORTANT: Please use your company account to send feedback as it may contain sensitive information about the product.");
      return stringBuilder.ToString();
    }

    private async Task<EmailMessage> CreateCrashReportEmailAsync(string error)
    {
      EmailMessage message = this.CreateEmailMessage("Crash Report");
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("IMPORTANT: Please use your company account to send feedback as it may contain sensitive information about the product.");
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("------------------------------");
      stringBuilder.AppendLine("Crash Report Information:");
      stringBuilder.AppendLine("------------------------------");
      stringBuilder.AppendLine();
      stringBuilder.AppendLine(error);
      stringBuilder.AppendLine();
      message.Body = stringBuilder.ToString();
      try
      {
        await this.permissionService.RequestPermissionsAsync(FeaturePermissions.Feedback);
        IFile file = await this.CaptureDiagnosisPackageAsync((DiagnosticsUserFeedback) null, (IEnumerable<IFile>) null, false, true);
        message.Attachments.Add(file);
      }
      catch (Exception ex)
      {
        await this.errorHandlingService.HandleExceptionAsync(ex);
        DiagnosticsServiceBase.Logger.Error(ex, "Failed to capture diagnosis package");
        EmailMessage emailMessage = message;
        emailMessage.Body = emailMessage.Body + Environment.NewLine + (object) ex;
      }
      return message;
    }
  }
}
