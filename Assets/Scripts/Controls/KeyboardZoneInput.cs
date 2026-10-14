using UnityEngine;
using UnityEngine.InputSystem;

namespace TerraForma.Controls
{
    public class KeyboardZoneInput : IZoneInput, System.IDisposable
    {
        private readonly InputAction _move;
        private readonly InputAction _accumulate;
        private readonly InputAction _reset;

        public KeyboardZoneInput()
        {
            _move = new InputAction("Move", InputActionType.Value);
            _move.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");

            _accumulate = new InputAction("Accumulate", InputActionType.Button, "<Keyboard>/space");
            _reset = new InputAction("Reset", InputActionType.Button, "<Keyboard>/r");

            _move.Enable();
            _accumulate.Enable();
            _reset.Enable();
        }

        public Vector2 Move => _move.ReadValue<Vector2>();
        public bool IsAccumulating => _accumulate.IsPressed();
        public bool ResetPressed => _reset.WasPressedThisFrame();

        public void Dispose()
        {
            _move.Dispose();
            _accumulate.Dispose();
            _reset.Dispose();
        }
    }
}