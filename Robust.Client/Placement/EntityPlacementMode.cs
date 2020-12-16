using System.Collections.Generic;
using Robust.Client.Graphics.Drawing;
using Robust.Shared.Map;
using Robust.Shared.Maths;

namespace Robust.Client.Placement
{
    public abstract class EntityPlacementMode
    {
        public readonly EntityPlacementManager EntityPlacementManager;

        protected EntityPlacementMode(EntityPlacementManager pMan)
        {
            EntityPlacementManager = pMan;
        }

        /// <summary>
        /// Name of this placement mode, used in UI.
        /// </summary>
        public virtual string ModeName => GetType().Name;

        /// <summary>
        /// Controls if this placement mode supports line mass placement.
        /// </summary>
        public virtual bool HasLineSupport => false;

        /// <summary>
        /// Controls if this placement mode supports grid mass placement.
        /// </summary>
        public virtual bool HasGridSupport => false;

        /// <summary>
        /// Controls if this placement mode uses the built-in grid lines.
        /// </summary>
        public virtual bool ShowsGridLines => false;

        /// <summary>
        /// Controls if a range check should be done; Meant for in-game placement.
        /// </summary>
        public virtual bool TestWithinRange => false;

        /// <summary>
        /// Tests if, under this placement mode, an object can be placed at these coordinated on the grid.
        /// </summary>
        /// <param name="mouseCoords">Coordinates of the mouse relative to the grid.</param>
        /// <returns></returns>
        public abstract bool CanPlaceAt(EntityCoordinates mouseCoords, Angle rotation);

        /// <summary>
        /// Handles aligning a placement.
        /// </summary>
        /// <param name="mouseCoords">Coordinates of the mouse relative to the grid.</param>
        /// <param name="rotation">Rotation of the objects being placed.</param>
        /// <returns>Enumerable list of placement locations.</returns>
        public abstract IEnumerable<EntityCoordinates> GeneratePlacements(EntityCoordinates mouseCoords, Angle rotation);

        /// <summary>
        /// Handles aligning a placement to a line.
        /// </summary>
        /// <param name="startPosition">Starting position of the line.</param>
        /// <param name="endPosition">Ending position of the line.</param>
        /// <param name="rotation">Rotation of the objects being placed.</param>
        /// <returns>Enumerable list of placement locations.</returns>
        public abstract IEnumerable<EntityCoordinates> GenerateLinePlacements(EntityCoordinates startPosition,
            EntityCoordinates endPosition, Angle rotation);

        /// <summary>
        /// Handles aligning a placement to a grid selection.
        /// </summary>
        /// <param name="startCorner">Starting corner of the grid selection.</param>
        /// <param name="endCorner">Ending corner of the grid selection.</param>
        /// <param name="rotation">Rotation of the objects being placed.</param>
        /// <returns>Enumerable list of placement locations.</returns>
        public abstract IEnumerable<EntityCoordinates> GenerateGridPlacements(EntityCoordinates startCorner,
            EntityCoordinates endCorner, Angle rotation);

        /// <summary>
        /// Figures out the tiles that objects are being placed on in single-object mode.
        /// </summary>
        /// <param name="mouseCoords">Coordinates of the mouse relative to the grid.</param>
        /// <param name="rotation">Rotation of the objects being placed.</param>
        /// <returns>Enumerable list of tiles.</returns>
        public abstract IEnumerable<TileRef> FindDestinationTilesSingle(EntityCoordinates mouseCoords, Angle rotation);

        /// <summary>
        /// Figures out the tiles that objects are being placed on in line-object mode.
        /// </summary>
        /// <param name="startPosition">Starting position of the line.</param>
        /// <param name="endPosition">Ending position of the line.</param>
        /// <param name="rotation">Rotation of the objects being placed.</param>
        /// <returns>Enumerable list of tiles.</returns>
        public abstract IEnumerable<TileRef> FindDestinationTilesLine(EntityCoordinates startPosition,
            EntityCoordinates endPosition, Angle rotation);

        /// <summary>
        /// Figures out the tiles that objects are being placed on in grid mode.
        /// </summary>
        /// <param name="startCorner">Starting corner of the grid selection.</param>
        /// <param name="endCorner">Ending corner of the grid selection.</param>
        /// <param name="rotation">Rotation of the objects being placed.</param>
        /// <returns>Enumerable list of tiles.</returns>
        public abstract IEnumerable<TileRef> FindDestinationTilesGrid(EntityCoordinates startCorner,
            EntityCoordinates endCorner, Angle rotation);

        /// <summary>
        /// Used to draw helpers, like placement grid lines.
        /// </summary>
        /// <param name="handle">Handle used for drawing.</param>
        /// <param name="lastMouseCoords">Last known coordinates of the mouse relative to the grid.</param>
        /// <param name="rotation">Rotation of the objects being placed.</param>
        public void RenderHelpers(DrawingHandleWorld handle, EntityCoordinates lastMouseCoords, Angle rotation)
        {

        }

        /// <summary>
        /// Generates the coordinates used to render placement ghosts in single-object mode.
        /// </summary>
        /// <param name="lastMouseCoords">Last known coordinates of the mouse relative to the grid.</param>
        /// <param name="rotation">Rotation of the objects being placed.</param>
        /// <returns>List of coordinates for ghosts.</returns>
        public virtual IEnumerable<EntityCoordinates> SingleGhostCoordinates(EntityCoordinates lastMouseCoords, Angle rotation)
        {
            return GeneratePlacements(lastMouseCoords, rotation);
        }

        /// <summary>
        /// Generates the coordinates used to render placement ghosts in line mode.
        /// </summary>
        /// <param name="startPosition">Starting position of the line.</param>
        /// <param name="endPosition">Ending position of the line.</param>
        /// <param name="rotation">Rotation of the objects being placed.</param>
        /// <returns>List of coordinates for ghosts.</returns>
        public virtual IEnumerable<EntityCoordinates> LineGhostCoordinates(EntityCoordinates startPosition,
            EntityCoordinates endPosition, Angle rotation)
        {
            return GenerateLinePlacements(startPosition, endPosition, rotation);
        }

        /// <summary>
        /// Generates the coordinates used to render placement ghosts in grid mode.
        /// </summary>
        /// <param name="startCorner">Starting corner of the grid selection.</param>
        /// <param name="endCorner">Ending corner of the grid selection.</param>
        /// <param name="rotation">Rotation of the objects being placed.</param>
        /// <returns>List of coordinates for ghosts.</returns>
        public virtual IEnumerable<EntityCoordinates> GridGhostCoordinates(EntityCoordinates startCorner,
            EntityCoordinates endCorner, Angle rotation)
        {
            return GenerateGridPlacements(startCorner, endCorner, rotation);
        }

        /// <summary>
        /// Checks if a ghost should be shown as placeable at this location or not.
        /// </summary>
        /// <param name="ghostCoords">Coordinates of the ghost relative to the grid</param>
        /// <param name="rotation">Rotation of the objects being placed.</param>
        /// <returns>Boolean indicating the ghost's placeability.</returns>
        public virtual bool GhostIsPlaceable(EntityCoordinates ghostCoords, Angle rotation)
        {
            return CanPlaceAt(ghostCoords, rotation);
        }
    }
}
