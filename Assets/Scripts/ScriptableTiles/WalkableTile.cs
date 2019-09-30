using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TowerDefense.ScriptableTiles
{
    public class WalkableTile : Tile
    {
        public Sprite[] tileSprites;

        /// <summary>
        /// Refreshes this tile when something changes
        /// </summary>
        /// <param name="position">The tiles position in the grid</param>
        /// <param name="tilemap">A reference to the tilemap that this tile belongs to.</param>
        public override void RefreshTile(Vector3Int position, ITilemap tilemap)
        {
            for (int y = -1; y <= 1; y++) //Runs through all the tile's neighbours 
            {
                for (int x = -1; x <= 1; x++)
                {
                    //We store the position of the neighbour 
                    var nPos = new Vector3Int(position.x + x, position.y + y, position.z);

                    if (IsWalkable(tilemap, nPos)) // If the neighbour is walkable
                    {
                        tilemap.RefreshTile(nPos); // Then we make sure to refresh the neighbour as well
                    }
                }
            }
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            string composition = string.Empty;//Makes an empty string as compostion, we need this so that we change the sprite

            for (int x = -1; x <= 1; x++)//Runs through all neighbours 
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x != 0 || y != 0) //Makes sure that we aren't checking our self
                    {
                        //If the value is a watertile
                        if (IsWalkable(tilemap, new Vector3Int(position.x + x, position.y + y, position.z)))
                        {
                            composition += 'W';
                        }
                        else
                        {
                            composition += 'E';
                        }
                    }
                }
            }

            // Cell Indexes
            //   2 4 7
            //   1   6
            //   0 3 5


            // Tile Indexes
            // 0:   _| (DLGround)
            // 
            // 1:  _   (ULGround
            //      |
            //
            // 2:  |_  (DRGround)
            //       
            // 3:  _   (URGround)
            //    |
            //
            // 4: _    (HGround)
            //
            // 5: |    (VGround)

            if (composition[4] == 'W' && composition[1] == 'W')
            {
                // if the tile above us is walkable
                // replace this tile with an elbow up  
                tileData.sprite = tileSprites[0];
            }
            else if (composition[4] == 'W' && composition[6] == 'W')
            {
                tileData.sprite = tileSprites[2];
            }
            else if (composition[3] == 'W' && composition[6] == 'W')
            {
                tileData.sprite = tileSprites[3];
            }
            else if (composition[3] == 'W' && composition[1] == 'W')
            {
                tileData.sprite = tileSprites[1];
            }
            else if (composition[3] == 'W' || composition[4] == 'W') //  && (composition[1] != 'W' && composition[6] != 'W')
            {
                tileData.sprite = tileSprites[5];
            }
            else
            {
                tileData.sprite = tileSprites[4];
            }

        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/Tiles/WalkableTile")]
        public static void CreateWalkableTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Walkabletile", "New Walkabletile", "asset", "Save walkabletile", "Assets");
            if (path == "")
            {
                return;
            }

            AssetDatabase.CreateAsset(CreateInstance<WalkableTile>(), path);
        }
#endif

        private bool IsWalkable(ITilemap tilemap, Vector3Int position)
        {
            return tilemap.GetTile(position) == this;
        }
    }
}