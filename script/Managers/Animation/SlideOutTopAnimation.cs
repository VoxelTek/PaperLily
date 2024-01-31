using System;
using Godot;

namespace LacieEngine.Animation
{
    // Token: 0x0200014B RID: 331
    public class SlideOutTopAnimation : SlideOutAnimation
    {
        // Token: 0x06000B6C RID: 2924 RVA: 0x0002D916 File Offset: 0x0002BB16
        public SlideOutTopAnimation(Control node, float? displacement = null, Vector2? from = null, float duration = 0.15f)
            : base(node, displacement, from, duration)
        {
        }

        // Token: 0x06000B6D RID: 2925 RVA: 0x0002D924 File Offset: 0x0002BB24
        public override void InitialCalculations()
        {
            this._from = this.pFrom ?? this._node.RectPosition;
            this._to = this._from - new Vector2(0f, this._pDisplacement ?? this._node.RectSize.y);
            this._track = this._to - this._from;
        }
    }
}
