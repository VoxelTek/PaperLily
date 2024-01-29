using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;
using LacieEngine.UI;

namespace LacieEngine.StoryPlayer
{
	public class CgDisplayManager
	{
		public enum CgLayer
		{
			CG,
			BG,
			FG
		}

		private const float DefaultFadeDuration = 0.25f;

		private const float DefaultPanDuration = 2f;

		private readonly Color ColorNotVisible = UIUtil.Transparent;

		private readonly Color ColorVisible = Colors.White;

		private StoryPlayer storyPlayer;

		private Control mainCgNode;

		private Control altCgNode;

		private CgLayer curCgLayer;

		private Control curCg;

		public CgDisplayManager(StoryPlayer storyPlayer)
		{
			this.storyPlayer = storyPlayer;
			mainCgNode = storyPlayer.nCg;
			altCgNode = storyPlayer.nCgAlt;
			curCgLayer = CgLayer.CG;
			curCg = null;
		}

		public void Reset()
		{
			mainCgNode.Clear();
			altCgNode.Clear();
			mainCgNode.Modulate = ColorNotVisible;
			altCgNode.Modulate = ColorNotVisible;
		}

		public void Execute(StoryPlayerCgCommand block)
		{
			if (block.Operation == StoryPlayerCgCommand.CGOperation.Show)
			{
				ShowCG(block.Image, block.Layer, block.Position, block.Time ?? 0.25f, block.Scene, block.Mini);
			}
			else if (block.Operation == StoryPlayerCgCommand.CGOperation.Hide)
			{
				StopInOutAnimations();
				Game.Screen.UndarkenScreen();
				Color color2 = (mainCgNode.Modulate = (altCgNode.Modulate = ColorNotVisible));
				StoryPlayerCgCommand.TransitionType transition = ((block.Time == 0f) ? StoryPlayerCgCommand.TransitionType.Instant : StoryPlayerCgCommand.TransitionType.Fade);
				HideCG(curCg, transition, block.Time ?? 0.25f);
				curCg = null;
			}
			else
			{
				if (block.Operation != StoryPlayerCgCommand.CGOperation.Pan)
				{
					return;
				}
				StopPanningAnimations();
				if (curCg.IsEmpty() || !(curCg.GetChild(0) is TextureRect))
				{
					Log.Error("Attempt to PAN something that isn't a CG or no CG present");
					return;
				}
				TextureRect img = curCg.GetChild<TextureRect>(0);
				float height = (float)img.Texture.GetHeight() * Game.Settings.BaseResolution.x / (float)img.Texture.GetWidth();
				float startX = 0f;
				float startY = 0f;
				float endX = 0f;
				float endY = 0f;
				if (block.Direction == Direction.Up)
				{
					startY = 0f - height + Game.Settings.BaseResolution.y;
				}
				else if (block.Direction == Direction.Down)
				{
					endY = 0f - height + Game.Settings.BaseResolution.y;
				}
				Game.Animations.Play(new PanAnimationControl(img, new Vector2(startX, startY), new Vector2(endX, endY), block.Time ?? 2f));
			}
		}

