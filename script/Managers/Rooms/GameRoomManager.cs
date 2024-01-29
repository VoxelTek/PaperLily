using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;

namespace LacieEngine.Rooms
{
	[Injectable]
	public class GameRoomManager : IGameRoomManager, IModule
	{
		private class CameraTrackingShader : Sprite
		{
			public override void _Ready()
			{
				base.Scale = Game.Settings.BaseResolution;
				base.Texture = GD.Load<Texture>("res://assets/sprite/common/white.png");
				base.Centered = false;
			}

			public override void _Process(float delta)
			{
				if (base.Visible && Game.Camera.Node.IsValid())
				{
					base.Position = Game.Camera.Node.GetCameraScreenCenter() - Game.Settings.BaseResolution / 2f;
				}
			}
		}

		public const float DEFAULT_ROOMCHANGE_TIME = 1f;

		private Node2D nRoomContainer;

		private WorldEnvironment nEnvironment;

		private CanvasModulate nModulate;

		private CanvasLayer nBgLayer;

		private Sprite nBgColor;

		private CanvasModulate nBgModulate;

		private CameraTrackingShader nShader;

		public IPlayer Player { get; private set; }

		public GameRoom CurrentRoom { get; private set; }

		public IGameCamera Camera { get; private set; }

		public bool Cutscene { get; private set; }

		public bool Visible
		{
			get
			{
				if (nRoomContainer.IsValid())
				{
					return nRoomContainer.Visible;
				}
				return false;
			}
			set
			{
				nRoomContainer.Visible = value;
			}
		}

		public List<IReflectable> MirrorReflections { get; private set; } = new List<IReflectable>();


		public Dictionary<string, Node2D> RegisteredNPCs { get; } = new Dictionary<string, Node2D>(StringComparer.OrdinalIgnoreCase);


		public Dictionary<string, SpawnPoint> RegisteredPoints { get; } = new Dictionary<string, SpawnPoint>(StringComparer.OrdinalIgnoreCase);


		public List<IAction> RegisteredRoomUpdates { get; } = new List<IAction>();


		public Dictionary<string, IAction> RegisteredRoomActions { get; } = new Dictionary<string, IAction>();


		public Color Modulate
		{
			get
			{
				return nModulate.Color;
			}
			set
			{
				nModulate.Color = value;
				nBgModulate.Color = value;
			}
		}

		public async Task ChangeRoom(string room, string point, Vector2 pos, string dir, float? time, bool cutscene)
		{
			Log.Info("Changing room -> ", room);
			Game.InputProcessor = Inputs.Processor.None;
			float fadeInOutTime = (time ?? 1f) / 2f;
			EnsureRoomMgrNodes();
			if (CurrentRoom.IsValid())
			{
				CurrentRoom._BeforeFadeOut();
				if (fadeInOutTime > 0f)
				{
					await Game.Screen.ShowLoadingScreen(fadeInOutTime);
				}
				CurrentRoom._AfterFadeOut();
				nRoomContainer.Clear();
				Camera.Current = false;
			}
			Player = null;
			Camera = null;
			Cutscene = cutscene;
			nRoomContainer.Visible = true;
			MirrorReflections.Clear();
			RegisteredNPCs.Clear();
			RegisteredPoints.Clear();
			RegisteredRoomUpdates.Clear();
			RegisteredRoomActions.Clear();
			await LoadRoom(room, point, pos, Direction.FromString(dir));
			await GDUtil.DelayOneFrame();
			await FinishChangingRoom(fadeInOutTime);
		}

		public async Task ChangeRoom(string room, string point, Vector2 pos, string dir, float? time)
		{
			await ChangeRoom(room, point, pos, dir, time, cutscene: false);
		}

		public async Task ChangeRoom(string room, string point, Vector2 pos, string dir)
		{
			await ChangeRoom(room, point, pos, dir, 1f);
		}

		public void ApplyLighting(Resource lightingRes)
		{
			if (!(lightingRes is Lighting lighting))
			{
				return;
			}
			Modulate = lighting.Modulate;
			lighting.ApplyToWorldEnvironment(nEnvironment);
			ApplyRoomShader(lighting.Material);
			if (Game.Player != null)
			{
				if (lighting.PlayerLightNode != null)
				{
					Game.Player.SetLight(lighting.PlayerLightNode.Instance<Light2D>());
					Game.Player.GetLight().Energy = lighting.PlayerLightEnergy;
					Game.Player.GetLight().Mode = lighting.PlayerLightMode.ToGodotLight2DMode();
				}
				else
				{
					Game.Player.SetLight(null);
				}
				SetPlayerLight(lighting.PlayerLightState);
			}
		}

