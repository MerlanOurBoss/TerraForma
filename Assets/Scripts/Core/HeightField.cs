using UnityEngine;

namespace TerraForma.Core
{
    public class HeightField
    {
        private readonly float[] _heights;

        public int Resolution { get; }
        public float CellSize { get; }
        public Vector2 Origin { get; }
        
        public bool IsDirty { get; private set; }
        public int DirtyMinX { get; private set; }
        public int DirtyMinZ { get; private set; }
        public int DirtyMaxX { get; private set; }
        public int DirtyMaxZ { get; private set; }

        public HeightField(int resolution, float cellSize, Vector2 origin)
        {
            Resolution = resolution;
            CellSize = cellSize;
            Origin = origin;
            _heights = new float[resolution * resolution];
        }

        public float this[int x, int z]
        {
            get => _heights[z * Resolution + x];
            set => _heights[z * Resolution + x] = value;
        }

        public Rect Bounds => new Rect(Origin, Vector2.one * ((Resolution - 1) * CellSize));

        public void Reset()
        {
            System.Array.Clear(_heights, 0, _heights.Length);
            MarkDirty(0, 0, Resolution - 1, Resolution - 1);
        }

        public void MarkDirty(int minX, int minZ, int maxX, int maxZ)
        {
            if (!IsDirty)
            {
                DirtyMinX = minX; DirtyMinZ = minZ; DirtyMaxX = maxX; DirtyMaxZ = maxZ;
                IsDirty = true;
                return;
            }
            if (minX < DirtyMinX) DirtyMinX = minX;
            if (minZ < DirtyMinZ) DirtyMinZ = minZ;
            if (maxX > DirtyMaxX) DirtyMaxX = maxX;
            if (maxZ > DirtyMaxZ) DirtyMaxZ = maxZ;
        }
        
        public Vector3 GetNormal(int x, int z)
        {
            int xl = Mathf.Max(x - 1, 0), xr = Mathf.Min(x + 1, Resolution - 1);
            int zl = Mathf.Max(z - 1, 0), zr = Mathf.Min(z + 1, Resolution - 1);

            float dhdx = (this[xr, z] - this[xl, z]) / ((xr - xl) * CellSize);
            float dhdz = (this[x, zr] - this[x, zl]) / ((zr - zl) * CellSize);
            
            return new Vector3(-dhdx, 1f, -dhdz).normalized;
        }

        public void ClearDirty() => IsDirty = false;
    }
}