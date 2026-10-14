using TerraForma.Config;
using UnityEngine;

namespace TerraForma.Core
{
    public class ZoneMover
    {
        private readonly ZoneSettings _settings;
        private readonly Rect _bounds;

        public Vector2 Position { get; private set; }

        public ZoneMover(ZoneSettings settings, Rect bounds, Vector2 startPosition)
        {
            _settings = settings;
            _bounds = bounds;
            Position = startPosition;
        }

        public void Tick(Vector2 direction, float deltaTime)
        {
            Vector2 p = Position + direction * (_settings.moveSpeed * deltaTime);
            p.x = Mathf.Clamp(p.x, _bounds.xMin, _bounds.xMax);
            p.y = Mathf.Clamp(p.y, _bounds.yMin, _bounds.yMax);
            Position = p;
        }
    }
}