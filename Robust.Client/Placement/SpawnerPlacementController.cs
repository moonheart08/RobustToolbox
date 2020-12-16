using System.Collections.Generic;
using Robust.Shared.GameObjects;
using Robust.Shared.Interfaces.GameObjects;
using Robust.Shared.Map;

namespace Robust.Client.Placement
{
    public abstract class SpawnerPlacementController: IConstructionController
    {
        public bool AttemptEntityPlacements(IEnumerable<EntityCoordinates> coords, EntityPrototype proto)
        {
            throw new System.NotImplementedException();
        }

        public bool AttemptEntityDeletions(IEnumerable<IEntity> entities)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<EntityCoordinates> FilterShownEntityGhosts(IEnumerable<EntityCoordinates> ghostCoords, EntityPrototype proto)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<bool> DecideEntityGhostPlaceability(IEnumerable<EntityCoordinates> ghostCoords, EntityPrototype proto)
        {
            throw new System.NotImplementedException();
        }

        public bool AttemptTilePlacements(IEnumerable<EntityCoordinates> coords, Tile ty)
        {
            throw new System.NotImplementedException();
        }

        public bool AttemptTileDeletions(IEnumerable<TileRef> tiles)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<EntityCoordinates> FilterShownTileGhosts(IEnumerable<EntityCoordinates> ghostCoords, Tile ty)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<bool> DecideTileGhostPlaceability(IEnumerable<EntityCoordinates> ghostCoords, Tile ty)
        {
            throw new System.NotImplementedException();
        }

        public abstract void UnregisterController();
    }
}
