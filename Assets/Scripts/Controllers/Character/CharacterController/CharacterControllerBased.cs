using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.Controller
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterControllerBased : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private CharacterController _cc;

        private Vector3 _movement = Vector3.zero;
        private float _gravity = 1f;

        public bool isGrounded => this._cc.isGrounded;

        private void Awake()
        {
            this._cc = GetComponent<CharacterController>();
        }

        private void Update()
        {
            this._GravitySence();
            this._cc.Move(this._movement * Time.deltaTime);
        }

        public void SetGravity(float gravity)
        {
            this._gravity = gravity;
        }

        public void Move(Vector3 direction)
        {
            this._movement.x = direction.x;
            this._movement.z = direction.z;
        }

        public void Jump (float jumpForce)
        {
            this._movement.y = jumpForce;
        }

        private void _GravitySence()
        {
            if (this._cc.isGrounded != true)
            {
                this._movement.y += Physics.gravity.y * Time.deltaTime * this._gravity;
            }
        }

        public void RotateTo(Vector3 direction)
        {
            this._cc.transform.eulerAngles = direction;
        }
    }
}