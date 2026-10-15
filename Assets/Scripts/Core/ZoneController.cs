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
        [SerializeField] private AccumulationSettings accumulation;
        
        private HeightField _field;
        private MaterialAccumulator _accumulator;
        private IZoneInput _input;
        private ZoneMover _mover;
        private RadiusAnimator _radius;

        public Vector2 PreviousPosition { get; private set; }
        public float PreviousRadius { get; private set; }
        public Vector2 Position => _mover.Position;
        public float Radius => _radius.Radius;

        private void Awake()
        {
            _field = new HeightField(accumulation.resolution, accumulation.cellSize,
                -Vector2.one * ((accumulation.resolution - 1) * accumulation.cellSize * 0.5f));
            _accumulator = new MaterialAccumulator(_field, accumulation);
            
            _input = new KeyboardZoneInput();
            _mover = new ZoneMover(settings, _field.Bounds, Vector2.zero);
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
            
            if (_input.IsAccumulating)
                _accumulator.Apply(PreviousPosition, PreviousRadius, _mover.Position, _radius.Radius, dt);

            if (_input.ResetPressed)
                _field.Reset();
            
            transform.position = new Vector3(_mover.Position.x, 0f, _mover.Position.y);
            visual.SetRadius(_radius.Radius);
        }
        
        private void OnDestroy()
        {
            (_input as System.IDisposable)?.Dispose();
        }
    }
}