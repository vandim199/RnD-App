using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.IO;
using System.Linq;

public class MapGeneration : MonoBehaviour
{
    private static MapGeneration mapSingleton;
    public static MapGeneration MapSingleton { get { return mapSingleton; } }

    public float gridWidth;
    public int desiredSpaces;
    public Grid grid;
    public Tilemap map;

    public TileInfo tileInfo;
    public PresetOptions presetPaths;
    //public string[] presetPaths;

    public float noiseScale;
    public float noiseTolerance;

    private int counter = 0;
    private int lastY = 0;

    public float mapSpeed;
    public float spawnInterval;

    public int spawnY;

    public GameObject coinPrefab;


    private TextAsset lastPreset;

    void Awake()
    {
        if (mapSingleton != null && mapSingleton != this) Destroy(gameObject);
        else mapSingleton = this;

    }

    void Start()
    {
        var camWidth = Camera.main.ScreenToWorldPoint(new Vector2(0,0));
        gridWidth = Mathf.Abs(camWidth.x * 2);
        
        float width =  gridWidth / (float)desiredSpaces;
        grid.cellSize = new Vector3(width,width,0);

        coinPrefab.transform.localScale = new Vector3(width, width, 1);

        Generate(0, 10, presetPaths.startPreset);
        Generate(10, 10, presetPaths.options[UnityEngine.Random.Range(0, presetPaths.options.Count)]);
        Generate(20, 10, presetPaths.options[UnityEngine.Random.Range(0, presetPaths.options.Count)]);

        CalculateMapMovement();

        map.orientationMatrix = new Matrix4x4(new Vector4(width,0,0,0),
                                              new Vector4(0,width,0,0),
                                              new Vector4(0,0,1,0),
                                              new Vector4(0,0,0,1));

        //counter = (int)spawnInterval - ((int)grid.cellSize.y * 30);
    }

    void FixedUpdate()
    {
        //CalculateMapMovement();

        counter++;

        GameManager.GameManagerSingleton.AddScore(1);

        if(counter >= spawnInterval)
        {
            counter = 0;

            Generate(lastY, spawnY, presetPaths.options[UnityEngine.Random.Range(0, presetPaths.options.Count)]);

            Despawn(lastY - spawnY * 4);
        }
    }

    void CalculateMapMovement()
    {
        spawnInterval = grid.cellSize.y * spawnY / mapSpeed;
    }

    void Generate(int startingY, int rows, TextAsset chosenPreset)
    {
        int border = desiredSpaces / 2;

        string[] text;

        text = chosenPreset.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        text = text.Where(a => a.Contains(",")).ToArray();


        for (int y = 0 + startingY; y < rows + startingY; y++)
        {
            var line = text[text.Length - (y - startingY) - 1];

            //int[] lineArr = Array.ConvertAll(line.Split(','), s=> int.Parse(s));
            var lineArr = line.Split(',');

            lineArr = lineArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            for (int x = -border; x < border; x++)
            {
                if (x == -border || x == border - 1)
                {
                    map.SetTile(new Vector3Int(x, y, 0), tileInfo.spritemap[2]);
                    continue;
                }

                
                //float perlin = Mathf.PerlinNoise((float)(x + desiredSpaces / 2) * noiseScale, (float)y * noiseScale);

                //map.SetTile(new Vector3Int(x, y, 0), perlin > 0.3f ? tile1 : tile2);
                map.SetTile(new Vector3Int(x, y, 0), tileInfo.spritemap[int.Parse(lineArr[x+border-1])]);

                if(int.Parse(lineArr[x + border - 1]) == 1)
                {
                    if (UnityEngine.Random.Range(0, 10) == 1)
                    {
                        Instantiate(coinPrefab, map.GetCellCenterWorld(new Vector3Int(x, y, 0)), Quaternion.identity);
                    }
                }
                
            }
        }

        lastPreset = chosenPreset;
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
