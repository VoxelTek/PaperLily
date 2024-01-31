using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;

namespace LacieEngine.Rooms
{
    // Token: 0x020000D5 RID: 213
    [Injectable]
    public class GameRoomManager : IGameRoomManager, IModule
    {
        // Token: 0x1700015D RID: 349
        // (get) Token: 0x060007D8 RID: 2008 RVA: 0x0001D3A6 File Offset: 0x0001B5A6
        // (set) Token: 0x060007D9 RID: 2009 RVA: 0x0001D3AE File Offset: 0x0001B5AE
        public IPlayer Player { get; private set; }

        // Token: 0x1700015E RID: 350
        // (get) Token: 0x060007DA RID: 2010 RVA: 0x0001D3B7 File Offset: 0x0001B5B7
        // (set) Token: 0x060007DB RID: 2011 RVA: 0x0001D3BF File Offset: 0x0001B5BF
        public GameRoom CurrentRoom { get; private set; }

        // Token: 0x1700015F RID: 351
        // (get) Token: 0x060007DC RID: 2012 RVA: 0x0001D3C8 File Offset: 0x0001B5C8
        // (set) Token: 0x060007DD RID: 2013 RVA: 0x0001D3D0 File Offset: 0x0001B5D0
        public IGameCamera Camera { get; private set; }

        // Token: 0x17000160 RID: 352
        // (get) Token: 0x060007DE RID: 2014 RVA: 0x0001D3D9 File Offset: 0x0001B5D9
        // (set) Token: 0x060007DF RID: 2015 RVA: 0x0001D3E1 File Offset: 0x0001B5E1
        public bool Cutscene { get; private set; }

        // Token: 0x17000161 RID: 353
        // (get) Token: 0x060007E0 RID: 2016 RVA: 0x0001D3EA File Offset: 0x0001B5EA
        // (set) Token: 0x060007E1 RID: 2017 RVA: 0x0001D406 File Offset: 0x0001B606
        public bool Visible {
            get {
                return this.nRoomContainer.IsValid() && this.nRoomContainer.Visible;
            }
            set {
                this.nRoomContainer.Visible = value;
            }
        }

        // Token: 0x17000162 RID: 354
        // (get) Token: 0x060007E2 RID: 2018 RVA: 0x0001D414 File Offset: 0x0001B614
        // (set) Token: 0x060007E3 RID: 2019 RVA: 0x0001D41C File Offset: 0x0001B61C
        public List<IReflectable> MirrorReflections { get; private set; } = new List<IReflectable>();

        // Token: 0x17000163 RID: 355
        // (get) Token: 0x060007E4 RID: 2020 RVA: 0x0001D425 File Offset: 0x0001B625
        public Dictionary<string, Node2D> RegisteredNPCs { get; } = new Dictionary<string, Node2D>(StringComparer.OrdinalIgnoreCase);

        // Token: 0x17000164 RID: 356
        // (get) Token: 0x060007E5 RID: 2021 RVA: 0x0001D42D File Offset: 0x0001B62D
        public Dictionary<string, SpawnPoint> RegisteredPoints { get; } = new Dictionary<string, SpawnPoint>(StringComparer.OrdinalIgnoreCase);

        // Token: 0x17000165 RID: 357
        // (get) Token: 0x060007E6 RID: 2022 RVA: 0x0001D435 File Offset: 0x0001B635
        public List<IAction> RegisteredRoomUpdates { get; } = new List<IAction>();

        // Token: 0x17000166 RID: 358
        // (get) Token: 0x060007E7 RID: 2023 RVA: 0x0001D43D File Offset: 0x0001B63D
        public Dictionary<string, IAction> RegisteredRoomActions { get; } = new Dictionary<string, IAction>();

        // Token: 0x17000167 RID: 359
        // (get) Token: 0x060007E8 RID: 2024 RVA: 0x0001D445 File Offset: 0x0001B645
        // (set) Token: 0x060007E9 RID: 2025 RVA: 0x0001D452 File Offset: 0x0001B652
        public Color Modulate {
            get {
                return this.nModulate.Color;
            }
            set {
                this.nModulate.Color = value;
                this.nBgModulate.Color = value;
            }
        }

