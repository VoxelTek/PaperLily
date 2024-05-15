// Decompiled with JetBrains decompiler
// Type: LacieEngine.Minigames.Battle
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Animation;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.UI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable disable
namespace LacieEngine.Minigames
{
    public class Battle : Control, IMinigame
    {
        public const double PLAYER_INVULNERABILITY_TIMER = 2.0;
        public const double PLAYER_DMG_FREQUENCY = 75.398223686155035;
        public const double TELEGRAPH_FREQUENCY = 50.26548245743669;
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
        [LacieEngine.API.GetNode("PlayArea")]
        protected Node2D nPlayArea;
        [LacieEngine.API.GetNode("Main")]
        protected Node2D nMain;
        protected CharacterSprite nPlayer;
        protected Control nTransitionOverlay;
        protected Control nFadeOverlay;
        public Battle.Cell[,] Cells;
        private string _characterName;
        private int _maxHp;
        private TextureRect[] _hpTextures;
        private double _invulnerabilityTimer;
        private AudioStream _previousTrack;
        private ConcurrentQueue<string> _inputQueue = new ConcurrentQueue<string>();
        private int _curAtkId;
        public bool Invincible;
        private SortedSet<Battle.BattleProcess> _pendingProcesses = new SortedSet<Battle.BattleProcess>((IComparer<Battle.BattleProcess>)new Battle.BattleProcess.Comparer());
        private LinkedList<Battle.BattleProcess> _currentProcesses = new LinkedList<Battle.BattleProcess>();
        private LinkedList<Battle.BattleSound> _currentSounds = new LinkedList<Battle.BattleSound>();

        public bool CustomTransition => true;

        public Node2D MainLayer => this.nMain;

        public int _playerX { get; private set; }

        public int _playerY { get; private set; }

        protected double _elapsed { get; private set; }

        protected int _hp { get; private set; }

        public int CurAttackId => ++this._curAtkId;

        public virtual void Init()
        {
            Vector2 size = this.texPanel.GetSize();
            this.Cells = new Battle.Cell[this.Width, this.Height];
            Vector2 vector2 = ((this.PlayArea.Size - new Vector2((float)((double)this.Width * (double)size.x + (double)(this.Width - 1) * (double)this.CellPadding.x), (float)((double)this.Height * (double)size.y + (double)(this.Height - 1) * (double)this.CellPadding.y))) / 2f).Floor();
            if (vector2 < Vector2.Zero)
            {
                Log.Warn((object)"Battle: PlayArea too small!");
                vector2 = Vector2.Zero;
            }
            this.nPlayArea.Position = this.PlayArea.Position;
            this.nMain.Position = this.nPlayArea.Position;
            int index1 = 0;
            int index2 = 0;
            for (int index3 = 0; index3 < this.Width * this.Height; ++index3)
            {
                Sprite sprite1 = GDUtil.MakeNode<Sprite>("Panel" + index3.ToString());
                sprite1.Texture = this.texPanel;
                sprite1.Position = vector2 + new Vector2((float)((double)index1 * (double)size.x + (double)index1 * (double)this.CellPadding.x), (float)((double)index2 * (double)size.y + (double)index2 * (double)this.CellPadding.y)) + size / 2f;
                this.nPlayArea.AddChild((Node)sprite1);
                Sprite sprite2 = GDUtil.MakeNode<Sprite>("PanelMask");
                sprite2.Texture = this.texPanelMask;
                sprite2.Modulate = Godot.Colors.Transparent;
                sprite1.AddChild((Node)sprite2);
                Sprite sprite3 = GDUtil.MakeNode<Sprite>("PanelAlert");
                sprite3.Texture = this.texPanelAlert ?? this.texPanelMask;
                sprite3.Modulate = Godot.Colors.Transparent;
                sprite1.AddChild((Node)sprite3);
                this.Cells[index1, index2] = new Battle.Cell();
                this.Cells[index1, index2].Sprite = sprite1;
                this.Cells[index1, index2].Mask = sprite2;
                this.Cells[index1, index2].Telegraph = sprite3;
                this.Cells[index1, index2].Rect = new Rect2(sprite1.Position - size / 2f, size);
                if (++index1 >= this.Width)
                {
                    index1 = 0;
                    ++index2;
                }
            }
            this._characterName = Game.State.Party[0];
            this.nPlayer = GDUtil.MakeNode<CharacterSprite>("Player");
            this.nPlayer.CharacterId = this._characterName;
            this.nPlayer.State = "battle_idle";
            this.nMain.AddChild((Node)this.nPlayer);
            this._maxHp = Math.Max(Game.Variables.GetIntVariable("general.maxhp." + this._characterName), 1);
            this._hp = Math.Max(Game.Variables.GetIntVariable("general.hp." + this._characterName), 1);
            this._playerX = (int)Math.Ceiling((Decimal)this.Width / 2M) - 1;
            this._playerY = (int)Math.Ceiling((Decimal)this.Height / 2M) - 1;
            this.nPlayer.Position = this.Cells[this._playerX, this._playerY].Sprite.Position;
            this.nFadeOverlay = (Control)UIUtil.CreateOverlay(Godot.Colors.Black);
            this.AddChild((Node)this.nFadeOverlay);
        }

