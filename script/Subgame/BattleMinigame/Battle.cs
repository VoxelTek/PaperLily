using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using LacieEngine.Animation;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.UI;

namespace LacieEngine.Minigames
{
	public class Battle : Control, IMinigame
	{
		public class Cell
		{
			public bool Damaging;

			public bool Telegraphing;

			public Sprite Sprite;

			public Sprite Mask;

			public Sprite Telegraph;

			public Rect2 Rect;
		}

		public abstract class BattleProcess
		{
			public class Comparer : IComparer<BattleProcess>
			{
				public int Compare(BattleProcess x, BattleProcess y)
				{
					int compare = x.InitTime.CompareTo(y.InitTime);
					if (compare != 0)
					{
						return compare;
					}
					return x.Id.CompareTo(y.Id);
				}
			}

			public abstract int Id { get; }

			public abstract float InitTime { get; }

			public bool ShouldDelete { get; set; }

			public abstract void PreInit(Battle battle);

			public abstract void Init();

			public abstract void Process(float time);
		}

		public class BattleSound
		{
			public class Comparer : IComparer<BattleSound>
			{
				public int Compare(BattleSound x, BattleSound y)
				{
					return x.PlayTime.CompareTo(y.PlayTime);
				}
			}

			public AudioStream Sfx;

			public float PlayTime;

			public float Volume = 1f;

			public bool Played { get; set; }

			public BattleSound(AudioStream sfx, float time)
			{
				Sfx = sfx;
				PlayTime = time;
			}

			public BattleSound(AudioStream sfx, float time, float volume)
				: this(sfx, time)
			{
				Volume = volume;
			}
		}

		public const double PLAYER_INVULNERABILITY_TIMER = 2.0;

		public const double PLAYER_DMG_FREQUENCY = Math.PI * 24.0;

		public const double TELEGRAPH_FREQUENCY = Math.PI * 16.0;

		public const double DAMAGE_DEFAULT_TIMER = 0.1;

		public const string SPRITE_STATE_IDLE = "battle_idle";

		public const string SPRITE_STATE_MOVE = "battle_move";

		[Export(PropertyHint.None, "")]
		public int Width = 3;

		[Export(PropertyHint.None, "")]
		public int Height = 3;

		[Export(PropertyHint.None, "")]
		public Vector2 CellPadding = Vector2.Zero;

		[Export(PropertyHint.None, "")]
		public Rect2 PlayArea;

		[Export(PropertyHint.None, "")]
		public Texture texPanel;

		[Export(PropertyHint.None, "")]
		public Texture texPanelMask;

		[Export(PropertyHint.None, "")]
		public Texture texPanelAlert;

		[Export(PropertyHint.None, "")]
		public string SuccessEvent;

		[Export(PropertyHint.None, "")]
		public string FailureEvent;

		[Export(PropertyHint.None, "")]
		public bool TimeoutWin;

		[Export(PropertyHint.None, "")]
		public float TimeoutTime;

		[Export(PropertyHint.None, "")]
		public Color PlayerColor = new Color("#64FFFFFF");

		[Export(PropertyHint.None, "")]
		public Color TelegraphColor = new Color("#E9B862");

		[Export(PropertyHint.None, "")]
		public Color TelegraphColor2 = new Color("00FFFFFF");

		[Export(PropertyHint.None, "")]
		public Color DamageColor = new Color("#9B1E2E");

		[Export(PropertyHint.None, "")]
		public Material matTransition;

		[Export(PropertyHint.None, "")]
		public AudioStream bgmTrack;

		[Export(PropertyHint.None, "")]
		public AudioStream sfxTransition;

		[Export(PropertyHint.None, "")]
		public AudioStream sfxDamage;

		[Export(PropertyHint.None, "")]
		public AudioStream sfxDeath;

		[Export(PropertyHint.None, "")]
		public Texture texUi;

		[Export(PropertyHint.None, "")]
		public Texture texHpFull;

		[Export(PropertyHint.None, "")]
		public Texture texHpEmpty;

		[GetNode("PlayArea")]
		protected Node2D nPlayArea;

		[GetNode("Main")]
		protected Node2D nMain;

		protected CharacterSprite nPlayer;

		protected Control nTransitionOverlay;

		protected Control nFadeOverlay;

		public Cell[,] Cells;

		private string _characterName;

		private int _maxHp;

		private TextureRect[] _hpTextures;

		private double _invulnerabilityTimer;

		private AudioStream _previousTrack;

		private ConcurrentQueue<string> _inputQueue = new ConcurrentQueue<string>();

		private int _curAtkId;

		private SortedSet<BattleProcess> _pendingProcesses = new SortedSet<BattleProcess>(new BattleProcess.Comparer());

		private LinkedList<BattleProcess> _currentProcesses = new LinkedList<BattleProcess>();

