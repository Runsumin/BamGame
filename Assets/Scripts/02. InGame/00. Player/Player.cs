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

        #region [Enum] Player Direction
        public enum eDirection
        {
            FORWARD, BACK, LEFT, RIGHT, Max
        }
        #endregion

        #region [Enum] Player InputSetting
        public enum ePlayerInputState
        {
            Mouse, KeyBoard
        }
        #endregion

#region [Enum] 
#endregion

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
            public ePlayerInputState InputSetting;
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

        public eDirection PlayerDir;        // �÷��̾� ����
        public eDirection CameraDir;        // ī�޶� �ٶ󺸴� ����

        private Vector3 fixforward = new Vector3(0, 0, 1);
        #endregion

        #region [Variable] Input Delay
        private bool InputAble;   // ���� �Է��� ��������.        
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
            //InputDelay();           // ���� ��ǲ ������ ����
            switch (playerSetting.InputSetting)
            {
                case ePlayerInputState.Mouse:
                    SetPlayerDirection();
                    break;
                case ePlayerInputState.KeyBoard:
                    PlayerInput();          // ���� ��ǲ 
                    break;
            }
            PlayerRouteCheck();     // ��� ����
            GameEndCheck();         // ���� ���� üũ
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
            //if (Input.GetKeyDown(KeyCode.A))
            //{
            //    PlayerDirection = new Vector3(-transform.forward.z, 0, transform.forward.x);
            //    PlayerDir = eDirection.LEFT;
            //}
            //if (Input.GetKeyDown(KeyCode.D))
            //{
            //    PlayerDirection = new Vector3(transform.forward.z, 0, -transform.forward.x);
            //    PlayerDir = eDirection.RIGHT;
            //}
            //if (Input.GetKeyDown(KeyCode.W))
            //{
            //    PlayerDirection = transform.forward;
            //    PlayerDir = eDirection.FORWARD;
            //}
            //if (Input.GetKeyDown(KeyCode.S))
            //{
            //    PlayerDirection = -transform.forward;
            //    PlayerDir = eDirection.BACK;
            //}

            switch (PlayerDir)
            {
                case eDirection.FORWARD:
                case eDirection.BACK:
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
                    break;
                case eDirection.LEFT:
                case eDirection.RIGHT:
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
                    break;
            }
        }
        #endregion

        #region [PlayerInput] DelayInput
        private void InputDelay()
        {
            var instance = TileMap_StageBase.Instance;
            if (instance == null) return;

            // ���� �÷��̾� ��ġ
            var playerposition = transform.position;
            // ���� �÷��̾ ��ġ�� Ÿ�� ��ġ
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
            if (flowTime >= playerSetting.Delaytime)
            {
                switch (playerSetting.InputSetting)
                {
                    case ePlayerInputState.Mouse:
                        transform.position = MoveByTile(CameraDir);
                        break;
                    case ePlayerInputState.KeyBoard:
                        transform.position = MoveByTile(PlayerDir);
                        break;
                }
                flowTime = 0;
            }
        }
        #endregion

        #region [DirectionSetting] PlayerMoveBytile
        private Vector3 MoveByTile(eDirection dir)
        {
            var targetTile = TileMap_StageBase.Instance.GetNeighborTile(NowPlayerTilePos, dir);

            if (targetTile == null)
            {
                UnityEditor.EditorApplication.isPlaying = false;
                return new Vector3(0, 0, 0);
            }

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

        public void GameEndCheck()
        {
            // ��ֹ�
            if (NowPlayerTile.TileBaseSetting.TileType == TileBase.eTileType.Obstacle)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
            // ����
            foreach (var data in PlayerTail)
            {
                var nowtailtile = TileMap_StageBase.Instance.GetTileBase(data.TileIndexX, data.TileIndexZ);
                // ������ ��ֹ�
                if (nowtailtile.TileBaseSetting.TileType == TileBase.eTileType.Obstacle)
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                }
                // ������ �÷��̾�
                if (nowtailtile.TileIndexX == NowPlayerTileIndexX && nowtailtile.TileIndexZ == NowPlayerTileIndexZ)
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                }
            }
            // �÷��̾�� ����
        }

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

