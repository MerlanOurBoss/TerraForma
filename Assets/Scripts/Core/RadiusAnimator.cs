using TerraForma.Config;
using UnityEngine;

namespace TerraForma.Core
{
    /// <summary>
    /// Считает текущий радиус зоны по времени и настройкам.
    /// </summary>
    public class RadiusAnimator
    {
        private readonly ZoneSettings _settings;
        private float _phase;

        public float Radius { get; private set; }

        public RadiusAnimator(ZoneSettings settings)
        {
            _settings = settings;
            Radius = Evaluate();
        }

        public void Tick(float deltaTime)
        {
            _phase = Mathf.Repeat(_phase + deltaTime * _settings.frequency, 1f);
            Radius = Evaluate();
        }

        private float Evaluate()
        {
            float r = _settings.baseRadius + _settings.amplitude * _settings.radiusCurve.Evaluate(_phase);
            return Mathf.Max(0.05f, r);
        }
    }
}