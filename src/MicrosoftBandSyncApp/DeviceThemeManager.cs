// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.DeviceThemeManager
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;
using System;
using System.ComponentModel;

namespace DesktopSyncApp
{
  public class DeviceThemeManager : INotifyPropertyChanged
  {
    private ThemeComponent currentThemeComponent;
    private Command showColorSets;
    private Command showPatterns;
    private bool updatingDeviceTheme;
    private bool needToSave;
    private int originalColorIndex;
    private int originalPatternIndex;
    private BandClass currentBandClass;

    public event PropertyChangedEventHandler PropertyChanged;

    public DeviceThemeManager(BandClass bandClass) => this.SetBandClass(bandClass);

    public DeviceThemeManager() => this.SetBandClass((BandClass) 2);

    public void SetBandClass(BandClass bandClass)
    {
      if ((int)bandClass == 2)
        this.InitializeForEnvoy();
      else
        this.InitializeForCargo();
      this.ColorSets.TrySetSelectedItemById(this.DefaultColorId);
      this.Patterns.TrySetSelectedItemById(this.DefaultPatternId);
      this.ColorSets.SelectedIndexChanged += new PropertyValueChangedEventHandler(this.ColorOrPatternsSelectedIndexChanged);
      this.Patterns.SelectedIndexChanged += new PropertyValueChangedEventHandler(this.ColorOrPatternsSelectedIndexChanged);
    }

    public int DefaultColorId
    {
      get
      {
        BandClass currentBandClass = this.currentBandClass;
        if ((int)currentBandClass == 1)
          return 1;
        if ((int)currentBandClass == 2)
          return 13;
        throw new InvalidOperationException("Internal error: Unknown band class");
      }
    }

    public int DefaultPatternId
    {
      get
      {
        BandClass currentBandClass = this.currentBandClass;
        if ((int)currentBandClass == 1)
          return 4;
        if ((int)currentBandClass == 2)
          return 1;
        throw new InvalidOperationException("Internal error: Unknown band class");
      }
    }

