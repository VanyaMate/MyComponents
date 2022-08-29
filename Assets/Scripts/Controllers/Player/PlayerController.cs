using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VM.Controller;

namespace VM.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Controllers")]
        [SerializeField] private CharacterControllerBased _cc;
        [SerializeField] private Animator _animator;
        [SerializeField] private Camera _camera;

        [Header("Props")]
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _jumpTime;

        private void Awake()
        {
            this._camera = Camera.main;
            
            if (this._cc != null)
            {
                this._cc.SetGravity(this._jumpTime);
            }
        }

        private void Update()
        {
            this._Move();
            this._Jump();
        }

        private void _Move()
        {
            float hor = Input.GetAxisRaw("Horizontal");
            float ver = Input.GetAxisRaw("Vertical");

            Vector3 currentPosition = transform.position;
            Vector3 cameraPosition = this._camera.transform.position;

            currentPosition.y = 0;
            cameraPosition.y = 0;

            Vector3 direction = Vector3.zero;
            Vector3 zeroDirection = (currentPosition - cameraPosition).normalized;

            bool run = false;

            // Right
            if (hor > 0)
            {
                direction += new Vector3(zeroDirection.z, 0, -zeroDirection.x);
                run = true;
            }
            // Left 
            else if (hor < 0)
            {
                direction += new Vector3(-zeroDirection.z, 0, zeroDirection.x);
                run = true;
            }

            // Forward
            if (ver > 0)
            {
                direction += zeroDirection;
                run = true;
            }
            // Back
            else if (ver < 0)
            {
                direction += -zeroDirection;
                run = true;
            }

            this._animator.SetBool("run", run);

            direction = direction.normalized;
            direction *= this._speed;

            this._cc.Move(direction);
        }

        private void _Jump()
        {
            if (this._cc.isGrounded)
            {
                this._animator.SetBool("jump", false);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    this._cc.Jump(this._jumpForce);
                }
            }
            else
            {
                this._animator.SetBool("jump", true);
            }
        }
    }
}