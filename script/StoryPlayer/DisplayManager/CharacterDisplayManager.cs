using System.Collections.Generic;
using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;

namespace LacieEngine.StoryPlayer
{
	public class CharacterDisplayManager
	{
		private static readonly Vector2 CharLeftVisiblePos = new Vector2(0f, 0f);

		private static readonly Vector2 CharRightVisiblePos = new Vector2(640f, 0f);

		private const float CharAnimationDisplacement = 50f;

		private StoryPlayer storyPlayer;

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

		public CharacterDisplayManager(StoryPlayer storyPlayer)
		{
			this.storyPlayer = storyPlayer;
			curCharLeftNode = this.storyPlayer.nCharLeftMain;
			altCharLeftNode = this.storyPlayer.nCharLeftAlt;
			curCharRightNode = this.storyPlayer.nCharRightMain;
			altCharRightNode = this.storyPlayer.nCharRightAlt;
		}

		public void Reset()
		{
			curCharLeft = null;
			curMoodLeft = null;
			curCharRight = null;
			curMoodRight = null;
			characterLocations = new Dictionary<string, string>();
			characterMoods = new Dictionary<string, string>();
			Locked = false;
		}

		public void ShowCharacter(string characterId, string mood = null)
		{
			if (characterId == null)
			{
				FocusNoChars();
				return;
			}
			if (!IsCharacterSet(characterId))
			{
				SetCharacter(characterId, characterLocations.IsEmpty() ? "left" : "right");
			}
			if (!characterMoods.ContainsKey(characterId))
			{
				characterMoods[characterId] = "default";
			}
			if (!mood.IsNullOrEmpty() && !Locked)
			{
				characterMoods[characterId] = mood;
			}
			if (mood == null)
			{
				mood = characterMoods[characterId];
			}
			if (characterLocations[characterId] == "left")
			{
				FocusCharLeft(characterId);
			}
			else if (characterLocations[characterId] == "right")
			{
				FocusCharRight(characterId);
			}
			if (Locked || !Game.Characters.Get(characterId).Moods.Contains(mood))
			{
				return;
			}
			if (characterLocations[characterId] == "left")
			{
				if (characterId != curCharLeft)
				{
					AnimateInLeft(Game.Characters.GetMoodTexture(characterId, mood));
				}
				else if (mood != curMoodLeft)
				{
					SetTextureToRect(curCharLeftNode, Game.Characters.GetMoodTexture(characterId, mood));
				}
				curCharLeft = characterId;
				curMoodLeft = mood;
			}
			else if (characterLocations[characterId] == "right")
			{
				if (characterId != curCharRight)
				{
					AnimateInRight(Game.Characters.GetMoodTexture(characterId, mood));
				}
				else if (mood != curMoodRight)
				{
					SetTextureToRect(curCharRightNode, Game.Characters.GetMoodTexture(characterId, mood));
				}
				curCharRight = characterId;
				curMoodRight = mood;
			}
		}

		public void HideCharacter(string characterId)
		{
			if (!Locked)
			{
				if (characterLocations[characterId] == "left")
				{
					AnimateOutLeft();
					curCharLeft = null;
					curMoodLeft = null;
				}
				else if (characterLocations[characterId] == "right")
				{
					AnimateOutRight();
					curCharRight = null;
					curMoodRight = null;
				}
				else
				{
					Log.Warn("Attempt to hide a character that isn't currently in display: ", characterId);
				}
			}
		}

		public void HideAllCharacters()
		{
			if (!Locked)
			{
				if (curCharLeft != null)
				{
					AnimateOutLeft();
				}
				if (curCharRight != null)
				{
					AnimateOutRight();
				}
				curCharLeft = null;
				curMoodLeft = null;
				curCharRight = null;
				curMoodRight = null;
			}
		}

		public void SetCharacter(string who, string placement)
		{
			if (placement.IsNullOrEmpty())
			{
				placement = "left";
			}
			characterLocations[who] = placement;
		}

