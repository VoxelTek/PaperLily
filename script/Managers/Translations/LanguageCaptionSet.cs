// Decompiled with JetBrains decompiler
// Type: LacieEngine.Translations.LanguageCaptionSet
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.API;
using LacieEngine.Core;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Translations
{
  public class LanguageCaptionSet : ICaptionSet
  {
    private Dictionary<string, LanguageCaptionSet.LanguageCaptionEntry> _entries = new Dictionary<string, LanguageCaptionSet.LanguageCaptionEntry>();

    public bool ContainsKey(string id) => this._entries.ContainsKey(id);

    public string Get(string id)
    {
      return id.IsNullOrEmpty() || !this._entries.ContainsKey(id) ? id : this._entries[id].MessageStr;
    }

    public string Get(string id, string context)
    {
      if (context.IsNullOrEmpty())
        return this.Get(id);
      if (id.IsNullOrEmpty() || !this._entries.ContainsKey(id))
        return id;
      return !this._entries[id].HasContext(context) ? this._entries[id].MessageStr : this._entries[id].Contexts[context];
    }

    public void AddCaption(LanguageCaption caption)
    {
      if (!this._entries.ContainsKey(caption.Id))
        this._entries[caption.Id] = new LanguageCaptionSet.LanguageCaptionEntry(caption.Str);
      else if (caption.Context.IsNullOrEmpty())
        this._entries[caption.Id].UpdateMessage(caption.Str);
      if (caption.Context.IsNullOrEmpty())
        return;
      this._entries[caption.Id].AddContext(caption.Context, caption.Str);
    }

    private class LanguageCaptionEntry
    {
      public string MessageStr { get; private set; }

      public Dictionary<string, string> Contexts { get; private set; }

      public LanguageCaptionEntry(string str) => this.MessageStr = str;

      public void AddContext(string context, string str)
      {
        if (this.Contexts == null)
        {
          Dictionary<string, string> dictionary;
          this.Contexts = dictionary = new Dictionary<string, string>();
        }
        this.Contexts[context] = str;
      }

      public void UpdateMessage(string message) => this.MessageStr = message;

      public bool HasContext(string context)
      {
        return this.Contexts != null && this.Contexts.ContainsKey(context);
      }
    }
  }
}