        // Token: 0x060007EA RID: 2026 RVA: 0x0001D46C File Offset: 0x0001B66C
        public async Task ChangeRoom(string room, string point, Vector2 pos, string dir, float? time, bool cutscene)
        {
            Log.Info(new object[] { "Changing room -> ", room });
            Game.InputProcessor = Inputs.Processor.None;
            float fadeInOutTime = (time ?? 1f) / 2f;
            this.EnsureRoomMgrNodes();
            if (this.CurrentRoom.IsValid())
            {
                this.CurrentRoom._BeforeFadeOut();
                if (fadeInOutTime > 0f)
                {
                    await Game.Screen.ShowLoadingScreen(fadeInOutTime);
                }
                this.CurrentRoom._AfterFadeOut();
                this.nRoomContainer.Clear();
                this.Camera.Current = false;
            }
            this.Player = null;
            this.Camera = null;
            this.Cutscene = cutscene;
            this.nRoomContainer.Visible = true;
            this.MirrorReflections.Clear();
            this.RegisteredNPCs.Clear();
            this.RegisteredPoints.Clear();
            this.RegisteredRoomUpdates.Clear();
            this.RegisteredRoomActions.Clear();
            await this.LoadRoom(room, point, pos, Direction.FromString(dir));
            await GDUtil.DelayOneFrame();
            await this.FinishChangingRoom(fadeInOutTime);
        }

        // Token: 0x060007EB RID: 2027 RVA: 0x0001D4E4 File Offset: 0x0001B6E4
        public async Task ChangeRoom(string room, string point, Vector2 pos, string dir, float? time)
        {
            await this.ChangeRoom(room, point, pos, dir, time, false);
        }

        // Token: 0x060007EC RID: 2028 RVA: 0x0001D554 File Offset: 0x0001B754
        public async Task ChangeRoom(string room, string point, Vector2 pos, string dir)
        {
            await this.ChangeRoom(room, point, pos, dir, new float?(1f));
        }

        // Token: 0x060007ED RID: 2029 RVA: 0x0001D5B8 File Offset: 0x0001B7B8
        public void ApplyLighting(Resource lightingRes)
        {
            Lighting lighting = lightingRes as Lighting;
            if (lighting == null)
            {
                return;
            }
            this.Modulate = lighting.Modulate;
            lighting.ApplyToWorldEnvironment(this.nEnvironment);
            this.ApplyRoomShader(lighting.Material);
            if (Game.Player == null)
            {
                return;
            }
            if (lighting.PlayerLightNode != null)
            {
                Game.Player.SetLight(lighting.PlayerLightNode.Instance<Light2D>(PackedScene.GenEditState.Disabled));
                Game.Player.GetLight().Energy = lighting.PlayerLightEnergy;
                Game.Player.GetLight().Mode = lighting.PlayerLightMode.ToGodotLight2DMode();
            }
            else
            {
                Game.Player.SetLight(null);
            }
            this.SetPlayerLight(lighting.PlayerLightState);
        }

        // Token: 0x060007EE RID: 2030 RVA: 0x0001D662 File Offset: 0x0001B862
        public void ResetLighting()
        {
            this.ApplyLighting(GD.Load<Lighting>("res://resources/lighting/basic.tres"));
        }

        // Token: 0x060007EF RID: 2031 RVA: 0x0001D674 File Offset: 0x0001B874
        private void SetPlayerLight(bool on)
        {
            if (Game.Player == null || Game.Player.GetLight() == null)
            {
                return;
            }
            Game.Player.GetLight().Visible = on;
        }

        // Token: 0x060007F0 RID: 2032 RVA: 0x0001D69C File Offset: 0x0001B89C
        public void ApplyRoomShader(Material material)
        {
            this.nShader.DeleteIfValid();
            if (material == null)
            {
                return;
            }
            this.nShader = GDUtil.MakeNode<GameRoomManager.CameraTrackingShader>("Shader");
            this.nRoomContainer.AddChild(this.nShader, false);
            this.nShader.Visible = true;
            this.nShader.Material = material;
        }

