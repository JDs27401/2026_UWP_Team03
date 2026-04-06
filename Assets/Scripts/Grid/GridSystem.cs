// using C__Classes.Systems;
// using UnityEngine;
//
// namespace Scripts
// {
//     public class GridSystem : MonoBehaviour
//     {
//         public int width;
//         public int height;
//         public float cellSize;
//
//         public Vector3 GetWorldPosition(int x, int z)
//         {
//             return new Vector3(x, 0, z) * cellSize;
//         }
//
//         public Vector2Int GetGridPosition(Vector3 worldPos)
//         {
//             int x = Mathf.FloorToInt(worldPos.x / cellSize);
//             int z = Mathf.FloorToInt(worldPos.z / cellSize);
//             return new Vector2Int(x, z);
//         }
//         
//         public void OnDrawGizmos()
//         {
//             for (int x = 0; x < width; x++)
//             {
//                 for (int z = 0; z < height; z++)
//                 {
//                     Gizmos.color = Color.white;
//                     Gizmos.DrawWireCube(GetWorldPosition(x, z), Vector3.one * cellSize);
//                 }
//             }
//         }
//     }
// }