using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minecart : MonoBehaviour
{
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Vector3Int tilePos = MapGeneration.MapSingleton.map.WorldToCell(transform.position);
        tilePos.z = 0;

        Move(tilePos);

        var tile = MapGeneration.MapSingleton.map.GetTile(tilePos);
        if (tile != null)
        {
            if (tile.name != "SandFloor") Destroy(gameObject);
        }
    }

    void Move(Vector3Int tilePos)
    {
        var tileFront = MapGeneration.MapSingleton.map.GetTile(tilePos + new Vector3Int(0,1,0));
        if (tileFront.name != "SandFloor")
        {
            var tileRight = MapGeneration.MapSingleton.map.GetTile(tilePos + new Vector3Int(1, 0, 0));
            var tileLeft = MapGeneration.MapSingleton.map.GetTile(tilePos + new Vector3Int(-1, 0, 0));
            if (tileRight.name == "SandFloor" && tileRight != null) transform.position = MapGeneration.MapSingleton.map.CellToWorld(tilePos + new Vector3Int(1, 0, 0)) + new Vector3(0.75f,0,0);
            else if (tileLeft.name == "SandFloor" && tileRight != null) transform.position = MapGeneration.MapSingleton.map.CellToWorld(tilePos + new Vector3Int(-1, 0, 0)) + new Vector3(0.75f, 0, 0);
        }
    }
}
