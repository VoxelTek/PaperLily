using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;

namespace LacieEngine.Rooms
{
	[Tool]
	[ExportType(icon = "room")]
	public class GameRoom : Node2D, INodeWithInjections, IInspectorCustomizer
	{
		[Export(PropertyHint.None, "")]
		public bool Cutscene;

		[Export(PropertyHint.None, "")]
		public Lighting Lighting;

		[Export(PropertyHint.Range, "0.1,3.0")]
		public float CameraZoom = 1f;

		[Export(PropertyHint.None, "")]
		public int CameraLimitLeft = -10000000;

		[Export(PropertyHint.None, "")]
		public int CameraLimitTop = -10000000;

		[Export(PropertyHint.None, "")]
		public int CameraLimitRight = 10000000;

		[Export(PropertyHint.None, "")]
		public int CameraLimitBottom = 10000000;

		[Export(PropertyHint.None, "")]
		public AudioStream Bgm;

		[Export(PropertyHint.Range, "0,1")]
		public float BgmVolume = 1f;

		[Export(PropertyHint.None, "")]
		public bool BgmCrossfade;

		[Export(PropertyHint.Enum, "default,sidescroller")]
		public string Type = "default";

		[Export(PropertyHint.None, "")]
		public bool DisableRunning;

		[Export(PropertyHint.None, "")]
		public bool EnableSneaking;

		[Export(PropertyHint.None, "")]
		public bool HideFollowers;

		[Export(PropertyHint.None, "")]
		public string SaveLocation = "";

		[Export(PropertyHint.None, "")]
		public string SaveImage = "";

		public bool Ready { get; set; }

		public sealed override void _Ready()
		{
		}

		public sealed override void _EnterTree()
		{
			if (Engine.EditorHint)
			{
				SetProcess(enable: false);
			}
		}

		public sealed override void _Process(float delta)
		{
			if (Ready)
			{
				_RoomProcess(delta);
			}
		}

		public sealed override void _PhysicsProcess(float delta)
		{
		}

		public virtual void _Initialize()
		{
		}

		public virtual void _BeforeFadeIn()
		{
		}

		public virtual void _BeforeFadeOut()
		{
		}

		public virtual void _AfterFadeIn()
		{
		}

		public virtual void _AfterFadeOut()
		{
		}

		public virtual void _UpdateRoom()
		{
		}

		public virtual void _RoomProcess(float delta)
		{
		}

		public virtual Node2D GetMainLayer()
		{
			return GetNode<Node2D>("Main");
		}

		public virtual Node FindNodeInRoom(string nodeName)
		{
			if (Game.Room.RegisteredNPCs.ContainsKey(nodeName.ToLower()))
			{
				return Game.Room.RegisteredNPCs[nodeName.ToLower()];
			}
			if (Game.Room.RegisteredPoints.ContainsKey(nodeName.ToLower()))
			{
				return Game.Room.RegisteredPoints[nodeName.ToLower()];
			}
			if (GetMainLayer().HasNode(nodeName))
			{
				return GetMainLayer().GetNode(nodeName);
			}
			if (HasNode("Background/" + nodeName))
			{
				return GetNode("Background/" + nodeName);
			}
			if (HasNode("Foreground/" + nodeName))
			{
				return GetNode("Foreground/" + nodeName);
			}
			if (HasNode("Events/" + nodeName))
			{
				return GetNode("Events/" + nodeName);
			}
			return null;
		}

		public virtual SpawnPoint GetSpawnPoint(string pointName)
		{
			if (Game.Room.RegisteredPoints.ContainsKey(pointName))
			{
				return Game.Room.RegisteredPoints[pointName];
			}
			Log.Warn("Registered point not found: ", pointName);
			return null;
		}

		public virtual Vector2 GetPoint(string pointName)
		{
			return GetSpawnPoint(pointName)?.Position ?? Vector2.Zero;
		}

		public virtual void ChangeLayer(int newLayer)
		{
		}
	}
}