    private void InitializeForCargo()
    {
      this.ColorSets = new ThemeItemCollection<ThemeColorSet>(this, new ThemeColorSet[13]
      {
        new ThemeColorSet(this, 0, 13, "Cornflower", "cornflower", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4281558732U), ColorUtils.RgbToBandColor(4282022109U), ColorUtils.RgbToBandColor(4281427386U), ColorUtils.RgbToBandColor(4289177770U), ColorUtils.RgbToBandColor(4282022109U), ColorUtils.RgbToBandColor(4279780988U))),
        new ThemeColorSet(this, 1, 1, "Violet", "violet", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4286071503U), ColorUtils.RgbToBandColor(4287324658U), ColorUtils.RgbToBandColor(4285087676U), ColorUtils.RgbToBandColor(4289177514U), ColorUtils.RgbToBandColor(4287126265U), ColorUtils.RgbToBandColor(4282920332U))),
        new ThemeColorSet(this, 2, 2, "Coral", "coral", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4292430950U), ColorUtils.RgbToBandColor(4293547125U), ColorUtils.RgbToBandColor(4291184483U), ColorUtils.RgbToBandColor(4287994781U), ColorUtils.RgbToBandColor(4294397306U), ColorUtils.RgbToBandColor(4288230212U))),
        new ThemeColorSet(this, 3, 3, "Cyber", "cyber", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4281974639U), ColorUtils.RgbToBandColor(4282502778U), ColorUtils.RgbToBandColor(4281707109U), ColorUtils.RgbToBandColor(4288190870U), ColorUtils.RgbToBandColor(4281852540U), ColorUtils.RgbToBandColor(4281431887U))),
        new ThemeColorSet(this, 4, 4, "Joule", "joule", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4294946560U), ColorUtils.RgbToBandColor(4294946560U), ColorUtils.RgbToBandColor(4294547971U), ColorUtils.RgbToBandColor(4288387736U), ColorUtils.RgbToBandColor(4294949888U), ColorUtils.RgbToBandColor(4288899072U))),
        new ThemeColorSet(this, 5, 5, "Orchid", "orchid", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4288120751U), ColorUtils.RgbToBandColor(4289503169U), ColorUtils.RgbToBandColor(4286477966U), ColorUtils.RgbToBandColor(4288059033U), ColorUtils.RgbToBandColor(4290225619U), ColorUtils.RgbToBandColor(4285030770U))),
        new ThemeColorSet(this, 6, 6, "Electric", "electric", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4278237682U), ColorUtils.RgbToBandColor(4284145919U), ColorUtils.RgbToBandColor(4278235867U), ColorUtils.RgbToBandColor(4288584605U), ColorUtils.RgbToBandColor(4279095807U), ColorUtils.RgbToBandColor(4278413941U))),
        new ThemeColorSet(this, 7, 7, "Flame", "flame", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4293939982U), ColorUtils.RgbToBandColor(4294731325U), ColorUtils.RgbToBandColor(4292690958U), ColorUtils.RgbToBandColor(4288584605U), ColorUtils.RgbToBandColor(4294930248U), ColorUtils.RgbToBandColor(4286521115U))),
        new ThemeColorSet(this, 8, 8, "Fuchsia", "fuchsia", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4292425433U), ColorUtils.RgbToBandColor(4294192895U), ColorUtils.RgbToBandColor(4290917574U), ColorUtils.RgbToBandColor(4288584605U), ColorUtils.RgbToBandColor(4293938169U), ColorUtils.RgbToBandColor(4286254980U))),
        new ThemeColorSet(this, 9, 9, "Lime", "lime", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4288268308U), ColorUtils.RgbToBandColor(4289846038U), ColorUtils.RgbToBandColor(4286162991U), ColorUtils.RgbToBandColor(4288584605U), ColorUtils.RgbToBandColor(4288142144U), ColorUtils.RgbToBandColor(4283523866U))),
        new ThemeColorSet(this, 10, 10, "Storm", "storm", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4279571733U), ColorUtils.RgbToBandColor(4282112767U), ColorUtils.RgbToBandColor(4279308561U), ColorUtils.RgbToBandColor(4286217340U), ColorUtils.RgbToBandColor(4281348144U), ColorUtils.RgbToBandColor(4278224549U))),
        new ThemeColorSet(this, 11, 11, "Tuxedo", "tuxedo", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4279571733U), ColorUtils.RgbToBandColor(4290230199U), ColorUtils.RgbToBandColor(4279308561U), ColorUtils.RgbToBandColor(4286217340U), ColorUtils.RgbToBandColor(4281348144U), ColorUtils.RgbToBandColor(4282729797U))),
        new ThemeColorSet(this, 12, 12, "Penguin", "penguin", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4279571733U), ColorUtils.RgbToBandColor(4294946560U), ColorUtils.RgbToBandColor(4279308561U), ColorUtils.RgbToBandColor(4286217340U), ColorUtils.RgbToBandColor(4281348144U), ColorUtils.RgbToBandColor(4288899072U)))
      });
      this.Patterns = new ThemePatternCollection(this, new ThemePattern[12]
      {
        new ThemePattern(this, 0, 12, "Blank", "blank"),
        new ThemePattern(this, 1, 1, "Angle", "angle"),
        new ThemePattern(this, 2, 2, "Blinds", "blinds"),
        new ThemePattern(this, 3, 3, "Folds", "folds"),
        new ThemePattern(this, 4, 4, "Honeycomb", "honeycomb"),
        new ThemePattern(this, 5, 5, "Mesh", "mesh"),
        new ThemePattern(this, 6, 6, "Petals", "petals"),
        new ThemePattern(this, 7, 7, "Pixels", "pixels"),
        new ThemePattern(this, 8, 8, "Sequins", "sequins"),
        new ThemePattern(this, 9, 9, "Stripes", "stripes"),
        new ThemePattern(this, 10, 10, "Triangles", "triangles"),
        new ThemePattern(this, 11, 11, "Vortex", "vortex")
      });
      this.CurrentBandClass = (BandClass) 1;
    }

    private void InitializeForEnvoy()
    {
      this.ColorSets = new ThemeItemCollection<ThemeColorSet>(this, new ThemeColorSet[18]
      {
        new ThemeColorSet(this, 0, 1, "Electric", "electric", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4278237682U), ColorUtils.RgbToBandColor(4284145919U), ColorUtils.RgbToBandColor(4278230478U), ColorUtils.RgbToBandColor(4278230478U), ColorUtils.RgbToBandColor(4279095807U), ColorUtils.RgbToBandColor(4278209124U))),
        new ThemeColorSet(this, 1, 2, "Skyline", "skyline", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4278212470U), ColorUtils.RgbToBandColor(4278827478U), ColorUtils.RgbToBandColor(4278209124U), ColorUtils.RgbToBandColor(4278491322U), ColorUtils.RgbToBandColor(4279920772U), ColorUtils.RgbToBandColor(4278212470U))),
        new ThemeColorSet(this, 2, 3, "Kale", "kale", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4278418556U), ColorUtils.RgbToBandColor(4282974172U), ColorUtils.RgbToBandColor(4278546544U), ColorUtils.RgbToBandColor(4279089841U), ColorUtils.RgbToBandColor(4280125833U), ColorUtils.RgbToBandColor(4278413409U))),
        new ThemeColorSet(this, 3, 4, "Cyber", "cyber", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4281974639U), ColorUtils.RgbToBandColor(4280088447U), ColorUtils.RgbToBandColor(4281443166U), ColorUtils.RgbToBandColor(4279881336U), ColorUtils.RgbToBandColor(4281852540U), ColorUtils.RgbToBandColor(4279789111U))),
        new ThemeColorSet(this, 4, 5, "Lime", "lime", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4288925209U), ColorUtils.RgbToBandColor(4290314013U), ColorUtils.RgbToBandColor(4288263964U), ColorUtils.RgbToBandColor(4287278093U), ColorUtils.RgbToBandColor(4290766111U), ColorUtils.RgbToBandColor(4283721738U))),
        new ThemeColorSet(this, 5, 6, "Tangerine", "tangerine", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4294417174U), ColorUtils.RgbToBandColor(4294949888U), ColorUtils.RgbToBandColor(4293758493U), ColorUtils.RgbToBandColor(4293564451U), ColorUtils.RgbToBandColor(4294485294U), ColorUtils.RgbToBandColor(4289554719U))),
        new ThemeColorSet(this, 6, 7, "Tang", "tang", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4294009890U), ColorUtils.RgbToBandColor(4294929477U), ColorUtils.RgbToBandColor(4292236321U), ColorUtils.RgbToBandColor(4291578649U), ColorUtils.RgbToBandColor(4293752379U), ColorUtils.RgbToBandColor(4287050514U))),
        new ThemeColorSet(this, 7, 8, "Coral", "coral", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4293347414U), ColorUtils.RgbToBandColor(4294922834U), ColorUtils.RgbToBandColor(4291116361U), ColorUtils.RgbToBandColor(4293347414U), ColorUtils.RgbToBandColor(4294922855U), ColorUtils.RgbToBandColor(4288230212U))),
        new ThemeColorSet(this, 8, 9, "Kool-Aid", "koolaid", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4291890557U), ColorUtils.RgbToBandColor(4294919094U), ColorUtils.RgbToBandColor(4289858410U), ColorUtils.RgbToBandColor(4292222353U), ColorUtils.RgbToBandColor(4292358544U), ColorUtils.RgbToBandColor(4287172692U))),
        new ThemeColorSet(this, 9, 10, "Berry", "berry", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4285996668U), ColorUtils.RgbToBandColor(4293412351U), ColorUtils.RgbToBandColor(4284160096U), ColorUtils.RgbToBandColor(4289608902U), ColorUtils.RgbToBandColor(4287309203U), ColorUtils.RgbToBandColor(4285996668U))),
        new ThemeColorSet(this, 10, 11, "Cargo", "cargo", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4286071503U), ColorUtils.RgbToBandColor(4289227257U), ColorUtils.RgbToBandColor(4284889264U), ColorUtils.RgbToBandColor(4287646706U), ColorUtils.RgbToBandColor(4287258574U), ColorUtils.RgbToBandColor(4282590324U))),
        new ThemeColorSet(this, 11, 12, "Tuxedo", "tuxedo", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4279571733U), ColorUtils.RgbToBandColor(4294111986U), ColorUtils.RgbToBandColor(4279308561U), ColorUtils.RgbToBandColor(4286217340U), ColorUtils.RgbToBandColor(4280624421U), ColorUtils.RgbToBandColor(4281545523U))),
        new ThemeColorSet(this, 12, 13, "Storm", "storm", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4279571733U), ColorUtils.RgbToBandColor(4284145919U), ColorUtils.RgbToBandColor(4279308561U), ColorUtils.RgbToBandColor(4278230478U), ColorUtils.RgbToBandColor(4280624421U), ColorUtils.RgbToBandColor(4278209124U))),
        new ThemeColorSet(this, 13, 14, "DJ", "dj", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4279571733U), ColorUtils.RgbToBandColor(4280088447U), ColorUtils.RgbToBandColor(4279308561U), ColorUtils.RgbToBandColor(4279881336U), ColorUtils.RgbToBandColor(4280624421U), ColorUtils.RgbToBandColor(4279789111U))),
        new ThemeColorSet(this, 14, 15, "California", "california", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4279571733U), ColorUtils.RgbToBandColor(4290314013U), ColorUtils.RgbToBandColor(4279308561U), ColorUtils.RgbToBandColor(4290423821U), ColorUtils.RgbToBandColor(4280624421U), ColorUtils.RgbToBandColor(4283721738U))),
        new ThemeColorSet(this, 15, 16, "Killa Bee", "killabee", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4279571733U), ColorUtils.RgbToBandColor(4294949888U), ColorUtils.RgbToBandColor(4279308561U), ColorUtils.RgbToBandColor(4293564451U), ColorUtils.RgbToBandColor(4280624421U), ColorUtils.RgbToBandColor(4289554719U))),
        new ThemeColorSet(this, 16, 17, "Pizza", "pizza", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4279571733U), ColorUtils.RgbToBandColor(4294922834U), ColorUtils.RgbToBandColor(4279308561U), ColorUtils.RgbToBandColor(4293347414U), ColorUtils.RgbToBandColor(4280624421U), ColorUtils.RgbToBandColor(4288230212U))),
        new ThemeColorSet(this, 17, 18, "Lasertag", "lasertag", new ThemeColorPaletteProxy(ColorUtils.RgbToBandColor(4279571733U), ColorUtils.RgbToBandColor(4294919094U), ColorUtils.RgbToBandColor(4279308561U), ColorUtils.RgbToBandColor(4292222353U), ColorUtils.RgbToBandColor(4280624421U), ColorUtils.RgbToBandColor(4287172692U)))
      });
      this.Patterns = new ThemePatternCollection(this, new ThemePattern[10]
      {
        new ThemePattern(this, 0, 10, "Blank", "blank"),
        new ThemePattern(this, 1, 1, "Chevs", "chevs"),
        new ThemePattern(this, 2, 2, "Curves", "curves"),
        new ThemePattern(this, 3, 3, "Dan", "dan"),
        new ThemePattern(this, 4, 4, "Fast", "fast"),
        new ThemePattern(this, 5, 5, "Fiber", "fiber"),
        new ThemePattern(this, 6, 6, "Fwd", "fwd"),
        new ThemePattern(this, 7, 7, "Noods", "noods"),
        new ThemePattern(this, 8, 8, "Plates", "plates"),
        new ThemePattern(this, 9, 9, "Time", "time")
      });
      this.CurrentBandClass = (BandClass) 2;
    }

    private void ColorOrPatternsSelectedIndexChanged(object sender, PropertyChangedEventArgs e) => this.NeedToSave = this.originalColorIndex != this.ColorSets.SelectedIndex || this.originalPatternIndex != this.Patterns.SelectedIndex;

    public ThemeComponent CurrentThemeComponent
    {
      get => this.currentThemeComponent;
      set
      {
        if (this.currentThemeComponent == value)
          return;
        this.currentThemeComponent = value;
        this.OnPropertyChanged(nameof (CurrentThemeComponent), this.PropertyChanged);
      }
    }

    public ThemeItemCollection<ThemeColorSet> ColorSets { get; private set; }

    public ThemePatternCollection Patterns { get; private set; }

    public Command ShowColorSets => this.showColorSets ?? (this.showColorSets = new Command((ExecuteHandler) ((param, eventArgs) => this.CurrentThemeComponent = ThemeComponent.ColorSet)));

    public Command ShowPatterns => this.showPatterns ?? (this.showPatterns = new Command((ExecuteHandler) ((param, eventArgs) => this.CurrentThemeComponent = ThemeComponent.Pattern)));

    public bool UpdatingDeviceTheme
    {
      get => this.updatingDeviceTheme;
      set
      {
        if (this.updatingDeviceTheme == value)
          return;
        this.updatingDeviceTheme = value;
        this.OnPropertyChanged(nameof (UpdatingDeviceTheme), this.PropertyChanged);
      }
    }

    public BandClass CurrentBandClass
    {
      get => this.currentBandClass;
      set
      {
        if (this.currentBandClass == value)
          return;
        this.currentBandClass = value;
        this.OnPropertyChanged(nameof (CurrentBandClass), this.PropertyChanged);
      }
    }

    public void SwitchToTheme(uint id)
    {
      if (!this.ColorSets.TrySetSelectedItemById(((int) id & -65536) >> 16))
        this.ColorSets.TrySetSelectedItemById(this.DefaultColorId);
      if (!this.Patterns.TrySetSelectedItemById((int) id & (int) ushort.MaxValue))
        this.Patterns.TrySetSelectedItemById(this.DefaultPatternId);
      this.originalColorIndex = this.ColorSets.SelectedIndex;
      this.originalPatternIndex = this.Patterns.SelectedIndex;
      this.NeedToSave = false;
    }

    public bool NeedToSave
    {
      get => this.needToSave;
      set
      {
        if (this.needToSave == value)
          return;
        this.needToSave = value;
        this.OnPropertyChanged(nameof (NeedToSave), this.PropertyChanged);
      }
    }
  }
}
