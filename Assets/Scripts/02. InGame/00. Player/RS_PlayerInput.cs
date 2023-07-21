using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // RockSpirits_PlayerInput
    //      플레이어 인풋 관리 클래스
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class RS_PlayerInput : RS_Player
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable]
        private Vector3 moveDirection;
        private float moveSpeed = 4f;
        #endregion
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

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

        #region [Update]
        void Update()
        {
            bool isaction = (moveDirection != Vector3.zero);
            if(isaction)
            {
                playerTransform.rotation = Quaternion.LookRotation(moveDirection);
                playerTransform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
            }
        }
        #endregion
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Actions
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //#region [Actions] Move
        //public void OnMove(InputValue value)
        //{
        //    if (value.isPressed)
        //    {
        //        PlayerActionState = RS_Player.ePlayerActionState.WALK;
        //        Vector2 input = value.Get<Vector2>();
        //        moveDirection = new Vector3(input.x, 0f, input.y);
        //    }
        //    else
        //        PlayerActionState = RS_Player.ePlayerActionState.IDLE;
        //}
        //#endregion

        //#region [Actions] Move
        //public void OnMove(InputAction.CallbackContext context)
        //{

        //}
        //#endregion

    }

}