// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace AmongUsClone.Shared.DataStructures
{
    /**
     * I created this class myself because I have some problems with System.Numerics.Vectors installation.
     * This class will be removed later after migrating the server into unity project
     */
    public class Vector2
    {
        public float x;
        public float y;
        
        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