		public bool IsCharacterSet(string characterId)
		{
			return characterLocations.ContainsKey(characterId);
		}

		private void AnimateInLeft(Texture newTexture)
		{
			Game.Animations.StopAnimations<FadeAnimation>(curCharLeftNode);
			Game.Animations.StopAnimations<FadeAnimation>(altCharLeftNode);
			if (curCharLeft == null)
			{
				SetTextureToRect(curCharLeftNode, newTexture);
				Game.Animations.Play(new SlideInLeftAnimation(curCharLeftNode, 50f, CharLeftVisiblePos));
				return;
			}
			SetTextureToRect(altCharLeftNode, newTexture);
			Game.Animations.Play(new SlideOutLeftAnimation(curCharLeftNode, 50f, CharLeftVisiblePos));
			Game.Animations.Play(new SlideInLeftAnimation(altCharLeftNode, 50f, CharLeftVisiblePos));
			TextureRect textureRect = curCharLeftNode;
			TextureRect textureRect2 = altCharLeftNode;
			altCharLeftNode = textureRect;
			curCharLeftNode = textureRect2;
		}

		private void AnimateInRight(Texture newTexture)
		{
			Game.Animations.StopAnimations<FadeAnimation>(curCharRightNode);
			Game.Animations.StopAnimations<FadeAnimation>(altCharRightNode);
			if (curCharRight == null)
			{
				SetTextureToRect(curCharRightNode, newTexture);
				Game.Animations.Play(new SlideInRightAnimation(curCharRightNode, 50f, CharRightVisiblePos));
				return;
			}
			SetTextureToRect(altCharRightNode, newTexture);
			Game.Animations.Play(new SlideOutRightAnimation(curCharRightNode, 50f, CharRightVisiblePos));
			Game.Animations.Play(new SlideInRightAnimation(altCharRightNode, 50f, CharRightVisiblePos));
			TextureRect textureRect = curCharRightNode;
			TextureRect textureRect2 = altCharRightNode;
			altCharRightNode = textureRect;
			curCharRightNode = textureRect2;
		}

		private void AnimateOutLeft()
		{
			Game.Animations.StopAnimations<FadeAnimation>(curCharLeftNode);
			Game.Animations.Play(new SlideOutLeftAnimation(curCharLeftNode, 50f, CharLeftVisiblePos));
		}

		private void AnimateOutRight()
		{
			Game.Animations.StopAnimations<FadeAnimation>(curCharRightNode);
			Game.Animations.Play(new SlideOutRightAnimation(curCharRightNode, 50f, CharRightVisiblePos));
		}

		private void FocusNoChars()
		{
			storyPlayer.nDialogueFrame.NamePlate.Visible = false;
			storyPlayer.nDialogueFrame.SpeakerName.Text = null;
			HideAllCharacters();
		}

		private void FocusCharLeft(string characterId)
		{
			storyPlayer.nDialogueFrame.NamePlate.Visible = true;
			storyPlayer.nDialogueFrame.SpeakerName.Text = Game.Characters.Get(characterId).Name;
			storyPlayer.nCharLeft.Modulate = Colors.White;
			if (curCharRight != null)
			{
				HideCharacter(curCharRight);
			}
		}

		private void FocusCharRight(string characterId)
		{
			storyPlayer.nDialogueFrame.NamePlate.Visible = true;
			storyPlayer.nDialogueFrame.SpeakerName.Text = Game.Characters.Get(characterId).Name;
			storyPlayer.nCharRight.Modulate = Colors.White;
			if (curCharLeft != null)
			{
				HideCharacter(curCharLeft);
			}
		}

		private void SetTextureToRect(TextureRect rect, Texture texture)
		{
			if (texture != null)
			{
				rect.Texture = texture;
				rect.RectMinSize = texture.GetSize() / 3f;
				rect.SetDeferred("rect_size", texture.GetSize() / 3f);
				rect.SetAnchorsPreset(Control.LayoutPreset.BottomRight);
			}
			else
			{
				rect.Texture = null;
			}
		}
	}
}
