// using System;
// using UnityEngine;
// using UnityEngine.AI;
//
// namespace DefaultNamespace
// {
//     public class BaseEnemy : MonoBehaviour
//     {
//         public event Action OnDeath;
//         
//         private NavMeshAgent _agent;
//
//         public Transform Target { get; set; }
//
//         protected void Start()
//         {
//             _agent = GetComponent<NavMeshAgent>();
//             _agent.SetDestination(Target.position);
//         }
//
//         public void Kill()
//         {
//             OnDeath?.Invoke();
//             Destroy(gameObject);
//         }
//     }
// }