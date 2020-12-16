using Robust.Client.Graphics.Drawing;
using Robust.Client.Graphics.Overlays;

namespace Robust.Client.Placement
{
    internal class PlacementOverlay : Overlay
    {
        private readonly EntityPlacementManager _manager;
        public override bool AlwaysDirty => true;
        public override OverlaySpace Space => OverlaySpace.WorldSpace;

        public PlacementOverlay(EntityPlacementManager manager) : base("placement")
        {
            _manager = manager;
            ZIndex = 100;
        }

        protected override void Draw(DrawingHandleBase handle, OverlaySpace currentSpace)
        {

        }
    }
}
