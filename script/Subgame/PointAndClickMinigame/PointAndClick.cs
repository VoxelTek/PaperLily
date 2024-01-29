using System.Collections.Generic;
using System.Linq;
using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;
using LacieEngine.UI;

namespace LacieEngine.Minigames
{
	public class PointAndClick : Control, IMinigame
	{
		private const int CursorSpeed = 300;

		private const int SlowCursorSpeed = 100;

		private static readonly string[] HandledInputs = new string[2] { "input_action", "input_cancel" };

		protected TextureRect nCursor;

		private List<Control> _targets = new List<Control>();

		private bool _ready;

		private Direction _cursorDirection = Direction.None;

		[Export(PropertyHint.None, "")]
		public string OnCloseEvent { get; private set; }

		[Export(PropertyHint.None, "")]
		public bool SnapEnabled { get; private set; }

		[Export(PropertyHint.None, "")]
		public bool HideCursor { get; private set; }

		[Export(PropertyHint.None, "")]
		public Dictionary<string, string> EventTargets { get; private set; }

		[Export(PropertyHint.None, "")]
		public Dictionary<string, string> CloseEventTargets { get; private set; }

		[Export(PropertyHint.None, "")]
		public Dictionary<string, string> FunctionTargets { get; private set; }

		[Export(PropertyHint.None, "")]
		public ShaderMaterial HighlightShader { get; private set; }

		public override void _Ready()
		{
			foreach (Node target in GetNode("Targets").GetChildren())
			{
				_targets.Add((Control)target);
			}
			_targets.Reverse();
			if (SnapEnabled)
			{
				NavigateToFirstTarget();
			}
			_ready = true;
		}

		public virtual void Init()
		{
			nCursor = new TextureRect();
			nCursor.Expand = true;
			nCursor.Texture = GD.Load<Texture>("res://assets/img/ui/cursor.png");
			nCursor.RectSize = new Vector2(32f, 32f);
			nCursor.RectPosition = new Vector2(base.RectMinSize.x / 2f, base.RectMinSize.y / 2f);
			if (HideCursor)
			{
				nCursor.Visible = false;
			}
			AddChild(nCursor);
			if (EventTargets == null)
			{
				Dictionary<string, string> dictionary2 = (EventTargets = new Dictionary<string, string>());
			}
			if (CloseEventTargets == null)
			{
				Dictionary<string, string> dictionary2 = (CloseEventTargets = new Dictionary<string, string>());
			}
			if (FunctionTargets == null)
			{
				Dictionary<string, string> dictionary2 = (FunctionTargets = new Dictionary<string, string>());
			}
		}

		public void AddUiElements(Control parent)
		{
			if (!SnapEnabled)
			{
				Control infoBox = UIUtil.CreateInfoBoxWithInputIcons("system.minigames.pointandclick.info", "input_run");
				infoBox.Name = "MinigameInfo";
				infoBox.SetAnchorsAndMarginsPreset(LayoutPreset.TopLeft);
				parent.AddChild(infoBox);
				Game.Animations.Play(new SlideInTopAnimation(infoBox));
			}
		}

		public override void _Input(InputEvent @event)
		{
			string text = Inputs.Handle(@event, Inputs.Processor.Minigame, HandledInputs);
			if (!(text == "input_action"))
			{
				if (text == "input_cancel")
				{
					Game.InputProcessor = Inputs.Processor.None;
					Game.Minigames.End(OnCloseEvent);
				}
			}
			else
			{
				Control target = GetTargetUnderCursor();
				if (target != null)
				{
					CallTarget(target.Name);
				}
			}
			if (!SnapEnabled || !(GetTargetUnderCursor() is PointAndClickSnapTarget snapTarget))
			{
				return;
			}
			switch (Inputs.Handle(@event, Inputs.Processor.Minigame, Inputs.AllMovement))
			{
			case "input_left":
				if (snapTarget.Left != null)
				{
					NavigateToSnapTarget(snapTarget.Left);
				}
				break;
			case "input_up":
				if (snapTarget.Up != null)
				{
					NavigateToSnapTarget(snapTarget.Up);
				}
				break;
			case "input_right":
				if (snapTarget.Right != null)
				{
					NavigateToSnapTarget(snapTarget.Right);
				}
				break;
			case "input_down":
				if (snapTarget.Down != null)
				{
					NavigateToSnapTarget(snapTarget.Down);
				}
				break;
			}
		}

