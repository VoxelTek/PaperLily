// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.SaveLoadMenu
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.UI
{
    public class SaveLoadMenu : SimpleVerticalMenu
    {
        private const int SLOTS_PER_PAGE = 3;
        private const int SLOTS_MAX = 30;
        private const float PAGINATION_ANIM_OFFSET = 60f;
        private const int LAST_PAGE = 9;
        private SaveLoadMenu.Mode _mode;
        private int _page;
        private Control _entriesCotainer;
        private Control _floatingContainer;
        private Control[] _drawnEntries;
        private Vector2[] _rowGlobalPositions;
        private bool _paginating;

        public SaveLoadMenu(
          SaveLoadMenu.Mode mode,
          IMenuEntryList parentMenu,
          IMenuEntryContainer container)
        {
            this.Parent = parentMenu;
            this.Container = container;
            this._mode = mode;
            this.InitEntries();
        }

        public override Control DrawContent()
        {
            VBoxContainer box = GDUtil.MakeNode<VBoxContainer>("EntryList");
            box.SetSeparation(10);
            box.Alignment = BoxContainer.AlignMode.Center;
            box.SizeFlagsHorizontal = 4;
            this._entriesCotainer = (Control)box;
            this.DrawEntries();
            this._floatingContainer = GDUtil.MakeNode<Control>("FloatingContainer");
            this._floatingContainer.SizeFlagsHorizontal = 3;
            this._floatingContainer.SizeFlagsVertical = 3;
            TextureRect node1 = GDUtil.MakeNode<TextureRect>("ArrowL");
            node1.Texture = GD.Load<Texture>("res://assets/img/ui/arrow_up.png");
            node1.RectRotation = -90f;
            node1.RectPosition = new Vector2(10f, 60f);
            node1.RectPivotOffset = node1.Texture.GetSize() / 2f;
            TextureRect node2 = GDUtil.MakeNode<TextureRect>("ArrowR");
            node2.Texture = GD.Load<Texture>("res://assets/img/ui/arrow_up.png");
            node2.RectRotation = 90f;
            node2.RectPosition = new Vector2(412f, 60f);
            node2.RectPivotOffset = node1.Texture.GetSize() / 2f;
            Control child = GDUtil.MakeNode<Control>("FloatingArrows");
            child.AddChild((Node)node1);
            child.AddChild((Node)node2);
            this.Container.AddToFrame(this._floatingContainer);
            this.Container.AddToFrame(child);
            Game.Animations.PlayDelayed((LacieAnimation)new BounceAnimation((Control)node1, Direction.Left), 0.1f);
            Game.Animations.PlayDelayed((LacieAnimation)new BounceAnimation((Control)node2, Direction.Right), 0.1f);
            return (Control)box;
        }

        public override void HighlightSelection()
        {
            foreach (SaveLoadSlotMenuEntry entry in this.Entries)
                entry.Deselect();
            if (this.Selection <= -1)
                return;
            ((SaveLoadSlotMenuEntry)this.Entries[this.Selection]).Select();
        }

        public override void HandleInput(InputEvent @event)
        {
            string str = Inputs.Handle(@event, Inputs.Processor.Menu, Inputs.AllUi, true);
            if (str == null || this._paginating)
                return;
            switch (str)
            {
                case "input_up":
                    Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
                    if (--this.Selection <= -1)
                        this.Selection = this.Entries.Count - 1;
                    this.HighlightSelection();
                    break;
                case "input_down":
                    Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
                    if (++this.Selection >= this.Entries.Count)
                        this.Selection = 0;
                    this.HighlightSelection();
                    break;
                case "input_left":
                    Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
                    this.Paginate(false);
                    break;
                case "input_right":
                    Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
                    this.Paginate(true);
                    break;
                case "input_action":
                    this.Entries[this.Selection].Accept();
                    break;
                case "input_cancel":
                    Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation2.ogg");
                    this.Back();
                    break;
            }
        }

        private void InitEntries()
        {
            this.Entries = new List<IMenuEntry>();
            this.Entries.Add((IMenuEntry)this.MakeEntrySlot(this._mode, SaveFileInformation.GetSaveFileInformation("slot" + (this._page * 3 + 1).ToString()), (IMenuEntryList)this));
            this.Entries.Add((IMenuEntry)this.MakeEntrySlot(this._mode, SaveFileInformation.GetSaveFileInformation("slot" + (this._page * 3 + 2).ToString()), (IMenuEntryList)this));
            this.Entries.Add((IMenuEntry)this.MakeEntrySlot(this._mode, SaveFileInformation.GetSaveFileInformation("slot" + (this._page * 3 + 3).ToString()), (IMenuEntryList)this));
        }

        private void RefreshPage()
        {
            this.InitEntries();
            this.DrawEntries();
            this.HighlightSelection();
        }

        private void DrawEntries()
        {
            this._entriesCotainer.Clear();
            this._drawnEntries = new Control[this.Entries.Count];
            int index = 0;
            foreach (IMenuEntry entry in this.Entries)
            {
                MarginContainer marginContainer = GDUtil.MakeNode<MarginContainer>("Option" + index.ToString());
                Control control = entry.DrawEntry();
                this._drawnEntries[index] = control;
                marginContainer.AddChild((Node)control);
                this._entriesCotainer.AddChild((Node)marginContainer);
                ++index;
            }
        }

        private async void Paginate(bool right)
        {
            SaveLoadMenu saveLoadMenu1 = this;
            saveLoadMenu1._paginating = true;
            saveLoadMenu1._floatingContainer.Clear();
            if (right)
            {
                SaveLoadMenu saveLoadMenu2 = saveLoadMenu1;
                int num1 = saveLoadMenu1._page + 1;
                int num2 = num1;
                saveLoadMenu2._page = num2;
                if (num1 > 9)
                    saveLoadMenu1._page = 0;
            }
            if (!right)
            {
                SaveLoadMenu saveLoadMenu3 = saveLoadMenu1;
                int num3 = saveLoadMenu1._page - 1;
                int num4 = num3;
                saveLoadMenu3._page = num4;
                if (num3 < 0)
                    saveLoadMenu1._page = 9;
            }
            if (saveLoadMenu1._rowGlobalPositions == null)
            {
                saveLoadMenu1._rowGlobalPositions = new Vector2[3];
                for (int index = 0; index < 3; ++index)
                    saveLoadMenu1._rowGlobalPositions[index] = saveLoadMenu1._drawnEntries[index].RectGlobalPosition;
            }
            for (int index = 0; index < 3; ++index)
            {
                saveLoadMenu1._drawnEntries[index].Reparent((Node)saveLoadMenu1._floatingContainer);
                saveLoadMenu1._drawnEntries[index].RectGlobalPosition = saveLoadMenu1._rowGlobalPositions[index];
            }
            int newSlotNumber = saveLoadMenu1._page * 3 + 1;
            int num;
            for (int i = 0; i < 3; num = i++)
            {
                if (right)
                    Game.Animations.Play((LacieAnimation)new SlideOutLeftAnimation(saveLoadMenu1._drawnEntries[i], new float?(60f)));
                else
                    Game.Animations.Play((LacieAnimation)new SlideOutRightAnimation(saveLoadMenu1._drawnEntries[i], new float?(60f)));
                SaveLoadMenu saveLoadMenu4 = saveLoadMenu1;
                int mode = (int)saveLoadMenu1._mode;
                num = newSlotNumber++;
                SaveFileInformation saveFileInformation = SaveFileInformation.GetSaveFileInformation("slot" + num.ToString());
                SaveLoadMenu parentMenu = saveLoadMenu1;
                Control newSlot = saveLoadMenu4.MakeEntrySlot((SaveLoadMenu.Mode)mode, saveFileInformation, (IMenuEntryList)parentMenu).DrawEntry();
                saveLoadMenu1._floatingContainer.AddChild((Node)newSlot);
                newSlot.RectGlobalPosition = saveLoadMenu1._rowGlobalPositions[i];
                newSlot.Modulate = Godot.Colors.Transparent;
                await DrkieUtil.DelaySeconds(0.05);
                if (right)
                    Game.Animations.Play((LacieAnimation)new SlideInRightAnimation(newSlot, new float?(60f)));
                else
                    Game.Animations.Play((LacieAnimation)new SlideInLeftAnimation(newSlot, new float?(60f)));
                newSlot = (Control)null;
            }
            await DrkieUtil.DelaySeconds(0.05);
            saveLoadMenu1.RefreshPage();
            saveLoadMenu1._paginating = false;
        }

        private SaveLoadSlotMenuEntry MakeEntrySlot(
          SaveLoadMenu.Mode mode,
          SaveFileInformation saveInfo,
          IMenuEntryList parentMenu)
        {
            return GameState.SaveExists(saveInfo.Id) ? (SaveLoadSlotMenuEntry)new SaveLoadSlotUsedEntry(mode, saveInfo, parentMenu) : (SaveLoadSlotMenuEntry)new SaveLoadSlotEmptyEntry(mode, saveInfo, parentMenu);
        }

        public enum Mode
        {
            Save,
            Load,
        }
    }
}