		private LinkedList<BattleSound> _currentSounds = new LinkedList<BattleSound>();

		public bool CustomTransition => true;

		public Node2D MainLayer => nMain;

		public int _playerX { get; private set; }

		public int _playerY { get; private set; }

		protected double _elapsed { get; private set; }

		protected int _hp { get; private set; }

		public int CurAttackId => ++_curAtkId;

		public virtual void Init()
		{
			Vector2 panelSize = texPanel.GetSize();
			Cells = new Cell[Width, Height];
			Vector2 actualPlayAreaSize = new Vector2((float)Width * panelSize.x + (float)(Width - 1) * CellPadding.x, (float)Height * panelSize.y + (float)(Height - 1) * CellPadding.y);
			Vector2 initialPosition = ((PlayArea.Size - actualPlayAreaSize) / 2f).Floor();
			if (initialPosition < Vector2.Zero)
			{
				Log.Warn("Battle: PlayArea too small!");
				initialPosition = Vector2.Zero;
			}
			nPlayArea.Position = PlayArea.Position;
			nMain.Position = nPlayArea.Position;
			int row = 0;
			int col = 0;
			for (int i = 0; i < Width * Height; i++)
			{
				Sprite panel = GDUtil.MakeNode<Sprite>("Panel" + i);
				panel.Texture = texPanel;
				panel.Position = initialPosition + new Vector2((float)row * panelSize.x + (float)row * CellPadding.x, (float)col * panelSize.y + (float)col * CellPadding.y) + panelSize / 2f;
				nPlayArea.AddChild(panel);
				Sprite panelMask = GDUtil.MakeNode<Sprite>("PanelMask");
				panelMask.Texture = texPanelMask;
				panelMask.Modulate = Colors.Transparent;
				panel.AddChild(panelMask);
				Sprite panelAlert = GDUtil.MakeNode<Sprite>("PanelAlert");
				panelAlert.Texture = texPanelAlert ?? texPanelMask;
				panelAlert.Modulate = Colors.Transparent;
				panel.AddChild(panelAlert);
				Cells[row, col] = new Cell();
				Cells[row, col].Sprite = panel;
				Cells[row, col].Mask = panelMask;
				Cells[row, col].Telegraph = panelAlert;
				Cells[row, col].Rect = new Rect2(panel.Position - panelSize / 2f, panelSize);
				if (++row >= Width)
				{
					row = 0;
					col++;
				}
			}
			_characterName = Game.State.Party[0];
			nPlayer = GDUtil.MakeNode<CharacterSprite>("Player");
			nPlayer.CharacterId = _characterName;
			nPlayer.State = "battle_idle";
			nMain.AddChild(nPlayer);
			_maxHp = Math.Max(Game.Variables.GetIntVariable("general.maxhp." + _characterName), 1);
			_hp = Math.Max(Game.Variables.GetIntVariable("general.hp." + _characterName), 1);
			_playerX = (int)Math.Ceiling((decimal)Width / 2m) - 1;
			_playerY = (int)Math.Ceiling((decimal)Height / 2m) - 1;
			nPlayer.Position = Cells[_playerX, _playerY].Sprite.Position;
			nFadeOverlay = UIUtil.CreateOverlay(Colors.Black);
			AddChild(nFadeOverlay);
		}

		public virtual async Task TransitionIn()
		{
			if (bgmTrack != null)
			{
				_previousTrack = Game.Audio.CurrentBgm;
				Game.Audio.StopBgm(0.1f);
				await DrkieUtil.DelaySeconds(0.20000000298023224);
				Game.Audio.PlayBgm(bgmTrack);
			}
			nTransitionOverlay = UIUtil.CreateOverlay(Colors.Black);
			nTransitionOverlay.Material = matTransition.Duplicate() as Material;
			Game.Screen.AddToLayer(IScreenManager.Layer.Minigame, nTransitionOverlay);
			ShaderProgressAnimation transition = new ShaderProgressAnimation(nTransitionOverlay, 0.5f);
			Game.Animations.Play(transition);
			Game.Audio.PlaySfx(sfxTransition);
			await transition.WaitUntilFinished();
			Game.Room.Visible = false;
			await DrkieUtil.DelaySeconds(1.0);
		}

		public virtual async Task TransitionOut()
		{
			Control overlay = UIUtil.CreateOverlay(Colors.Black);
			GetParent().AddChild(overlay);
			if (_hp > 0)
			{
				if (bgmTrack != null)
				{
					Game.Audio.PlayBgm(_previousTrack);
				}
				FadeInAnimation transition = new FadeInAnimation(overlay, 1f);
				Game.Animations.Play(transition);
				await transition.WaitUntilFinished();
				base.Visible = false;
				Game.Room.Visible = true;
				FadeOutAnimation transition2 = new FadeOutAnimation(overlay, 0.5f);
				Game.Animations.Play(transition2);
				await transition2.WaitUntilFinished();
			}
			else
			{
				base.Visible = false;
				Game.Room.Visible = true;
				await this.DelaySeconds(0.1);
			}
		}

