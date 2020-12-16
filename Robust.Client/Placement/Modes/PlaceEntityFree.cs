using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using Robust.Shared.Map;
using Robust.Shared.Maths;

namespace Robust.Client.Placement.Modes
{
    public class PlaceEntityFree: EntityPlacementMode
    {
        public PlaceEntityFree(EntityPlacementManager pMan) : base(pMan)
        {
        }

        public override bool CanPlaceAt(EntityCoordinates mouseCoords, Angle rotation)
        {
            return true;
        }

        public override IEnumerable<EntityCoordinates> GeneratePlacements(EntityCoordinates mouseCoords, Angle rotation)
        {
            yield return mouseCoords;
        }

        public override IEnumerable<EntityCoordinates> GenerateLinePlacements(EntityCoordinates startPosition, EntityCoordinates endPosition, Angle rotation)
        {
            var line = new Line(Vector2.Zero, endPosition.Position - startPosition.Position);
            var partingDistance = 1.0f; //TODO: Calculate to be the smallest value for this slope of line where the requested entities do not collide. This is a horrendous pain, good luck.
            foreach (var offs in line.NearestPointsAlongLine(partingDistance))
            {
                yield return startPosition.Offset(offs);
            }
        }

        public override IEnumerable<EntityCoordinates> GenerateGridPlacements(EntityCoordinates startCorner, EntityCoordinates endCorner, Angle rotation)
        {
            var a1 = startCorner.Position;
            a1.Y = 0;
            var b1 = endCorner.Position;
            b1.Y = 0;
            var a2 = startCorner.Position;
            a1.X = 0;
            var b2 = endCorner.Position;
            b1.X = 0;
            var xline = new Line(a1, b1);
            var yline = new Line(a2, b2);
            var x_parting_distance = 1.0f;
            var y_parting_distance = 1.0f;

            var yEvenSubdiv = yline.NearestSubdivison(y_parting_distance);
            var icount = (int)Math.Round(yline.Length / yEvenSubdiv);

            foreach (var xoffs in xline.NearestPointsAlongLine(x_parting_distance))
            {
                var yoffs = new Vector2(0.0f, yEvenSubdiv);
                var a = startCorner.Offset(xoffs);
                foreach (var i in Enumerable.Range(0, icount))
                {
                    yield return a.Offset(yoffs * i);
                }
            }
        }

        public override IEnumerable<TileRef> FindDestinationTilesSingle(EntityCoordinates mouseCoords, Angle rotation)
        {
            throw new System.NotImplementedException();
        }

        public override IEnumerable<TileRef> FindDestinationTilesLine(EntityCoordinates startPosition, EntityCoordinates endPosition, Angle rotation)
        {
            throw new System.NotImplementedException();
        }

        public override IEnumerable<TileRef> FindDestinationTilesGrid(EntityCoordinates startCorner, EntityCoordinates endCorner, Angle rotation)
        {
            throw new System.NotImplementedException();
        }
    }
}
