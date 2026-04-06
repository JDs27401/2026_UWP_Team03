using C__Classes.Systems;
using UnityEngine;

namespace Scripts
{
    public class GridSystem : MonoBehaviour
    {
        public int width;
        public int height;
        public float cellSize;

        public Vector3 GetWorldPosition(int x, int z)
        {
            return new Vector3(x, 0, z) * cellSize;
        }

        public Vector2Int GetGridPosition(Vector3 worldPos)
        {
            int x = Mathf.FloorToInt(worldPos.x / cellSize);
            int z = Mathf.FloorToInt(worldPos.z / cellSize);
            return new Vector2Int(x, z);
        }
    }
}