		public virtual async void Start()
		{
			Game.InputProcessor = Inputs.Processor.None;
			FadeAnimation fadeOut = new FadeOutAnimation(nFadeOverlay, 0.5f);
			Game.Animations.Play(fadeOut);
			await fadeOut.WaitUntilFinished();
			nTransitionOverlay.DeleteIfValid();
			nFadeOverlay.DeleteIfValid();
			Game.InputProcessor = Inputs.Processor.Minigame;
		}

		public void AddUiElements(Control parent)
		{
			TextureRect hpUi = GDUtil.MakeNode<TextureRect>("HpUi");
			hpUi.Texture = texUi;
			hpUi.RectPosition = new Vector2(8f, 8f);
			parent.AddChild(hpUi);
			HBoxContainer hpContainer = GDUtil.MakeNode<HBoxContainer>("HpContainer");
			hpContainer.RectPosition = new Vector2(42f, 12f);
			hpContainer.SetSeparation(-8);
			_hpTextures = new TextureRect[_maxHp];
			for (int i = 0; i < _maxHp; i++)
			{
				_hpTextures[i] = GDUtil.MakeNode<TextureRect>("Hp" + i);
				_hpTextures[i].Texture = texHpFull;
				hpContainer.AddChild(_hpTextures[i]);
			}
			UpdateHp();
			hpUi.AddChild(hpContainer);
			Game.Animations.Play(new SlideInTopAnimation(hpUi));
		}

		public override void _Input(InputEvent @event)
		{
			if (CanProcessInput())
			{
				ProcessInput(Inputs.Handle(@event, Inputs.Processor.Minigame, Inputs.AllMovement));
			}
			else
			{
				_inputQueue.Enqueue(Inputs.Handle(@event, Inputs.Processor.Minigame, Inputs.AllMovement));
			}
		}

		public void ProcessInput(string input)
		{
			switch (input)
			{
			case "input_left":
				if (CanMovePlayer(_playerX - 1, _playerY))
				{
					nPlayer.State = "battle_move";
					nPlayer.Direction = Direction.Left;
					MovePlayer(_playerX - 1, _playerY);
				}
				break;
			case "input_up":
				if (CanMovePlayer(_playerX, _playerY - 1))
				{
					nPlayer.State = "battle_move";
					nPlayer.Direction = Direction.Up;
					MovePlayer(_playerX, _playerY - 1);
				}
				break;
			case "input_right":
				if (CanMovePlayer(_playerX + 1, _playerY))
				{
					nPlayer.State = "battle_move";
					nPlayer.Direction = Direction.Right;
					MovePlayer(_playerX + 1, _playerY);
				}
				break;
			case "input_down":
				if (CanMovePlayer(_playerX, _playerY + 1))
				{
					nPlayer.State = "battle_move";
					nPlayer.Direction = Direction.Down;
					MovePlayer(_playerX, _playerY + 1);
				}
				break;
			}
		}

		private void ProcessQueuedInputs()
		{
			string input;
			while (!_inputQueue.IsEmpty && CanProcessInput() && _inputQueue.TryDequeue(out input))
			{
				ProcessInput(input);
			}
		}