		public override void _Process(float delta)
		{
			if (_ready && !SnapEnabled)
			{
				_cursorDirection = Inputs.GetDirectionFromInput(_cursorDirection, Inputs.Processor.Minigame);
				if (Game.InputProcessor == Inputs.Processor.Minigame && Inputs.IsActionPressed("input_run"))
				{
					nCursor.RectPosition += _cursorDirection.ToVector() * 100f * delta;
				}
				else
				{
					nCursor.RectPosition += _cursorDirection.ToVector() * 300f * delta;
				}
				if (nCursor.RectPosition.x < 0f)
				{
					nCursor.RectPosition = new Vector2(0f, nCursor.RectPosition.y);
				}
				else if (nCursor.RectPosition.x > base.RectMinSize.x)
				{
					nCursor.RectPosition = new Vector2(base.RectMinSize.x, nCursor.RectPosition.y);
				}
				if (nCursor.RectPosition.y < 0f)
				{
					nCursor.RectPosition = new Vector2(nCursor.RectPosition.x, 0f);
				}
				else if (nCursor.RectPosition.y > base.RectMinSize.y)
				{
					nCursor.RectPosition = new Vector2(nCursor.RectPosition.x, base.RectMinSize.y);
				}
			}
		}

		public void CallFunction(string functionName)
		{
			Log.Debug("Call minigame function: ", functionName);
			if (!HasMethod(functionName))
			{
				Log.Error("Minigame function not found: ", functionName);
			}
			else
			{
				Call(functionName);
			}
		}

		private void NavigateToFirstTarget()
		{
			foreach (PointAndClickSnapTarget target in _targets.Cast<PointAndClickSnapTarget>())
			{
				if (target.FirstTarget)
				{
					nCursor.RectPosition = target.RectPosition + target.SnapOffset;
					if (HighlightShader != null)
					{
						target.Material = HighlightShader;
					}
					break;
				}
			}
		}

		private void NavigateToSnapTarget(string targetName)
		{
			foreach (Control target in _targets)
			{
				target.Material = null;
				if (target.Name == targetName)
				{
					PointAndClickSnapTarget newTarget = target as PointAndClickSnapTarget;
					nCursor.RectPosition = newTarget.RectPosition + newTarget.SnapOffset;
					if (HighlightShader != null)
					{
						newTarget.Material = HighlightShader;
					}
					Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
				}
			}
		}

		private Control GetTargetUnderCursor()
		{
			foreach (Control target in _targets)
			{
				if (target.Visible && target.GetRect().HasPoint(nCursor.RectPosition))
				{
					if (!(target is TextureRect texture))
					{
						return target;
					}
					Vector2 relativeClickPosition = nCursor.RectPosition - target.RectPosition;
					if (texture.GetPixelAt(relativeClickPosition).a != 0f)
					{
						return target;
					}
				}
			}
			return null;
		}

		private void CallTarget(string target)
		{
			Log.Debug("Selected PnC target: ", target);
			if (EventTargets.ContainsKey(target))
			{
				Game.InputProcessor = Inputs.Processor.None;
				Game.Events.PlayEvent(EventTargets[target]);
			}
			else if (CloseEventTargets.ContainsKey(target))
			{
				Game.InputProcessor = Inputs.Processor.None;
				Game.Minigames.End(CloseEventTargets[target]);
			}
			else if (FunctionTargets.ContainsKey(target))
			{
				CallFunction(FunctionTargets[target]);
			}
		}
	}
}
