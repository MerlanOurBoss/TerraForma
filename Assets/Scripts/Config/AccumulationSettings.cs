using UnityEngine;

namespace TerraForma.Config
{
    [CreateAssetMenu(menuName = "TerraForma/Accumulation Settings")]
    public class AccumulationSettings : ScriptableObject
    {
        [Min(0f), Tooltip("Прирост высоты в единицах мира в секунду")]
        public float rate = 1.5f;

        [Min(8)] public int resolution = 129;
        [Min(0.01f)] public float cellSize = 0.15f;
    }
}