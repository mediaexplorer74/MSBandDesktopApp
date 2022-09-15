// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxPostfixAwareViewToViewModelNameMapping
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

namespace Cirrious.MvvmCross.ViewModels
{
  public class MvxPostfixAwareViewToViewModelNameMapping : MvxViewToViewModelNameMapping
  {
    private readonly string[] _postfixes;

    public MvxPostfixAwareViewToViewModelNameMapping(params string[] postfixes) => this._postfixes = postfixes;

    public override string Map(string inputName)
    {
      foreach (string postfix in this._postfixes)
      {
        if (inputName.EndsWith(postfix) && inputName.Length > postfix.Length)
        {
          inputName = inputName.Substring(0, inputName.Length - postfix.Length);
          break;
        }
      }
      return base.Map(inputName);
    }
  }
}