		public override void _Process(float delta)
		{
			if (CanProcessInput())
			{
				ProcessQueuedInputs();
			}
			_elapsed += delta;
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					ResetCellState(x, y);
				}
			}
			nPlayer.Modulate = Colors.White;
			if (_invulnerabilityTimer > 0.0)
			{
				_invulnerabilityTimer -= delta;
			}
			while (!_pendingProcesses.IsEmpty() && _elapsed >= (double)_pendingProcesses.Min.InitTime)
			{
				BattleProcess process = _pendingProcesses.Min;
				_currentProcesses.AddLast(process);
				_pendingProcesses.Remove(process);
				process.Init();
			}
			foreach (BattleProcess currentProcess in _currentProcesses)
			{
				currentProcess.Process((float)_elapsed);
			}
			foreach (BattleSound sound2 in _currentSounds)
			{
				if (_elapsed >= (double)sound2.PlayTime)
				{
					Game.Audio.PlaySfx(sound2.Sfx, sound2.Volume);
					sound2.Played = true;
				}
			}
			for (int x2 = 0; x2 < Width; x2++)
			{
				for (int y2 = 0; y2 < Height; y2++)
				{
					if (Cells[x2, y2].Telegraphing)
					{
						ProcessTelegraph(x2, y2);
					}
					if (Cells[x2, y2].Damaging)
					{
						ProcessDamage(x2, y2);
					}
				}
			}
			if (Cells[_playerX, _playerY].Mask.Modulate == Colors.Transparent)
			{
				Cells[_playerX, _playerY].Mask.Modulate = PlayerColor;
			}
			if (_invulnerabilityTimer > 0.0)
			{
				ProcessPlayerDamage();
			}
			if (_hp <= 0)
			{
				SetProcess(enable: false);
				SetProcessInput(enable: false);
				Game.Minigames.End(FailureEvent);
				return;
			}
			_currentProcesses.RemoveAll((BattleProcess attack) => attack.ShouldDelete);
			_currentSounds.RemoveAll((BattleSound sound) => sound.Played);
			if (TimeoutWin && (double)TimeoutTime < _elapsed)
			{
				SetProcess(enable: false);
				SetProcessInput(enable: false);
				Game.Variables.SetVariable("general.hp." + _characterName, _hp.ToString());
				Game.Minigames.End(SuccessEvent);
			}
		}

		private void ResetCellState(int x, int y)
		{
			Cells[x, y].Damaging = false;
			Cells[x, y].Telegraphing = false;
			Cells[x, y].Telegraph.Modulate = Colors.Transparent;
			Cells[x, y].Mask.Modulate = Colors.Transparent;
		}

		public void AddProcess(BattleProcess process)
		{
			process.PreInit(this);
			_pendingProcesses.Add(process);
		}

		public void AddSound(BattleSound sound)
		{
			foreach (BattleSound existingSound in _currentSounds)
			{
				if (existingSound.Sfx == sound.Sfx && existingSound.PlayTime == sound.PlayTime)
				{
					return;
				}
			}
			_currentSounds.AddLast(sound);
		}

		private bool CanProcessInput()
		{
			if (nPlayer.State == "battle_move")
			{
				return false;
			}
			return true;
		}

		private bool CanMovePlayer(int x, int y)
		{
			return CellIsValid(x, y);
		}

		public bool CellIsValid(int x, int y)
		{
			if (x < 0)
			{
				return false;
			}
			if (x >= Width)
			{
				return false;
			}
			if (y < 0)
			{
				return false;
			}
			if (y >= Height)
			{
				return false;
			}
			return true;
		}

		private async void MovePlayer(int x, int y)
		{
			await this.DelaySeconds(0.05);
			int oldPlayerX = _playerX;
			int oldPlayerY = _playerY;
			_playerX = x;
			_playerY = y;
			nPlayer.Position = Cells[_playerX, _playerY].Sprite.Position;
			Cells[oldPlayerX, oldPlayerY].Mask.Modulate = Colors.Transparent;
			Cells[_playerX, _playerY].Mask.Modulate = PlayerColor;
			await this.DelaySeconds(0.05);
			nPlayer.State = "battle_idle";
		}

		private void ProcessTelegraph(int x, int y)
		{
			bool color = Math.Sin(_elapsed * (Math.PI * 16.0)) > 0.0;
			if (color)
			{
				Cells[x, y].Telegraph.Modulate = TelegraphColor;
			}
			else
			{
				Cells[x, y].Telegraph.Modulate = TelegraphColor2;
			}
			if (_playerX == x && _playerY == y)
			{
				if (color)
				{
					Cells[x, y].Mask.Modulate = TelegraphColor2;
				}
				else
				{
					Cells[x, y].Mask.Modulate = TelegraphColor;
				}
			}
		}

		private void ProcessDamage(int x, int y)
		{
			Cells[x, y].Mask.Modulate = DamageColor;
			if (_playerX == x && _playerY == y && _invulnerabilityTimer <= 0.0)
			{
				_hp--;
				_invulnerabilityTimer = 2.0;
				if (_hp > 0)
				{
					Game.Audio.PlaySfx(sfxDamage);
				}
				else
				{
					Game.Audio.PlaySfx(sfxDeath);
				}
				UpdateHp();
			}
		}

		private void ProcessPlayerDamage()
		{
			if (Math.Sin(_elapsed * (Math.PI * 24.0)) > 0.0)
			{
				nPlayer.Modulate = Colors.Transparent;
			}
			else
			{
				nPlayer.Modulate = Colors.White;
			}
		}

		public void DamagePanel(int x, int y)
		{
			if (x >= 0 && y >= 0 && x < Width && y < Height)
			{
				Cells[x, y].Damaging = true;
			}
		}

		public void DamagePanelAtPixel(Vector2 point)
		{
			Cell[,] cells = Cells;
			foreach (Cell cell in cells)
			{
				if (cell.Rect.HasPoint(point))
				{
					cell.Damaging = true;
					return;
				}
			}
		}

		private void UpdateHp()
		{
			for (int i = 0; i < _maxHp; i++)
			{
				_hpTextures[i].Texture = ((_hp > i) ? texHpFull : texHpEmpty);
			}
		}
	}
}
