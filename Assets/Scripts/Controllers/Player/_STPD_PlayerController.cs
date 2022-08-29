using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Controller;

namespace VM.Player
{
    [RequireComponent(typeof(NavMeshController))]
    public class _STPD_PlayerController : MonoBehaviour
    {
        [SerializeField] private NavMeshController _navMeshController;
        [SerializeField] private Rigidbody _rb;

        private Camera _camera;

        private void Awake()
        {
            this._navMeshController = GetComponent<NavMeshController>();
            this._rb = GetComponent<Rigidbody>();
            this._camera = Camera.main;
        }

        private void FixedUpdate()
        {
            Vector3 direction = Vector3.zero;

            float horizontalAxis = Input.GetAxisRaw("Horizontal");
            float verticalAxis = Input.GetAxisRaw("Vertical");

            Vector3 cur = transform.position;
            Vector3 cam = this._camera.transform.position;

            cur.y = 0;
            cam.y = 0;

            Vector3 zeroDirection = (cur - cam).normalized;

            // Right
            if (horizontalAxis > 0)
            {
                direction += new Vector3(zeroDirection.z, 0, -zeroDirection.x);
            }
            // Left
            else if (horizontalAxis < 0)
            {
                direction += new Vector3(-zeroDirection.z, 0, zeroDirection.x);
            }

            // Up
            if (verticalAxis > 0)
            {
                direction += zeroDirection;
            }
            // Down
            else if (verticalAxis < 0)
            {
                direction += -zeroDirection;
            }

            this._navMeshController.MoveToDirection(direction.normalized * 5);

            // this._rb.MovePosition(transform.position + direction.normalized * 5 * Time.deltaTime);
        }
    }
}
