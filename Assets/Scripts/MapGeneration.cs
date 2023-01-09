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

    public Grid grid;

    [SerializeField]
    private float mapSpeed;
    [SerializeField]
    private int presetHeight;
    [SerializeField]
    private float gridWidth;
    [SerializeField]
    private int desiredSpaces;
    [SerializeField]
    private Tilemap map;
    [SerializeField]
    private TileInfo tileInfo;
    [SerializeField]
    private PresetOptions levelPresets;
    [SerializeField]
    public GameObject coinPrefab;

    private float spawnInterval;
    private int counter = 0;
    private int lastY = 0;
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

        Generate(0, 10, levelPresets.startPreset);
        Generate(10, 10, levelPresets.options[1]);
        Generate(20, 10, levelPresets.options[UnityEngine.Random.Range(0, levelPresets.options.Count)]);

        CalculateMapMovement();

        map.orientationMatrix = new Matrix4x4(new Vector4(width,0,0,0),
                                              new Vector4(0,width,0,0),
                                              new Vector4(0,0,1,0),
                                              new Vector4(0,0,0,1));
    }

    void FixedUpdate()
    {
        counter++;

        GameManager.GameManagerSingleton.AddScore(1);

        if(counter >= spawnInterval)
        {
            counter = 0;

            Generate(lastY, presetHeight, levelPresets.options[UnityEngine.Random.Range(0, levelPresets.options.Count)]);

            Despawn(lastY - presetHeight * 4);
        }
    }

    void CalculateMapMovement()
    {
        spawnInterval = grid.cellSize.y * presetHeight / mapSpeed;
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

            var lineArr = line.Split(',');

            lineArr = lineArr.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            for (int x = -border; x < border; x++)
            {
                if (x == -border || x == border - 1)
                {
                    map.SetTile(new Vector3Int(x, y, 0), tileInfo.spritemap[2]);
                    continue;
                }

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
        for (int y = row; y > row - presetHeight; y--)
        {
            for (int x = -desiredSpaces/2; x < desiredSpaces/2; x++)
            {
                map.SetTile(new Vector3Int(x, y, 0), null);
            }
        }
    }
}
