// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.App
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace DesktopSyncApp
{
  public sealed partial class App : Application, IDisposable
  {
    private Mutex singletonMutex;
    private ViewModel model;
    private bool _contentLoaded;

    public static App Current => Application.Current as App;

    public AppMainWindow MainWindow
    {
      get => base.MainWindow as AppMainWindow;
      set => this.MainWindow = (Window) value;
    }

    public TaskScheduler ForegroundTaskScheduler { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
      bool createdNew;
      this.singletonMutex = new Mutex(false, "Local\\MicrosoftBandSyncSingleton", out createdNew);
      if (!createdNew)
      {
        this.ShowRunningApp();
        this.Shutdown();
      }
      else
      {
        this.ForegroundTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
        this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(this.App_DispatcherUnhandledException);
        AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.CurrentDomain_UnhandledException);
        base.OnStartup(e);
        DynamicSettings dynamicSettings = new DynamicSettings();
        this.model = new ViewModel(this, dynamicSettings, this.ParseCommandLine(e.Args));
        this.VerifyWebBrowserControlIE8Compatibility();
        this.UpdateAutoStartRegistrySetting();
        dynamicSettings.Save();
        this.MainWindow = new AppMainWindow(this, this.model);
        Task.Run((Action) (() => this.RPCServerTaskHandler()));
      }
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
      ErrorInfo error = new ErrorInfo(nameof (CurrentDomain_UnhandledException), "Unhandled domain error", e.ExceptionObject as Exception);
      this.model.LogError(error);
      DesktopTelemetry.LogError(error);
      this.model.TelemetryListener.Flush();
    }

    private void App_DispatcherUnhandledException(
      object sender,
      DispatcherUnhandledExceptionEventArgs e)
    {
      e.Handled = true;
      ErrorInfo error = new ErrorInfo(nameof (App_DispatcherUnhandledException), "Unhandled dispatcher error", e.Exception);
      this.model.LogError(error);
      DesktopTelemetry.LogError(error);
      this.model.TelemetryListener.Flush();
    }

    private void Application_Exit(object sender, ExitEventArgs e) => this.ApplicationCleanup();

    private void Application_SessionEnding(object sender, SessionEndingCancelEventArgs e) => this.ApplicationCleanup();

    private void ApplicationCleanup()
    {
      if (this.model == null)
        return;
      this.model.SaveInsightsData();
      this.model.DynamicSettings.Save();
      this.UpdateAutoStartRegistrySetting();
    }

    private CommandLineSettings ParseCommandLine(string[] args)
    {
      CommandLineSettings commandLineSettings = new CommandLineSettings();
      foreach (string str in args)
      {
        string lower = str.ToLower();
        if (lower == "-h" || lower == "--hidden")
          commandLineSettings.Hidden = true;
      }
      return commandLineSettings;
    }

    private void VerifyWebBrowserControlIE8Compatibility()
    {
      try
      {
        using (RegistryKey subKey = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION"))
          subKey.SetValue(Globals.ApplicationFileName, (object) 8000U, RegistryValueKind.DWord);
      }
      catch
      {
      }
    }

    private void UpdateAutoStartRegistrySetting()
    {
      try
      {
        using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true))
        {
          if (this.model.DynamicSettings.StartOnLogin)
            registryKey.SetValue(Globals.ApplicationName, (object) string.Format("\"{0}\" --hidden", (object) Globals.ApplicationFilePath));
          else
            registryKey.DeleteValue(Globals.ApplicationName, false);
        }
      }
      catch
      {
      }
    }

    public void Dispose() => this.singletonMutex.Dispose();

    private void RPCServerTaskHandler()
    {
      while (true)
      {
        try
        {
          using (NamedPipeServerStream pipeServerStream = new NamedPipeServerStream(string.Format("\\\\.\\Pipe\\MicrosoftBandSyncIPC_{0:0000000000}", (object) Process.GetCurrentProcess().SessionId)))
          {
            pipeServerStream.WaitForConnection();
            using (StreamWriter streamWriter = new StreamWriter((Stream) pipeServerStream, Encoding.UTF8, 32, true))
            {
              using (StreamReader streamReader = new StreamReader((Stream) pipeServerStream, Encoding.UTF8, false, 32, true))
              {
                bool flag = true;
                while (flag)
                {
                  switch (streamReader.ReadLine())
                  {
                    case "GET_PROCESS_ID":
                      streamWriter.WriteLine("{0}", (object) Process.GetCurrentProcess().Id);
                      streamWriter.Flush();
                      continue;
                    case "SHOW":
                      Task.Factory.StartNew((Action) (() => this.ShowAppTaskHandler()), CancellationToken.None, TaskCreationOptions.None, this.ForegroundTaskScheduler);
                      continue;
                    case null:
                      flag = false;
                      continue;
                    default:
                      flag = false;
                      continue;
                  }
                }
              }
            }
          }
        }
        catch
        {
        }
      }
    }

    private void ShowAppTaskHandler() => this.model.ShowAppCommand.Execute((object) null);

    private void ShowRunningApp()
    {
      using (NamedPipeClientStream pipeClientStream = new NamedPipeClientStream(string.Format("\\\\.\\Pipe\\MicrosoftBandSyncIPC_{0:0000000000}", (object) Process.GetCurrentProcess().SessionId)))
      {
        try
        {
          pipeClientStream.Connect(500);
          using (StreamWriter streamWriter = new StreamWriter((Stream) pipeClientStream, Encoding.UTF8, 32, true))
          {
            using (StreamReader streamReader = new StreamReader((Stream) pipeClientStream, Encoding.UTF8, false, 32, true))
            {
              streamWriter.WriteLine("GET_PROCESS_ID");
              streamWriter.Flush();
              string s = streamReader.ReadLine();
              int result;
              if (s == null || !int.TryParse(s, out result))
                return;
              User32.AllowSetForegroundWindow(result);
              streamWriter.WriteLine("SHOW");
              streamWriter.Flush();
            }
          }
        }
        catch
        {
        }
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      this.Exit += new ExitEventHandler(this.Application_Exit);
      this.SessionEnding += new SessionEndingCancelEventHandler(this.Application_SessionEnding);
      Application.LoadComponent((object) this, new Uri("/Microsoft Band Sync;component/app.xaml", UriKind.Relative));
    }

    [STAThread]
    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public static void Main()
    {
      App app = new App();
      app.InitializeComponent();
      app.Run();
    }
  }
}