		public void ResetLighting()
		{
			ApplyLighting(GD.Load<Lighting>("res://resources/lighting/basic.tres"));
		}

		private void SetPlayerLight(bool on)
		{
			if (Game.Player != null && Game.Player.GetLight() != null)
			{
				Game.Player.GetLight().Visible = on;
			}
		}

		public void ApplyRoomShader(Material material)
		{
			nShader.DeleteIfValid();
			if (material != null)
			{
				nShader = GDUtil.MakeNode<CameraTrackingShader>("Shader");
				nRoomContainer.AddChild(nShader);
				nShader.Visible = true;
				nShader.Material = material;
			}
		}

		public void RegisterMirrorReflection(IReflectable reflectable)
		{
			MirrorReflections.Add(reflectable);
		}

		private async Task LoadRoom(string room, string point, Vector2 pos, Direction dir)
		{
			_ = 1;
			try
			{
				Game.Events.ClearMappings();
				Game.Memory.Cache("res://resources/nodes/rooms/" + room + ".tscn");
				Game.Events.LoadMappings(room);
				await Game.Memory.WaitForCompletion();
				CurrentRoom = GDUtil.Instance<GameRoom>("res://resources/nodes/rooms/" + room + ".tscn");
				if (CurrentRoom.Cutscene)
				{
					Cutscene = true;
				}
				ResolveRoomInjections(CurrentRoom);
				nRoomContainer.AddChild(CurrentRoom);
				await GDUtil.DelayOneFrame();
				Game.State.Room = room;
				int layer = 1;
				CurrentRoom._Initialize();
				if (!point.IsNullOrEmpty())
				{
					SpawnPoint spawnPoint = CurrentRoom.GetSpawnPoint(point);
					pos = spawnPoint.Position;
					layer = spawnPoint.Layer;
					if (dir == Direction.None)
					{
						dir = spawnPoint.Direction;
					}
					Log.Debug("Using spawn point ", point, " at ", pos.ToString());
				}
				GameCamera camera = (GameCamera)(Camera = new GameCamera());
				if (Cutscene)
				{
					camera.Position = pos;
				}
				CurrentRoom.AddChild(camera);
				camera.ApplyRoomSettings();
				camera.Current = true;
				CurrentRoom.ChangeLayer(layer);
				if (!Cutscene)
				{
					if (Game.State.Party.IsEmpty())
					{
						Game.State.Party.Add(Game.Settings.DefaultCharacter);
					}
					if (dir == Direction.None)
					{
						dir = Direction.Down;
					}
					IPlayer player = PreparePlayerCharacter(pos, dir, CurrentRoom);
					CurrentRoom.GetMainLayer().AddChild(player.Node);
					if (CurrentRoom.DisableRunning)
					{
						player.DisableRunning();
					}
					player.SneakingEnabled = CurrentRoom.EnableSneaking;
					if (Game.State.Party.Count > 1 && !CurrentRoom.HideFollowers)
					{
						for (int i = 1; i < Game.State.Party.Count; i++)
						{
							if (dir != Direction.Up)
							{
								pos += new Vector2(0f, -1f);
							}
							NPCFollower follower = PrepareFollower(Game.State.Party[i], pos, dir, 32 * i);
							CurrentRoom.GetMainLayer().AddChild(follower);
						}
					}
					camera.TrackNode(Player.Node);
				}
				GameRoom currentRoom = CurrentRoom;
				if (currentRoom.Lighting == null)
				{
					currentRoom.Lighting = GD.Load<Lighting>("res://resources/lighting/basic.tres");
				}
				nBgColor.Modulate = CurrentRoom.Lighting.BackgroundColor;
				ApplyLighting(CurrentRoom.Lighting);
				if (Cutscene)
				{
					return;
				}
				if (Game.State.OverrideBgm != null)
				{
					if (Game.State.OverrideBgm == "res://assets/bgm/silent.ogg")
					{
						Game.Audio.StopBgm();
					}
					else
					{
						Game.Audio.PlayBgm(GD.Load<AudioStream>(Game.State.OverrideBgm), CurrentRoom.BgmVolume);
					}
				}
				else if (CurrentRoom.Bgm != null)
				{
					Game.Audio.PlayBgm(CurrentRoom.Bgm, CurrentRoom.BgmVolume, CurrentRoom.BgmCrossfade);
				}
			}
			catch (Exception exception)
			{
				Log.Exception(exception, "An error occurred while loading the room.");
			}
		}

