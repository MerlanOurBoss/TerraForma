using TerraForma.Config;
using UnityEngine;

namespace TerraForma.Core
{
    public class MaterialAccumulator
    {
        private readonly HeightField _field;
        private readonly AccumulationSettings _settings;

        public MaterialAccumulator(HeightField field, AccumulationSettings settings)
        {
            _field = field;
            _settings = settings;
        }

        public void Apply(Vector2 p0, float r0, Vector2 p1, float r1, float deltaTime)
        {
            float add = _settings.rate * deltaTime;
            if (add <= 0f) return;

            float cell = _field.CellSize;
            float maxR = Mathf.Max(r0, r1);

            int minX = Mathf.Max(0, Mathf.FloorToInt((Mathf.Min(p0.x, p1.x) - maxR - _field.Origin.x) / cell));
            int maxX = Mathf.Min(_field.Resolution - 1, Mathf.CeilToInt((Mathf.Max(p0.x, p1.x) + maxR - _field.Origin.x) / cell));
            int minZ = Mathf.Max(0, Mathf.FloorToInt((Mathf.Min(p0.y, p1.y) - maxR - _field.Origin.y) / cell));
            int maxZ = Mathf.Min(_field.Resolution - 1, Mathf.CeilToInt((Mathf.Max(p0.y, p1.y) + maxR + 0f - _field.Origin.y) / cell));
            if (minX > maxX || minZ > maxZ) return;

            Vector2 seg = p1 - p0;
            float segLenSq = seg.sqrMagnitude;
            bool changed = false;

            for (int z = minZ; z <= maxZ; z++)
            {
                float wz = _field.Origin.y + z * cell;
                for (int x = minX; x <= maxX; x++)
                {
                    float wx = _field.Origin.x + x * cell;

                    float t = 0f;
                    if (segLenSq > 1e-8f)
                        t = Mathf.Clamp01(((wx - p0.x) * seg.x + (wz - p0.y) * seg.y) / segLenSq);

                    float dx = wx - (p0.x + seg.x * t);
                    float dz = wz - (p0.y + seg.y * t);
                    float distSq = dx * dx + dz * dz;

                    float r = Mathf.Lerp(r0, r1, t);
                    float rSq = r * r;
                    if (distSq >= rSq) continue;

                    float cap = Mathf.Sqrt(rSq - distSq);
                    float h = _field[x, z];
                    if (h >= cap) continue;

                    _field[x, z] = Mathf.Min(h + add, cap);
                    changed = true;
                }
            }
            if (changed) _field.MarkDirty(minX, minZ, maxX, maxZ);
        }
    }
}