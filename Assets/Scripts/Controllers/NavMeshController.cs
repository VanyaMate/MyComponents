using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace VM.Controller
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavMeshController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navAgent;

        private void Awake()
        {
            this._navAgent = GetComponent<NavMeshAgent>();
        }

        public void MoveToPosition (Vector3 position)
        {
            this._navAgent.SetDestination(position);
        }

        public void MoveToDirection (Vector3 direction)
        {
            Debug.Log("dir: " + direction);
            this._navAgent.velocity = direction;
        }

        public void AngSpeed (float speed)
        {
            this._navAgent.angularSpeed = speed;
        }
    }
}