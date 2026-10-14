using UnityEngine;

namespace TerraForma.View
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class ZoneVisual : MonoBehaviour
    {
        [SerializeField, Min(8)] private int segments = 32;
        [SerializeField, Min(2)] private int rings = 12;

        private void Awake()
        {
            GetComponent<MeshFilter>().sharedMesh = HemisphereMeshBuilder.Build(segments, rings);
        }

        public void SetRadius(float radius)
        {
            transform.localScale = Vector3.one * radius;
        }
    }
}