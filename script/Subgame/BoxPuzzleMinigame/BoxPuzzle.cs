using System;
using System.Collections.Generic;
using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;
using LacieEngine.UI;

namespace LacieEngine.Minigames
{
	internal class BoxPuzzle : Control, IMinigame
	{
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
			PLAYER_GOAL
		}

		private static readonly string[] HandledInputs = new string[7] { "input_right", "input_left", "input_up", "input_down", "input_special", "input_cancel", "debug_key" };

		private TextureRect[,] _cells;

		private Dictionary<Block, Texture> _textures;

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
			_startingGrid = ParseGrid(Grid);
			_textures = new Dictionary<Block, Texture>();
			_textures[Block.EMPTY] = TexEmpty;
			_textures[Block.BLOCK] = TexBlock;
			_textures[Block.DEST] = TexDest;
			_textures[Block.BLOCK_DEST] = TexBlockDest;
			_textures[Block.PLAYER] = TexPlayer ?? GD.Load<Texture>("res://assets/sprite/common/character/" + Game.State.Party[0] + "/face.png");
			_textures[Block.PLAYER_DEST] = TexDest;
			_textures[Block.FIXED] = TexFixed;
			_textures[Block.GOAL] = TexGoal;
			_textures[Block.BLOCK_GOAL] = TexBlockGoal;
			_textures[Block.PLAYER_GOAL] = TexGoal;
			TextureRect bg = new TextureRect();
			bg.Texture = TexBg;
			bg.Expand = true;
			bg.RectMinSize = new Vector2(BgSizeX, BgSizeY);
			MarginContainer marginContainer = new MarginContainer();
			GridContainer grid = new GridContainer();
			marginContainer.SetContainerMarginTop(BgMarginTop);
			marginContainer.SetContainerMarginBottom(BgMarginBottom);
			marginContainer.SetContainerMarginLeft(BgMarginLeft);
			marginContainer.SetContainerMarginRight(BgMarginRight);
			marginContainer.RectMinSize = new Vector2(CellSize.x * (float)Columns + (float)BgMarginLeft + (float)BgMarginRight, CellSize.y * (float)Rows + (float)BgMarginTop + (float)BgMarginBottom);
			marginContainer.AddChild(grid);
			grid.Columns = Columns;
			grid.SetSeparation(0, 0);
			grid.RectMinSize = new Vector2(CellSize.x * (float)Columns, CellSize.y * (float)Rows);
			_cells = new TextureRect[Rows, Columns];
			for (int i = 0; i < Rows; i++)
			{
				for (int j = 0; j < Columns; j++)
				{
					TextureRect rect = new TextureRect();
					grid.AddChild(rect);
					rect.Expand = true;
					rect.SizeFlagsHorizontal = 2;
					rect.SizeFlagsVertical = 2;
					rect.RectMinSize = CellSize;
					_cells[i, j] = rect;
				}
			}
			InitializeGrid();
			PaintAll();
			base.RectMinSize = new Vector2(CellSize.x * (float)Columns + (float)BgMarginLeft + (float)BgMarginRight, CellSize.y * (float)Rows + (float)BgMarginTop + (float)BgMarginBottom);
			AddChild(bg);
			AddChild(marginContainer);
		}

		public void AddUiElements(Control parent)
		{
			Control infoBox = UIUtil.CreateInfoBoxWithInputIcons("system.minigames.boxpuzzle.info", "input_cancel", "input_special");
			infoBox.Name = "MinigameInfo";
			infoBox.SetAnchorsAndMarginsPreset(LayoutPreset.TopLeft);
			parent.AddChild(infoBox);
			Game.Animations.Play(new SlideInTopAnimation(infoBox));
		}

		public override void _Input(InputEvent @event)
		{
			string input = Inputs.Handle(@event, Inputs.Processor.Minigame, HandledInputs);
			if (input == null)
			{
				return;
			}
			FindPlayer(out var x, out var y);
			if (input != null)
			{
				switch (input.Length)
				{
				case 10:
					switch (input[6])
					{
					case 'l':
						if (input == "input_left")
						{
							Block cell = FindAtLeft(x, y);
							if (CanMoveTo(cell))
							{
								MoveLeft(x, y);
							}
							if (IsBlock(cell) && CanMoveTo(FindAtLeft(x - 1, y)))
							{
								MoveBoxLeft(x - 1, y);
							}
						}
						break;
					case 'd':
						if (input == "input_down")
						{
							Block cell = FindBelow(x, y);
							if (CanMoveTo(cell))
							{
								MoveDown(x, y);
							}
							if (IsBlock(cell) && CanMoveTo(FindBelow(x, y + 1)))
							{
								MoveBoxDown(x, y + 1);
							}
						}
						break;
					}
					break;
				case 11:
					if (input == "input_right")
					{
						Block cell = FindAtRight(x, y);
						if (CanMoveTo(cell))
						{
							MoveRight(x, y);
						}
						if (IsBlock(cell) && CanMoveTo(FindAtRight(x + 1, y)))
						{
							MoveBoxRight(x + 1, y);
						}
					}
					break;
				case 8:
					if (input == "input_up")
					{
						Block cell = FindAbove(x, y);
						if (CanMoveTo(cell))
						{
							MoveUp(x, y);
						}
						if (IsBlock(cell) && CanMoveTo(FindAbove(x, y - 1)))
						{
							MoveBoxUp(x, y - 1);
						}
					}
					break;
				case 13:
					if (input == "input_special")
					{
						Game.Audio.PlaySystemSound("res://assets/sfx/ui_bad.ogg");
						InitializeGrid();
					}
					break;
				case 12:
					if (input == "input_cancel")
					{
						Game.Audio.PlaySystemSound("res://assets/sfx/ui_bad.ogg");
						Game.InputProcessor = Inputs.Processor.None;
						Game.Minigames.End(OnFailureEvent);
					}
					break;
				case 9:
					if (input == "debug_key")
					{
						Game.InputProcessor = Inputs.Processor.None;
						Game.Minigames.End(OnSuccessEvent);
					}
					break;
				}
			}
			PaintAll();
			CheckSuccess();
		}

		public void FindPlayer(out int x, out int y)
		{
			x = (y = 0);
			for (int i = 0; i < Rows; i++)
			{
				for (int j = 0; j < Columns; j++)
				{
					if (IsPlayer((Block)_grid[i, j]))
					{
						y = i;
						x = j;
						return;
					}
				}
			}
		}

		public bool CanMoveTo(Block cell)
		{
			if (cell == Block.EMPTY || cell == Block.DEST || cell == Block.GOAL)
			{
				return true;
			}
			return false;
		}

		public bool IsPlayer(Block cell)
		{
			if (cell - 4 <= Block.BLOCK || cell == Block.PLAYER_GOAL)
			{
				return true;
			}
			return false;
		}

		public bool IsBlock(Block cell)
		{
			if (cell == Block.BLOCK || cell == Block.BLOCK_DEST || cell == Block.BLOCK_GOAL)
			{
				return true;
			}
			return false;
		}

		public bool IsDest(Block cell)
		{
			switch (cell)
			{
			case Block.DEST:
			case Block.PLAYER_DEST:
			case Block.GOAL:
			case Block.BLOCK_GOAL:
				return true;
			default:
				return false;
			}
		}

		public async void EndMinigameOnSuccess()
		{
			Game.Audio.PlaySystemSound("res://assets/sfx/minigame_fanfare.ogg");
			await GDUtil.DelayOneFrame();
			Game.Minigames.End(OnSuccessEvent);
		}

		private void CheckSuccess()
		{
			for (int i = 0; i < Rows; i++)
			{
				for (int j = 0; j < Columns; j++)
				{
					if (IsDest(GetCell(j, i)))
					{
						return;
					}
				}
			}
			Game.InputProcessor = Inputs.Processor.None;
			EndMinigameOnSuccess();
		}

		public Block GetCell(int x, int y)
		{
			return (Block)_grid[y, x];
		}

		public void MoveLeft(int x, int y)
		{
			Block cell = FindAtLeft(x, y);
			Block oldCell = MovePlayerOutOf(GetCell(x, y));
			Block newCell = MovePlayerInto(cell);
			Move(x, y, x - 1, y, oldCell, newCell);
		}

		public void MoveRight(int x, int y)
		{
			Block cell = FindAtRight(x, y);
			Block oldCell = MovePlayerOutOf(GetCell(x, y));
			Block newCell = MovePlayerInto(cell);
			Move(x, y, x + 1, y, oldCell, newCell);
		}

		public void MoveUp(int x, int y)
		{
			Block cell = FindAbove(x, y);
			Block oldCell = MovePlayerOutOf(GetCell(x, y));
			Block newCell = MovePlayerInto(cell);
			Move(x, y, x, y - 1, oldCell, newCell);
		}

		public void MoveDown(int x, int y)
		{
			Block cell = FindBelow(x, y);
			Block oldCell = MovePlayerOutOf(GetCell(x, y));
			Block newCell = MovePlayerInto(cell);
			Move(x, y, x, y + 1, oldCell, newCell);
		}

		public void MoveBoxLeft(int x, int y)
		{
			Game.Audio.PlaySfx("res://assets/sfx/minigame_block_slide.ogg");
			Block cell = FindAtLeft(x, y);
			Block oldCell = MoveBoxOutOf(GetCell(x, y));
			Block newCell = MoveBoxInto(cell);
			Move(x, y, x - 1, y, oldCell, newCell);
			MoveLeft(x + 1, y);
		}

		public void MoveBoxRight(int x, int y)
		{
			Game.Audio.PlaySfx("res://assets/sfx/minigame_block_slide.ogg");
			Block cell = FindAtRight(x, y);
			Block oldCell = MoveBoxOutOf(GetCell(x, y));
			Block newCell = MoveBoxInto(cell);
			Move(x, y, x + 1, y, oldCell, newCell);
			MoveRight(x - 1, y);
		}

		public void MoveBoxUp(int x, int y)
		{
			Game.Audio.PlaySfx("res://assets/sfx/minigame_block_slide.ogg");
			Block cell = FindAbove(x, y);
			Block oldCell = MoveBoxOutOf(GetCell(x, y));
			Block newCell = MoveBoxInto(cell);
			Move(x, y, x, y - 1, oldCell, newCell);
			MoveUp(x, y + 1);
		}

		public void MoveBoxDown(int x, int y)
		{
			Game.Audio.PlaySfx("res://assets/sfx/minigame_block_slide.ogg");
			Block cell = FindBelow(x, y);
			Block oldCell = MoveBoxOutOf(GetCell(x, y));
			Block newCell = MoveBoxInto(cell);
			Move(x, y, x, y + 1, oldCell, newCell);
			MoveDown(x, y - 1);
		}

		public void Move(int x, int y, int newX, int newY, Block oldCell, Block newCell)
		{
			_grid[y, x] = (byte)oldCell;
			_grid[newY, newX] = (byte)newCell;
		}

		private Block FindAtLeft(int x, int y)
		{
			if (x == 0)
			{
				return Block.FIXED;
			}
			return (Block)_grid[y, x - 1];
		}

		private Block FindAtRight(int x, int y)
		{
			if (x + 1 == Columns)
			{
				return Block.FIXED;
			}
			return (Block)_grid[y, x + 1];
		}

		private Block FindAbove(int x, int y)
		{
			if (y == 0)
			{
				return Block.FIXED;
			}
			return (Block)_grid[y - 1, x];
		}

		private Block FindBelow(int x, int y)
		{
			if (y + 1 == Rows)
			{
				return Block.FIXED;
			}
			return (Block)_grid[y + 1, x];
		}

		private Block MovePlayerOutOf(Block cell)
		{
			return cell switch
			{
				Block.PLAYER_DEST => Block.DEST, 
				Block.PLAYER_GOAL => Block.GOAL, 
				_ => Block.EMPTY, 
			};
		}

		private Block MovePlayerInto(Block cell)
		{
			return cell switch
			{
				Block.DEST => Block.PLAYER_DEST, 
				Block.GOAL => Block.PLAYER_GOAL, 
				_ => Block.PLAYER, 
			};
		}

		private Block MoveBoxOutOf(Block cell)
		{
			return cell switch
			{
				Block.BLOCK_DEST => Block.DEST, 
				Block.BLOCK_GOAL => Block.GOAL, 
				_ => Block.EMPTY, 
			};
		}

		private Block MoveBoxInto(Block cell)
		{
			return cell switch
			{
				Block.DEST => Block.BLOCK_DEST, 
				Block.GOAL => Block.BLOCK_GOAL, 
				_ => Block.BLOCK, 
			};
		}

		private void InitializeGrid()
		{
			_grid = new byte[Rows, Columns];
			for (int i = 0; i < Rows; i++)
			{
				for (int j = 0; j < Columns; j++)
				{
					_grid[i, j] = _startingGrid[i, j];
				}
			}
		}

		private void PaintAll()
		{
			for (int i = 0; i < Rows; i++)
			{
				for (int j = 0; j < Columns; j++)
				{
					Paint(j, i);
				}
			}
		}

		private byte[,] ParseGrid(string definitionGrid)
		{
			string[] lines = definitionGrid.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			byte[,] grid = new byte[Rows, Columns];
			for (int i = 0; i < lines.Length; i++)
			{
				for (int j = 0; j < lines[i].Length; j++)
				{
					grid[i, j] = byte.Parse(lines[i][j].ToString());
				}
			}
			return grid;
		}

		private void Paint(int x, int y)
		{
			Block block = (Block)_grid[y, x];
			TextureRect node = _cells[y, x];
			node.Clear();
			node.Texture = _textures[block];
			if (IsPlayer(block))
			{
				node.AddChild(MakePlayerTexture());
			}
		}

		private TextureRect MakePlayerTexture()
		{
			return new TextureRect
			{
				Expand = true,
				SizeFlagsHorizontal = 2,
				SizeFlagsVertical = 2,
				RectMinSize = CellSize,
				Texture = _textures[Block.PLAYER]
			};
		}
	}
}
