using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using LacieEngine.Animation;
using LacieEngine.API;
using LacieEngine.UI;

namespace LacieEngine.Core
{
	[Injectable]
	public class ScreenManager : Control, IScreenManager, IModule
	{
		private const int FLYOUT_MARGIN_TOP = 15;

		private const int FLYOUT_MARGIN_RIGHT = 15;

		private const float FLYOUT_ANIMATION_TIME = 0.3f;

		private const float DEFAULT_FLYOUT_DURATION_TIME = 3f;

		private const float DEFAULT_FADE_TIME = 0.5f;

		private TextureRect nPixelLayerDisplay;

		private Viewport nPixelLayer;

		private ColorRect nScreenDarkener;

		private Control nBlackOverlay;

		private LoadingScreen nLoadingScreen;

		private FpsLabel nFPS;

		private Dictionary<IScreenManager.Layer, Node> layers;

		private bool flyoutInProgress;

		private Queue<Tuple<Control, float>> pendingFlyouts = new Queue<Tuple<Control, float>>();

		public void Init()
		{
			if (!IsInsideTree())
			{
				base.Name = "Screen";
				SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
				nPixelLayer = GDUtil.MakeNode<Viewport>("PixelLayer");
				nPixelLayer.Size = Game.Settings.BaseResolution * Game.Settings.ResolutionScalePixel;
				nPixelLayer.HandleInputLocally = false;
				nPixelLayer.RenderTargetUpdateMode = Viewport.UpdateMode.Always;
				nPixelLayer.OwnWorld = true;
				nPixelLayer.GetTexture().Flags = 4u;
				nPixelLayerDisplay = GDUtil.MakeNode<TextureRect>("PixelLayerDisplay");
				nPixelLayerDisplay.SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
				nPixelLayerDisplay.Texture = nPixelLayer.GetTexture();
				nPixelLayerDisplay.Expand = true;
				nPixelLayerDisplay.Material = GD.Load<Material>("res://resources/material/pixel_layer.tres");
				nPixelLayerDisplay.FlipV = true;
				UpdateShaders();
				nScreenDarkener = GDUtil.MakeNode<ColorRect>("ScreenDarkener");
				nScreenDarkener.SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
				nScreenDarkener.Visible = false;
				nScreenDarkener.Color = UIUtil.ScreenDarkenerColor;
				Control nMinigameLayer = GDUtil.MakeNode<Control>("MinigameLayer");
				nMinigameLayer.SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
				Control nStoryPlayerLayer = GDUtil.MakeNode<Control>("StoryPlayerLayer");
				nStoryPlayerLayer.SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
				Control nUiLayer = GDUtil.MakeNode<Control>("UILayer");
				nUiLayer.SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
				Control nScreenLayer = GDUtil.MakeNode<Control>("ScreenLayer");
				nScreenLayer.SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
				nBlackOverlay = UIUtil.CreateOverlay(Colors.Black);
				nBlackOverlay.Visible = false;
				Control nFlyoutLayer = GDUtil.MakeNode<Control>("FlyoutLayer");
				nFlyoutLayer.SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
				Control nHudLayer = GDUtil.MakeNode<Control>("HUDLayer");
				nHudLayer.SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
				nFPS = new FpsLabel();
				layers = new Dictionary<IScreenManager.Layer, Node>();
				layers[IScreenManager.Layer.Pixel] = nPixelLayer;
				layers[IScreenManager.Layer.Minigame] = nMinigameLayer;
				layers[IScreenManager.Layer.StoryPlayer] = nStoryPlayerLayer;
				layers[IScreenManager.Layer.UI] = nUiLayer;
				layers[IScreenManager.Layer.Screen] = nScreenLayer;
				layers[IScreenManager.Layer.Flyout] = nFlyoutLayer;
				layers[IScreenManager.Layer.HUD] = nHudLayer;
				AddChild(nPixelLayer);
				AddChild(nPixelLayerDisplay);
				AddChild(nScreenDarkener);
				AddChild(layers[IScreenManager.Layer.Minigame]);
				AddChild(layers[IScreenManager.Layer.StoryPlayer]);
				AddChild(layers[IScreenManager.Layer.UI]);
				AddChild(layers[IScreenManager.Layer.Screen]);
				AddChild(nBlackOverlay);
				AddChild(layers[IScreenManager.Layer.Flyout]);
				AddChild(layers[IScreenManager.Layer.HUD]);
				if (OS.IsDebugBuild())
				{
					AddChild(nFPS);
				}
				Game.Root.AddChild(this);
			}
		}

		public override void _Input(InputEvent @event)
		{
			if (Game.InputProcessor != 0)
			{
				nPixelLayer.Input(@event);
			}
		}

		public void TakeScreenshot()
		{
			if (!Game.Settings.SteamBuild)
			{
				Image data = GetTree().Root.GetTexture().GetData();
				data.FlipY();
				GDUtil.EnsureFolderExists("user://screenshot/");
				string datetime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss.fffffff");
				string filename = "user://screenshot/" + "Screenshot-" + datetime + ".png";
				if (data.SavePng(filename) == Error.Ok)
				{
					Log.Info("Screenshot saved as ", filename);
				}
			}
		}

		public void ToggleFpsMeter()
		{
			if (nFPS.IsValid())
			{
				nFPS.Visible = !nFPS.Visible;
			}
		}

		public void AddToLayer(IScreenManager.Layer layer, Node node)
		{
			layers[layer].AddChild(node);
		}

		public Node GetFromLayer(IScreenManager.Layer layer, string path)
		{
			return layers[layer].GetNodeOrNull(path);
		}

