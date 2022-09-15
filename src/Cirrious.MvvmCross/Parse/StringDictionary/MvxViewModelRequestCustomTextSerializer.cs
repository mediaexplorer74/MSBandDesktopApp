// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.Parse.StringDictionary.MvxViewModelRequestCustomTextSerializer
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Parse.StringDictionary
{
  public class MvxViewModelRequestCustomTextSerializer : IMvxTextSerializer
  {
    private IMvxViewModelByNameLookup _byNameLookup;

    protected IMvxViewModelByNameLookup ByNameLookup
    {
      get
      {
        this._byNameLookup = this._byNameLookup ?? Mvx.Resolve<IMvxViewModelByNameLookup>();
        return this._byNameLookup;
      }
    }

    public T DeserializeObject<T>(string inputText) => (T) this.DeserializeObject(typeof (T), inputText);

    public string SerializeObject(object toSerialise)
    {
      switch (toSerialise)
      {
        case MvxViewModelRequest _:
          return this.Serialize((MvxViewModelRequest) toSerialise);
        case IDictionary<string, string> _:
          return this.Serialize((IDictionary<string, string>) toSerialise);
        default:
          throw new MvxException("This serializer only knows about MvxViewModelRequest and IDictionary<string,string>");
      }
    }

    public object DeserializeObject(Type type, string inputText)
    {
      if ((object) type == (object) typeof (MvxViewModelRequest))
        return (object) this.DeserializeViewModelRequest(inputText);
      if (ReflectionExtensions.IsAssignableFrom(typeof (IDictionary<string, string>), type))
        return (object) this.DeserializeStringDictionary(inputText);
      throw new MvxException("This serializer only knows about MvxViewModelRequest and IDictionary<string,string>");
    }

    protected virtual IDictionary<string, string> DeserializeStringDictionary(
      string inputText)
    {
      return new MvxStringDictionaryParser().Parse(inputText);
    }

    protected virtual MvxViewModelRequest DeserializeViewModelRequest(
      string inputText)
    {
      MvxStringDictionaryParser dictionaryParser = new MvxStringDictionaryParser();
      IDictionary<string, string> dictionary = dictionaryParser.Parse(inputText);
      MvxViewModelRequest viewModelRequest = new MvxViewModelRequest();
      string viewModelTypeName = this.SafeGetValue(dictionary, "Type");
      viewModelRequest.ViewModelType = this.DeserializeViewModelType(viewModelTypeName);
      viewModelRequest.RequestedBy = new MvxRequestedBy()
      {
        Type = (MvxRequestedByType) int.Parse(this.SafeGetValue(dictionary, "By")),
        AdditionalInfo = this.SafeGetValue(dictionary, "Info")
      };
      viewModelRequest.ParameterValues = dictionaryParser.Parse(this.SafeGetValue(dictionary, "Params"));
      viewModelRequest.PresentationValues = dictionaryParser.Parse(this.SafeGetValue(dictionary, "Pres"));
      return viewModelRequest;
    }

    protected virtual string Serialize(IDictionary<string, string> toSerialise) => new MvxStringDictionaryWriter().Write(toSerialise);

    protected virtual string Serialize(MvxViewModelRequest toSerialise)
    {
      MvxStringDictionaryWriter dictionaryWriter = new MvxStringDictionaryWriter();
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      dictionary["Type"] = this.SerializeViewModelName(toSerialise.ViewModelType);
      MvxRequestedBy mvxRequestedBy = toSerialise.RequestedBy ?? new MvxRequestedBy();
      dictionary["By"] = ((int) mvxRequestedBy.Type).ToString();
      dictionary["Info"] = mvxRequestedBy.AdditionalInfo;
      dictionary["Params"] = dictionaryWriter.Write(toSerialise.ParameterValues);
      dictionary["Pres"] = dictionaryWriter.Write(toSerialise.PresentationValues);
      return dictionaryWriter.Write((IDictionary<string, string>) dictionary);
    }

    protected virtual string SerializeViewModelName(Type viewModelType) => viewModelType.FullName;

    protected virtual Type DeserializeViewModelType(string viewModelTypeName)
    {
      Type viewModelType;
      if (!this.ByNameLookup.TryLookupByFullName(viewModelTypeName, out viewModelType))
        throw new MvxException("Failed to find viewmodel for {0} - is the ViewModel in the same Assembly as App.cs? If not, you can add it by overriding GetViewModelAssemblies() in setup", new object[1]
        {
          (object) viewModelTypeName
        });
      return viewModelType;
    }

    private string SafeGetValue(IDictionary<string, string> dictionary, string key)
    {
      string str;
      if (!dictionary.TryGetValue(key, out str))
        throw new MvxException("Dictionary missing required keyvalue pair for key {0}", new object[1]
        {
          (object) key
        });
      return str;
    }
  }
}
