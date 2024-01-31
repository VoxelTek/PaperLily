// Decompiled with JetBrains decompiler
// Type: LacieEngine.Minigames.HpMeter
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.Minigames
{
  public class HpMeter : Control
  {
    [Export(PropertyHint.None, "")]
    public Texture texUi;
    [Export(PropertyHint.None, "")]
    public Texture texHpFull;
    [Export(PropertyHint.None, "")]
    public Texture texHpEmpty;
    private TextureRect[] _hpTextures;
    private int _maxHp;

    public string CharacterName { get; set; }

    public override void _EnterTree()
    {
      if (this.CharacterName == null)
      {
        string str;
        this.CharacterName = str = Game.State.Party[0];
      }
      this._maxHp = Math.Max(Game.Variables.GetIntVariable("general.maxhp." + this.CharacterName), 1);
      TextureRect node = GDUtil.MakeNode<TextureRect>("HpUi");
      node.Texture = this.texUi;
      node.RectPosition = new Vector2(8f, 8f);
      this.AddChild((Node) node);
      HBoxContainer box = GDUtil.MakeNode<HBoxContainer>("HpContainer");
      box.RectPosition = new Vector2(42f, 12f);
      box.SetSeparation(-8);
      this._hpTextures = new TextureRect[this._maxHp];
      for (int index = 0; index < this._maxHp; ++index)
      {
        this._hpTextures[index] = GDUtil.MakeNode<TextureRect>("Hp" + index.ToString());
        this._hpTextures[index].Texture = this.texHpFull;
        box.AddChild((Node) this._hpTextures[index]);
      }
      this.UpdateHp();
      node.AddChild((Node) box);
      Game.Animations.Play((LacieAnimation) new SlideInTopAnimation((Control) node));
    }

    public void UpdateHp()
    {
      int intVariable = Game.Variables.GetIntVariable("general.hp." + this.CharacterName);
      for (int index = 0; index < this._maxHp; ++index)
        this._hpTextures[index].Texture = intVariable > index ? this.texHpFull : this.texHpEmpty;
    }
  }
}
