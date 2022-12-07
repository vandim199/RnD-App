using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            GetTile(touch.position);
        }

        if(Input.GetMouseButton(0))
        {
            GetTile(Input.mousePosition);
        }
    }

    public void GetTile(Vector3 position)
    {
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(position);
        touchPos.z = 0;
        Vector2 tapPos = new Vector2(touchPos.x, touchPos.y);

        //Debug.Log(mousePos);

        Vector3Int tilePos = MapGeneration.MapSingleton.map.WorldToCell(touchPos);

        //Debug.Log(tilePos);

        //var tile = MapGeneration.MapSingleton.map.GetInstantiatedObject(tilePos);
        var tile = MapGeneration.MapSingleton.map.GetTile(tilePos);
        //Debug.Log(tile);

        if (tile != null)
        {
            transform.position = MapGeneration.MapSingleton.map.CellToWorld(tilePos);
            //Debug.Log(tile.name);
            //gameObject.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        }
    }
}
