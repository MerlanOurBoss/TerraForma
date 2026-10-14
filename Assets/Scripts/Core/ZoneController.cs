using TerraForma.Config;
using TerraForma.Controls;
using TerraForma.View;
using UnityEngine;

namespace TerraForma.Core
{
    public class ZoneController : MonoBehaviour
    {
        [SerializeField] private ZoneSettings settings;
        [SerializeField] private ZoneVisual visual;
        [SerializeField, Tooltip("Половина стороны поверхности")] private float halfSize = 10f;

        private IZoneInput _input;
        private ZoneMover _mover;
        private RadiusAnimator _radius;

        public Vector2 PreviousPosition { get; private set; }
        public float PreviousRadius { get; private set; }
        public Vector2 Position => _mover.Position;
        public float Radius => _radius.Radius;

        private void Awake()
        {
            _input = new KeyboardZoneInput();
            _mover = new ZoneMover(settings, Rect.MinMaxRect(-halfSize, -halfSize, halfSize, halfSize), Vector2.zero);
            _radius = new RadiusAnimator(settings);
            PreviousPosition = _mover.Position;
            PreviousRadius = _radius.Radius;
        }

        private void Update()
        {
            PreviousPosition = _mover.Position;
            PreviousRadius = _radius.Radius;

            float dt = Time.deltaTime;
            _mover.Tick(_input.Move, dt);
            _radius.Tick(dt);

            transform.position = new Vector3(_mover.Position.x, 0f, _mover.Position.y);
            visual.SetRadius(_radius.Radius);
        }
        
        private void OnDestroy()
        {
            (_input as System.IDisposable)?.Dispose();
        }
    }
}