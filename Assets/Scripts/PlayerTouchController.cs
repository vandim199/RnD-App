using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerTouchController : MonoBehaviour
{
    float touchStart = 0;
    public float holdInterval = 1000;
    Vector3Int lastPos;

    public TileInfo sandTile;
    //public UnityEngine.Tilemaps.AnimatedTile;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) touchStart = Time.time; 
            
            GetTile(touch.position);
        }

        if(Input.GetMouseButtonDown(0))
        {
            touchStart = Time.time;
        }

        if(Input.GetMouseButton(0))
        {
            var tilePos = GetTile(Input.mousePosition);
            if (Time.time >= touchStart + holdInterval)
            {
                //MapGeneration.MapSingleton.SwapState(tilePos);
                touchStart = Time.time;
            }
        }
    }

    public Vector3Int GetTile(Vector3 position)
    {
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(position);
        touchPos.z = 0;
        Vector2 tapPos = new Vector2(touchPos.x, touchPos.y);

        //Debug.Log(mousePos);

        Vector3Int tilePos = MapGeneration.MapSingleton.map.WorldToCell(touchPos);

        if (lastPos != tilePos) touchStart = Time.time;
        //Debug.Log(tilePos);

        //var tile = MapGeneration.MapSingleton.map.GetInstantiatedObject(tilePos);
        var tile = MapGeneration.MapSingleton.map.GetTile(tilePos);
        //Debug.Log(tile);

        if (tile != null)
        {
            transform.position = MapGeneration.MapSingleton.map.CellToWorld(tilePos);
            //Debug.Log(tile.name);
        }

        

        lastPos = tilePos;

        return tilePos;
    }
}
