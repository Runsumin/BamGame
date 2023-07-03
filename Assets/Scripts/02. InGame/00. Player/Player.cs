using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // Player
    //
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class Player : ObjectBase
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Enum
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public enum eDirection
        {
            FORWARD, BACK, LEFT, RIGHT, Max
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Nested] PlayerSetting
        [Serializable]
        public class NPlayerSetting
        {
            public float Speed;
        }
        #endregion
        public NPlayerSetting playerSetting = new NPlayerSetting();


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] PlayerDirection
        public Vector3 PlayerDirection;
        public float HAxis;
        public float VAxis;
        public Transform Cameratrs;

        public eDirection PlayerDir;
        public eDirection CameraDir;

        private Vector3 fixforward = new Vector3(0, 0, 1);
        #endregion

        #region [Variable] Input Delay
        private bool InputAble;   // 현재 입력이 가능한지.

        #endregion
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 0. Base Methods
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Init] Start
        public override void Start()
        {
            base.Start();
            PlayerDirection = Vector3.forward;
        }
        #endregion

        #region [Update]
        void Update()
        {
            PlayerInput();
            SetPlayerDirection();
        }
        #endregion

        #region [FixedUpdate]
        private void FixedUpdate()
        {
            Move();
        }

        #endregion
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Player Input
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [PlayerInput] KeyInput
        private void PlayerInput()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                PlayerDirection = transform.forward;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                PlayerDirection = -transform.forward;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                PlayerDirection = new Vector3(-transform.forward.z, 0, transform.forward.x);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                PlayerDirection = new Vector3(transform.forward.z, 0, -transform.forward.x);
            }
        }
        #endregion

        #region [PlayerInput] DelayInput
        private void InputDelay()
        {
            var instance = TileMap_StageBase.Instance;
            if (instance == null) return;

            // 현재 플레이어 위치
            var playerposition = transform.position;
            // 현재 플레이어가 위치한 타일 위치
            var nowPlayerTile = instance.WorldToTile(playerposition);
            var nowtile = instance.GetTileBase((int)nowPlayerTile.x, (int)nowPlayerTile.z);

        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Player Direction Setting
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [DirectionSetting] GetCameraDirection
        public Vector3 GetCameraDirection()
        {
            var CamDirection = transform.position - Cameratrs.transform.position;
            var camdirnor = new Vector3(CamDirection.x, 0, CamDirection.z).normalized;

            return camdirnor;
        }
        #endregion

        #region [DirectionSetting] SetPlayerDirection
        public void SetPlayerDirection()
        {
            var CamDir = GetCameraDirection();
            var playerdir = fixforward;
            var playerdirright = Vector3.Cross(Vector3.up, playerdir);

            float angle = Vector3.Angle(CamDir, playerdir);
            float sign = Mathf.Sign(Vector3.Dot(CamDir, playerdirright));
            float finalAngle = sign * angle;

            // Forward
            if (finalAngle > -45 && finalAngle < 45)
            {
                PlayerDir = eDirection.FORWARD;
                transform.forward = Vector3.forward;
            }
            // Right
            else if (finalAngle >= 45 && finalAngle < 135)
            {
                PlayerDir = eDirection.RIGHT;
                transform.forward = Vector3.right;
            }
            // Back
            else if (finalAngle >= 135 || finalAngle < -135)
            {
                PlayerDir = eDirection.BACK;
                transform.forward = Vector3.back;
            }
            // Left
            else if (finalAngle >= -135 && finalAngle <= -45)
            {
                PlayerDir = eDirection.LEFT;
                transform.forward = Vector3.left;
            }
        }
        #endregion

        #region [DirectionSetting] Player Move
        private void Move()
        {
            transform.position += PlayerDirection * Time.deltaTime * playerSetting.Speed;
        }
        #endregion

    }

}

