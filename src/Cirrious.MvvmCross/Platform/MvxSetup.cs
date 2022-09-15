// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.Platform.MvxSetup
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cirrious.MvvmCross.Platform
{
  public abstract class MvxSetup
  {
    private MvxSetup.MvxSetupState _state;

    protected abstract IMvxTrace CreateDebugTrace();

    protected abstract IMvxApplication CreateApp();

    protected abstract IMvxViewsContainer CreateViewsContainer();

    protected abstract IMvxViewDispatcher CreateViewDispatcher();

    public virtual void Initialize()
    {
      this.InitializePrimary();
      this.InitializeSecondary();
    }

    public virtual void InitializePrimary()
    {
      this.State = this.State == MvxSetup.MvxSetupState.Uninitialized ? MvxSetup.MvxSetupState.InitializingPrimary : throw new MvxException("Cannot start primary - as state already {0}", new object[1]
      {
        (object) this.State
      });
      MvxTrace.Trace("Setup: Primary start");
      this.InitializeIoC();
      this.State = MvxSetup.MvxSetupState.InitializedPrimary;
      this.State = this.State == MvxSetup.MvxSetupState.InitializedPrimary ? MvxSetup.MvxSetupState.InitializingSecondary : throw new MvxException("Cannot start seconday - as state is currently {0}", new object[1]
      {
        (object) this.State
      });
      MvxTrace.Trace("Setup: FirstChance start");
      this.InitializeFirstChance();
      MvxTrace.Trace("Setup: DebugServices start");
      this.InitializeDebugServices();
      MvxTrace.Trace("Setup: PlatformServices start");
      this.InitializePlatformServices();
      MvxTrace.Trace("Setup: MvvmCross settings start");
      this.InitializeSettings();
      MvxTrace.Trace("Setup: Singleton Cache start");
      this.InitializeSingletonCache();
    }

    public virtual void InitializeSecondary()
    {
      MvxTrace.Trace("Setup: Bootstrap actions");
      this.PerformBootstrapActions();
      MvxTrace.Trace("Setup: StringToTypeParser start");
      this.InitializeStringToTypeParser();
      MvxTrace.Trace("Setup: CommandHelper start");
      this.InitializeCommandHelper();
      MvxTrace.Trace("Setup: ViewModelFramework start");
      this.InitializeViewModelFramework();
      MvxTrace.Trace("Setup: PluginManagerFramework start");
      IMvxPluginManager pluginManager = this.InitializePluginFramework();
      MvxTrace.Trace("Setup: App start");
      this.InitializeApp(pluginManager);
      MvxTrace.Trace("Setup: ViewModelTypeFinder start");
      this.InitializeViewModelTypeFinder();
      MvxTrace.Trace("Setup: ViewsContainer start");
      this.InitializeViewsContainer();
      MvxTrace.Trace("Setup: ViewDispatcher start");
      this.InitializeViewDispatcher();
      MvxTrace.Trace("Setup: Views start");
      this.InitializeViewLookup();
      MvxTrace.Trace("Setup: CommandCollectionBuilder start");
      this.InitializeCommandCollectionBuilder();
      MvxTrace.Trace("Setup: NavigationSerializer start");
      this.InitializeNavigationSerializer();
      MvxTrace.Trace("Setup: InpcInterception start");
      this.InitializeInpcInterception();
      MvxTrace.Trace("Setup: LastChance start");
      this.InitializeLastChance();
      MvxTrace.Trace("Setup: Secondary end");
      this.State = MvxSetup.MvxSetupState.Initialized;
    }

    protected virtual void InitializeCommandHelper() => Mvx.RegisterType<IMvxCommandHelper, MvxWeakCommandHelper>();

    protected virtual void InitializeSingletonCache() => MvxSingletonCache.Initialize();

    protected virtual void InitializeInpcInterception()
    {
    }

    protected virtual void InitializeSettings() => Mvx.RegisterSingleton<IMvxSettings>(this.CreateSettings());

    protected virtual IMvxSettings CreateSettings() => (IMvxSettings) new MvxSettings();

    protected virtual void InitializeStringToTypeParser()
    {
      MvxStringToTypeParser stringToTypeParser = this.CreateStringToTypeParser();
      Mvx.RegisterSingleton<IMvxStringToTypeParser>((IMvxStringToTypeParser) stringToTypeParser);
      Mvx.RegisterSingleton<IMvxFillableStringToTypeParser>((IMvxFillableStringToTypeParser) stringToTypeParser);
    }

    protected virtual MvxStringToTypeParser CreateStringToTypeParser() => new MvxStringToTypeParser();

    protected virtual void PerformBootstrapActions()
    {
      MvxBootstrapRunner mvxBootstrapRunner = new MvxBootstrapRunner();
      foreach (Assembly bootstrapOwningAssembly in this.GetBootstrapOwningAssemblies())
        mvxBootstrapRunner.Run(bootstrapOwningAssembly);
    }

    protected virtual void InitializeNavigationSerializer() => Mvx.RegisterSingleton<IMvxNavigationSerializer>(this.CreateNavigationSerializer());

    protected virtual IMvxNavigationSerializer CreateNavigationSerializer() => (IMvxNavigationSerializer) new MvxStringDictionaryNavigationSerializer();

    protected virtual void InitializeCommandCollectionBuilder() => Mvx.RegisterSingleton<IMvxCommandCollectionBuilder>((Func<IMvxCommandCollectionBuilder>) (() => this.CreateCommandCollectionBuilder()));

    protected virtual IMvxCommandCollectionBuilder CreateCommandCollectionBuilder() => (IMvxCommandCollectionBuilder) new MvxCommandCollectionBuilder();

    protected virtual void InitializeIoC() => Mvx.RegisterSingleton<IMvxIoCProvider>(this.CreateIocProvider());

    protected virtual IMvxIocOptions CreateIocOptions() => (IMvxIocOptions) new MvxIocOptions();

    protected virtual IMvxIoCProvider CreateIocProvider() => MvxSimpleIoCContainer.Initialize(this.CreateIocOptions());

    protected virtual void InitializeFirstChance()
    {
    }

    protected virtual void InitializePlatformServices()
    {
    }

    protected virtual void InitializeDebugServices()
    {
      Mvx.RegisterSingleton<IMvxTrace>(this.CreateDebugTrace());
      MvxTrace.Initialize();
    }

    protected virtual void InitializeViewModelFramework() => Mvx.RegisterSingleton<IMvxViewModelLoader>(this.CreateViewModelLoader());

    protected virtual IMvxViewModelLoader CreateViewModelLoader() => (IMvxViewModelLoader) new MvxViewModelLoader();

    protected virtual IMvxPluginManager InitializePluginFramework()
    {
      IMvxPluginManager pluginManager = this.CreatePluginManager();
      pluginManager.ConfigurationSource = new Func<Type, IMvxPluginConfiguration>(this.GetPluginConfiguration);
      Mvx.RegisterSingleton<IMvxPluginManager>(pluginManager);
      this.LoadPlugins(pluginManager);
      return pluginManager;
    }

    protected abstract IMvxPluginManager CreatePluginManager();

    protected virtual IMvxPluginConfiguration GetPluginConfiguration(
      Type plugin)
    {
      return (IMvxPluginConfiguration) null;
    }

    public virtual void LoadPlugins(IMvxPluginManager pluginManager)
    {
    }

    protected virtual void InitializeApp(IMvxPluginManager pluginManager)
    {
      IMvxApplication andInitializeApp = this.CreateAndInitializeApp(pluginManager);
      Mvx.RegisterSingleton<IMvxApplication>(andInitializeApp);
      Mvx.RegisterSingleton<IMvxViewModelLocatorCollection>((IMvxViewModelLocatorCollection) andInitializeApp);
    }

    protected virtual IMvxApplication CreateAndInitializeApp(
      IMvxPluginManager pluginManager)
    {
      IMvxApplication app = this.CreateApp();
      app.LoadPlugins(pluginManager);
      app.Initialize();
      return app;
    }

    protected virtual void InitializeViewsContainer() => Mvx.RegisterSingleton<IMvxViewsContainer>(this.CreateViewsContainer());

    protected virtual void InitializeViewDispatcher()
    {
      IMvxViewDispatcher viewDispatcher = this.CreateViewDispatcher();
      Mvx.RegisterSingleton<IMvxViewDispatcher>(viewDispatcher);
      Mvx.RegisterSingleton<IMvxMainThreadDispatcher>((IMvxMainThreadDispatcher) viewDispatcher);
    }

    protected virtual Assembly[] GetViewAssemblies() => new Assembly[1]
    {
      this.GetType().GetTypeInfo().Assembly
    };

    protected virtual Assembly[] GetViewModelAssemblies() => new Assembly[1]
    {
      Mvx.Resolve<IMvxApplication>().GetType().GetTypeInfo().Assembly
    };

    protected virtual Assembly[] GetBootstrapOwningAssemblies()
    {
      List<Assembly> source = new List<Assembly>();
      source.AddRange((IEnumerable<Assembly>) this.GetViewAssemblies());
      return source.Distinct<Assembly>().ToArray<Assembly>();
    }

    protected abstract IMvxNameMapping CreateViewToViewModelNaming();

    protected virtual void InitializeViewModelTypeFinder()
    {
      MvxViewModelByNameLookup modelByNameLookup = new MvxViewModelByNameLookup();
      foreach (Assembly viewModelAssembly in this.GetViewModelAssemblies())
        modelByNameLookup.AddAll(viewModelAssembly);
      Mvx.RegisterSingleton<IMvxViewModelByNameLookup>((IMvxViewModelByNameLookup) modelByNameLookup);
      Mvx.RegisterSingleton<IMvxViewModelByNameRegistry>((IMvxViewModelByNameRegistry) modelByNameLookup);
      IMvxNameMapping toViewModelNaming = this.CreateViewToViewModelNaming();
      Mvx.RegisterSingleton<IMvxViewModelTypeFinder>((IMvxViewModelTypeFinder) new MvxViewModelViewTypeFinder((IMvxViewModelByNameLookup) modelByNameLookup, toViewModelNaming));
    }

    protected virtual void InitializeViewLookup()
    {
      IDictionary<Type, Type> viewModelViewLookup = new MvxViewModelViewLookupBuilder().Build(this.GetViewAssemblies());
      if (viewModelViewLookup == null)
        return;
      Mvx.Resolve<IMvxViewsContainer>().AddAll(viewModelViewLookup);
    }

    protected virtual void InitializeLastChance()
    {
    }

    protected IEnumerable<Type> CreatableTypes() => this.CreatableTypes(this.GetType().GetTypeInfo().Assembly);

    protected IEnumerable<Type> CreatableTypes(Assembly assembly) => assembly.CreatableTypes();

    public event EventHandler<MvxSetup.MvxSetupStateEventArgs> StateChanged;

    public MvxSetup.MvxSetupState State
    {
      get => this._state;
      private set
      {
        this._state = value;
        this.FireStateChange(value);
      }
    }

    private void FireStateChange(MvxSetup.MvxSetupState state)
    {
      EventHandler<MvxSetup.MvxSetupStateEventArgs> stateChanged = this.StateChanged;
      if (stateChanged == null)
        return;
      stateChanged((object) this, new MvxSetup.MvxSetupStateEventArgs(state));
    }

    public virtual void EnsureInitialized(Type requiredBy)
    {
      switch (this.State)
      {
        case MvxSetup.MvxSetupState.Uninitialized:
          this.Initialize();
          break;
        case MvxSetup.MvxSetupState.InitializingPrimary:
        case MvxSetup.MvxSetupState.InitializedPrimary:
        case MvxSetup.MvxSetupState.InitializingSecondary:
          throw new MvxException("The default EnsureInitialized method does not handle partial initialization");
        case MvxSetup.MvxSetupState.Initialized:
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public enum MvxSetupState
    {
      Uninitialized,
      InitializingPrimary,
      InitializedPrimary,
      InitializingSecondary,
      Initialized,
    }

    public class MvxSetupStateEventArgs : EventArgs
    {
      public MvxSetupStateEventArgs(MvxSetup.MvxSetupState setupState) => this.SetupState = setupState;

      public MvxSetup.MvxSetupState SetupState { get; private set; }
    }
  }
}
