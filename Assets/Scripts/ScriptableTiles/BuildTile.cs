using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TowerDefense.ScriptableTiles
{
    public class BuildTile : Tile
    {
        public Sprite buildSprite;

        /// <summary>
        /// Changes the tiles sprite to the correct sprites based on the situation
        /// </summary>
        /// <param name="location">The location of this sprite</param>
        /// <param name="tilemap">A reference to the tilemap, that this tile belongs to</param>
        /// <param name="tileData">A reference to the actual object, that this tile belongs to</param>
        public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = buildSprite;
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/Tiles/BuildTile")]
        public static void CreateWaterTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Buildtile", "New Buildtile", "asset", "Save buildtile", "Assets");
            if (path == "")
            {
                return;
            }

            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<BuildTile>(), path);
        }

#endif

    }
}