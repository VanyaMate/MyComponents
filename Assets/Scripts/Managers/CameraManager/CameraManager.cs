using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VM.CameraTools
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance;

        [Header("Zoom props")]
        [SerializeField] private float _cameraZoomStep;
        [SerializeField] private float _cameraZoomMin;
        [SerializeField] private float _cameraZoomMax;
        [SerializeField] private float _cameraZoomSpeed;

        [SerializeField] private float _currentZoom;
        [SerializeField] private Vector3 _targetZoom;
        [SerializeField] private float _targetTime;

        [Header("Rotate props")]
        [SerializeField] private float _cameraRotateYMin;
        [SerializeField] private float _cameraRotateYMax;
        [SerializeField] private float _cameraRotateSencX;
        [SerializeField] private float _cameraRotateSencY;

        [SerializeField] private float _currentRotate;

        [Header("Camera")]
        [SerializeField] private Camera _mainCamera;

        [Header("Cinemachine")]
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

        private CinemachineTransposer _cinemachineTransposer;

        private void Awake()
        {
            Instance = this;

            this._mainCamera = Camera.main;
            this._cinemachineTransposer = this._cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            this._currentZoom = this._cinemachineTransposer.m_FollowOffset.y;
            this._currentRotate = 3.5f;
            this._targetZoom = this._cinemachineTransposer.m_FollowOffset;

            this._ZoomHandler();
            this._RotateHandler();
        }

        private void Update()
        {
            this._ZoomObserver();
            this._RotateObserver();
        }

        private void _RotateObserver ()
        {
            if (Input.GetMouseButton(2))
            {
                this._RotateHandler();
            }
        }

        private void _ZoomObserver()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                this._ZoomHandler();    
            }

            if (this._targetZoom != this._cinemachineTransposer.m_FollowOffset)
            {
                this._cinemachineTransposer.m_FollowOffset = Vector3.Lerp(
                    this._cinemachineTransposer.m_FollowOffset,
                    this._targetZoom,
                    this._targetTime
                );

                this._targetTime += Time.deltaTime;
            }
        }

        private void _RotateHandler ()
        {
            float y = Input.GetAxis("Mouse Y");
            float x = Input.GetAxis("Mouse X");
            Vector3 rotate = new Vector3(y, -x, 0) * this._cameraRotateSencX;
            Vector3 toRotate = this._cinemachineVirtualCamera.m_Follow.transform.eulerAngles - rotate;
            toRotate.x = 0;
            toRotate.z = 0;

            this._cinemachineVirtualCamera.m_Follow.transform.eulerAngles = toRotate;

            this._currentRotate += y * this._cameraRotateSencY;

            if (this._currentRotate > this._cameraRotateYMax)
            {
                this._currentRotate = this._cameraRotateYMax;
            }
            else if (this._currentRotate < this._cameraRotateYMin)
            {
                this._currentRotate = this._cameraRotateYMin;
            }

            this._targetZoom = new Vector3(0, this._currentZoom - this._currentRotate, -this._currentZoom * (this._currentRotate * .25f));
        }

        private void _ZoomHandler ()
        {
            this._currentZoom -= Input.mouseScrollDelta.y;

            if (this._currentZoom > this._cameraZoomMax)
            {
                this._currentZoom = this._cameraZoomMax;
            }
            else if (this._currentZoom < this._cameraZoomMin)
            {
                this._currentZoom = this._cameraZoomMin;
            }

            this._targetTime = 0;
            this._targetZoom = new Vector3(0, this._currentZoom - this._currentRotate, -this._currentZoom * (this._currentRotate * .25f));
        }
    }
}