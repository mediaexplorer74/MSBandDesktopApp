// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.AnimatedPageControl
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DesktopSyncApp
{
  public partial class AnimatedPageControl : Grid, IComponentConnector
  {
    private static TimeSpan slideDuration = TimeSpan.FromMilliseconds(300.0);
    private static double accRatio = 0.5;
    private static double decRatio = 0.5;
    private LinkedList<SyncAppPageControl> pageStack = new LinkedList<SyncAppPageControl>();
    private LinkedList<SyncAppPageControl> modalPageStack = new LinkedList<SyncAppPageControl>();
    
    //private bool _contentLoaded;

    public AnimatedPageControl() => this.InitializeComponent();

    public SyncAppPageControl TopPage => this.pageStack.Count <= 0 ? (SyncAppPageControl) null : this.pageStack.First.Value;

    public SyncAppPageControl ModelPage => this.modalPageStack.Count <= 0 ? (SyncAppPageControl) null : this.modalPageStack.First.Value;

    public void ShowPage(SyncAppPageControl page, bool animate = true, PageSlideDirection direction = PageSlideDirection.Left)
    {
      if (page == null)
        throw new ArgumentNullException(nameof (page));
      SyncAppPageControl syncAppPageControl = (SyncAppPageControl) null;
      if (this.pageStack.Count > 0)
        syncAppPageControl = this.pageStack.First.Value;
      if (syncAppPageControl == page)
        return;
      if (syncAppPageControl != null)
      {
        if (this.modalPageStack.Count == 0 & animate)
        {
          TranslateTransform translateTransform = new TranslateTransform();
          syncAppPageControl.RenderTransform = (Transform) translateTransform;
          DoubleAnimation doubleAnimation = new DoubleAnimation();
          doubleAnimation.From = new double?(0.0);
          switch (direction)
          {
            case PageSlideDirection.Left:
              doubleAnimation.To = new double?(-this.ActualWidth);
              break;
            case PageSlideDirection.Right:
              doubleAnimation.To = new double?(this.ActualWidth);
              break;
            case PageSlideDirection.Up:
              doubleAnimation.To = new double?(-this.ActualHeight);
              break;
            case PageSlideDirection.Down:
              doubleAnimation.To = new double?(this.ActualHeight);
              break;
          }
          doubleAnimation.Duration = (Duration) AnimatedPageControl.slideDuration;
          doubleAnimation.AccelerationRatio = AnimatedPageControl.accRatio;
          doubleAnimation.DecelerationRatio = AnimatedPageControl.decRatio;
          translateTransform.BeginAnimation(TranslateTransform.XProperty, (AnimationTimeline) doubleAnimation);
        }
        else
        {
          syncAppPageControl.RenderTransform = (Transform) null;
          syncAppPageControl.Visibility = Visibility.Hidden;
        }
        syncAppPageControl.OnHide();
        syncAppPageControl.IsEnabled = false;
      }
      if (this.modalPageStack.Count == 0 & animate)
      {
        TranslateTransform translateTransform = new TranslateTransform();
        page.RenderTransform = (Transform) translateTransform;
        DoubleAnimation doubleAnimation = new DoubleAnimation();
        switch (direction)
        {
          case PageSlideDirection.Left:
            doubleAnimation.From = new double?(this.ActualWidth);
            break;
          case PageSlideDirection.Right:
            doubleAnimation.From = new double?(-this.ActualWidth);
            break;
          case PageSlideDirection.Up:
            doubleAnimation.From = new double?(this.ActualHeight);
            break;
          case PageSlideDirection.Down:
            doubleAnimation.From = new double?(-this.ActualHeight);
            break;
        }
        doubleAnimation.To = new double?(0.0);
        doubleAnimation.Duration = (Duration) AnimatedPageControl.slideDuration;
        doubleAnimation.AccelerationRatio = AnimatedPageControl.accRatio;
        doubleAnimation.DecelerationRatio = AnimatedPageControl.decRatio;
        translateTransform.BeginAnimation(TranslateTransform.XProperty, (AnimationTimeline) doubleAnimation);
      }
      else
        page.RenderTransform = (Transform) null;
      page.Visibility = Visibility.Visible;
      page.pageControl = this;
      page.IsEnabled = true;
      page.OnShow();
      LinkedListNode<SyncAppPageControl> node = this.pageStack.Find(page);
      if (node != null)
        this.pageStack.Remove(node);
      if (!this.Children.Contains((UIElement) page))
        this.Children.Add((UIElement) page);
      this.pageStack.AddFirst(page);
    }

    public void ShowModalPage(SyncAppPageControl page, bool animate = true)
    {
      if (page == null)
        throw new ArgumentNullException(nameof (page));
      SyncAppPageControl syncAppPageControl = (SyncAppPageControl) null;
      if (this.modalPageStack.Count != 0)
      {
        syncAppPageControl = this.modalPageStack.First.Value;
        if (syncAppPageControl == page)
          return;
      }
      else if (this.pageStack.Count > 0)
        syncAppPageControl = this.pageStack.First.Value;
      if (syncAppPageControl != null)
      {
        if (animate)
        {
          TranslateTransform translateTransform = new TranslateTransform();
          syncAppPageControl.RenderTransform = (Transform) translateTransform;
          DoubleAnimation doubleAnimation = new DoubleAnimation();
          doubleAnimation.From = new double?(0.0);
          doubleAnimation.To = new double?(this.ActualWidth);
          doubleAnimation.Duration = (Duration) AnimatedPageControl.slideDuration;
          doubleAnimation.AccelerationRatio = AnimatedPageControl.accRatio;
          doubleAnimation.DecelerationRatio = AnimatedPageControl.decRatio;
          translateTransform.BeginAnimation(TranslateTransform.XProperty, (AnimationTimeline) doubleAnimation);
        }
        else
          syncAppPageControl.RenderTransform = (Transform) new TranslateTransform(-this.ActualWidth, 0.0);
        syncAppPageControl.IsEnabled = false;
      }
      if (animate)
      {
        TranslateTransform translateTransform = new TranslateTransform();
        page.RenderTransform = (Transform) translateTransform;
        DoubleAnimation doubleAnimation = new DoubleAnimation();
        doubleAnimation.From = new double?(-this.ActualWidth);
        doubleAnimation.To = new double?(0.0);
        doubleAnimation.Duration = (Duration) AnimatedPageControl.slideDuration;
        doubleAnimation.AccelerationRatio = AnimatedPageControl.accRatio;
        doubleAnimation.DecelerationRatio = AnimatedPageControl.decRatio;
        translateTransform.BeginAnimation(TranslateTransform.XProperty, (AnimationTimeline) doubleAnimation);
      }
      else
        page.RenderTransform = (Transform) new TranslateTransform();
      if (!this.Children.Contains((UIElement) page))
        this.Children.Add((UIElement) page);
      this.modalPageStack.AddFirst(page);
      page.pageControl = this;
      page.IsEnabled = true;
      page.OnShow();
    }

    public void HideModalPage(bool animate = true)
    {
      if (this.modalPageStack.Count == 0)
        return;
      SyncAppPageControl syncAppPageControl1 = this.modalPageStack.First.Value;
      SyncAppPageControl syncAppPageControl2 = (SyncAppPageControl) null;
      if (this.modalPageStack.Count >= 2)
        syncAppPageControl2 = this.modalPageStack.First.Next.Value;
      else if (this.pageStack.Count > 0)
        syncAppPageControl2 = this.pageStack.First.Value;
      if (syncAppPageControl2 != null)
      {
        if (animate)
        {
          TranslateTransform translateTransform = new TranslateTransform();
          syncAppPageControl2.RenderTransform = (Transform) translateTransform;
          DoubleAnimation doubleAnimation = new DoubleAnimation();
          doubleAnimation.From = new double?(this.ActualWidth);
          doubleAnimation.To = new double?(0.0);
          doubleAnimation.Duration = (Duration) AnimatedPageControl.slideDuration;
          doubleAnimation.AccelerationRatio = AnimatedPageControl.accRatio;
          doubleAnimation.DecelerationRatio = AnimatedPageControl.decRatio;
          translateTransform.BeginAnimation(TranslateTransform.XProperty, (AnimationTimeline) doubleAnimation);
        }
        else
          syncAppPageControl2.RenderTransform = (Transform) new TranslateTransform();
        syncAppPageControl2.IsEnabled = true;
      }
            if (animate)
            {
                TranslateTransform translateTransform = new TranslateTransform();
                syncAppPageControl1.RenderTransform = (Transform)translateTransform;
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.From = new double?(0.0);
                doubleAnimation.To = new double?(-this.ActualWidth);
                doubleAnimation.Duration = (Duration)AnimatedPageControl.slideDuration;
                doubleAnimation.AccelerationRatio = AnimatedPageControl.accRatio;
                doubleAnimation.DecelerationRatio = AnimatedPageControl.decRatio;
                translateTransform.BeginAnimation(TranslateTransform.XProperty, (AnimationTimeline)doubleAnimation);
            }
            else
            {
                syncAppPageControl1.RenderTransform = (Transform)new TranslateTransform(-this.ActualWidth, 0.0);
            }

      syncAppPageControl1.pageControl = (AnimatedPageControl) null;
      syncAppPageControl1.OnHide();
      syncAppPageControl1.IsEnabled = false;
      this.modalPageStack.RemoveFirst();
    }

    //[DebuggerNonUserCode]
    //[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    /*
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Microsoft Band Sync;component/basecontrols/animatedpagecontrol.xaml", UriKind.Relative));
    }
    */

    //[DebuggerNonUserCode]
    //[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    //[EditorBrowsable(EditorBrowsableState.Never)]
    /*
    void IComponentConnector.Connect(int connectionId, object target) => this._contentLoaded = true;
    */
  }
}
