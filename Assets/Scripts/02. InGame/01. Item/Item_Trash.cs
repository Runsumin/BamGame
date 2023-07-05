using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // Item_Trash
    // 
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class Item_Trash : InterObject_Base
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Enum
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public enum eTrashState
        {
            OnField,
            PlayerStack,
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [NestedClass] Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        //[Serializable]
        //public class NSetting
        //{

        //}
        //public NSetting Setting = new NSetting();
        #endregion



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        // 
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] Base
        private Collider ItemCollision;
        public eTrashState Item_TrashState;
        public int Index;
        #endregion



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Property] Base
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public int TileIndexX => (int)TileMap_StageBase.Instance.WorldToTileX(transform.position.x);
        public int TileIndexZ => (int)TileMap_StageBase.Instance.WorldToTileZ(transform.position.z);
        #endregion




        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 0. Base Methods
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Init] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public override void Start()
        {
            base.Start();
            ObjectType = InterObject_Base.etype.Item;
            Item_TrashState = eTrashState.OnField;
            ItemCollision = GetComponent<Collider>();
            if (ItemCollision == null)
            {
                gameObject.AddComponent<Collider>();
                ItemCollision = GetComponent<Collider>();
            }
        }
        #endregion

        #region [Destroy]
        public override void Destroy()
        {
            base.Destroy();
            Destroy(gameObject);
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Item Collision
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        private void OnTriggerEnter(Collider other)
        {
            if (Item_TrashState == eTrashState.OnField && other.gameObject.name == "Player")
            {
                var playerTail = other.gameObject.GetComponent<Player>().PlayerTail;
                var playerRoute = other.gameObject.GetComponent<Player>().PlayerRoute;

                var playerdir = other.gameObject.GetComponent<Player>().PlayerDir;
                switch (ObjectType)
                {
                    case InterObject_Base.etype.Item:
                        Index = playerTail.Count;
                        playerTail.Add(this);
                        Item_TrashState = eTrashState.PlayerStack;
                        transform.SetParent(other.transform);
                        transform.position = playerRoute[playerRoute.Count - playerTail.Count];
                        break;
                    case InterObject_Base.etype.Obstacle:
                        break;
                    case InterObject_Base.etype.Portal:
                        break;
                }

            }
        }
    }

}
