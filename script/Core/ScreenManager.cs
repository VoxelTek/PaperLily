using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using LacieEngine.Animation;
using LacieEngine.API;
using LacieEngine.UI;

namespace LacieEngine.Core
{
	// Token: 0x020001CA RID: 458
	[Injectable]
	public class ScreenManager : Control, IScreenManager, IModule
	{
		// Token: 0x0600102E RID: 4142 RVA: 0x0003B578 File Offset: 0x00039778
		public void Init()
		{
			if (base.IsInsideTree())
			{
				return;
			}
			base.Name = "Screen";
			base.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide, Control.LayoutPresetMode.Minsize, 0);
			this.nPixelLayer = GDUtil.MakeNode<Viewport>("PixelLayer");
			this.nPixelLayer.Size = Game.Settings.BaseResolution * (float)Game.Settings.ResolutionScalePixel;
			this.nPixelLayer.HandleInputLocally = false;
			this.nPixelLayer.RenderTargetUpdateMode = Viewport.UpdateMode.Always;
			this.nPixelLayer.OwnWorld = true;
			this.nPixelLayer.GetTexture().Flags = 4U;
			this.nPixelLayerDisplay = GDUtil.MakeNode<TextureRect>("PixelLayerDisplay");
			this.nPixelLayerDisplay.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide, Control.LayoutPresetMode.Minsize, 0);
			this.nPixelLayerDisplay.Texture = this.nPixelLayer.GetTexture();
			this.nPixelLayerDisplay.Expand = true;
			this.nPixelLayerDisplay.Material = GD.Load<Material>("res://resources/material/pixel_layer.tres");
			this.nPixelLayerDisplay.FlipV = true;
			this.UpdateShaders();
			this.nScreenDarkener = GDUtil.MakeNode<ColorRect>("ScreenDarkener");
			this.nScreenDarkener.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide, Control.LayoutPresetMode.Minsize, 0);
			this.nScreenDarkener.Visible = false;
			this.nScreenDarkener.Color = UIUtil.ScreenDarkenerColor;
			Control nMinigameLayer = GDUtil.MakeNode<Control>("MinigameLayer");
			nMinigameLayer.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide, Control.LayoutPresetMode.Minsize, 0);
			Control nStoryPlayerLayer = GDUtil.MakeNode<Control>("StoryPlayerLayer");
			nStoryPlayerLayer.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide, Control.LayoutPresetMode.Minsize, 0);
			Control nUiLayer = GDUtil.MakeNode<Control>("UILayer");
			nUiLayer.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide, Control.LayoutPresetMode.Minsize, 0);
			Control nScreenLayer = GDUtil.MakeNode<Control>("ScreenLayer");
			nScreenLayer.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide, Control.LayoutPresetMode.Minsize, 0);
			this.nBlackOverlay = UIUtil.CreateOverlay(Colors.Black);
			this.nBlackOverlay.Visible = false;
			Control nFlyoutLayer = GDUtil.MakeNode<Control>("FlyoutLayer");
			nFlyoutLayer.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide, Control.LayoutPresetMode.Minsize, 0);
			Control nHudLayer = GDUtil.MakeNode<Control>("HUDLayer");
			nHudLayer.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide, Control.LayoutPresetMode.Minsize, 0);
			this.nFPS = new FpsLabel();
			this.layers = new Dictionary<IScreenManager.Layer, Node>();
			this.layers[IScreenManager.Layer.Pixel] = this.nPixelLayer;
			this.layers[IScreenManager.Layer.Minigame] = nMinigameLayer;
			this.layers[IScreenManager.Layer.StoryPlayer] = nStoryPlayerLayer;
			this.layers[IScreenManager.Layer.UI] = nUiLayer;
			this.layers[IScreenManager.Layer.Screen] = nScreenLayer;
			this.layers[IScreenManager.Layer.Flyout] = nFlyoutLayer;
			this.layers[IScreenManager.Layer.HUD] = nHudLayer;
			base.AddChild(this.nPixelLayer, false);
			base.AddChild(this.nPixelLayerDisplay, false);
			base.AddChild(this.nScreenDarkener, false);
			base.AddChild(this.layers[IScreenManager.Layer.Minigame], false);
			base.AddChild(this.layers[IScreenManager.Layer.StoryPlayer], false);
			base.AddChild(this.layers[IScreenManager.Layer.UI], false);
			base.AddChild(this.layers[IScreenManager.Layer.Screen], false);
			base.AddChild(this.nBlackOverlay, false);
			base.AddChild(this.layers[IScreenManager.Layer.Flyout], false);
			base.AddChild(this.layers[IScreenManager.Layer.HUD], false);
			if (OS.IsDebugBuild())
			{
				base.AddChild(this.nFPS, false);
			}
			Game.Root.AddChild(this, false);
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x0003B88C File Offset: 0x00039A8C
		public override void _Input(InputEvent @event)
		{
			if (Game.InputProcessor != Inputs.Processor.None)
			{
				this.nPixelLayer.Input(@event);
			}
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x0003B8A4 File Offset: 0x00039AA4
		public void TakeScreenshot()
		{
			if (Game.Settings.SteamBuild)
			{
				return;
			}
			Image data = base.GetTree().Root.GetTexture().GetData();
			data.FlipY();
			string text = "user://screenshot/";
			GDUtil.EnsureFolderExists(text);
			string datetime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss.fffffff");
			string filename = text + "Screenshot-" + datetime + ".png";
			if (data.SavePng(filename) == Error.Ok)
			{
				Log.Info(new object[] { "Screenshot saved as ", filename });
			}
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x0003B927 File Offset: 0x00039B27
		public void ToggleFpsMeter()
		{
			if (this.nFPS.IsValid())
			{
				this.nFPS.Visible = !this.nFPS.Visible;
			}
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x0003B94F File Offset: 0x00039B4F
		public void AddToLayer(IScreenManager.Layer layer, Node node)
		{
			this.layers[layer].AddChild(node, false);
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x0003B964 File Offset: 0x00039B64
		public Node GetFromLayer(IScreenManager.Layer layer, string path)
		{
			return this.layers[layer].GetNodeOrNull(path);
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x0003B980 File Offset: 0x00039B80
		public void Clean()
		{
			this.layers[IScreenManager.Layer.Pixel].Clear();
			this.layers[IScreenManager.Layer.StoryPlayer].Clear();
			this.layers[IScreenManager.Layer.Minigame].Clear();
			this.layers[IScreenManager.Layer.Screen].Clear();
			this.layers[IScreenManager.Layer.UI].Clear();
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x0003B9E4 File Offset: 0x00039BE4
		public void UpdateShaders()
		{
			ShaderMaterial shaderMaterial = this.nPixelLayerDisplay.Material as ShaderMaterial;
			shaderMaterial.SetShaderParam("brightness", decimal.ToSingle(Game.Settings.Brightness));
			shaderMaterial.SetShaderParam("contrast", decimal.ToSingle(Game.Settings.Contrast));
			shaderMaterial.SetShaderParam("saturation", decimal.ToSingle(Game.Settings.Saturation));
			shaderMaterial.SetShaderParam("gamma", decimal.ToSingle(Game.Settings.Gamma));
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x0003BA7C File Offset: 0x00039C7C
		public void OpenMenu()
		{
			if (this.layers[IScreenManager.Layer.UI].HasNode("GameMenu"))
			{
				return;
			}
			Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
			Game.InputProcessor = Inputs.Processor.Menu;
			GameMenuContainer gameMenu = new GameMenuContainer();
			gameMenu.OnClose = delegate {
				this.CloseMenu();
			};
			this.layers[IScreenManager.Layer.UI].AddChild(gameMenu, false);
			this.layers[IScreenManager.Layer.Flyout].Clear();
			this.pendingFlyouts.Clear();
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x0003BB04 File Offset: 0x00039D04
		public void CloseMenu()
		{
			if (!this.layers[IScreenManager.Layer.UI].HasNode("GameMenu"))
			{
				return;
			}
			Game.InputProcessor = Inputs.Processor.Player;
			this.layers[IScreenManager.Layer.UI].GetNode("GameMenu").Delete();
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0003BB58 File Offset: 0x00039D58
		public async Task FadeToBlack(float time)
		{
			if (!this.nBlackOverlay.Visible)
			{
				this.nBlackOverlay.Visible = true;
				FadeAnimation fadeIn = new FadeInAnimation(this.nBlackOverlay, time);
				Game.Animations.Play(fadeIn);
				await fadeIn.WaitUntilFinished();
			}
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x0003BBA4 File Offset: 0x00039DA4
		public async Task FadeToBlack()
		{
			await this.FadeToBlack(0.5f);
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x0003BBE8 File Offset: 0x00039DE8
		public async Task FadeFromBlack(float time)
		{
			if (this.nBlackOverlay.Visible)
			{
				FadeAnimation fadeOut = new FadeOutAnimation(this.nBlackOverlay, time);
				Game.Animations.Play(fadeOut);
				await fadeOut.WaitUntilFinished();
				this.nBlackOverlay.Visible = false;
			}
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x0003BC34 File Offset: 0x00039E34
		public async Task FadeFromBlack()
		{
			await this.FadeFromBlack(0.5f);
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x0003BC78 File Offset: 0x00039E78
		public async Task ShowLoadingScreen(float time)
		{
			await this.FadeToBlack(time);
			this.nLoadingScreen = Game.Settings.SceneProvider.LoadingScreen.Instance<LoadingScreen>(PackedScene.GenEditState.Disabled);
			base.AddChild(this.nLoadingScreen, false);
			await GDUtil.DelayOneFrame();
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x0003BCC4 File Offset: 0x00039EC4
		public async Task ShowLoadingScreen()
		{
			await this.ShowLoadingScreen(0.5f);
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x0003BD08 File Offset: 0x00039F08
		public async Task ShowLoadingScreenInstantly()
		{
			this.nBlackOverlay.Visible = true;
			this.nLoadingScreen = Game.Settings.SceneProvider.LoadingScreen.Instance<LoadingScreen>(PackedScene.GenEditState.Disabled);
			this.nLoadingScreen.Instant = true;
			base.AddChild(this.nLoadingScreen, false);
			await GDUtil.DelayOneFrame();
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x0003BD4C File Offset: 0x00039F4C
		public async Task HideLoadingScreen(float time)
		{
			this.nLoadingScreen.DeleteIfValid();
			this.nLoadingScreen = null;
			await this.FadeFromBlack(time);
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x0003BD98 File Offset: 0x00039F98
		public async Task HideLoadingScreen()
		{
			await this.HideLoadingScreen(0.5f);
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x0003BDDB File Offset: 0x00039FDB
		public void DarkenScreen()
		{
			this.nScreenDarkener.Visible = true;
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x0003BDE9 File Offset: 0x00039FE9
		public void UndarkenScreen()
		{
			this.nScreenDarkener.Visible = false;
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x0003BDF8 File Offset: 0x00039FF8
		public async void ShowSaving()
		{
			Control savingScreen = Game.Settings.SceneProvider.SavingScreen.Instance<Control>(PackedScene.GenEditState.Disabled);
			base.AddChild(savingScreen, false);
			await DrkieUtil.DelaySeconds(2.0);
			savingScreen.Delete();
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x0003BE30 File Offset: 0x0003A030
		public async Task ShowFlyout(Control flyout, float duration)
		{
			if (this.flyoutInProgress)
			{
				this.pendingFlyouts.Enqueue(new Tuple<Control, float>(flyout, duration));
			}
			else
			{
				try
				{
					this.flyoutInProgress = true;
					Control nFlyoutLayer = (Control)this.layers[IScreenManager.Layer.Flyout];
					nFlyoutLayer.AddChild(flyout, false);
					new Vector2(Game.Settings.BaseResolution.x, 15f);
					Vector2 to = new Vector2(Game.Settings.BaseResolution.x - flyout.RectSize.x - 15f, 15f);
					LacieAnimation slideIn = new SlideInRightAnimation(nFlyoutLayer, new float?(flyout.RectSize.x - 15f), new Vector2?(to), 0.3f);
					LacieAnimation slideOut = new SlideOutRightAnimation(nFlyoutLayer, new float?(flyout.RectSize.x - 15f), new Vector2?(to), 0.3f);
					Game.Audio.PlaySystemSound("res://assets/sfx/ui_objective.ogg");
					Game.Animations.Play(slideIn);
					await slideIn.WaitUntilFinished();
					await DrkieUtil.DelaySeconds((double)duration);
					Game.Animations.Play(slideOut);
					await slideOut.WaitUntilFinished();
					flyout.DeleteIfValid();
					slideOut = null;
				}
				finally
				{
					this.flyoutInProgress = false;
					if (this.pendingFlyouts.Count > 0)
					{
						Tuple<Control, float> nextFlyout = this.pendingFlyouts.Dequeue();
						this.ShowFlyout(nextFlyout.Item1, nextFlyout.Item2);
					}
				}
			}
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x0003BE84 File Offset: 0x0003A084
		public async Task ShowFlyout(Control flyout)
		{
			await this.ShowFlyout(flyout, 3f);
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x0003BECF File Offset: 0x0003A0CF
		public void RefreshPixelScaling()
		{
			this.nPixelLayer.Size = Game.Settings.BaseResolution * (float)Game.Settings.ResolutionScalePixel;
		}

		// Token: 0x04000BA2 RID: 2978
		private const int FLYOUT_MARGIN_TOP = 15;

		// Token: 0x04000BA3 RID: 2979
		private const int FLYOUT_MARGIN_RIGHT = 15;

		// Token: 0x04000BA4 RID: 2980
		private const float FLYOUT_ANIMATION_TIME = 0.3f;

		// Token: 0x04000BA5 RID: 2981
		private const float DEFAULT_FLYOUT_DURATION_TIME = 3f;

		// Token: 0x04000BA6 RID: 2982
		private const float DEFAULT_FADE_TIME = 0.5f;

		// Token: 0x04000BA7 RID: 2983
		private TextureRect nPixelLayerDisplay;

		// Token: 0x04000BA8 RID: 2984
		private Viewport nPixelLayer;

		// Token: 0x04000BA9 RID: 2985
		private ColorRect nScreenDarkener;

		// Token: 0x04000BAA RID: 2986
		private Control nBlackOverlay;

		// Token: 0x04000BAB RID: 2987
		private LoadingScreen nLoadingScreen;

		// Token: 0x04000BAC RID: 2988
		private FpsLabel nFPS;

		// Token: 0x04000BAD RID: 2989
		private Dictionary<IScreenManager.Layer, Node> layers;

		// Token: 0x04000BAE RID: 2990
		private bool flyoutInProgress;

		// Token: 0x04000BAF RID: 2991
		private Queue<Tuple<Control, float>> pendingFlyouts = new Queue<Tuple<Control, float>>();
	}
}