        public virtual async Task TransitionIn()
        {
            if (this.bgmTrack != null)
            {
                this._previousTrack = Game.Audio.CurrentBgm;
                Game.Audio.StopBgm(0.1f);
                await DrkieUtil.DelaySeconds(0.20000000298023224);
                Game.Audio.PlayBgm(this.bgmTrack);
            }
            this.nTransitionOverlay = (Control)UIUtil.CreateOverlay(Godot.Colors.Black);
            this.nTransitionOverlay.Material = this.matTransition.Duplicate() as Material;
            Game.Screen.AddToLayer(IScreenManager.Layer.Minigame, (Node)this.nTransitionOverlay);
            ShaderProgressAnimation animation = new ShaderProgressAnimation((CanvasItem)this.nTransitionOverlay, 0.5f);
            Game.Animations.Play((LacieAnimation)animation);
            Game.Audio.PlaySfx(this.sfxTransition);
            await animation.WaitUntilFinished();
            Game.Room.Visible = false;
            await DrkieUtil.DelaySeconds(1.0);
        }

        public virtual async Task TransitionOut()
        {
            Battle baseNode = this;
            Control overlay = (Control)UIUtil.CreateOverlay(Godot.Colors.Black);
            baseNode.GetParent().AddChild((Node)overlay);
            if (baseNode._hp > 0)
            {
                if (baseNode.bgmTrack != null)
                    Game.Audio.PlayBgm(baseNode._previousTrack);
                FadeInAnimation animation1 = new FadeInAnimation((CanvasItem)overlay, 1f);
                Game.Animations.Play((LacieAnimation)animation1);
                await animation1.WaitUntilFinished();
                baseNode.Visible = false;
                Game.Room.Visible = true;
                FadeOutAnimation animation2 = new FadeOutAnimation((CanvasItem)overlay, 0.5f);
                Game.Animations.Play((LacieAnimation)animation2);
                await animation2.WaitUntilFinished();
                overlay = (Control)null;
            }
            else
            {
                baseNode.Visible = false;
                Game.Room.Visible = true;
                await baseNode.DelaySeconds(0.1);
                overlay = (Control)null;
            }
        }

        public virtual async void Start()
        {
            Game.InputProcessor = Inputs.Processor.None;
            FadeAnimation animation = (FadeAnimation)new FadeOutAnimation((CanvasItem)this.nFadeOverlay, 0.5f);
            Game.Animations.Play((LacieAnimation)animation);
            await animation.WaitUntilFinished();
            this.nTransitionOverlay.DeleteIfValid();
            this.nFadeOverlay.DeleteIfValid();
            Game.InputProcessor = Inputs.Processor.Minigame;
        }

        public void AddUiElements(Control parent)
        {
            TextureRect node = GDUtil.MakeNode<TextureRect>("HpUi");
            node.Texture = this.texUi;
            node.RectPosition = new Vector2(8f, 8f);
            parent.AddChild((Node)node);
            HBoxContainer box = GDUtil.MakeNode<HBoxContainer>("HpContainer");
            box.RectPosition = new Vector2(42f, 12f);
            box.SetSeparation(-8);
            this._hpTextures = new TextureRect[this._maxHp];
            for (int index = 0; index < this._maxHp; ++index)
            {
                this._hpTextures[index] = GDUtil.MakeNode<TextureRect>("Hp" + index.ToString());
                this._hpTextures[index].Texture = this.texHpFull;
                box.AddChild((Node)this._hpTextures[index]);
            }
            this.UpdateHp();
            node.AddChild((Node)box);
            Game.Animations.Play((LacieAnimation)new SlideInTopAnimation((Control)node));
        }

        public override void _Input(InputEvent @event)
        {
            if (this.CanProcessInput())
                this.ProcessInput(Inputs.Handle(@event, Inputs.Processor.Minigame, Inputs.AllMovement));
            else
                this._inputQueue.Enqueue(Inputs.Handle(@event, Inputs.Processor.Minigame, Inputs.AllMovement));
        }

