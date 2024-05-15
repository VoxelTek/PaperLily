// Decompiled with JetBrains decompiler
// Type: LacieEngine.Translations.TranslatableMessages
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace LacieEngine.Translations
{
  public class TranslatableMessages : IEnumerable
  {
    private List<LanguageCaption> _entries = new List<LanguageCaption>();

    public string Name { get; set; }

    public int Count => this._entries.Count;

    public bool IsEmpty() => this._entries.Count == 0;

    public void Add(string msgid) => this.Add(new LanguageCaption(msgid));

    public void Add(string msgid, string msgctxt)
    {
      this.Add(new LanguageCaption(msgid, string.Empty, msgctxt));
    }

    public void Add(LanguageCaption caption) => this._entries.Add(caption);

    public string Get(string msgid, string msgctxt = null)
    {
      if (!msgctxt.IsNullOrEmpty())
      {
        foreach (LanguageCaption entry in this._entries)
        {
          if (msgid == entry.Id && msgctxt == entry.Context)
            return entry.Str;
        }
      }
      foreach (LanguageCaption entry in this._entries)
      {
        if (msgid == entry.Id)
          return entry.Str;
      }
      return (string) null;
    }

    public TranslatableMessages()
    {
    }

    public TranslatableMessages(string name) => this.Name = name;

    public void Clean()
    {
      for (int index = 0; index < this._entries.Count; ++index)
      {
        if (!MsgIdAlreadyExists(this._entries[index].Id, index) && !this._entries[index].Context.IsNullOrEmpty())
          this._entries[index] = new LanguageCaption(this._entries[index].Id, this._entries[index].Str);
      }
      for (int index = 0; index < this._entries.Count; ++index)
      {
        if (EntryDuplicateExists(this._entries[index], index))
          Log.Warn((object) "Duplicate entry: ", (object) this._entries[index].Id, (object) " in ", (object) this.Name);
      }
      int count = this._entries.Count;
      this._entries = this._entries.Distinct<LanguageCaption>().ToList<LanguageCaption>();
      if (count <= this._entries.Count)
        return;
      Log.Warn((object) "Duplicate captions were consumed: ", (object) (count - this._entries.Count), (object) " in ", (object) this.Name);

      bool MsgIdAlreadyExists(string msgid, int msgidx)
      {
        for (int index = 0; index < this._entries.Count; ++index)
        {
          if (index != msgidx && this._entries[index].Id == msgid)
            return true;
        }
        return false;
      }

      bool EntryDuplicateExists(LanguageCaption entry, int msgidx)
      {
        for (int index = 0; index < this._entries.Count; ++index)
        {
          if (index != msgidx && this._entries[index].Equals((object) entry))
            return true;
        }
        return false;
      }
    }

    public void RemoveContext()
    {
      for (int index = 0; index < this._entries.Count; ++index)
      {
        if (!this._entries[index].Context.IsNullOrEmpty())
          this._entries[index] = new LanguageCaption(this._entries[index].Id, this._entries[index].Str);
      }
      this._entries = this._entries.Distinct<LanguageCaption>().ToList<LanguageCaption>();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._entries.GetEnumerator();
  }
}
