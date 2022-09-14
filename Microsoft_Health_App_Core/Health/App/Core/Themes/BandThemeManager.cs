// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Themes.BandThemeManager
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Themes
{
  public class BandThemeManager : HealthObservableObject, IBandThemeManager, INotifyPropertyChanged
  {
    public const string RecoveredBandThemeIdStorageKey = "RecoveredDeviceTheme3";
    private const string CurrentBandThemeIdStorageKey = "CurrentDeviceTheme3";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Themes\\BandThemeManager.cs");
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly IConfigProvider configProvider;
    private Dictionary<ushort, BandBackgroundStyle> backgroundStylesDictionary;
    private Dictionary<ushort, BandColorSet> colorSetDictionary;
    private IEnumerable<BandColorSet> colorSets;
    private AppBandTheme currentTheme;
    private AppBandTheme defaultTheme;
    private BandClass currentBandClass;
    private IEnumerable<BandBackgroundStyle> backgroundStyles;
    private IEnumerable<AppBandTheme> themes;
    private Dictionary<uint, AppBandTheme> themesDictionary;
    private static readonly BandColorSet Coral = new BandColorSet((ushort) 2, nameof (Coral), new ArgbColor32(4292430950U), new ArgbColor32(4293547125U), new ArgbColor32(4291184483U), new ArgbColor32(4287994781U), new ArgbColor32(4294397306U), new ArgbColor32(4288230212U));
    private static readonly BandColorSet Cornflower = new BandColorSet((ushort) 13, nameof (Cornflower), new ArgbColor32(4281558732U), new ArgbColor32(4282022109U), new ArgbColor32(4281427386U), new ArgbColor32(4289177770U), new ArgbColor32(4282022109U), new ArgbColor32(4279780988U));
    private static readonly BandColorSet Cyber = new BandColorSet((ushort) 3, nameof (Cyber), new ArgbColor32(4281974639U), new ArgbColor32(4282502778U), new ArgbColor32(4281707109U), new ArgbColor32(4288190870U), new ArgbColor32(4281852540U), new ArgbColor32(4281431887U));
    private static readonly BandColorSet Electric = new BandColorSet((ushort) 6, nameof (Electric), new ArgbColor32(4278237682U), new ArgbColor32(4284145919U), new ArgbColor32(4278235867U), new ArgbColor32(4288584605U), new ArgbColor32(4279095807U), new ArgbColor32(4278413941U));
    private static readonly BandColorSet Flame = new BandColorSet((ushort) 7, nameof (Flame), new ArgbColor32(4293939982U), new ArgbColor32(4294731325U), new ArgbColor32(4292690958U), new ArgbColor32(4288584605U), new ArgbColor32(4294930248U), new ArgbColor32(4286521115U));
    private static readonly BandColorSet Fuchsia = new BandColorSet((ushort) 8, nameof (Fuchsia), new ArgbColor32(4292425433U), new ArgbColor32(4294192895U), new ArgbColor32(4290917574U), new ArgbColor32(4288584605U), new ArgbColor32(4293938169U), new ArgbColor32(4286254980U));
    private static readonly BandColorSet Joule = new BandColorSet((ushort) 4, nameof (Joule), new ArgbColor32(4294946560U), new ArgbColor32(4294946560U), new ArgbColor32(4294547971U), new ArgbColor32(4288387736U), new ArgbColor32(4294949888U), new ArgbColor32(4288899072U));
    private static readonly BandColorSet Lime = new BandColorSet((ushort) 9, nameof (Lime), new ArgbColor32(4288268308U), new ArgbColor32(4289846038U), new ArgbColor32(4286162991U), new ArgbColor32(4288584605U), new ArgbColor32(4288142144U), new ArgbColor32(4283523866U));
    private static readonly BandColorSet Orchid = new BandColorSet((ushort) 5, nameof (Orchid), new ArgbColor32(4288120751U), new ArgbColor32(4289503169U), new ArgbColor32(4286477966U), new ArgbColor32(4288059033U), new ArgbColor32(4290225619U), new ArgbColor32(4285030770U));
    private static readonly BandColorSet Penguin = new BandColorSet((ushort) 12, nameof (Penguin), new ArgbColor32(4279571733U), new ArgbColor32(4294946560U), new ArgbColor32(4279308561U), new ArgbColor32(4286217340U), new ArgbColor32(4281348144U), new ArgbColor32(4288899072U));
    private static readonly BandColorSet Storm = new BandColorSet((ushort) 10, nameof (Storm), new ArgbColor32(4279571733U), new ArgbColor32(4282112767U), new ArgbColor32(4279308561U), new ArgbColor32(4286217340U), new ArgbColor32(4281348144U), new ArgbColor32(4278224549U));
    private static readonly BandColorSet Tuxedo = new BandColorSet((ushort) 11, nameof (Tuxedo), new ArgbColor32(4279571733U), new ArgbColor32(4290230199U), new ArgbColor32(4279308561U), new ArgbColor32(4286217340U), new ArgbColor32(4281348144U), new ArgbColor32(4282729797U));
    private static readonly BandColorSet Violet = new BandColorSet((ushort) 1, nameof (Violet), new ArgbColor32(4286071503U), new ArgbColor32(4287324658U), new ArgbColor32(4285087676U), new ArgbColor32(4289177514U), new ArgbColor32(4287126265U), new ArgbColor32(4282920332U));
    private static readonly BandColorSet BerryNeon = new BandColorSet((ushort) 10, "Berry", new ArgbColor32(4285996668U), new ArgbColor32(4293412351U), new ArgbColor32(4284160096U), new ArgbColor32(4289608902U), new ArgbColor32(4287309203U), new ArgbColor32(4285996668U));
    private static readonly BandColorSet CaliforniaNeon = new BandColorSet((ushort) 15, "California", new ArgbColor32(4279571733U), new ArgbColor32(4290314013U), new ArgbColor32(4279308561U), new ArgbColor32(4290423821U), new ArgbColor32(4280624421U), new ArgbColor32(4283721738U));
    private static readonly BandColorSet CargoNeon = new BandColorSet((ushort) 11, "Cargo", new ArgbColor32(4286071503U), new ArgbColor32(4289227257U), new ArgbColor32(4284889264U), new ArgbColor32(4287646706U), new ArgbColor32(4287258574U), new ArgbColor32(4282590324U));
    private static readonly BandColorSet CoralNeon = new BandColorSet((ushort) 8, nameof (Coral), new ArgbColor32(4293347414U), new ArgbColor32(4294922834U), new ArgbColor32(4291116361U), new ArgbColor32(4293347414U), new ArgbColor32(4294922855U), new ArgbColor32(4288230212U));
    private static readonly BandColorSet CyberNeon = new BandColorSet((ushort) 4, nameof (Cyber), new ArgbColor32(4281974639U), new ArgbColor32(4280088447U), new ArgbColor32(4281443166U), new ArgbColor32(4279881336U), new ArgbColor32(4281852540U), new ArgbColor32(4279789111U));
    private static readonly BandColorSet DJNeon = new BandColorSet((ushort) 14, "DJ", new ArgbColor32(4279571733U), new ArgbColor32(4280088447U), new ArgbColor32(4279308561U), new ArgbColor32(4279881336U), new ArgbColor32(4280624421U), new ArgbColor32(4279789111U));
    private static readonly BandColorSet ElectricNeon = new BandColorSet((ushort) 1, nameof (Electric), new ArgbColor32(4278237682U), new ArgbColor32(4284145919U), new ArgbColor32(4278230478U), new ArgbColor32(4278230478U), new ArgbColor32(4279095807U), new ArgbColor32(4278209124U));
    private static readonly BandColorSet KaleNeon = new BandColorSet((ushort) 3, "Kale", new ArgbColor32(4278418556U), new ArgbColor32(4282974172U), new ArgbColor32(4278546544U), new ArgbColor32(4279089841U), new ArgbColor32(4280125833U), new ArgbColor32(4278413409U));
    private static readonly BandColorSet KoolAidNeon = new BandColorSet((ushort) 9, "KoolAid", new ArgbColor32(4291890557U), new ArgbColor32(4294919094U), new ArgbColor32(4289858410U), new ArgbColor32(4292222353U), new ArgbColor32(4292358544U), new ArgbColor32(4287172692U));
    private static readonly BandColorSet KillaBeeNeon = new BandColorSet((ushort) 16, "KillaBee", new ArgbColor32(4279571733U), new ArgbColor32(4294949888U), new ArgbColor32(4279308561U), new ArgbColor32(4293564451U), new ArgbColor32(4280624421U), new ArgbColor32(4289554719U));
    private static readonly BandColorSet LasertagNeon = new BandColorSet((ushort) 18, "Lasertag", new ArgbColor32(4279571733U), new ArgbColor32(4294919094U), new ArgbColor32(4279308561U), new ArgbColor32(4292222353U), new ArgbColor32(4280624421U), new ArgbColor32(4287172692U));
    private static readonly BandColorSet LimeNeon = new BandColorSet((ushort) 5, nameof (Lime), new ArgbColor32(4288925209U), new ArgbColor32(4290314013U), new ArgbColor32(4288263964U), new ArgbColor32(4287278093U), new ArgbColor32(4290766111U), new ArgbColor32(4283721738U));
    private static readonly BandColorSet PizzaNeon = new BandColorSet((ushort) 17, "Pizza", new ArgbColor32(4279571733U), new ArgbColor32(4294922834U), new ArgbColor32(4279308561U), new ArgbColor32(4293347414U), new ArgbColor32(4280624421U), new ArgbColor32(4288230212U));
    private static readonly BandColorSet SkylineNeon = new BandColorSet((ushort) 2, "Skyline", new ArgbColor32(4278212470U), new ArgbColor32(4278827478U), new ArgbColor32(4278209124U), new ArgbColor32(4278491322U), new ArgbColor32(4279920772U), new ArgbColor32(4278212470U));
    private static readonly BandColorSet StormNeon = new BandColorSet((ushort) 13, nameof (Storm), new ArgbColor32(4279571733U), new ArgbColor32(4284145919U), new ArgbColor32(4279308561U), new ArgbColor32(4278230478U), new ArgbColor32(4280624421U), new ArgbColor32(4278209124U));
    private static readonly BandColorSet TangNeon = new BandColorSet((ushort) 7, "Tang", new ArgbColor32(4294009890U), new ArgbColor32(4294929477U), new ArgbColor32(4292236321U), new ArgbColor32(4291578649U), new ArgbColor32(4293752379U), new ArgbColor32(4287050514U));
    private static readonly BandColorSet TangerineNeon = new BandColorSet((ushort) 6, "Tangerine", new ArgbColor32(4294417174U), new ArgbColor32(4294949888U), new ArgbColor32(4293758493U), new ArgbColor32(4293564451U), new ArgbColor32(4294485294U), new ArgbColor32(4289554719U));
    private static readonly BandColorSet TuxedoNeon = new BandColorSet((ushort) 12, nameof (Tuxedo), new ArgbColor32(4279571733U), new ArgbColor32(4294111986U), new ArgbColor32(4279308561U), new ArgbColor32(4286217340U), new ArgbColor32(4280624421U), new ArgbColor32(4281545523U));
    private static readonly BandBackgroundStyle Angle = new BandBackgroundStyle((ushort) 1, nameof (Angle));
    private static readonly BandBackgroundStyle Blank = new BandBackgroundStyle((ushort) 12, nameof (Blank));
    private static readonly BandBackgroundStyle Blinds = new BandBackgroundStyle((ushort) 2, nameof (Blinds));
    private static readonly BandBackgroundStyle Folds = new BandBackgroundStyle((ushort) 3, nameof (Folds));
    private static readonly BandBackgroundStyle Honeycomb = new BandBackgroundStyle((ushort) 4, nameof (Honeycomb));
    private static readonly BandBackgroundStyle Mesh = new BandBackgroundStyle((ushort) 5, nameof (Mesh));
    private static readonly BandBackgroundStyle Petals = new BandBackgroundStyle((ushort) 6, nameof (Petals));
    private static readonly BandBackgroundStyle Pixels = new BandBackgroundStyle((ushort) 7, nameof (Pixels));
    private static readonly BandBackgroundStyle Sequins = new BandBackgroundStyle((ushort) 8, nameof (Sequins));
    private static readonly BandBackgroundStyle Stripes = new BandBackgroundStyle((ushort) 9, nameof (Stripes));
    private static readonly BandBackgroundStyle Triangles = new BandBackgroundStyle((ushort) 10, nameof (Triangles));
    private static readonly BandBackgroundStyle Vortex = new BandBackgroundStyle((ushort) 11, nameof (Vortex));
    private static readonly BandBackgroundStyle BlankNeon = new BandBackgroundStyle((ushort) 10, nameof (Blank));
    private static readonly BandBackgroundStyle ChevsNeon = new BandBackgroundStyle((ushort) 1, "Chevs");
    private static readonly BandBackgroundStyle CurvesNeon = new BandBackgroundStyle((ushort) 2, "Curves");
    private static readonly BandBackgroundStyle DanNeon = new BandBackgroundStyle((ushort) 3, "Dan");
    private static readonly BandBackgroundStyle FastNeon = new BandBackgroundStyle((ushort) 4, "Fast");
    private static readonly BandBackgroundStyle FiberNeon = new BandBackgroundStyle((ushort) 5, "Fiber");
    private static readonly BandBackgroundStyle FwdNeon = new BandBackgroundStyle((ushort) 6, "Fwd");
    private static readonly BandBackgroundStyle NoodsNeon = new BandBackgroundStyle((ushort) 7, "Noods");
    private static readonly BandBackgroundStyle PlatesNeon = new BandBackgroundStyle((ushort) 8, "Plates");
    private static readonly BandBackgroundStyle TimeNeon = new BandBackgroundStyle((ushort) 9, "Time");

    public BandThemeManager(
      IBandConnectionFactory cargoConnectionFactory,
      IConfigProvider configProvider)
    {
      BandThemeManager.Logger.Debug((object) "Begin BandThemeManager()");
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.configProvider = configProvider;
      this.SetDeviceType(BandClass.Envoy);
      BandThemeManager.Logger.Debug((object) "End BandThemeManager()");
    }

    public void SetDeviceType(BandClass bandClass)
    {
      if (this.currentBandClass == bandClass)
        return;
      this.InitializeColorSets(bandClass);
      this.InitializeBackgroundStyles(bandClass);
      this.InitializeThemes(bandClass);
      this.currentBandClass = bandClass;
      this.DefaultTheme = bandClass != BandClass.Cargo ? this.themesDictionary[AppBandTheme.GetThemeId(BandThemeManager.StormNeon, BandThemeManager.ChevsNeon)] : this.themesDictionary[AppBandTheme.GetThemeId(BandThemeManager.Violet, BandThemeManager.Honeycomb)];
      this.SetCurrentTheme(this.configProvider.Get<uint>("CurrentDeviceTheme3", this.DefaultTheme.Id));
    }

    public async Task SetDeviceTypeAsync()
    {
      BandClass bandClass = BandClass.Unknown;
      using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
      {
        using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationTokenSource.Token))
          bandClass = (await cargoConnection.GetAppBandConstantsAsync(cancellationTokenSource.Token)).BandClass;
      }
      this.SetDeviceType(bandClass);
    }

    public IEnumerable<BandBackgroundStyle> BackgroundStyles
    {
      get => this.backgroundStyles;
      private set => this.SetProperty<IEnumerable<BandBackgroundStyle>>(ref this.backgroundStyles, value, nameof (BackgroundStyles));
    }

    public IEnumerable<BandColorSet> ColorSets
    {
      get => this.colorSets;
      private set => this.SetProperty<IEnumerable<BandColorSet>>(ref this.colorSets, value, nameof (ColorSets));
    }

    public IEnumerable<AppBandTheme> Themes
    {
      get => this.themes;
      private set => this.SetProperty<IEnumerable<AppBandTheme>>(ref this.themes, value, nameof (Themes));
    }

    public AppBandTheme CurrentTheme
    {
      get => this.currentTheme;
      private set => this.SetProperty<AppBandTheme>(ref this.currentTheme, value, nameof (CurrentTheme));
    }

    public AppBandTheme DefaultTheme
    {
      get => this.defaultTheme;
      private set => this.SetProperty<AppBandTheme>(ref this.defaultTheme, value, nameof (DefaultTheme));
    }

    public bool IsThemeNotSaved => (int) this.configProvider.Get<uint>("CurrentDeviceTheme3", this.DefaultTheme.Id) != (int) this.configProvider.Get<uint>("RecoveredDeviceTheme3", this.DefaultTheme.Id);

    public void SwitchActiveColorSet(BandColorSet bandColorSet) => this.SetCurrentTheme(AppBandTheme.GetThemeId(bandColorSet, this.CurrentTheme.BackgroundStyle));

    public void SwitchActiveBackgroundStyle(BandBackgroundStyle bandBackgroundStyle) => this.SetCurrentTheme(AppBandTheme.GetThemeId(this.CurrentTheme.ColorSet, bandBackgroundStyle));

    public void SetCurrentTheme(AppBandTheme bandTheme)
    {
      if (this.CurrentTheme == bandTheme)
        return;
      if (this.currentBandClass != bandTheme.BandClass)
        this.SetDeviceType(bandTheme.BandClass);
      BandThemeManager.Logger.Debug("<FLAG> switching currently selected band theme (id={0})", (object) bandTheme.Id);
      this.CurrentTheme = bandTheme;
      this.configProvider.Set<uint>("CurrentDeviceTheme3", this.CurrentTheme.Id);
      BandThemeManager.Logger.Debug("<FLAG> active band theme is now (colorName={0}, colorId={1}, styleName={2}, styleId={3})", (object) this.CurrentTheme.ColorSet.Name, (object) this.CurrentTheme.ColorSet.Id, (object) this.CurrentTheme.BackgroundStyle.Name, (object) this.CurrentTheme.BackgroundStyle.Id);
    }

    public async Task<AppBandTheme> GetCurrentThemeFromBandAsync(
      CancellationToken cancellationToken)
    {
      BandThemeManager.Logger.Debug((object) "<START> downloading currently set theme infomation from the band");
      using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
      {
        IDynamicBandConstants bandConstantsAsync = await cargoConnection.GetAppBandConstantsAsync(cancellationToken);
        if (this.currentBandClass != bandConstantsAsync.BandClass)
          this.SetDeviceType(bandConstantsAsync.BandClass);
        uint meTileIdAsync = await cargoConnection.GetMeTileIdAsync(cancellationToken);
        BandThemeManager.Logger.Debug((object) "<END> downloading currently set theme infomation from the band");
        this.SetCurrentTheme(meTileIdAsync);
      }
      this.configProvider.Set<uint>("RecoveredDeviceTheme3", this.CurrentTheme.Id);
      return this.CurrentTheme;
    }

    public AppBandTheme RevertTheme()
    {
      this.SetCurrentTheme(this.configProvider.Get<uint>("RecoveredDeviceTheme3", this.DefaultTheme.Id));
      return this.CurrentTheme;
    }

    private void InitializeColorSets(BandClass deviceType)
    {
      if (deviceType == BandClass.Cargo)
        this.ColorSets = (IEnumerable<BandColorSet>) new List<BandColorSet>()
        {
          BandThemeManager.Cornflower,
          BandThemeManager.Violet,
          BandThemeManager.Coral,
          BandThemeManager.Cyber,
          BandThemeManager.Joule,
          BandThemeManager.Orchid,
          BandThemeManager.Electric,
          BandThemeManager.Flame,
          BandThemeManager.Fuchsia,
          BandThemeManager.Lime,
          BandThemeManager.Storm,
          BandThemeManager.Tuxedo,
          BandThemeManager.Penguin
        };
      else
        this.ColorSets = (IEnumerable<BandColorSet>) new List<BandColorSet>()
        {
          BandThemeManager.ElectricNeon,
          BandThemeManager.SkylineNeon,
          BandThemeManager.KaleNeon,
          BandThemeManager.CyberNeon,
          BandThemeManager.LimeNeon,
          BandThemeManager.TangerineNeon,
          BandThemeManager.TangNeon,
          BandThemeManager.CoralNeon,
          BandThemeManager.KoolAidNeon,
          BandThemeManager.BerryNeon,
          BandThemeManager.CargoNeon,
          BandThemeManager.TuxedoNeon,
          BandThemeManager.StormNeon,
          BandThemeManager.DJNeon,
          BandThemeManager.CaliforniaNeon,
          BandThemeManager.KillaBeeNeon,
          BandThemeManager.PizzaNeon,
          BandThemeManager.LasertagNeon
        };
      this.colorSetDictionary = this.ColorSets.ToDictionary<BandColorSet, ushort>((Func<BandColorSet, ushort>) (colorSet => colorSet.Id));
    }

    private void InitializeBackgroundStyles(BandClass deviceType)
    {
      if (deviceType == BandClass.Cargo)
        this.BackgroundStyles = (IEnumerable<BandBackgroundStyle>) new List<BandBackgroundStyle>()
        {
          BandThemeManager.Blank,
          BandThemeManager.Angle,
          BandThemeManager.Blinds,
          BandThemeManager.Folds,
          BandThemeManager.Honeycomb,
          BandThemeManager.Mesh,
          BandThemeManager.Petals,
          BandThemeManager.Pixels,
          BandThemeManager.Sequins,
          BandThemeManager.Stripes,
          BandThemeManager.Triangles,
          BandThemeManager.Vortex
        };
      else
        this.BackgroundStyles = (IEnumerable<BandBackgroundStyle>) new List<BandBackgroundStyle>()
        {
          BandThemeManager.BlankNeon,
          BandThemeManager.ChevsNeon,
          BandThemeManager.CurvesNeon,
          BandThemeManager.DanNeon,
          BandThemeManager.FastNeon,
          BandThemeManager.FiberNeon,
          BandThemeManager.FwdNeon,
          BandThemeManager.NoodsNeon,
          BandThemeManager.PlatesNeon,
          BandThemeManager.TimeNeon
        };
      this.backgroundStylesDictionary = this.BackgroundStyles.ToDictionary<BandBackgroundStyle, ushort>((Func<BandBackgroundStyle, ushort>) (backgroundStyle => backgroundStyle.Id));
    }

    private void InitializeThemes(BandClass deviceType)
    {
      this.Themes = (IEnumerable<AppBandTheme>) this.colorSetDictionary.Values.SelectMany<BandColorSet, BandBackgroundStyle, AppBandTheme>((Func<BandColorSet, IEnumerable<BandBackgroundStyle>>) (colorSet => (IEnumerable<BandBackgroundStyle>) this.backgroundStylesDictionary.Values), (Func<BandColorSet, BandBackgroundStyle, AppBandTheme>) ((colorSet, bandBackgroundStyle) => new AppBandTheme(bandBackgroundStyle, colorSet, deviceType))).ToList<AppBandTheme>();
      this.themesDictionary = this.Themes.ToDictionary<AppBandTheme, uint>((Func<AppBandTheme, uint>) (bandTheme => bandTheme.Id));
    }

    private void SetCurrentTheme(uint id)
    {
      AppBandTheme bandTheme;
      if (this.themesDictionary.TryGetValue(id, out bandTheme))
      {
        this.SetCurrentTheme(bandTheme);
      }
      else
      {
        BandThemeManager.Logger.Warn("tried to switch to a theme that is not recognized, using default (badId={0})", (object) id);
        this.SetCurrentTheme(this.DefaultTheme);
      }
    }
  }
}
