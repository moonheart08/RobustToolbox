using System;
using System.Collections.Generic;
using System.Linq;
using Robust.Client.Interfaces;
using Robust.Client.Interfaces.Graphics;
using Robust.Client.Interfaces.Graphics.ClientEye;
using Robust.Client.Interfaces.Graphics.Overlays;
using Robust.Client.Interfaces.Input;
using Robust.Client.Interfaces.Placement;
using Robust.Client.Interfaces.ResourceManagement;
using Robust.Client.Player;
using Robust.Shared.GameObjects;
using Robust.Shared.Interfaces.GameObjects;
using Robust.Shared.Interfaces.Map;
using Robust.Shared.Interfaces.Network;
using Robust.Shared.Interfaces.Physics;
using Robust.Shared.Interfaces.Reflection;
using Robust.Shared.Interfaces.Timing;
using Robust.Shared.IoC;
using Robust.Shared.Map;
using Robust.Shared.Maths;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Robust.Client.Placement
{
    public enum PlacementTypes : byte
    {
        Single = 0,
        SnappedLine = 1,
        UnsnappedLine = 2,
        SnappedGrid = 3,
        UnsnappedGrid = 4,
    }

    public class EntityPlacementManager: IEntityPlacementManager
    {
        #region Dependencies
        [Dependency] public readonly IEyeManager EyeManager = default!;
        [Dependency] private readonly IOverlayManager _overlayManager = default!;
        #endregion

        private PlacementOverlay _drawOverlay = default!;

        #region Construction Controller
        private IConstructionController? _currentController;
        public IConstructionController? CurrentController
        {
            get => _currentController;
            set
            {
                _currentController?.UnregisterController();
                _currentController = value;
            }
        }
        #endregion

        #region Current entity placement
        public EntityPrototype? CurrentEntity { get; set; }
        public EntityCoordinates StartPoint { get; set; }
        public EntityCoordinates EndPoint { get; set; }
        public Angle Rotation { get; set; }

        private PlacementTypes _currentPlacementType;

        public PlacementTypes CurrentPlacementType
        {
            get => _currentPlacementType;
            set
            {
                switch (value)
                {
                    case PlacementTypes.Single:
                        _currentPlacementType = value;
                        break;
                    case PlacementTypes.SnappedLine:
                    case PlacementTypes.UnsnappedLine:
                        if (CurrentPlacementMode?.HasLineSupport == true)
                        {
                            _currentPlacementType = value;
                        }
                        break;
                    case PlacementTypes.SnappedGrid:
                    case PlacementTypes.UnsnappedGrid:
                        if (CurrentPlacementMode?.HasGridSupport == true)
                        {
                            _currentPlacementType = value;
                        }
                        break;
                }
            }
        }

        public EntityPlacementMode? CurrentPlacementMode { get; private set; }
        public RotationMode? CurrentRotationMode { get; private set; }
        #endregion

        public void Initialize()
        {
            _drawOverlay = new PlacementOverlay(this);
            _overlayManager.AddOverlay(_drawOverlay);
        }

        public void FrameUpdate(FrameEventArgs e)
        {

        }

        public bool DoPlacement()
        {
            IEnumerable<EntityCoordinates> coords = Enumerable.Empty<EntityCoordinates>(); // Not actually possible to use this, but C# is being a butt.
            if (CurrentPlacementMode == null || CurrentRotationMode == null || CurrentController == null || CurrentEntity == null)
            {
                throw new NullReferenceException();
            }

            switch (CurrentPlacementType)
            {
                case PlacementTypes.Single:
                    coords = CurrentPlacementMode.GeneratePlacements(StartPoint, Rotation);
                    break;

                case PlacementTypes.UnsnappedLine:
                    coords = CurrentPlacementMode.GenerateLinePlacements(StartPoint, EndPoint, Rotation);
                    break;

                case PlacementTypes.SnappedLine:
                    coords = CurrentPlacementMode.GenerateLinePlacements(StartPoint.SnapToGrid(), EndPoint.SnapToGrid(),
                        Rotation);
                    break;
                case PlacementTypes.UnsnappedGrid:
                    coords = CurrentPlacementMode.GenerateGridPlacements(StartPoint, EndPoint, Rotation);
                    break;
                case PlacementTypes.SnappedGrid:
                    coords = CurrentPlacementMode.GenerateGridPlacements(StartPoint.SnapToGrid(), EndPoint.SnapToGrid(),
                        Rotation);
                    break;
            }

            return CurrentController.AttemptEntityPlacements(coords, CurrentEntity);
        }

        public
            
    }
}
