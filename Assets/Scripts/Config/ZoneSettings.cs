using UnityEngine;

namespace TerraForma.Config
{
    [CreateAssetMenu(menuName = "TerraForma/Zone Settings")]
    public class ZoneSettings : ScriptableObject
    {
        [Header("Movement")]
        [Min(0f)] public float moveSpeed = 6f;

        [Header("Radius")]
        [Min(0.1f)] public float baseRadius = 2f;
        public float amplitude = 1f;
        [Min(0.01f), Tooltip("Полных циклов кривой в секунду")]
        public float frequency = 0.25f;
        [Tooltip("Значение кривой на [0..1] умножается на amplitude. Кривая зацикливается.")]
        public AnimationCurve radiusCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    }
}