        public void ProcessInput(string input)
        {
            switch (input)
            {
                case "input_left":
                    if (!this.CanMovePlayer(this._playerX - 1, this._playerY))
                        break;
                    this.nPlayer.State = "battle_move";
                    this.nPlayer.Direction = (Direction.Enum)Direction.Left;
                    this.MovePlayer(this._playerX - 1, this._playerY);
                    break;
                case "input_up":
                    if (!this.CanMovePlayer(this._playerX, this._playerY - 1))
                        break;
                    this.nPlayer.State = "battle_move";
                    this.nPlayer.Direction = (Direction.Enum)Direction.Up;
                    this.MovePlayer(this._playerX, this._playerY - 1);
                    break;
                case "input_right":
                    if (!this.CanMovePlayer(this._playerX + 1, this._playerY))
                        break;
                    this.nPlayer.State = "battle_move";
                    this.nPlayer.Direction = (Direction.Enum)Direction.Right;
                    this.MovePlayer(this._playerX + 1, this._playerY);
                    break;
                case "input_down":
                    if (!this.CanMovePlayer(this._playerX, this._playerY + 1))
                        break;
                    this.nPlayer.State = "battle_move";
                    this.nPlayer.Direction = (Direction.Enum)Direction.Down;
                    this.MovePlayer(this._playerX, this._playerY + 1);
                    break;
            }
        }

        private void ProcessQueuedInputs()
        {
            string result;
            while (!this._inputQueue.IsEmpty && this.CanProcessInput() && this._inputQueue.TryDequeue(out result))
                this.ProcessInput(result);
        }

        public override void _Process(float delta)
        {
            if (this.CanProcessInput())
                this.ProcessQueuedInputs();
            this._elapsed += (double)delta;
            for (int x = 0; x < this.Width; ++x)
            {
                for (int y = 0; y < this.Height; ++y)
                    this.ResetCellState(x, y);
            }
            this.nPlayer.Modulate = Godot.Colors.White;
            if (this._invulnerabilityTimer > 0.0)
                this._invulnerabilityTimer -= (double)delta;
            while (!this._pendingProcesses.IsEmpty<Battle.BattleProcess>() && this._elapsed >= (double)this._pendingProcesses.Min.InitTime)
            {
                Battle.BattleProcess min = this._pendingProcesses.Min;
                this._currentProcesses.AddLast(min);
                this._pendingProcesses.Remove(min);
                min.Init();
            }
            foreach (Battle.BattleProcess currentProcess in this._currentProcesses)
                currentProcess.Process((float)this._elapsed);
            foreach (Battle.BattleSound currentSound in this._currentSounds)
            {
                if (this._elapsed >= (double)currentSound.PlayTime)
                {
                    Game.Audio.PlaySfx(currentSound.Sfx, currentSound.Volume);
                    currentSound.Played = true;
                }
            }
            for (int x = 0; x < this.Width; ++x)
            {
                for (int y = 0; y < this.Height; ++y)
                {
                    if (this.Cells[x, y].Telegraphing)
                        this.ProcessTelegraph(x, y);
                    if (this.Cells[x, y].Damaging)
                        this.ProcessDamage(x, y);
                }
            }
            if (this.Cells[this._playerX, this._playerY].Mask.Modulate == Godot.Colors.Transparent)
                this.Cells[this._playerX, this._playerY].Mask.Modulate = this.PlayerColor;
            if (this._invulnerabilityTimer > 0.0)
                this.ProcessPlayerDamage();
            if (this._hp <= 0)
            {
                this.SetProcess(false);
                this.SetProcessInput(false);
                Game.Minigames.End(this.FailureEvent);
            }
            else
            {
                this._currentProcesses.RemoveAll<Battle.BattleProcess>((Func<Battle.BattleProcess, bool>)(attack => attack.ShouldDelete));
                this._currentSounds.RemoveAll<Battle.BattleSound>((Func<Battle.BattleSound, bool>)(sound => sound.Played));
                if (!this.TimeoutWin || (double)this.TimeoutTime >= this._elapsed)
                    return;
                this.SetProcess(false);
                this.SetProcessInput(false);
                Game.Variables.SetVariable("general.hp." + this._characterName, this._hp.ToString());
                Game.Minigames.End(this.SuccessEvent);
            }
        }

        private void ResetCellState(int x, int y)
        {
            this.Cells[x, y].Damaging = false;
            this.Cells[x, y].Telegraphing = false;
            this.Cells[x, y].Telegraph.Modulate = Godot.Colors.Transparent;
            this.Cells[x, y].Mask.Modulate = Godot.Colors.Transparent;
        }

        public void AddProcess(Battle.BattleProcess process)
        {
            process.PreInit(this);
            this._pendingProcesses.Add(process);
        }

        public void AddSound(Battle.BattleSound sound)
        {
            foreach (Battle.BattleSound currentSound in this._currentSounds)
            {
                if (currentSound.Sfx == sound.Sfx && (double)currentSound.PlayTime == (double)sound.PlayTime)
                    return;
            }
            this._currentSounds.AddLast(sound);
        }

