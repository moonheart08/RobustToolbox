using Robust.Shared.Map;
using Robust.Shared.Maths;

namespace Robust.Client.Placement
{
    public abstract class RotationMode
    {
        /// <summary>
        /// Calculates the nearest valid angle
        /// </summary>
        /// <param name="current">Attempted angle</param>
        /// <param name="mouseCoords">Coordinates of the mouse</param>
        /// <returns></returns>
        public virtual Angle GetNearestAngle(Angle current, EntityCoordinates mouseCoords)
        {
            return current;
        }
    }
}
