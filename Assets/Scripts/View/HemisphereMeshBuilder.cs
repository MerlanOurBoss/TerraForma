using UnityEngine;

namespace TerraForma.View
{
    /// <summary>
    /// Строит меш полусферы 
    /// </summary>
    public static class HemisphereMeshBuilder
    {
        public static Mesh Build(int segments = 32, int rings = 12)
        {
            int vertsPerRing = segments + 1;
            var vertices = new Vector3[(rings + 1) * vertsPerRing];
            var triangles = new int[rings * segments * 6];
            
            for (int r = 0; r <= rings; r++)
            {
                float polar = (float)r / rings * Mathf.PI * 0.5f;
                float y = Mathf.Sin(polar);
                float ringRadius = Mathf.Cos(polar);

                for (int s = 0; s <= segments; s++)
                {
                    float azimuth = (float)s / segments * Mathf.PI * 2f;
                    vertices[r * vertsPerRing + s] = new Vector3(
                        Mathf.Cos(azimuth) * ringRadius,
                        y,
                        Mathf.Sin(azimuth) * ringRadius);
                }
            }

            int t = 0;
            for (int r = 0; r < rings; r++)
            {
                for (int s = 0; s < segments; s++)
                {
                    int a = r * vertsPerRing + s;
                    int b = a + vertsPerRing;
                    
                    triangles[t++] = a;     triangles[t++] = b;     triangles[t++] = a + 1;
                    triangles[t++] = a + 1; triangles[t++] = b;     triangles[t++] = b + 1;
                }
            }

            var mesh = new Mesh { name = "Hemisphere" };
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }
    }
}