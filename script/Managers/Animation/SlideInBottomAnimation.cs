using System;
using Godot;

namespace LacieEngine.Animation
{
    // Token: 0x02000143 RID: 323
    public class SlideInBottomAnimation : SlideInAnimation
    {
        // Token: 0x06000B5A RID: 2906 RVA: 0x0002D3CD File Offset: 0x0002B5CD
        public SlideInBottomAnimation(Control node, float? displacement = null, Vector2? to = null, float duration = 0.15f)
            : base(node, displacement, to, duration)
        {
        }

        // Token: 0x06000B5B RID: 2907 RVA: 0x0002D3DC File Offset: 0x0002B5DC
        public override void InitialCalculations()
        {
            this._to = this._pTo ?? this._node.RectPosition;
            this._from = this._to + new Vector2(0f, this._pDisplacement ?? this._node.RectSize.y);
            this._track = this._to - this._from;
        }
    }
}
