using System.Threading.Tasks;
using Godot;

namespace LacieEngine.API
{
	[InjectableInterface(unique = true)]
	public interface IScreenManager : IModule
	{
		public enum Layer
		{
			Pixel,
			Screen,
			Minigame,
			UI,
			Flyout,
			StoryPlayer,
			HUD
		}

		void AddToLayer(Layer layer, Node node);

		Node GetFromLayer(Layer layer, string path);

		void CloseMenu();

		void DarkenScreen();

		Task FadeFromBlack(float time);

		Task FadeFromBlack();

		Task FadeToBlack(float time);

		Task FadeToBlack();

		Task HideLoadingScreen(float time);

		Task HideLoadingScreen();

		void OpenMenu();

		Task ShowFlyout(Control flyout, float duration);

		Task ShowFlyout(Control flyout);

		Task ShowLoadingScreen(float time);

		Task ShowLoadingScreen();

		Task ShowLoadingScreenInstantly();

		void ShowSaving();

		void TakeScreenshot();

		void ToggleFpsMeter();

		void UndarkenScreen();

		void UpdateShaders();

		void RefreshPixelScaling();
	}
}
