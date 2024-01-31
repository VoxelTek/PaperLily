// Decompiled with JetBrains decompiler
// Type: LacieEngine.Settings.Setting`1
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Settings
{
  public abstract class Setting<T> : SettingParent
  {
    public string Name { get; protected set; }

    public string Path { get; protected set; }

    public T Value { get; set; }

    public bool OwnSound { get; protected set; }

    public virtual T ReadValue() => GDUtil.ReadSetting<T>(this.Path);

    public override void WriteValue(ConfigFile configFile)
    {
      if (this.ReadValue().Equals((object) this.Value))
        return;
      ProjectSettings.SetSetting(this.Path, (object) this.Value);
      string section = this.Path.Substring(0, this.Path.IndexOf("/"));
      string path = this.Path;
      int startIndex = this.Path.IndexOf("/") + 1;
      string key = path.Substring(startIndex, path.Length - startIndex);
      configFile.SetValue(section, key, (object) this.Value);
    }

    public virtual string CaptionLabel() => this.Name;

    public virtual string ValueLabel() => this.Value.ToString();

    public abstract void Increment();

    public abstract void Decrement();
  }
}
