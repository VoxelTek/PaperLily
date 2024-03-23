// Decompiled with JetBrains decompiler
// Type: LacieEngine.Modules.Tutorials.TutorialManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Modules.Tutorials
{
    [Injectable]
    public class TutorialManager : ITutorialManager, IModule, ITranslatable
    {
        private TutorialScreen currentTutorial;

        public event Action TutorialStarted;

        public event Action TutorialEnded;

        public void ApplyCaptions(Tutorial tutorial)
        {
            tutorial.Name = Game.Language.GetCaption(tutorial.Name, TutorialNameContext(tutorial));
            tutorial.Text = Game.Language.GetCaption(tutorial.Text, TutorialTextContext(tutorial));
        }

        public void ShowTutorial(string tutorialId, bool animate)
        {
            Game.InputProcessor = Inputs.Processor.None;
            if (!Game.State.SeenTutorials.Contains(tutorialId))
                Game.State.SeenTutorials.Add(tutorialId);
            currentTutorial = new TutorialScreen {
                Tutorial = LoadTutorial(tutorialId)
            };
            Game.Screen.AddToLayer(IScreenManager.Layer.UI, currentTutorial);
            currentTutorial.Start(animate);
            var tutorialStarted = TutorialStarted;
            if (tutorialStarted == null)
                return;
            tutorialStarted();
        }

        public void ShowTutorial(string tutorialId) => ShowTutorial(tutorialId, true);

        public void HideTutorial()
        {
            if (currentTutorial == null)
                return;
            currentTutorial.Delete();
            currentTutorial = null;
            Game.Core.AssignInputProcessor();
            var tutorialEnded = TutorialEnded;
            if (tutorialEnded == null)
                return;
            tutorialEnded();
        }

        public bool Exists(string id) => GDUtil.FileExists(IdToPath(id));

        public string IdToPath(string id) => "res://resources/tutorials/" + id + ".tres";

        public Tutorial LoadTutorial(string id)
        {
            var tutorial = GD.Load<Tutorial>(IdToPath(id));
            tutorial.Id = id;
            tutorial.Name = Game.Language.GetCaption(tutorial.Name);
            tutorial.Text = Game.Language.GetCaption(tutorial.Text);
            return tutorial;
        }

        public void LoadResourcesForTutorial(string tutorialId)
        {
            Game.Memory.Cache(IdToPath(tutorialId));
        }

        public IList<string> GetDependencies(string tutorialId)
        {
            return new List<string> {
                IdToPath(tutorialId)
            };
        }

        public void ApplyTranslationOverrides()
        {
        }

        private string TutorialNameContext(Tutorial tutorial) => "tutorials.name." + tutorial.Id;

        private string TutorialTextContext(Tutorial tutorial) => "tutorials.text." + tutorial.Id;
    }
}