		private void ShowCG(string image, CgLayer layer, StoryPlayerCgCommand.CGPosition position, float duration, bool scene, bool mini)
		{
			StopInOutAnimations();
			bool num = curCg != null;
			Control newCgNode = (num ? altCgNode : mainCgNode);
			if (num && (curCgLayer == layer || layer == CgLayer.BG))
			{
				newCgNode.Modulate = ColorVisible;
			}
			else
			{
				newCgNode.Modulate = ColorNotVisible;
			}
			if (num && curCgLayer == layer)
			{
				MoveUnderCurrentCgNode(newCgNode);
			}
			else
			{
				switch (layer)
				{
				case CgLayer.CG:
					MoveToCgPosition(newCgNode);
					break;
				case CgLayer.BG:
					MoveToBgPosition(newCgNode);
					break;
				case CgLayer.FG:
					MoveToFgPosition(newCgNode);
					break;
				}
			}
			if (mini)
			{
				Game.Screen.DarkenScreen();
			}
			newCgNode.Clear();
			if (scene)
			{
				Node scn = GDUtil.Instance<Node>("res://resources/nodes/cg/" + image + ".tscn");
				newCgNode.AddChild(scn);
			}
			else
			{
				TextureRect img = GDUtil.MakeNode<TextureRect>("Image");
				img.Expand = true;
				img.StretchMode = TextureRect.StretchModeEnum.KeepAspectCovered;
				img.Texture = GD.Load<Texture>("res://assets/img/cg/" + image + ".png");
				newCgNode.AddChild(img);
				switch (position)
				{
				case StoryPlayerCgCommand.CGPosition.Fill:
					img.RectSize = Game.Settings.BaseResolution;
					img.RectPosition = new Vector2(0f, 0f);
					break;
				case StoryPlayerCgCommand.CGPosition.Top:
				case StoryPlayerCgCommand.CGPosition.Bottom:
				{
					float height = (float)img.Texture.GetHeight() * Game.Settings.BaseResolution.x / (float)img.Texture.GetWidth();
					img.RectSize = new Vector2(Game.Settings.BaseResolution.x, height);
					float vPos = ((position == StoryPlayerCgCommand.CGPosition.Top) ? 0f : (0f - height + Game.Settings.BaseResolution.y));
					img.RectPosition = new Vector2(0f, vPos);
					break;
				}
				case StoryPlayerCgCommand.CGPosition.AboveText:
				{
					float scale = 0.8f;
					img.RectSize = new Vector2(Game.Settings.BaseResolution.x * scale, Game.Settings.BaseResolution.y * scale);
					img.RectPosition = new Vector2((1f - scale) * (Game.Settings.BaseResolution.x / 2f), 0f);
					break;
				}
				}
			}
			StoryPlayerCgCommand.TransitionType transition = ((!(duration > 0f)) ? StoryPlayerCgCommand.TransitionType.Instant : StoryPlayerCgCommand.TransitionType.Fade);
			if (num && (curCgLayer == layer || layer == CgLayer.BG))
			{
				HideCG(mainCgNode, transition, duration);
			}
			else
			{
				ShowCG(newCgNode, transition, duration);
			}
			curCg = newCgNode;
			curCgLayer = layer;
			if (num)
			{
				FlipIdentifiers();
			}
		}

		private void StopInOutAnimations()
		{
			Game.Animations.StopAnimations<FadeAnimation>(mainCgNode);
			Game.Animations.StopAnimations<FadeAnimation>(altCgNode);
		}

		private void StopPanningAnimations()
		{
			Game.Animations.StopAnimations<PanAnimationControl>(mainCgNode);
			Game.Animations.StopAnimations<PanAnimationControl>(altCgNode);
		}

		private void MoveToCgPosition(Node node)
		{
			storyPlayer.MoveChild(node, storyPlayer.nCharRight.GetIndex() + 1);
		}

		private void MoveToBgPosition(Node node)
		{
			storyPlayer.MoveChild(node, 0);
		}

		private void MoveToFgPosition(Node node)
		{
			storyPlayer.MoveChild(node, storyPlayer.GetChildren().Count - 1);
		}

		public void MoveUnderCurrentCgNode(Node node)
		{
			storyPlayer.MoveChild(node, mainCgNode.GetIndex());
		}

		private void FlipIdentifiers()
		{
			Control control = mainCgNode;
			Control control2 = altCgNode;
			altCgNode = control;
			mainCgNode = control2;
		}

		private void ShowCG(Control img, StoryPlayerCgCommand.TransitionType mode, float duration)
		{
			if (mode == StoryPlayerCgCommand.TransitionType.Fade)
			{
				Game.Animations.Play(new FadeInAnimation(img, duration));
			}
			else
			{
				img.Modulate = ColorVisible;
			}
		}

		private void HideCG(Control img, StoryPlayerCgCommand.TransitionType mode, float duration)
		{
			if (!img.IsValid())
			{
				Log.Warn("Attempted to hide a CG when no CG is being shown.");
			}
			else if (mode == StoryPlayerCgCommand.TransitionType.Fade)
			{
				Game.Animations.Play(new FadeOutAnimation(img, duration));
			}
			else
			{
				img.Modulate = ColorNotVisible;
			}
		}
	}
}