		private void EnsureRoomMgrNodes()
		{
			if (!nBgLayer.IsValid())
			{
				nBgColor = GDUtil.MakeNode<Sprite>("BgSprite");
				nBgColor.Texture = GD.Load<Texture>("res://assets/sprite/common/white.png");
				nBgColor.Scale = Game.Settings.BaseResolution * Game.Settings.ResolutionScalePixel;
				nBgColor.Centered = false;
				nBgModulate = GDUtil.MakeNode<CanvasModulate>("Modulate");
				nBgLayer = GDUtil.MakeNode<CanvasLayer>("Background");
				nBgLayer.Layer = -10;
				nBgLayer.AddChild(nBgColor);
				nBgLayer.AddChild(nBgModulate);
				Game.Screen.AddToLayer(IScreenManager.Layer.Pixel, nBgLayer);
			}
			if (!nRoomContainer.IsValid())
			{
				nRoomContainer = GDUtil.MakeNode<Node2D>("Room");
				Game.Screen.AddToLayer(IScreenManager.Layer.Pixel, nRoomContainer);
			}
			if (!nEnvironment.IsValid())
			{
				nEnvironment = GDUtil.MakeWorldEnvironment();
				Game.Screen.AddToLayer(IScreenManager.Layer.Pixel, nEnvironment);
			}
			if (!nModulate.IsValid())
			{
				nModulate = GDUtil.MakeNode<CanvasModulate>("Modulate");
				Game.Screen.AddToLayer(IScreenManager.Layer.Pixel, nModulate);
			}
		}

		private async Task FinishChangingRoom(float time)
		{
			try
			{
				CurrentRoom._BeforeFadeIn();
				UpdateRoomState();
				await Game.Screen.HideLoadingScreen(time);
				CurrentRoom._AfterFadeIn();
				CurrentRoom.Ready = true;
				if (!Cutscene)
				{
					Game.Events.PlayOnEnterEvents();
				}
				if (Game.StoryPlayer.Running)
				{
					ReturnToStoryPlayer();
				}
				else
				{
					Game.InputProcessor = Inputs.Processor.Player;
				}
			}
			catch (Exception exception)
			{
				Log.Exception(exception, "An error occurred while entering the room.");
			}
		}

		private void ReturnToStoryPlayer()
		{
			Game.InputProcessor = Inputs.Processor.StoryPlayer;
			Game.StoryPlayer.SetNextBlockContinue();
			Game.StoryPlayer.Next();
		}

		private IPlayer PreparePlayerCharacter(Vector2 pos, Direction dir, GameRoom room)
		{
			IPlayer player = (Player = ((!(room.Type == "sidescroller")) ? GDUtil.Instance<Player>("res://resources/nodes/common/Player.tscn") : GDUtil.Instance<PlayerSidescroller>("res://resources/nodes/common/PlayerSidescroller.tscn")));
			player.Node.Position = pos;
			player.SetDirection(dir);
			return player;
		}

		private NPCFollower PrepareFollower(string characterId, Vector2 pos, Direction dir, float distanceToTarget)
		{
			return new NPCFollower(characterId, dir, distanceToTarget)
			{
				Position = pos
			};
		}

		public void RoomFunction(string function)
		{
			if (CurrentRoom.HasMethod(function))
			{
				CurrentRoom.Call(function);
				return;
			}
			Log.Warn("Method ", function, "() not found in room: ", CurrentRoom.Name);
		}

		public void UpdateRoomState()
		{
			RegisteredRoomUpdates.ForEach(delegate(IAction u)
			{
				u.Execute();
			});
			CurrentRoom._UpdateRoom();
			if (!Game.StoryPlayer.Running)
			{
				Game.Events.UpdateBannedEvents();
			}
		}

		public Node FindNodeInRoom(string nodeName)
		{
			return CurrentRoom.FindNodeInRoom(nodeName);
		}

		public Path2D GetPath(string pathName)
		{
			return CurrentRoom.GetNode("Points")?.GetNode(pathName) as Path2D;
		}

		private void ResolveRoomInjections(GameRoom room)
		{
			if (room != null)
			{
				Injector.Resolve(room);
				room.InjectNodes();
			}
		}

		public void Clean()
		{
			Player = null;
			Camera = null;
		}

		public void RefreshPixelScaling()
		{
			if (nBgLayer.IsValid())
			{
				nBgColor.Scale = Game.Settings.BaseResolution * Game.Settings.ResolutionScalePixel;
			}
		}
	}
}
