// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.UnityContainerExtension
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using System;

namespace Microsoft.Practices.Unity
{
  public abstract class UnityContainerExtension : IUnityContainerExtensionConfigurator
  {
    private IUnityContainer container;
    private ExtensionContext context;

    public void InitializeExtension(ExtensionContext context)
    {
      this.container = context != null ? context.Container : throw new ArgumentNullException(nameof (context));
      this.context = context;
      this.Initialize();
    }

    public IUnityContainer Container => this.container;

    protected ExtensionContext Context => this.context;

    protected abstract void Initialize();

    public virtual void Remove()
    {
    }
  }
}
