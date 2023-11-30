using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace Pasta
{
    public class SurfaceChecker : MonoBehaviour
    {
        public Tilemap tileMap;
        private Vector2 originPoint;
        private Vector2Int originPoint2int;
        private Vector3Int originPoint3int;
        private TileBase tile;
        public Level level;
        private Region region;
        private bool gotTilemap = false;
        [SerializeField] private Tile[] woodtiles;
        // Start is called before the first frame update
        private void Awake()
        {
            level = FindFirstObjectByType<Level>();
            if(level != null)
            {
                tileMap = level.tilemap;
            }
            
        }

        private void OnEnable()
        {
            EventActions.OnEvent += OnEvent;
        }

        private void OnDisable()
        {
            EventActions.OnEvent -= OnEvent;
        }

        private void OnEvent(EventContext obj)
        {
            if (obj.EventType != EventActionType.OnRoomEnter) return;
            StartCoroutine(tilemap());
        }

        
        IEnumerator tilemap()
        {
            yield return new WaitForSeconds(0.2f);
            level = FindFirstObjectByType<Level>();
            tileMap = level.tilemap;
        }

        // Update is called once per frame
        void Update()
        {
            
            if(tileMap != null)
            {
                originPoint = transform.position;
                originPoint2int = Vector2Int.RoundToInt(originPoint);
                originPoint3int = ((Vector3Int)originPoint2int);
                if (tileMap.HasTile(originPoint3int))
                {
                    tile = tileMap.GetTile(originPoint3int);
                    for (int i = 0; i < woodtiles.Length; i++)
                    {
                        if (tile = woodtiles[i])
                        {
                            // play sound. 
                        }
                    }
                }
            }
            
        }
    }
}
