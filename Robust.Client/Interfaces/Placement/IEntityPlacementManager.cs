using System;
using System.Collections.Generic;
using Robust.Client.Input;
using Robust.Client.Placement;
using Robust.Shared;
using Robust.Shared.Enums;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Maths;
using Robust.Shared.Timing;

namespace Robust.Client.Interfaces.Placement
{
    public interface IEntityPlacementManager
    {
        public void Initialize();
        public EntityPrototype? CurrentEntity { get; set; }
        public EntityCoordinates StartPoint { get; set; }
        public EntityCoordinates EndPoint { get; set; }
        public Angle Rotation { get; set; }
        public PlacementTypes CurrentPlacementType { get; set; }
        public IConstructionController? CurrentController { get; set; }

        void FrameUpdate(FrameEventArgs e);
        public bool DoPlacement();
    }
}