		public void Clean()
		{
			layers[IScreenManager.Layer.Pixel].Clear();
			layers[IScreenManager.Layer.StoryPlayer].Clear();
			layers[IScreenManager.Layer.Minigame].Clear();
			layers[IScreenManager.Layer.Screen].Clear();
			layers[IScreenManager.Layer.UI].Clear();
		}

		public void UpdateShaders()
		{
			ShaderMaterial obj = nPixelLayerDisplay.Material as ShaderMaterial;
			obj.SetShaderParam("brightness", decimal.ToSingle(Game.Settings.Brightness));
			obj.SetShaderParam("contrast", decimal.ToSingle(Game.Settings.Contrast));
			obj.SetShaderParam("saturation", decimal.ToSingle(Game.Settings.Saturation));
			obj.SetShaderParam("gamma", decimal.ToSingle(Game.Settings.Gamma));
		}

		public void OpenMenu()
		{
			if (!layers[IScreenManager.Layer.UI].HasNode("GameMenu"))
			{
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
				Game.InputProcessor = Inputs.Processor.Menu;
				GameMenuContainer gameMenu = new GameMenuContainer();
				gameMenu.OnClose = delegate
				{
					CloseMenu();
				};
				layers[IScreenManager.Layer.UI].AddChild(gameMenu);
				layers[IScreenManager.Layer.Flyout].Clear();
				pendingFlyouts.Clear();
			}
		}

		public void CloseMenu()
		{
			if (layers[IScreenManager.Layer.UI].HasNode("GameMenu"))
			{
				Game.InputProcessor = Inputs.Processor.Player;
				layers[IScreenManager.Layer.UI].GetNode("GameMenu").Delete();
			}
		}

		public async Task FadeToBlack(float time)
		{
			if (!nBlackOverlay.Visible)
			{
				nBlackOverlay.Visible = true;
				FadeAnimation fadeIn = new FadeInAnimation(nBlackOverlay, time);
				Game.Animations.Play(fadeIn);
				await fadeIn.WaitUntilFinished();
			}
		}

		public async Task FadeToBlack()
		{
			await FadeToBlack(0.5f);
		}

		public async Task FadeFromBlack(float time)
		{
			if (nBlackOverlay.Visible)
			{
				FadeAnimation fadeOut = new FadeOutAnimation(nBlackOverlay, time);
				Game.Animations.Play(fadeOut);
				await fadeOut.WaitUntilFinished();
				nBlackOverlay.Visible = false;
			}
		}

		public async Task FadeFromBlack()
		{
			await FadeFromBlack(0.5f);
		}

		public async Task ShowLoadingScreen(float time)
		{
			await FadeToBlack(time);
			nLoadingScreen = Game.Settings.SceneProvider.LoadingScreen.Instance<LoadingScreen>();
			AddChild(nLoadingScreen);
			await GDUtil.DelayOneFrame();
		}

		public async Task ShowLoadingScreen()
		{
			await ShowLoadingScreen(0.5f);
		}

		public async Task ShowLoadingScreenInstantly()
		{
			nBlackOverlay.Visible = true;
			nLoadingScreen = Game.Settings.SceneProvider.LoadingScreen.Instance<LoadingScreen>();
			nLoadingScreen.Instant = true;
			AddChild(nLoadingScreen);
			await GDUtil.DelayOneFrame();
		}

		public async Task HideLoadingScreen(float time)
		{
			nLoadingScreen.DeleteIfValid();
			nLoadingScreen = null;
			await FadeFromBlack(time);
		}

		public async Task HideLoadingScreen()
		{
			await HideLoadingScreen(0.5f);
		}

		public void DarkenScreen()
		{
			nScreenDarkener.Visible = true;
		}

		public void UndarkenScreen()
		{
			nScreenDarkener.Visible = false;
		}

		public async void ShowSaving()
		{
			Control savingScreen = Game.Settings.SceneProvider.SavingScreen.Instance<Control>();
			AddChild(savingScreen);
			await DrkieUtil.DelaySeconds(2.0);
			savingScreen.Delete();
		}

		public async Task ShowFlyout(Control flyout, float duration)
		{
			if (flyoutInProgress)
			{
				pendingFlyouts.Enqueue(new Tuple<Control, float>(flyout, duration));
				return;
			}
			try
			{
				flyoutInProgress = true;
				Control nFlyoutLayer = (Control)layers[IScreenManager.Layer.Flyout];
				nFlyoutLayer.AddChild(flyout);
				new Vector2(Game.Settings.BaseResolution.x, 15f);
				Vector2 to = new Vector2(Game.Settings.BaseResolution.x - flyout.RectSize.x - 15f, 15f);
				LacieAnimation slideIn = new SlideInRightAnimation(nFlyoutLayer, flyout.RectSize.x - 15f, to, 0.3f);
				LacieAnimation slideOut = new SlideOutRightAnimation(nFlyoutLayer, flyout.RectSize.x - 15f, to, 0.3f);
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_objective.ogg");
				Game.Animations.Play(slideIn);
				await slideIn.WaitUntilFinished();
				await DrkieUtil.DelaySeconds(duration);
				Game.Animations.Play(slideOut);
				await slideOut.WaitUntilFinished();
				flyout.DeleteIfValid();
			}
			finally
			{
				flyoutInProgress = false;
				if (pendingFlyouts.Count > 0)
				{
					Tuple<Control, float> nextFlyout = pendingFlyouts.Dequeue();
					ShowFlyout(nextFlyout.Item1, nextFlyout.Item2);
				}
			}
		}

		public async Task ShowFlyout(Control flyout)
		{
			await ShowFlyout(flyout, 3f);
		}

		public void RefreshPixelScaling()
		{
			nPixelLayer.Size = Game.Settings.BaseResolution * Game.Settings.ResolutionScalePixel;
		}
	}
}
