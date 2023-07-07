using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // TileMapBase
    // 
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public enum eTileType
    {
        Quad, Hexa
    }


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

        #region [Variable] TileMapType
        public eTileType TileMapType;
        public float HexTileMapXOffSet;     // ���� Ÿ�� Ȧ���� X�� ������
        public float HexTileMapZOffSet;     // ���� Ÿ�� Ȧ���� Z�� ������
        public Vector3 OriginPosition;
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

            int indexX = 0;
            int indexZ = 0;

            if (mapsize > 0)
            {
                _tileList = new TileBase[mapsize];
                for (int i = 0; i < mapsize; i++)
                {

                    tileMaps[i].TileBaseSetting.TileIndex.x = indexX;
                    tileMaps[i].TileBaseSetting.TileIndex.y = indexZ;
                    tileMaps[i].TileBaseSetting.TileWidth = Setting.TileWidth;
                    tileMaps[i].TileBaseSetting.TileHeight = Setting.TileHeight;

                    var tile = tileMaps[i];
                    _tileList[indexZ * Setting.TileCountX + indexX] = tile;

                    indexX++;
                    if (indexX == Setting.TileCountX)
                    {
                        indexX = 0;
                        indexZ++;
                    }

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
        public Vector3 TileToWorld(Vector3 tilePos) => new Vector3(TileToWorldX(tilePos.x), -0.05f, TileToWorldZ(tilePos.z));
        public Vector3 TileToWorld(int tileX, int tileZ) => new Vector3(TileToWorldX(tileX), 0f, TileToWorldZ(tileZ));
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public float WorldToTileX(float worldPosX) => (int)(((worldPosX + 0.1f) - Setting.TileStartX) / Setting.TileWidth);
        public float WorldToTileZ(float worldPosZ) => (int)(((worldPosZ + 0.1f) - Setting.TileStartZ) / Setting.TileHeight);
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

        public TileBase GetNeighborTile(Vector3 tilePosition, Player.eDirection moveDir)
        {
            TileBase tileMap = null;
            switch (moveDir)
            {

                case Player.eDirection.FORWARD: tileMap = GetTileBase((int)tilePosition.x, (int)tilePosition.z + 1); break;
                case Player.eDirection.BACK: tileMap = GetTileBase((int)tilePosition.x, (int)tilePosition.z - 1); break;
                case Player.eDirection.LEFT: tileMap = GetTileBase((int)tilePosition.x - 1, (int)tilePosition.z); break;
                case Player.eDirection.RIGHT: tileMap = GetTileBase((int)tilePosition.x + 1, (int)tilePosition.z); break;
            }

            return tileMap;
        }
        #endregion

        #region [HexCoordinate] Tile To World
        public Vector3 HexTileToWorldPosition(Vector3 pos)
        {
            float TileSize = 3f;

            return new Vector3((int)pos.x, 0, 0) * TileSize +
                new Vector3(0, 0, (int)pos.z) * TileSize * HexTileMapXOffSet +
                ((Mathf.Abs(pos.z) % 2) == 1 ? new Vector3(1, 0, 0) * TileSize * .5f : Vector3.zero) +
                OriginPosition;
        }
        #endregion

        #region [HexCoordinate] World To Tile
        public Vector3Int WorldToHexTileIndex(Vector3 pos)
        {
            float TileSize = 3f;

            int roughx = Mathf.RoundToInt((pos - OriginPosition).x / TileSize);
            int roughz = Mathf.RoundToInt((pos - OriginPosition).z / TileSize / HexTileMapXOffSet);

            Vector3Int roughXZ = new Vector3Int(roughx, (int)pos.y, roughz);

            bool oddRow = roughz % 2 == 1;

            List<Vector3Int> neighbourXZList = new List<Vector3Int>
            {
                roughXZ + new Vector3Int(-1,0,0),
                roughXZ + new Vector3Int(+1,0,0),

                roughXZ + new Vector3Int(oddRow ? +1 : -1,0,+1),
                roughXZ + new Vector3Int(+0,0,+1),

                roughXZ + new Vector3Int(oddRow ? +1 : -1,0,-1),
                roughXZ + new Vector3Int(+0,0,-1),


            };

            Vector3Int closestXZ = roughXZ;

            foreach (Vector3Int data in neighbourXZList)
            {
                if (Vector3.Distance(pos, HexTileToWorldPosition(data)) <
                    Vector3.Distance(pos, HexTileToWorldPosition(closestXZ)))
                {
                    closestXZ = data;
                }
            }
            return closestXZ;
        }
        #endregion

        #region [HexTileMap]
        public TileBase GetNeighborHexTile(Vector3 tilePosition, Player.eHexaDirection moveDir)
        {
            TileBase tileMap = null;
            switch (moveDir)
            {

                case Player.eHexaDirection.FORWARD_LEFT:
                    {
                        //¦
                        if (tilePosition.z % 2 == 0)
                            tileMap = GetTileBase((int)tilePosition.x - 1, (int)tilePosition.z + 1);
                        //Ȧ
                        else
                            tileMap = GetTileBase((int)tilePosition.x, (int)tilePosition.z + 1);
                    }
                    break;
                case Player.eHexaDirection.FORWARD_RIGHT:
                    {
                        //¦
                        if (tilePosition.z % 2 == 0)
                            tileMap = GetTileBase((int)tilePosition.x, (int)tilePosition.z + 1);
                        //Ȧ
                        else
                            tileMap = GetTileBase((int)tilePosition.x + 1, (int)tilePosition.z + 1);
                    }
                    break;
                case Player.eHexaDirection.LEFT: tileMap = GetTileBase((int)tilePosition.x - 1, (int)tilePosition.z); break;
                case Player.eHexaDirection.RIGHT: tileMap = GetTileBase((int)tilePosition.x + 1, (int)tilePosition.z); break;
                case Player.eHexaDirection.BACK_LEFT:
                    {
                        //¦
                        if (tilePosition.z % 2 == 0)
                            tileMap = GetTileBase((int)tilePosition.x - 1, (int)tilePosition.z - 1);
                        //Ȧ
                        else
                            tileMap = GetTileBase((int)tilePosition.x, (int)tilePosition.z - 1);
                    }
                    break;
                case Player.eHexaDirection.BACK_RIGHT:
                    {
                        //¦
                        if (tilePosition.z % 2 == 0)
                            tileMap = GetTileBase((int)tilePosition.x, (int)tilePosition.z - 1);
                        //Ȧ
                        else
                            tileMap = GetTileBase((int)tilePosition.x + 1, (int)tilePosition.z - 1);
                    }
                    break;
            }

            return tileMap;
        }
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