// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.External.OperationContextData
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
  internal sealed class OperationContextData
  {
    private readonly IDictionary<string, string> tags;

    internal OperationContextData(IDictionary<string, string> tags) => this.tags = tags;

    public string Id
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.OperationId);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.OperationId, value);
    }

    public string Name
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.OperationName);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.OperationName, value);
    }

    public string ParentId
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.OperationParentId);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.OperationParentId, value);
    }

    public string RootId
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.OperationRootId);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.OperationRootId, value);
    }

    public string SyntheticSource
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.OperationSyntheticSource);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.OperationSyntheticSource, value);
    }

    public bool? IsSynthetic
    {
      get => this.tags.GetTagBoolValueOrNull(ContextTagKeys.Keys.OperationIsSynthetic);
      set => this.tags.SetTagValueOrRemove<bool?>(ContextTagKeys.Keys.OperationIsSynthetic, value);
    }

    internal void SetDefaults(OperationContextData source)
    {
      this.tags.InitializeTagValue<string>(ContextTagKeys.Keys.OperationId, source.Id);
      this.tags.InitializeTagValue<string>(ContextTagKeys.Keys.OperationName, source.Name);
      this.tags.InitializeTagValue<string>(ContextTagKeys.Keys.OperationParentId, source.ParentId);
      this.tags.InitializeTagValue<string>(ContextTagKeys.Keys.OperationRootId, source.RootId);
      this.tags.InitializeTagValue<string>(ContextTagKeys.Keys.OperationSyntheticSource, source.SyntheticSource);
      this.tags.InitializeTagValue<bool?>(ContextTagKeys.Keys.OperationIsSynthetic, source.IsSynthetic);
    }
  }
}