        private bool CanProcessInput() => !(this.nPlayer.State == "battle_move");

        private bool CanMovePlayer(int x, int y) => this.CellIsValid(x, y);

        public bool CellIsValid(int x, int y) => x >= 0 && x < this.Width && y >= 0 && y < this.Height;

        private async void MovePlayer(int x, int y)
        {
            Battle baseNode = this;
            await baseNode.DelaySeconds(0.05);
            int playerX = baseNode._playerX;
            int playerY = baseNode._playerY;
            baseNode._playerX = x;
            baseNode._playerY = y;
            baseNode.nPlayer.Position = baseNode.Cells[baseNode._playerX, baseNode._playerY].Sprite.Position;
            baseNode.Cells[playerX, playerY].Mask.Modulate = Godot.Colors.Transparent;
            baseNode.Cells[baseNode._playerX, baseNode._playerY].Mask.Modulate = baseNode.PlayerColor;
            await baseNode.DelaySeconds(0.05);
            baseNode.nPlayer.State = "battle_idle";
        }

        private void ProcessTelegraph(int x, int y)
        {
            bool flag = Math.Sin(this._elapsed * (16.0 * Math.PI)) > 0.0;
            if (flag)
                this.Cells[x, y].Telegraph.Modulate = this.TelegraphColor;
            else
                this.Cells[x, y].Telegraph.Modulate = this.TelegraphColor2;
            if (this._playerX != x || this._playerY != y)
                return;
            if (flag)
                this.Cells[x, y].Mask.Modulate = this.TelegraphColor2;
            else
                this.Cells[x, y].Mask.Modulate = this.TelegraphColor;
        }

        private void ProcessDamage(int x, int y)
        {
            this.Cells[x, y].Mask.Modulate = this.DamageColor;
            if (!this.Invincible && this._playerX != x || this._playerY != y || this._invulnerabilityTimer > 0.0)
                return;
            --this._hp;
            this._invulnerabilityTimer = 2.0;
            if (this._hp > 0)
                Game.Audio.PlaySfx(this.sfxDamage);
            else
                Game.Audio.PlaySfx(this.sfxDeath);
            this.UpdateHp();
        }

        private void ProcessPlayerDamage()
        {
            if (Math.Sin(this._elapsed * (24.0 * Math.PI)) > 0.0)
                this.nPlayer.Modulate = Godot.Colors.Transparent;
            else
                this.nPlayer.Modulate = Godot.Colors.White;
        }

        public void DamagePanel(int x, int y)
        {
            if (x < 0 || y < 0 || x >= this.Width || y >= this.Height)
                return;
            this.Cells[x, y].Damaging = true;
        }

        public void DamagePanelAtPixel(Vector2 point)
        {
            Battle.Cell[,] cells = this.Cells;
            int upperBound1 = cells.GetUpperBound(0);
            int upperBound2 = cells.GetUpperBound(1);
            for (int lowerBound1 = cells.GetLowerBound(0); lowerBound1 <= upperBound1; ++lowerBound1)
            {
                for (int lowerBound2 = cells.GetLowerBound(1); lowerBound2 <= upperBound2; ++lowerBound2)
                {
                    Battle.Cell cell = cells[lowerBound1, lowerBound2];
                    if (cell.Rect.HasPoint(point))
                    {
                        cell.Damaging = true;
                        return;
                    }
                }
            }
        }

        private void UpdateHp()
        {
            for (int index = 0; index < this._maxHp; ++index)
                this._hpTextures[index].Texture = this._hp > index ? this.texHpFull : this.texHpEmpty;
        }

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
            public abstract int Id { get; }

            public abstract float InitTime { get; }

            public bool ShouldDelete { get; set; }

            public abstract void PreInit(Battle battle);

            public abstract void Init();

            public abstract void Process(float time);

            public class Comparer : IComparer<Battle.BattleProcess>
            {
                public int Compare(Battle.BattleProcess x, Battle.BattleProcess y)
                {
                    int num = x.InitTime.CompareTo(y.InitTime);
                    return num != 0 ? num : x.Id.CompareTo(y.Id);
                }
            }
        }

        public class BattleSound
        {
            public AudioStream Sfx;
            public float PlayTime;
            public float Volume = 1f;

            public bool Played { get; set; }

            public BattleSound(AudioStream sfx, float time)
            {
                this.Sfx = sfx;
                this.PlayTime = time;
            }

            public BattleSound(AudioStream sfx, float time, float volume)
              : this(sfx, time)
            {
                this.Volume = volume;
            }

            public class Comparer : IComparer<Battle.BattleSound>
            {
                public int Compare(Battle.BattleSound x, Battle.BattleSound y)
                {
                    return x.PlayTime.CompareTo(y.PlayTime);
                }
            }
        }
    }
}
