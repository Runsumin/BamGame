using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSM.Game
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
            public float Delaytime;
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

        public eDirection PlayerDir;        // 플레이어 방향
        public eDirection CameraDir;        // 카메라가 바라보는 방향

        private Vector3 fixforward = new Vector3(0, 0, 1);
        #endregion

        #region [Variable] Input Delay
        private bool InputAble;   // 현재 입력이 가능한지.        
        private float flowTime;
        #endregion

        #region [Variable] PlayerTail
        public List<Item_Trash> PlayerTail = new List<Item_Trash>();
        #endregion

        #region [Variable] PlayerRoot
        public List<Vector3> PlayerRoute = new List<Vector3>();
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Property] TileMap Coordinate
        public int NowPlayerTileIndexX => (int)TileMap_StageBase.Instance.WorldToTileX(transform.position.x);
        public int NowPlayerTileIndexZ => (int)TileMap_StageBase.Instance.WorldToTileZ(transform.position.z);
        public Vector3 NowPlayerTilePos => TileMap_StageBase.Instance.WorldToTile(transform.position);
        public TileBase NowPlayerTile => TileMap_StageBase.Instance.GetTileBase(NowPlayerTileIndexX, NowPlayerTileIndexZ);
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 0. Base Methods
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Init] Start
        public override void Start()
        {
            base.Start();
            PlayerDir = eDirection.FORWARD;
            PlayerDirection = Vector3.forward;
        }
        #endregion

        #region [Update]
        void Update()
        {
            InputDelay();           // 유저 인풋 딜레이 조절
            PlayerInput();          // 유저 인풋 
            PlayerRouteCheck();      // 경로 저장
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
            if (Input.GetKeyDown(KeyCode.A))
            {
                PlayerDirection = new Vector3(-transform.forward.z, 0, transform.forward.x);
                PlayerDir = eDirection.LEFT;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                PlayerDirection = new Vector3(transform.forward.z, 0, -transform.forward.x);
                PlayerDir = eDirection.RIGHT;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                PlayerDirection = transform.forward;
                PlayerDir = eDirection.FORWARD;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                PlayerDirection = -transform.forward;
                PlayerDir = eDirection.BACK;
            }

            //switch (PlayerDir)
            //{
            //    case eDirection.FORWARD:
            //    case eDirection.BACK:
            //        if (Input.GetKeyDown(KeyCode.A))
            //        {
            //            PlayerDirection = new Vector3(-transform.forward.z, 0, transform.forward.x);
            //            PlayerDir = eDirection.LEFT;
            //        }
            //        if (Input.GetKeyDown(KeyCode.D))
            //        {
            //            PlayerDirection = new Vector3(transform.forward.z, 0, -transform.forward.x);
            //            PlayerDir = eDirection.RIGHT;
            //        }
            //        break;
            //    case eDirection.LEFT:
            //    case eDirection.RIGHT:
            //        if (Input.GetKeyDown(KeyCode.W))
            //        {
            //            PlayerDirection = transform.forward;
            //            PlayerDir = eDirection.FORWARD;
            //        }
            //        if (Input.GetKeyDown(KeyCode.S))
            //        {
            //            PlayerDirection = -transform.forward;
            //            PlayerDir = eDirection.BACK;
            //        }
            //        break;
            //}
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
            Debug.Log(" x : " + nowtile.TileIndexX + " z : " + nowtile.TileIndexZ);

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
                CameraDir = eDirection.FORWARD;
                transform.forward = Vector3.forward;
            }
            // Right
            else if (finalAngle >= 45 && finalAngle < 135)
            {
                CameraDir = eDirection.RIGHT;
                transform.forward = Vector3.right;
            }
            // Back
            else if (finalAngle >= 135 || finalAngle < -135)
            {
                CameraDir = eDirection.BACK;
                transform.forward = Vector3.back;
            }
            // Left
            else if (finalAngle >= -135 && finalAngle <= -45)
            {
                CameraDir = eDirection.LEFT;
                transform.forward = Vector3.left;
            }
        }
        #endregion

        #region [DirectionSetting] Player Move
        private void Move()
        {
            flowTime += Time.deltaTime;
            if(flowTime >= playerSetting.Delaytime)
            {
                transform.position = MoveByTile(PlayerDir);
                flowTime = 0;
            }
        }
        #endregion

        #region [DirectionSetting] PlayerMoveBytile
        private Vector3 MoveByTile(eDirection dir)
        {
            var targetTile = TileMap_StageBase.Instance.GetNeighborTile(NowPlayerTilePos, dir);

            var final = targetTile.gameObject.transform.position;

            PlayerRoute.Add(transform.position);

            return new Vector3(final.x - 2.5f, transform.position.y, final.z + 2.5f);
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 2. Player Collision
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Player_Collision] TriggerEnter

        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 3. Player Root
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [PlayerRoot] PlayerRootCheck
        public void PlayerRouteCheck()
        {
            var cnt = PlayerTail.Count;
            for (int i = 0; i < cnt; i++)
            {
                PlayerTail[i].transform.position = PlayerRoute[PlayerRoute.Count - 1 - i];
            }
        }
        #endregion

    }

}

