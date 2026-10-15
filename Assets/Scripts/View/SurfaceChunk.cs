using TerraForma.Core;
using UnityEngine;

namespace TerraForma.View
{
    public class SurfaceChunk
    {
        private readonly HeightField _field;
        private readonly int _startX, _startZ, _cellsX, _cellsZ;
        private readonly Vector3[] _vertices;
        private readonly Vector3[] _normals;
        private readonly Mesh _mesh;

        public SurfaceChunk(HeightField field, int startX, int startZ, int cellsX, int cellsZ,
            Transform parent, Material material)
        {
            _field = field;
            _startX = startX; _startZ = startZ;
            _cellsX = cellsX; _cellsZ = cellsZ;

            int count = (cellsX + 1) * (cellsZ + 1);
            _vertices = new Vector3[count];
            _normals = new Vector3[count];

            _mesh = new Mesh { name = $"Chunk_{startX}_{startZ}" };
            _mesh.MarkDynamic();

            var go = new GameObject(_mesh.name, typeof(MeshFilter), typeof(MeshRenderer));
            go.transform.SetParent(parent, false);
            go.GetComponent<MeshFilter>().sharedMesh = _mesh;
            go.GetComponent<MeshRenderer>().sharedMaterial = material;

            Rebuild();
            _mesh.triangles = BuildTriangles();
        }

        public void Rebuild()
        {
            int i = 0;
            for (int lz = 0; lz <= _cellsZ; lz++)
            {
                int gz = _startZ + lz;
                for (int lx = 0; lx <= _cellsX; lx++, i++)
                {
                    int gx = _startX + lx;
                    _vertices[i] = new Vector3(
                        _field.Origin.x + gx * _field.CellSize,
                        _field[gx, gz],
                        _field.Origin.y + gz * _field.CellSize);
                    _normals[i] = _field.GetNormal(gx, gz);
                }
            }

            _mesh.SetVertices(_vertices);
            _mesh.SetNormals(_normals);
            _mesh.RecalculateBounds();
        }

        private int[] BuildTriangles()
        {
            var tris = new int[_cellsX * _cellsZ * 6];
            int t = 0, row = _cellsX + 1;
            for (int lz = 0; lz < _cellsZ; lz++)
            {
                for (int lx = 0; lx < _cellsX; lx++)
                {
                    int a = lz * row + lx;
                    int b = a + row;
                    tris[t++] = a;     tris[t++] = b;     tris[t++] = a + 1;
                    tris[t++] = a + 1; tris[t++] = b;     tris[t++] = b + 1;
                }
            }
            return tris;
        }
    }
}