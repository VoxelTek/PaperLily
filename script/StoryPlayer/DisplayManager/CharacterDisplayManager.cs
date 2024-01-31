// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.CharacterDisplayManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.StoryPlayer
{
  public class CharacterDisplayManager
  {
    private static readonly Vector2 CharLeftVisiblePos = new Vector2(0.0f, 0.0f);
    private static readonly Vector2 CharRightVisiblePos = new Vector2(640f, 0.0f);
    private const float CharAnimationDisplacement = 50f;
    private LacieEngine.StoryPlayer.StoryPlayer storyPlayer;
    private Dictionary<string, string> characterLocations;
    private Dictionary<string, string> characterMoods;
    private TextureRect curCharLeftNode;
    private TextureRect altCharLeftNode;
    private TextureRect curCharRightNode;
    private TextureRect altCharRightNode;
    private string curCharLeft;
    private string curCharRight;
    private string curMoodLeft;
    private string curMoodRight;

    public bool Locked { get; set; }

    public CharacterDisplayManager(LacieEngine.StoryPlayer.StoryPlayer storyPlayer)
    {
      this.storyPlayer = storyPlayer;
      this.curCharLeftNode = this.storyPlayer.nCharLeftMain;
      this.altCharLeftNode = this.storyPlayer.nCharLeftAlt;
      this.curCharRightNode = this.storyPlayer.nCharRightMain;
      this.altCharRightNode = this.storyPlayer.nCharRightAlt;
    }

    public void Reset()
    {
      this.curCharLeft = (string) null;
      this.curMoodLeft = (string) null;
      this.curCharRight = (string) null;
      this.curMoodRight = (string) null;
      this.characterLocations = new Dictionary<string, string>();
      this.characterMoods = new Dictionary<string, string>();
      this.Locked = false;
    }

    public void ShowCharacter(string characterId, string mood = null)
    {
      if (characterId == null)
      {
        this.FocusNoChars();
      }
      else
      {
        if (!this.IsCharacterSet(characterId))
          this.SetCharacter(characterId, this.characterLocations.IsEmpty<KeyValuePair<string, string>>() ? "left" : "right");
        if (!this.characterMoods.ContainsKey(characterId))
          this.characterMoods[characterId] = "default";
        if (!mood.IsNullOrEmpty() && !this.Locked)
          this.characterMoods[characterId] = mood;
        if (mood == null)
          mood = this.characterMoods[characterId];
        if (this.characterLocations[characterId] == "left")
          this.FocusCharLeft(characterId);
        else if (this.characterLocations[characterId] == "right")
          this.FocusCharRight(characterId);
        if (this.Locked || !Game.Characters.Get(characterId).Moods.Contains(mood))
          return;
        if (this.characterLocations[characterId] == "left")
        {
          if (characterId != this.curCharLeft)
            this.AnimateInLeft(Game.Characters.GetMoodTexture(characterId, mood));
          else if (mood != this.curMoodLeft)
            this.SetTextureToRect(this.curCharLeftNode, Game.Characters.GetMoodTexture(characterId, mood));
          this.curCharLeft = characterId;
          this.curMoodLeft = mood;
        }
        else
        {
          if (!(this.characterLocations[characterId] == "right"))
            return;
          if (characterId != this.curCharRight)
            this.AnimateInRight(Game.Characters.GetMoodTexture(characterId, mood));
          else if (mood != this.curMoodRight)
            this.SetTextureToRect(this.curCharRightNode, Game.Characters.GetMoodTexture(characterId, mood));
          this.curCharRight = characterId;
          this.curMoodRight = mood;
        }
      }
    }

    public void HideCharacter(string characterId)
    {
      if (this.Locked)
        return;
      if (this.characterLocations[characterId] == "left")
      {
        this.AnimateOutLeft();
        this.curCharLeft = (string) null;
        this.curMoodLeft = (string) null;
      }
      else if (this.characterLocations[characterId] == "right")
      {
        this.AnimateOutRight();
        this.curCharRight = (string) null;
        this.curMoodRight = (string) null;
      }
      else
        Log.Warn((object) "Attempt to hide a character that isn't currently in display: ", (object) characterId);
    }

    public void HideAllCharacters()
    {
      if (this.Locked)
        return;
      if (this.curCharLeft != null)
        this.AnimateOutLeft();
      if (this.curCharRight != null)
        this.AnimateOutRight();
      this.curCharLeft = (string) null;
      this.curMoodLeft = (string) null;
      this.curCharRight = (string) null;
      this.curMoodRight = (string) null;
    }

    public void SetCharacter(string who, string placement)
    {
      if (placement.IsNullOrEmpty())
        placement = "left";
      this.characterLocations[who] = placement;
    }

    public bool IsCharacterSet(string characterId)
    {
      return this.characterLocations.ContainsKey(characterId);
    }

    private void AnimateInLeft(Texture newTexture)
    {
      Game.Animations.StopAnimations<FadeAnimation>((Node) this.curCharLeftNode);
      Game.Animations.StopAnimations<FadeAnimation>((Node) this.altCharLeftNode);
      if (this.curCharLeft == null)
      {
        this.SetTextureToRect(this.curCharLeftNode, newTexture);
        Game.Animations.Play((LacieAnimation) new SlideInLeftAnimation((Control) this.curCharLeftNode, new float?(50f), new Vector2?(CharacterDisplayManager.CharLeftVisiblePos)));
      }
      else
      {
        this.SetTextureToRect(this.altCharLeftNode, newTexture);
        Game.Animations.Play((LacieAnimation) new SlideOutLeftAnimation((Control) this.curCharLeftNode, new float?(50f), new Vector2?(CharacterDisplayManager.CharLeftVisiblePos)));
        Game.Animations.Play((LacieAnimation) new SlideInLeftAnimation((Control) this.altCharLeftNode, new float?(50f), new Vector2?(CharacterDisplayManager.CharLeftVisiblePos)));
        TextureRect curCharLeftNode = this.curCharLeftNode;
        TextureRect altCharLeftNode = this.altCharLeftNode;
        this.altCharLeftNode = curCharLeftNode;
        this.curCharLeftNode = altCharLeftNode;
      }
    }

    private void AnimateInRight(Texture newTexture)
    {
      Game.Animations.StopAnimations<FadeAnimation>((Node) this.curCharRightNode);
      Game.Animations.StopAnimations<FadeAnimation>((Node) this.altCharRightNode);
      if (this.curCharRight == null)
      {
        this.SetTextureToRect(this.curCharRightNode, newTexture);
        Game.Animations.Play((LacieAnimation) new SlideInRightAnimation((Control) this.curCharRightNode, new float?(50f), new Vector2?(CharacterDisplayManager.CharRightVisiblePos)));
      }
      else
      {
        this.SetTextureToRect(this.altCharRightNode, newTexture);
        Game.Animations.Play((LacieAnimation) new SlideOutRightAnimation((Control) this.curCharRightNode, new float?(50f), new Vector2?(CharacterDisplayManager.CharRightVisiblePos)));
        Game.Animations.Play((LacieAnimation) new SlideInRightAnimation((Control) this.altCharRightNode, new float?(50f), new Vector2?(CharacterDisplayManager.CharRightVisiblePos)));
        TextureRect curCharRightNode = this.curCharRightNode;
        TextureRect altCharRightNode = this.altCharRightNode;
        this.altCharRightNode = curCharRightNode;
        this.curCharRightNode = altCharRightNode;
      }
    }

    private void AnimateOutLeft()
    {
      Game.Animations.StopAnimations<FadeAnimation>((Node) this.curCharLeftNode);
      Game.Animations.Play((LacieAnimation) new SlideOutLeftAnimation((Control) this.curCharLeftNode, new float?(50f), new Vector2?(CharacterDisplayManager.CharLeftVisiblePos)));
    }

    private void AnimateOutRight()
    {
      Game.Animations.StopAnimations<FadeAnimation>((Node) this.curCharRightNode);
      Game.Animations.Play((LacieAnimation) new SlideOutRightAnimation((Control) this.curCharRightNode, new float?(50f), new Vector2?(CharacterDisplayManager.CharRightVisiblePos)));
    }

    private void FocusNoChars()
    {
      this.storyPlayer.nDialogueFrame.NamePlate.Visible = false;
      this.storyPlayer.nDialogueFrame.SpeakerName.Text = (string) null;
      this.HideAllCharacters();
    }

    private void FocusCharLeft(string characterId)
    {
      this.storyPlayer.nDialogueFrame.NamePlate.Visible = true;
      this.storyPlayer.nDialogueFrame.SpeakerName.Text = Game.Characters.Get(characterId).Name;
      this.storyPlayer.nCharLeft.Modulate = Godot.Colors.White;
      if (this.curCharRight == null)
        return;
      this.HideCharacter(this.curCharRight);
    }

    private void FocusCharRight(string characterId)
    {
      this.storyPlayer.nDialogueFrame.NamePlate.Visible = true;
      this.storyPlayer.nDialogueFrame.SpeakerName.Text = Game.Characters.Get(characterId).Name;
      this.storyPlayer.nCharRight.Modulate = Godot.Colors.White;
      if (this.curCharLeft == null)
        return;
      this.HideCharacter(this.curCharLeft);
    }

    private void SetTextureToRect(TextureRect rect, Texture texture)
    {
      if (texture != null)
      {
        rect.Texture = texture;
        rect.RectMinSize = texture.GetSize() / 3f;
        rect.SetDeferred("rect_size", (object) (texture.GetSize() / 3f));
        rect.SetAnchorsPreset(Control.LayoutPreset.BottomRight);
      }
      else
        rect.Texture = (Texture) null;
    }
  }
}
