using TerraForma.Core;
using UnityEngine;

namespace TerraForma.View
{
    public class SurfaceMeshView : MonoBehaviour
    {
        [SerializeField] private Material material;
        [SerializeField, Min(4), Tooltip("Ячеек на сторону чанка")] private int chunkCells = 32;

        private HeightField _field;
        private SurfaceChunk[] _chunks;
        private int _chunksX, _chunksZ;

        public void Init(HeightField field)
        {
            _field = field;
            int cells = field.Resolution - 1;
            _chunksX = _chunksZ = Mathf.CeilToInt(cells / (float)chunkCells);
            _chunks = new SurfaceChunk[_chunksX * _chunksZ];

            for (int cz = 0; cz < _chunksZ; cz++)
            {
                for (int cx = 0; cx < _chunksX; cx++)
                {
                    int sx = cx * chunkCells, sz = cz * chunkCells;
                    int cellsX = Mathf.Min(chunkCells, cells - sx);
                    int cellsZ = Mathf.Min(chunkCells, cells - sz);
                    _chunks[cz * _chunksX + cx] = new SurfaceChunk(field, sx, sz, cellsX, cellsZ, transform, material);
                }
            }
            field.ClearDirty();
        }
        private void LateUpdate()
        {
            if (_field == null || !_field.IsDirty) return;
            
            int minX = Mathf.Max(0, _field.DirtyMinX - 1);
            int minZ = Mathf.Max(0, _field.DirtyMinZ - 1);
            int maxX = Mathf.Min(_field.Resolution - 1, _field.DirtyMaxX + 1);
            int maxZ = Mathf.Min(_field.Resolution - 1, _field.DirtyMaxZ + 1);
            
            int cxMin = Mathf.Max(0, (minX - 1) / chunkCells);
            int czMin = Mathf.Max(0, (minZ - 1) / chunkCells);
            int cxMax = Mathf.Min(_chunksX - 1, maxX / chunkCells);
            int czMax = Mathf.Min(_chunksZ - 1, maxZ / chunkCells);

            for (int cz = czMin; cz <= czMax; cz++)
                for (int cx = cxMin; cx <= cxMax; cx++)
                    _chunks[cz * _chunksX + cx].Rebuild();

            _field.ClearDirty();
        }
    }
}