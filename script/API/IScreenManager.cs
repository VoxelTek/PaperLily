// Decompiled with JetBrains decompiler
// Type: LacieEngine.API.IScreenManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using System.Threading.Tasks;

#nullable disable
namespace LacieEngine.API
{
  [InjectableInterface(unique = true)]
  public interface IScreenManager : IModule
  {
    void AddToLayer(IScreenManager.Layer layer, Node node);

    Node GetFromLayer(IScreenManager.Layer layer, string path);

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

    enum Layer
    {
      Pixel,
      Screen,
      Minigame,
      UI,
      Flyout,
      StoryPlayer,
      HUD,
    }
  }
}
