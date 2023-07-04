using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // PlayerCameraController
    //
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class CameraController : MonoBehaviour
    {

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Enum
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public enum eTouchRange
        {
            All, Left, Right, Max,
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // NestedClass
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Nesged] CameraSetting
        [Serializable]
        public class NCameraSetting
        {
            // 0, 1.5, 2
            // 12, 180, 0
            public Vector3 OffsetPos;
            public Vector3 OffsetRot;
            public Transform Follow;
            public float MixX;          // 거리 최소값
            public float MaxX;          // 거리 최대값
            public float Distance;      // 카메라 거리  
            public float ZoomSpeed;     // 마우스 휠 줌 속도
            public float MaxDisY;       // Y축 이동 최소값
            public float MinDisY;       // Y축 이동 최대값
            public float CamRotSpeed;   // 카메라 회전 속도
        }
        public NCameraSetting CamSetting = new NCameraSetting();
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        #region [Variable]
        public eTouchRange TouchRange;

        private Vector3 _mousePosition;
        private Vector3 _mouseMovement;
        private Vector3 _mouseDownPosition;
        private Vector3 _prevMouseDownPosition;


        private float _xRotate;
        private float _yRotate;
        private float finaly;
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 0. Base Methods
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Init] Start
        public void Start()
        {
            transform.position = CamSetting.OffsetPos;
            transform.rotation = Quaternion.Euler(CamSetting.OffsetRot);
        }
        #endregion

        #region [Update]
        void Update()
        {
            _mousePosition = Input.mousePosition;
            CheckMouseEvent();
            RotateCamera();
        }
        #endregion

        #region [Mouse] CheckMouseEvent
        private void CheckMouseEvent()
        {
            if ((_mousePosition.x < 0) || (_mousePosition.y < 0) || (_mousePosition.x > Screen.width) || (_mousePosition.y > Screen.height))
            {
                _mouseMovement = Vector3.zero;
                return;
            }

            if (Input.GetMouseButtonDown(1))
            {
                _mouseDownPosition = _prevMouseDownPosition = _mousePosition;
            }

            _mouseMovement = Vector2.zero;
            {
                _mouseMovement = _mousePosition - _prevMouseDownPosition;
                _prevMouseDownPosition = _mousePosition;
            }

            if (Input.GetMouseButtonUp(1))
            {

            }

        }
        #endregion

        #region [Camera] RotateCamera
        private void RotateCamera()
        {
            if (Input.GetMouseButton(1))
            {
                _xRotate -= _mouseMovement.y * Time.deltaTime * CamSetting.CamRotSpeed;
                _yRotate += _mouseMovement.x * Time.deltaTime * CamSetting.CamRotSpeed;
                _xRotate = Mathf.Clamp(_xRotate, CamSetting.MixX, CamSetting.MaxX);
            }

            var wheelScroll = Input.mouseScrollDelta.y;
            CamSetting.Distance -= (wheelScroll * CamSetting.ZoomSpeed);
            CamSetting.Distance = Mathf.Clamp(CamSetting.Distance, CamSetting.MixX, CamSetting.MaxX);

            var rotation = Quaternion.Euler(_xRotate, _yRotate, 0);
            var position = rotation * (new Vector3(0, 0, -CamSetting.Distance)) + (CamSetting.Follow.position);
            transform.position = position;
            transform.LookAt(CamSetting.Follow.position);

        }
        #endregion
    }

}
