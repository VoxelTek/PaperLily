// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1PncPhone
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Minigames;
using LacieEngine.UI;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  public class Ch1PncPhone : PointAndClick
  {
    [Export(PropertyHint.None, "")]
    public Texture TexDigits;
    [Export(PropertyHint.None, "")]
    public AudioStream[] SfxDialTones;
    public Dictionary<string, string> CorrectCombinations;
    [LacieEngine.API.GetNode("Display")]
    protected Control nDigitContainer;
    private const int MaxDigits = 8;
    private string _curCombination = "";

    public override void _Ready()
    {
      base._Ready();
      if (!this.CorrectCombinations.IsNullOrEmpty<KeyValuePair<string, string>>())
        return;
      Log.Error((object) "Phone combinations not initialized!");
      this.CorrectCombinations = new Dictionary<string, string>();
    }

    public void Dial0() => this.DialNumber("0");

    public void Dial1() => this.DialNumber("1");

    public void Dial2() => this.DialNumber("2");

    public void Dial3() => this.DialNumber("3");

    public void Dial4() => this.DialNumber("4");

    public void Dial5() => this.DialNumber("5");

    public void Dial6() => this.DialNumber("6");

    public void Dial7() => this.DialNumber("7");

    public void Dial8() => this.DialNumber("8");

    public void Dial9() => this.DialNumber("9");

    public void DialAsterisk() => this.DialNumber("*");

    public void DialPound() => this.DialNumber("#");

    private void DialNumber(string digit)
    {
      this.PlayDialTone(digit);
      this._curCombination += digit;
      this.nDigitContainer.AddChild((Node) this.MakeDigitTexture(digit));
      if (this.CorrectCombinations.ContainsKey(this._curCombination))
      {
        this.EndMinigame(this.CorrectCombinations[this._curCombination]);
      }
      else
      {
        if (this._curCombination.Length < 8)
          return;
        this.EndMinigame("event_dial_wrong_number");
      }
    }

    private async void EndMinigame(string @event)
    {
      Ch1PncPhone ch1PncPhone = this;
      Game.InputProcessor = Inputs.Processor.None;
      ch1PncPhone.nCursor.Visible = false;
      await DrkieUtil.DelaySeconds(1.0);
      Game.Minigames.End(@event);
    }

    private TextureRect MakeDigitTexture(string digit)
    {
      SlicedTextureRect slicedTextureRect1 = new SlicedTextureRect();
      slicedTextureRect1.Texture = this.TexDigits;
      slicedTextureRect1.Hframes = 3;
      slicedTextureRect1.Vframes = 4;
      SlicedTextureRect slicedTextureRect2 = slicedTextureRect1;
      int num;
      if (digit != null && digit.Length == 1)
      {
        switch (digit[0])
        {
          case '#':
            num = 11;
            goto label_15;
          case '*':
            num = 9;
            goto label_15;
          case '0':
            num = 10;
            goto label_15;
          case '1':
            num = 0;
            goto label_15;
          case '2':
            num = 1;
            goto label_15;
          case '3':
            num = 2;
            goto label_15;
          case '4':
            num = 3;
            goto label_15;
          case '5':
            num = 4;
            goto label_15;
          case '6':
            num = 5;
            goto label_15;
          case '7':
            num = 6;
            goto label_15;
          case '8':
            num = 7;
            goto label_15;
          case '9':
            num = 8;
            goto label_15;
        }
      }
      num = 0;
label_15:
      slicedTextureRect2.Frame = num;
      return (TextureRect) slicedTextureRect1;
    }

    private void PlayDialTone(string digit)
    {
      AudioStream sfxDialTone;
      if (digit != null && digit.Length == 1)
      {
        switch (digit[0])
        {
          case '#':
            sfxDialTone = this.SfxDialTones[11];
            goto label_15;
          case '*':
            sfxDialTone = this.SfxDialTones[10];
            goto label_15;
          case '0':
            sfxDialTone = this.SfxDialTones[0];
            goto label_15;
          case '1':
            sfxDialTone = this.SfxDialTones[1];
            goto label_15;
          case '2':
            sfxDialTone = this.SfxDialTones[2];
            goto label_15;
          case '3':
            sfxDialTone = this.SfxDialTones[3];
            goto label_15;
          case '4':
            sfxDialTone = this.SfxDialTones[4];
            goto label_15;
          case '5':
            sfxDialTone = this.SfxDialTones[5];
            goto label_15;
          case '6':
            sfxDialTone = this.SfxDialTones[6];
            goto label_15;
          case '7':
            sfxDialTone = this.SfxDialTones[7];
            goto label_15;
          case '8':
            sfxDialTone = this.SfxDialTones[8];
            goto label_15;
          case '9':
            sfxDialTone = this.SfxDialTones[9];
            goto label_15;
        }
      }
      sfxDialTone = this.SfxDialTones[0];
label_15:
      AudioStream track = sfxDialTone;
      Game.Audio.StopSfx();
      Game.Audio.PlaySfx(track);
    }
  }
}
