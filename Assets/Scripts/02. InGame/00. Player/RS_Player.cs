using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // RockSpirits_Player
    // 
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class RS_Player : ObjectBase
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // enum
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        #region [Enum] 플레이어 액션 상태
        public enum ePlayerActionState
        {
            IDLE,
            WALK,
            RUN,
            DASH,
            INTERACTION,
            DASH_ATTACK,
            ATTACK_LIGHT_COMBO_STEP_1,
            ATTACK_LIGHT_COMBO_STEP_2,
            ATTACK_LIGHT_COMBO_STEP_3,
            ATTACK_HEAVY_COMBO_STEP_1,
            ATTACK_HEAVY_COMBO_STEP_2,
            HIT,
            DIE,
        }
        #endregion

        #region [Enum] PlayerDirection 
        public enum ePlayerDirection
        {
            FORWARD,
            BACK,
            LEFT,
            RIGHT,
            FORWARDLEFT,
            FORWARDRIGHT,
            BACKLEFT,
            BACKRIGHT,
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Nested] 
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable]
        public ePlayerActionState PlayerActionState;
        public ePlayerDirection PlayerDirection;
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Property] Transform
        public Transform playerTransform => transform;
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 0. Base Methods
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Init] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Awake()
        {
            base.Awake();
        }
        public override void Start()
        {
            base.Start();
        }
        #endregion

        #region [Move]
        void MovePosition()
        {
            //if (Input.GetKey(KeyCode.A))
            //{
            //    transform.position += Vector3.left * Time.deltaTime;
            //}
            //if (Input.GetKey(KeyCode.D))
            //{
            //    transform.position += Vector3.right * Time.deltaTime;
            //}
            //if (Input.GetKey(KeyCode.W))
            //{
            //    transform.position += Vector3.forward * Time.deltaTime;
            //}
            //if (Input.GetKey(KeyCode.S))
            //{
            //    transform.position += Vector3.back * Time.deltaTime;
            //}

        }
        #endregion

        void Update()
        {
            MovePosition();
            //var index = TileMap_StageBase.Instance.WorldToTile(transform.position);
            //Debug.Log(index);
        }
    }

}