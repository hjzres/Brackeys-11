using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RandomizeTerrainHeights : MonoBehaviour
{
    public int width, height;
    public Vector3 startPosition;

    public void GenerateHeights()
    {
        int fDB = (int)Mathf.Floor(width / 50);
        int tDB = (int)Mathf.Floor((width % 50) / 20);
        //int ttDB = (int)Mathf.Floor(((width % 50) % 20) / 10);
        int ttDB = (int)Mathf.Floor((width - ((fDB * 50) + (tDB * 20))) / 10);  // either works
        print(fDB);
        print(tDB);
        print(ttDB);

        int xbills = (580 - (580 % 50)) / 50;
        int newVar = 580 - (xbills * 50);
        print(newVar);
        /*
        GameObject terrainParent = new GameObject("New Terrain", typeof(Terrain));
        terrainParent.transform.position = startPosition;

        for (int x = 1; x < width; x++)
        {
            for (int y = 1; y < height; y++)
            {
                TerrainData terrainData = new TerrainData();
                terrainData.size = new Vector3(width / 16f, 1, height / 16f);
                terrainData.name = "Terr";
                terrainData.baseMapResolution = 1024;
                terrainData.heightmapResolution = 513;
                terrainData.alphamapResolution = 512;
                terrainData.SetDetailResolution(1024, 8);

                GameObject terrain = Terrain.CreateTerrainGameObject(terrainData);
                terrain.name = "Terrain";
                terrain.transform.parent = terrainParent.transform;

                AssetDatabase.CreateAsset(terrainData, "Assets/Rendering/Terrain Data/Terr.asset");
            }
        }
        */
    }

/*
    private int heightmapResoltion          = 513;
    private int detailResolution            = 1024;
    private int detailResolutionPerPatch    = 8;
    private int controlTextureResolution    = 512;
    private int baseTextureReolution        = 1024;
    */
}
