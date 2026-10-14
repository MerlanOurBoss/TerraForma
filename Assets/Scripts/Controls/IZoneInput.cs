using UnityEngine;

namespace TerraForma.Controls
{
    public interface IZoneInput
    {
        Vector2 Move { get; }
        bool IsAccumulating { get; }
        bool ResetPressed { get; }
    }
}