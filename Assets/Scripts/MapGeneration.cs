using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGeneration : MonoBehaviour
{
    public float gridWidth;
    public int desiredSpaces;
    public Grid grid;
    public Tilemap map;

    public Tile tile1;
    public Tile tile2;

    public float noiseScale;
    public float noiseTolerance;

    int test = 0;
    int lastY = 0;

    public float mapSpeed;
    public float spawnInterval;

    public int spawnY;

    // Start is called before the first frame update
    void Start()
    {
        
        float width =  gridWidth / (float)desiredSpaces;
        grid.cellSize = new Vector3(width,width,0);

        Generate(0, 30);
        //Mathf.PerlinNoise(1, 1);

        spawnInterval = grid.cellSize.y * spawnY / mapSpeed;

        //test = (int)spawnInterval - ((int)grid.cellSize.y * 30);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        grid.transform.position -= new Vector3(0, mapSpeed, 0);

        test++;


        if(test >= spawnInterval)
        {
            test = 0;

            Generate(lastY, spawnY);

            Despawn(lastY - spawnY * 4);
        }
    }

    void Generate(int startingY, int rows)
    {
        for (int y = 0 + startingY; y < rows + startingY; y++)
        {
            for (int x = -desiredSpaces / 2; x < desiredSpaces / 2; x++)
            {
                float perlin = Mathf.PerlinNoise((float)(x + desiredSpaces / 2) * noiseScale, (float)y * noiseScale);
                //Debug.Log(perlin);
                map.SetTile(new Vector3Int(x, y, 0), perlin > 0.3f ? tile1 : tile2);
            }
        }

        lastY = startingY + rows;
    }

    void Despawn(int row)
    {
        for (int y = row; y > row - spawnY; y--)
        {
            for (int x = -desiredSpaces/2; x < desiredSpaces/2; x++)
            {
                map.SetTile(new Vector3Int(x, y, 0), null);
            }
        }
    }
}
