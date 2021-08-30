// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.RecoveryStack
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Utility;
using System.Collections.Generic;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class RecoveryStack : IRecoveryStack
  {
    private Stack<IRequiresRecovery> recoveries = new Stack<IRequiresRecovery>();
    private object lockObj = new object();

    public void Add(IRequiresRecovery recovery)
    {
      Guard.ArgumentNotNull((object) recovery, nameof (recovery));
      lock (this.lockObj)
        this.recoveries.Push(recovery);
    }

    public int Count
    {
      get
      {
        lock (this.lockObj)
          return this.recoveries.Count;
      }
    }

    public void ExecuteRecovery()
    {
      while (this.recoveries.Count > 0)
        this.recoveries.Pop().Recover();
    }
  }
}
