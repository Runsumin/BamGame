using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // TileMap
    // 
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class TileMap_StageBase : ObjectBase
    {
        public static TileMap_StageBase Instance;
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Nested Class
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [NestedClass] Setting
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        [Serializable]
        public class TileMapSetting
        {
            public int TileCountX;
            public int TileCountZ;
            public float TileStartX;
            public float TileStartZ;
            public float TileWidth;
            public float TileHeight;
        }
        public TileMapSetting Setting = new TileMapSetting();
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Variable
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Variable] Base
        public Transform TileRoot;
        protected TileBase[] _tileList;
        protected TileBase StartTile;
        protected TileBase EndTile;
        #endregion



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Property
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Property] Base
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public TileBase[] TileList => _tileList;
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
            Instance = this;
        }
        public override void Start()
        {
            base.Start();
            InitTileMap();
        }
        #endregion


        #region [Init] Init TileMap
        private void InitTileMap()
        {
            var tileMaps = TileRoot.GetComponentsInChildren<TileBase>();
            var mapsize = tileMaps.Length;
            if (mapsize > 0)
            {
                _tileList = new TileBase[mapsize];
                for(int i = 0; i < mapsize; i++)
                {
                    tileMaps[i].TileBaseSetting.TileIndex = i;
                    tileMaps[i].TileBaseSetting.TileWidth = 5f;
                    tileMaps[i].TileBaseSetting.TileHeight = 5f;

                    var tile = tileMaps[i];
                    var tilex = tile.TileIndexX;
                    var tilez = tile.TileIndexZ;

                    _tileList[tilez * Setting.TileCountX + tilex] = tile;
                }
            }
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 1. Coordinate
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Coordinate]
        public Vector3 WorldToTile(Vector3 worldPos) => new Vector3(WorldToTileX(worldPos.x), 0f, WorldToTileZ(worldPos.z));
        public Vector3 TileToWorld(Vector3 tilePos) => new Vector3(TileToWorldX(tilePos.x), -0.05f, TileToWorldX(tilePos.z));
        public Vector3 TileToWorld(int tileX, int tileZ) => new Vector3(TileToWorldX(tileX), 0f, TileToWorldX(tileZ));
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public float WorldToTileX(float worldPosX) => (int)(((worldPosX + 0.5f) - Setting.TileStartX) / Setting.TileWidth);
        public float WorldToTileZ(float worldPosZ) => (int)(((worldPosZ + 0.5f) - Setting.TileStartZ) / Setting.TileHeight);
        public float TileToWorldX(float tileX) => -Setting.TileStartX + (tileX * Setting.TileWidth);
        public float TileToWorldZ(float tileZ) => -Setting.TileStartZ + (tileZ * Setting.TileHeight);
        #endregion

        #region [TileMap]
        public TileBase GetTileBase(int tileX, int tileZ)
        {
            if ((tileX < 0) || (tileX >= Setting.TileCountX) || (tileZ < 0) || (tileZ >= Setting.TileCountZ))
                return null;
            int index = tileZ * Setting.TileCountX + tileX;
            return _tileList[index];
        }

        //public TileBase GetNeighborTile(Vector3 tilePosition, eMoveDirection moveDir)
        //{
        //    TileBase tileMap = null;
        //    switch (moveDir)
        //    {
        //        case eMoveDirection.Left: tileMap = GetTileBase((int)tilePosition.x - 1, (int)tilePosition.z); break;
        //        case eMoveDirection.Up: tileMap = GetTileBase((int)tilePosition.x, (int)tilePosition.z + 1); break;
        //        case eMoveDirection.Down: tileMap = GetTileBase((int)tilePosition.x, (int)tilePosition.z - 1); break;
        //        case eMoveDirection.Right: tileMap = GetTileBase((int)tilePosition.x + 1, (int)tilePosition.z); break;
        //    }

        //    return tileMap;
        //}
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 99. Utill
        //
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region [Utill] 
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        #endregion

    }

}