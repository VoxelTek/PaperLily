// Decompiled with JetBrains decompiler
// Type: LacieEngine.Minigames.BoxPuzzle
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;
using LacieEngine.UI;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Minigames
{
  internal class BoxPuzzle : Control, IMinigame
  {
    private static readonly string[] HandledInputs = new string[7]
    {
      "input_right",
      "input_left",
      "input_up",
      "input_down",
      "input_special",
      "input_cancel",
      "debug_key"
    };
    private TextureRect[,] _cells;
    private Dictionary<BoxPuzzle.Block, Texture> _textures;
    private byte[,] _startingGrid;
    private byte[,] _grid;

    [Export(PropertyHint.None, "")]
    public string OnSuccessEvent { get; private set; }

    [Export(PropertyHint.None, "")]
    public string OnFailureEvent { get; private set; }

    [Export(PropertyHint.None, "")]
    public int Rows { get; private set; }

    [Export(PropertyHint.None, "")]
    public int Columns { get; private set; }

    [Export(PropertyHint.MultilineText, "")]
    public string Grid { get; private set; }

    [Export(PropertyHint.None, "")]
    public Vector2 CellSize { get; private set; } = new Vector2(32f, 32f);

    [Export(PropertyHint.None, "")]
    public int BgSizeX { get; private set; }

    [Export(PropertyHint.None, "")]
    public int BgSizeY { get; private set; }

    [Export(PropertyHint.None, "")]
    public int BgMarginLeft { get; private set; }

    [Export(PropertyHint.None, "")]
    public int BgMarginTop { get; private set; }

    [Export(PropertyHint.None, "")]
    public int BgMarginRight { get; private set; }

    [Export(PropertyHint.None, "")]
    public int BgMarginBottom { get; private set; }

    [Export(PropertyHint.None, "")]
    public Texture TexBg { get; private set; }

    [Export(PropertyHint.None, "")]
    public Texture TexEmpty { get; private set; }

    [Export(PropertyHint.None, "")]
    public Texture TexBlock { get; private set; }

    [Export(PropertyHint.None, "")]
    public Texture TexDest { get; private set; }

    [Export(PropertyHint.None, "")]
    public Texture TexBlockDest { get; private set; }

    [Export(PropertyHint.None, "")]
    public Texture TexPlayer { get; private set; }

    [Export(PropertyHint.None, "")]
    public Texture TexFixed { get; private set; }

    [Export(PropertyHint.None, "")]
    public Texture TexGoal { get; private set; }

    [Export(PropertyHint.None, "")]
    public Texture TexBlockGoal { get; private set; }

    public void Init()
    {
      this._startingGrid = this.ParseGrid(this.Grid);
      this._textures = new Dictionary<BoxPuzzle.Block, Texture>();
      this._textures[BoxPuzzle.Block.EMPTY] = this.TexEmpty;
      this._textures[BoxPuzzle.Block.BLOCK] = this.TexBlock;
      this._textures[BoxPuzzle.Block.DEST] = this.TexDest;
      this._textures[BoxPuzzle.Block.BLOCK_DEST] = this.TexBlockDest;
      this._textures[BoxPuzzle.Block.PLAYER] = this.TexPlayer ?? GD.Load<Texture>("res://assets/sprite/common/character/" + Game.State.Party[0] + "/face.png");
      this._textures[BoxPuzzle.Block.PLAYER_DEST] = this.TexDest;
      this._textures[BoxPuzzle.Block.FIXED] = this.TexFixed;
      this._textures[BoxPuzzle.Block.GOAL] = this.TexGoal;
      this._textures[BoxPuzzle.Block.BLOCK_GOAL] = this.TexBlockGoal;
      this._textures[BoxPuzzle.Block.PLAYER_GOAL] = this.TexGoal;
      TextureRect textureRect1 = new TextureRect();
      textureRect1.Texture = this.TexBg;
      textureRect1.Expand = true;
      textureRect1.RectMinSize = new Vector2((float) this.BgSizeX, (float) this.BgSizeY);
      MarginContainer container = new MarginContainer();
      GridContainer box = new GridContainer();
      container.SetContainerMarginTop(this.BgMarginTop);
      container.SetContainerMarginBottom(this.BgMarginBottom);
      container.SetContainerMarginLeft(this.BgMarginLeft);
      container.SetContainerMarginRight(this.BgMarginRight);
      container.RectMinSize = new Vector2(this.CellSize.x * (float) this.Columns + (float) this.BgMarginLeft + (float) this.BgMarginRight, this.CellSize.y * (float) this.Rows + (float) this.BgMarginTop + (float) this.BgMarginBottom);
      container.AddChild((Node) box);
      box.Columns = this.Columns;
      box.SetSeparation(0, 0);
      box.RectMinSize = new Vector2(this.CellSize.x * (float) this.Columns, this.CellSize.y * (float) this.Rows);
      this._cells = new TextureRect[this.Rows, this.Columns];
      for (int index1 = 0; index1 < this.Rows; ++index1)
      {
        for (int index2 = 0; index2 < this.Columns; ++index2)
        {
          TextureRect textureRect2 = new TextureRect();
          box.AddChild((Node) textureRect2);
          textureRect2.Expand = true;
          textureRect2.SizeFlagsHorizontal = 2;
          textureRect2.SizeFlagsVertical = 2;
          textureRect2.RectMinSize = this.CellSize;
          this._cells[index1, index2] = textureRect2;
        }
      }
      this.InitializeGrid();
      this.PaintAll();
      this.RectMinSize = new Vector2(this.CellSize.x * (float) this.Columns + (float) this.BgMarginLeft + (float) this.BgMarginRight, this.CellSize.y * (float) this.Rows + (float) this.BgMarginTop + (float) this.BgMarginBottom);
      this.AddChild((Node) textureRect1);
      this.AddChild((Node) container);
    }

    public void AddUiElements(Control parent)
    {
      Control boxWithInputIcons = (Control) UIUtil.CreateInfoBoxWithInputIcons("system.minigames.boxpuzzle.info", "input_cancel", "input_special");
      boxWithInputIcons.Name = "MinigameInfo";
      boxWithInputIcons.SetAnchorsAndMarginsPreset(Control.LayoutPreset.TopLeft);
      parent.AddChild((Node) boxWithInputIcons);
      Game.Animations.Play((LacieAnimation) new SlideInTopAnimation(boxWithInputIcons));
    }

    public override void _Input(InputEvent @event)
    {
      string str = Inputs.Handle(@event, Inputs.Processor.Minigame, BoxPuzzle.HandledInputs);
      if (str == null)
        return;
      int x;
      int y;
      this.FindPlayer(out x, out y);
      if (str != null)
      {
        switch (str.Length)
        {
          case 8:
            if (str == "input_up")
            {
              BoxPuzzle.Block above = this.FindAbove(x, y);
              if (this.CanMoveTo(above))
                this.MoveUp(x, y);
              if (this.IsBlock(above) && this.CanMoveTo(this.FindAbove(x, y - 1)))
              {
                this.MoveBoxUp(x, y - 1);
                break;
              }
              break;
            }
            break;
          case 9:
            if (str == "debug_key")
            {
              Game.InputProcessor = Inputs.Processor.None;
              Game.Minigames.End(this.OnSuccessEvent);
              break;
            }
            break;
          case 10:
            switch (str[6])
            {
              case 'd':
                if (str == "input_down")
                {
                  BoxPuzzle.Block below = this.FindBelow(x, y);
                  if (this.CanMoveTo(below))
                    this.MoveDown(x, y);
                  if (this.IsBlock(below) && this.CanMoveTo(this.FindBelow(x, y + 1)))
                  {
                    this.MoveBoxDown(x, y + 1);
                    break;
                  }
                  break;
                }
                break;
              case 'l':
                if (str == "input_left")
                {
                  BoxPuzzle.Block atLeft = this.FindAtLeft(x, y);
                  if (this.CanMoveTo(atLeft))
                    this.MoveLeft(x, y);
                  if (this.IsBlock(atLeft) && this.CanMoveTo(this.FindAtLeft(x - 1, y)))
                  {
                    this.MoveBoxLeft(x - 1, y);
                    break;
                  }
                  break;
                }
                break;
            }
            break;
          case 11:
            if (str == "input_right")
            {
              BoxPuzzle.Block atRight = this.FindAtRight(x, y);
              if (this.CanMoveTo(atRight))
                this.MoveRight(x, y);
              if (this.IsBlock(atRight) && this.CanMoveTo(this.FindAtRight(x + 1, y)))
              {
                this.MoveBoxRight(x + 1, y);
                break;
              }
              break;
            }
            break;
          case 12:
            if (str == "input_cancel")
            {
              Game.Audio.PlaySystemSound("res://assets/sfx/ui_bad.ogg");
              Game.InputProcessor = Inputs.Processor.None;
              Game.Minigames.End(this.OnFailureEvent);
              break;
            }
            break;
          case 13:
            if (str == "input_special")
            {
              Game.Audio.PlaySystemSound("res://assets/sfx/ui_bad.ogg");
              this.InitializeGrid();
              break;
            }
            break;
        }
      }
      this.PaintAll();
      this.CheckSuccess();
    }

    public void FindPlayer(out int x, out int y)
    {
      x = y = 0;
      for (int index1 = 0; index1 < this.Rows; ++index1)
      {
        for (int index2 = 0; index2 < this.Columns; ++index2)
        {
          if (this.IsPlayer((BoxPuzzle.Block) this._grid[index1, index2]))
          {
            y = index1;
            x = index2;
            return;
          }
        }
      }
    }

    public bool CanMoveTo(BoxPuzzle.Block cell)
    {
      return cell == BoxPuzzle.Block.EMPTY || cell == BoxPuzzle.Block.DEST || cell == BoxPuzzle.Block.GOAL;
    }

    public bool IsPlayer(BoxPuzzle.Block cell)
    {
      switch (cell)
      {
        case BoxPuzzle.Block.PLAYER:
        case BoxPuzzle.Block.PLAYER_DEST:
        case BoxPuzzle.Block.PLAYER_GOAL:
          return true;
        default:
          return false;
      }
    }

    public bool IsBlock(BoxPuzzle.Block cell)
    {
      return cell == BoxPuzzle.Block.BLOCK || cell == BoxPuzzle.Block.BLOCK_DEST || cell == BoxPuzzle.Block.BLOCK_GOAL;
    }

    public bool IsDest(BoxPuzzle.Block cell)
    {
      switch (cell)
      {
        case BoxPuzzle.Block.DEST:
        case BoxPuzzle.Block.PLAYER_DEST:
        case BoxPuzzle.Block.GOAL:
        case BoxPuzzle.Block.BLOCK_GOAL:
          return true;
        default:
          return false;
      }
    }

    public async void EndMinigameOnSuccess()
    {
      Game.Audio.PlaySystemSound("res://assets/sfx/minigame_fanfare.ogg");
      await GDUtil.DelayOneFrame();
      Game.Minigames.End(this.OnSuccessEvent);
    }

    private void CheckSuccess()
    {
      for (int y = 0; y < this.Rows; ++y)
      {
        for (int x = 0; x < this.Columns; ++x)
        {
          if (this.IsDest(this.GetCell(x, y)))
            return;
        }
      }
      Game.InputProcessor = Inputs.Processor.None;
      this.EndMinigameOnSuccess();
    }

    public BoxPuzzle.Block GetCell(int x, int y) => (BoxPuzzle.Block) this._grid[y, x];

    public void MoveLeft(int x, int y)
    {
      BoxPuzzle.Block atLeft = this.FindAtLeft(x, y);
      BoxPuzzle.Block oldCell = this.MovePlayerOutOf(this.GetCell(x, y));
      BoxPuzzle.Block newCell = this.MovePlayerInto(atLeft);
      this.Move(x, y, x - 1, y, oldCell, newCell);
    }

    public void MoveRight(int x, int y)
    {
      BoxPuzzle.Block atRight = this.FindAtRight(x, y);
      BoxPuzzle.Block oldCell = this.MovePlayerOutOf(this.GetCell(x, y));
      BoxPuzzle.Block newCell = this.MovePlayerInto(atRight);
      this.Move(x, y, x + 1, y, oldCell, newCell);
    }

    public void MoveUp(int x, int y)
    {
      BoxPuzzle.Block above = this.FindAbove(x, y);
      BoxPuzzle.Block oldCell = this.MovePlayerOutOf(this.GetCell(x, y));
      BoxPuzzle.Block newCell = this.MovePlayerInto(above);
      this.Move(x, y, x, y - 1, oldCell, newCell);
    }

    public void MoveDown(int x, int y)
    {
      BoxPuzzle.Block below = this.FindBelow(x, y);
      BoxPuzzle.Block oldCell = this.MovePlayerOutOf(this.GetCell(x, y));
      BoxPuzzle.Block newCell = this.MovePlayerInto(below);
      this.Move(x, y, x, y + 1, oldCell, newCell);
    }

    public void MoveBoxLeft(int x, int y)
    {
      Game.Audio.PlaySfx("res://assets/sfx/minigame_block_slide.ogg");
      BoxPuzzle.Block atLeft = this.FindAtLeft(x, y);
      BoxPuzzle.Block oldCell = this.MoveBoxOutOf(this.GetCell(x, y));
      BoxPuzzle.Block newCell = this.MoveBoxInto(atLeft);
      this.Move(x, y, x - 1, y, oldCell, newCell);
      this.MoveLeft(x + 1, y);
    }

    public void MoveBoxRight(int x, int y)
    {
      Game.Audio.PlaySfx("res://assets/sfx/minigame_block_slide.ogg");
      BoxPuzzle.Block atRight = this.FindAtRight(x, y);
      BoxPuzzle.Block oldCell = this.MoveBoxOutOf(this.GetCell(x, y));
      BoxPuzzle.Block newCell = this.MoveBoxInto(atRight);
      this.Move(x, y, x + 1, y, oldCell, newCell);
      this.MoveRight(x - 1, y);
    }

    public void MoveBoxUp(int x, int y)
    {
      Game.Audio.PlaySfx("res://assets/sfx/minigame_block_slide.ogg");
      BoxPuzzle.Block above = this.FindAbove(x, y);
      BoxPuzzle.Block oldCell = this.MoveBoxOutOf(this.GetCell(x, y));
      BoxPuzzle.Block newCell = this.MoveBoxInto(above);
      this.Move(x, y, x, y - 1, oldCell, newCell);
      this.MoveUp(x, y + 1);
    }

    public void MoveBoxDown(int x, int y)
    {
      Game.Audio.PlaySfx("res://assets/sfx/minigame_block_slide.ogg");
      BoxPuzzle.Block below = this.FindBelow(x, y);
      BoxPuzzle.Block oldCell = this.MoveBoxOutOf(this.GetCell(x, y));
      BoxPuzzle.Block newCell = this.MoveBoxInto(below);
      this.Move(x, y, x, y + 1, oldCell, newCell);
      this.MoveDown(x, y - 1);
    }

    public void Move(
      int x,
      int y,
      int newX,
      int newY,
      BoxPuzzle.Block oldCell,
      BoxPuzzle.Block newCell)
    {
      this._grid[y, x] = (byte) oldCell;
      this._grid[newY, newX] = (byte) newCell;
    }

    private BoxPuzzle.Block FindAtLeft(int x, int y)
    {
      return x == 0 ? BoxPuzzle.Block.FIXED : (BoxPuzzle.Block) this._grid[y, x - 1];
    }

    private BoxPuzzle.Block FindAtRight(int x, int y)
    {
      return x + 1 == this.Columns ? BoxPuzzle.Block.FIXED : (BoxPuzzle.Block) this._grid[y, x + 1];
    }

    private BoxPuzzle.Block FindAbove(int x, int y)
    {
      return y == 0 ? BoxPuzzle.Block.FIXED : (BoxPuzzle.Block) this._grid[y - 1, x];
    }

    private BoxPuzzle.Block FindBelow(int x, int y)
    {
      return y + 1 == this.Rows ? BoxPuzzle.Block.FIXED : (BoxPuzzle.Block) this._grid[y + 1, x];
    }

    private BoxPuzzle.Block MovePlayerOutOf(BoxPuzzle.Block cell)
    {
      BoxPuzzle.Block block;
      switch (cell)
      {
        case BoxPuzzle.Block.PLAYER_DEST:
          block = BoxPuzzle.Block.DEST;
          break;
        case BoxPuzzle.Block.PLAYER_GOAL:
          block = BoxPuzzle.Block.GOAL;
          break;
        default:
          block = BoxPuzzle.Block.EMPTY;
          break;
      }
      return block;
    }

    private BoxPuzzle.Block MovePlayerInto(BoxPuzzle.Block cell)
    {
      BoxPuzzle.Block block;
      switch (cell)
      {
        case BoxPuzzle.Block.DEST:
          block = BoxPuzzle.Block.PLAYER_DEST;
          break;
        case BoxPuzzle.Block.GOAL:
          block = BoxPuzzle.Block.PLAYER_GOAL;
          break;
        default:
          block = BoxPuzzle.Block.PLAYER;
          break;
      }
      return block;
    }

    private BoxPuzzle.Block MoveBoxOutOf(BoxPuzzle.Block cell)
    {
      BoxPuzzle.Block block;
      switch (cell)
      {
        case BoxPuzzle.Block.BLOCK_DEST:
          block = BoxPuzzle.Block.DEST;
          break;
        case BoxPuzzle.Block.BLOCK_GOAL:
          block = BoxPuzzle.Block.GOAL;
          break;
        default:
          block = BoxPuzzle.Block.EMPTY;
          break;
      }
      return block;
    }

    private BoxPuzzle.Block MoveBoxInto(BoxPuzzle.Block cell)
    {
      BoxPuzzle.Block block;
      switch (cell)
      {
        case BoxPuzzle.Block.DEST:
          block = BoxPuzzle.Block.BLOCK_DEST;
          break;
        case BoxPuzzle.Block.GOAL:
          block = BoxPuzzle.Block.BLOCK_GOAL;
          break;
        default:
          block = BoxPuzzle.Block.BLOCK;
          break;
      }
      return block;
    }

    private void InitializeGrid()
    {
      this._grid = new byte[this.Rows, this.Columns];
      for (int index1 = 0; index1 < this.Rows; ++index1)
      {
        for (int index2 = 0; index2 < this.Columns; ++index2)
          this._grid[index1, index2] = this._startingGrid[index1, index2];
      }
    }

    private void PaintAll()
    {
      for (int y = 0; y < this.Rows; ++y)
      {
        for (int x = 0; x < this.Columns; ++x)
          this.Paint(x, y);
      }
    }

    private byte[,] ParseGrid(string definitionGrid)
    {
      string[] strArray = definitionGrid.Split(new char[2]
      {
        '\r',
        '\n'
      }, StringSplitOptions.RemoveEmptyEntries);
      byte[,] grid = new byte[this.Rows, this.Columns];
      for (int index1 = 0; index1 < strArray.Length; ++index1)
      {
        for (int index2 = 0; index2 < strArray[index1].Length; ++index2)
          grid[index1, index2] = byte.Parse(strArray[index1][index2].ToString());
      }
      return grid;
    }

    private void Paint(int x, int y)
    {
      BoxPuzzle.Block block = (BoxPuzzle.Block) this._grid[y, x];
      TextureRect cell = this._cells[y, x];
      cell.Clear();
      cell.Texture = this._textures[block];
      if (!this.IsPlayer(block))
        return;
      cell.AddChild((Node) this.MakePlayerTexture());
    }

    private TextureRect MakePlayerTexture()
    {
      TextureRect textureRect = new TextureRect();
      textureRect.Expand = true;
      textureRect.SizeFlagsHorizontal = 2;
      textureRect.SizeFlagsVertical = 2;
      textureRect.RectMinSize = this.CellSize;
      textureRect.Texture = this._textures[BoxPuzzle.Block.PLAYER];
      return textureRect;
    }

    public enum Block : byte
    {
      EMPTY,
      BLOCK,
      DEST,
      BLOCK_DEST,
      PLAYER,
      PLAYER_DEST,
      FIXED,
      GOAL,
      BLOCK_GOAL,
      PLAYER_GOAL,
    }
  }
}
