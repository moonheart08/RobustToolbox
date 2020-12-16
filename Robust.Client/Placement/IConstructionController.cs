using System.Collections.Generic;
using Robust.Shared.GameObjects;
using Robust.Shared.Interfaces.GameObjects;
using Robust.Shared.Map;

namespace Robust.Client.Placement
{
    public interface IConstructionController
    {
        public bool AttemptEntityPlacements(IEnumerable<EntityCoordinates> coords, EntityPrototype proto);
        public bool AttemptEntityDeletions(IEnumerable<IEntity> entities);
        public IEnumerable<EntityCoordinates> FilterShownEntityGhosts(IEnumerable<EntityCoordinates> ghostCoords, EntityPrototype proto);
        public IEnumerable<bool> DecideEntityGhostPlaceability(IEnumerable<EntityCoordinates> ghostCoords, EntityPrototype proto);

        public bool AttemptTilePlacements(IEnumerable<EntityCoordinates> coords, Tile ty);
        public bool AttemptTileDeletions(IEnumerable<TileRef> tiles);
        public IEnumerable<EntityCoordinates> FilterShownTileGhosts(IEnumerable<EntityCoordinates> ghostCoords, Tile ty);
        public IEnumerable<bool> DecideTileGhostPlaceability(IEnumerable<EntityCoordinates> ghostCoords, Tile ty);

        /// <summary>
        /// Called when something attempts to replace the currently active controller. Use it as a chance to clean up.
        /// </summary>
        public void UnregisterController();
    }
}