        // Token: 0x060007F1 RID: 2033 RVA: 0x0001D6F2 File Offset: 0x0001B8F2
        public void RegisterMirrorReflection(IReflectable reflectable)
        {
            this.MirrorReflections.Add(reflectable);
        }

        // Token: 0x060007F2 RID: 2034 RVA: 0x0001D700 File Offset: 0x0001B900
        private async Task LoadRoom(string room, string point, Vector2 pos, Direction dir)
        {
            try
            {
                Game.Events.ClearMappings();
                Game.Memory.Cache("res://resources/nodes/rooms/" + room + ".tscn");
                Game.Events.LoadMappings(room);
                await Game.Memory.WaitForCompletion();
                this.CurrentRoom = GDUtil.Instance<GameRoom>("res://resources/nodes/rooms/" + room + ".tscn");
                if (this.CurrentRoom.Cutscene)
                {
                    this.Cutscene = true;
                }
                this.ResolveRoomInjections(this.CurrentRoom);
                this.nRoomContainer.AddChild(this.CurrentRoom, false);
                await GDUtil.DelayOneFrame();
                Game.State.Room = room;
                int layer = 1;
                this.CurrentRoom._Initialize();
                if (!point.IsNullOrEmpty())
                {
                    SpawnPoint spawnPoint = this.CurrentRoom.GetSpawnPoint(point);
                    pos = spawnPoint.Position;
                    layer = spawnPoint.Layer;
                    if (dir == Direction.None)
                    {
                        dir = spawnPoint.Direction;
                    }
                    Log.Debug(new object[]
                    {
                        "Using spawn point ",
                        point,
                        " at ",
                        pos.ToString()
                    });
                }
                GameCamera camera = new GameCamera();
                this.Camera = camera;
                if (this.Cutscene)
                {
                    camera.Position = pos;
                }
                this.CurrentRoom.AddChild(camera, false);
                camera.ApplyRoomSettings();
                camera.Current = true;
                this.CurrentRoom.ChangeLayer(layer);
                if (!this.Cutscene)
                {
                    if (Game.State.Party.IsEmpty<string>())
                    {
                        Game.State.Party.Add(Game.Settings.DefaultCharacter);
                    }
                    if (dir == Direction.None)
                    {
                        dir = Direction.Down;
                    }
                    IPlayer player = this.PreparePlayerCharacter(pos, dir, this.CurrentRoom);
                    this.CurrentRoom.GetMainLayer().AddChild(player.Node, false);
                    if (this.CurrentRoom.DisableRunning)
                    {
                        player.DisableRunning();
                    }
                    player.SneakingEnabled = this.CurrentRoom.EnableSneaking;
                    if (Game.State.Party.Count > 1 && !this.CurrentRoom.HideFollowers)
                    {
                        for (int i = 1; i < Game.State.Party.Count; i++)
                        {
                            if (dir != Direction.Up)
                            {
                                pos += new Vector2(0f, -1f);
                            }
                            NPCFollower follower = this.PrepareFollower(Game.State.Party[i], pos, dir, (float)(32 * i));
                            this.CurrentRoom.GetMainLayer().AddChild(follower, false);
                        }
                    }
                    camera.TrackNode(this.Player.Node);
                }
                GameRoom currentRoom = this.CurrentRoom;
                if (currentRoom.Lighting == null)
                {
                    currentRoom.Lighting = GD.Load<Lighting>("res://resources/lighting/basic.tres");
                }
                this.nBgColor.Modulate = this.CurrentRoom.Lighting.BackgroundColor;
                this.ApplyLighting(this.CurrentRoom.Lighting);
                if (!this.Cutscene)
                {
                    if (Game.State.OverrideBgm != null)
                    {
                        if (Game.State.OverrideBgm == "res://assets/bgm/silent.ogg")
                        {
                            Game.Audio.StopBgm();
                        }
                        else
                        {
                            Game.Audio.PlayBgm(GD.Load<AudioStream>(Game.State.OverrideBgm), this.CurrentRoom.BgmVolume);
                        }
                    }
                    else if (this.CurrentRoom.Bgm != null)
                    {
                        Game.Audio.PlayBgm(this.CurrentRoom.Bgm, this.CurrentRoom.BgmVolume, this.CurrentRoom.BgmCrossfade);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex, new object[] { "An error occurred while loading the room." });
            }
        }

        // Token: 0x060007F3 RID: 2035 RVA: 0x0001D764 File Offset: 0x0001B964
        private void EnsureRoomMgrNodes()
        {
            if (!this.nBgLayer.IsValid())
            {
                this.nBgColor = GDUtil.MakeNode<Sprite>("BgSprite");
                this.nBgColor.Texture = GD.Load<Texture>("res://assets/sprite/common/white.png");
                this.nBgColor.Scale = Game.Settings.BaseResolution * (float)Game.Settings.ResolutionScalePixel;
                this.nBgColor.Centered = false;
                this.nBgModulate = GDUtil.MakeNode<CanvasModulate>("Modulate");
                this.nBgLayer = GDUtil.MakeNode<CanvasLayer>("Background");
                this.nBgLayer.Layer = -10;
                this.nBgLayer.AddChild(this.nBgColor, false);
                this.nBgLayer.AddChild(this.nBgModulate, false);
                Game.Screen.AddToLayer(IScreenManager.Layer.Pixel, this.nBgLayer);
            }
            if (!this.nRoomContainer.IsValid())
            {
                this.nRoomContainer = GDUtil.MakeNode<Node2D>("Room");
                Game.Screen.AddToLayer(IScreenManager.Layer.Pixel, this.nRoomContainer);
            }
            if (!this.nEnvironment.IsValid())
            {
                this.nEnvironment = GDUtil.MakeWorldEnvironment();
                Game.Screen.AddToLayer(IScreenManager.Layer.Pixel, this.nEnvironment);
            }
            if (!this.nModulate.IsValid())
            {
                this.nModulate = GDUtil.MakeNode<CanvasModulate>("Modulate");
                Game.Screen.AddToLayer(IScreenManager.Layer.Pixel, this.nModulate);
            }
        }

        // Token: 0x060007F4 RID: 2036 RVA: 0x0001D8C0 File Offset: 0x0001BAC0
        private async Task FinishChangingRoom(float time)
        {
            try
            {
                this.CurrentRoom._BeforeFadeIn();
                this.UpdateRoomState();
                await Game.Screen.HideLoadingScreen(time);
                this.CurrentRoom._AfterFadeIn();
                this.CurrentRoom.Ready = true;
                if (!this.Cutscene)
                {
                    Game.Events.PlayOnEnterEvents();
                }
                if (Game.StoryPlayer.Running)
                {
                    this.ReturnToStoryPlayer();
                }
                else
                {
                    Game.InputProcessor = Inputs.Processor.Player;
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex, new object[] { "An error occurred while entering the room." });
            }
        }

        // Token: 0x060007F5 RID: 2037 RVA: 0x0001D90B File Offset: 0x0001BB0B
        private void ReturnToStoryPlayer()
        {
            Game.InputProcessor = Inputs.Processor.StoryPlayer;
            Game.StoryPlayer.SetNextBlockContinue();
            Game.StoryPlayer.Next();
        }

        // Token: 0x060007F6 RID: 2038 RVA: 0x0001D928 File Offset: 0x0001BB28
        private IPlayer PreparePlayerCharacter(Vector2 pos, Direction dir, GameRoom room)
        {
            IPlayer player;
            if (room.Type == "sidescroller")
            {
                player = GDUtil.Instance<PlayerSidescroller>("res://resources/nodes/common/PlayerSidescroller.tscn");
            }
            else
            {
                player = GDUtil.Instance<Player>("res://resources/nodes/common/Player.tscn");
            }
            this.Player = player;
            player.Node.Position = pos;
            player.SetDirection(dir);
            return player;
        }

        // Token: 0x060007F7 RID: 2039 RVA: 0x0001D97A File Offset: 0x0001BB7A
        private NPCFollower PrepareFollower(string characterId, Vector2 pos, Direction dir, float distanceToTarget)
        {
            return new NPCFollower(characterId, dir, distanceToTarget) {
                Position = pos
            };
        }

        // Token: 0x060007F8 RID: 2040 RVA: 0x0001D98C File Offset: 0x0001BB8C
        public void RoomFunction(string function)
        {
            if (this.CurrentRoom.HasMethod(function))
            {
                this.CurrentRoom.Call(function, Array.Empty<object>());
                return;
            }
            Log.Warn(new object[]
            {
                "Method ",
                function,
                "() not found in room: ",
                this.CurrentRoom.Name
            });
        }

        // Token: 0x060007F9 RID: 2041 RVA: 0x0001D9E8 File Offset: 0x0001BBE8
        public void UpdateRoomState()
        {
            this.RegisteredRoomUpdates.ForEach(delegate (IAction u) {
                u.Execute();
            });
            this.CurrentRoom._UpdateRoom();
            if (!Game.StoryPlayer.Running)
            {
                Game.Events.UpdateBannedEvents();
            }
        }

        // Token: 0x060007FA RID: 2042 RVA: 0x0001DA40 File Offset: 0x0001BC40
        public Node FindNodeInRoom(string nodeName)
        {
            return this.CurrentRoom.FindNodeInRoom(nodeName);
        }

        // Token: 0x060007FB RID: 2043 RVA: 0x0001DA4E File Offset: 0x0001BC4E
        public Path2D GetPath(string pathName)
        {
            Node node = this.CurrentRoom.GetNode("Points");
            return ((node != null) ? node.GetNode(pathName) : null) as Path2D;
        }

        // Token: 0x060007FC RID: 2044 RVA: 0x0001DA7C File Offset: 0x0001BC7C
        private void ResolveRoomInjections(GameRoom room)
        {
            if (room == null)
            {
                return;
            }
            Injector.Resolve(room);
            room.InjectNodes();
        }

        // Token: 0x060007FD RID: 2045 RVA: 0x0001DA8E File Offset: 0x0001BC8E
        public void Clean()
        {
            this.Player = null;
            this.Camera = null;
        }

        // Token: 0x060007FE RID: 2046 RVA: 0x0001DA9E File Offset: 0x0001BC9E
        public void RefreshPixelScaling()
        {
            if (this.nBgLayer.IsValid())
            {
                this.nBgColor.Scale = Game.Settings.BaseResolution * (float)Game.Settings.ResolutionScalePixel;
            }
        }

        // Token: 0x040005D9 RID: 1497
        public const float DEFAULT_ROOMCHANGE_TIME = 1f;

        // Token: 0x040005DA RID: 1498
        private Node2D nRoomContainer;

        // Token: 0x040005DB RID: 1499
        private WorldEnvironment nEnvironment;

        // Token: 0x040005DC RID: 1500
        private CanvasModulate nModulate;

        // Token: 0x040005DD RID: 1501
        private CanvasLayer nBgLayer;

        // Token: 0x040005DE RID: 1502
        private Sprite nBgColor;

        // Token: 0x040005DF RID: 1503
        private CanvasModulate nBgModulate;

        // Token: 0x040005E0 RID: 1504
        private GameRoomManager.CameraTrackingShader nShader;

        // Token: 0x02000256 RID: 598
        private class CameraTrackingShader : Sprite
        {
            // Token: 0x06001247 RID: 4679 RVA: 0x0003EDEF File Offset: 0x0003CFEF
            public override void _Ready()
            {
                base.Scale = Game.Settings.BaseResolution;
                base.Texture = GD.Load<Texture>("res://assets/sprite/common/white.png");
                base.Centered = false;
            }

            // Token: 0x06001248 RID: 4680 RVA: 0x0003EE18 File Offset: 0x0003D018
            public override void _Process(float delta)
            {
                if (base.Visible && Game.Camera.Node.IsValid())
                {
                    base.Position = Game.Camera.Node.GetCameraScreenCenter() - Game.Settings.BaseResolution / 2f;
                }
            }
        }
    }
}
