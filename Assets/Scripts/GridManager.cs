using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{

    public Grid grid;
    public Tilemap tileMapRange;
    public Tilemap tileMapAoe;
    public TileBase tintTile;
    public Tilemap chao;
    public Tilemap paredes;
    public Tilemap buracos;
    public Tilemap EffectsMap;

    void Start()
    {
        grid = FindObjectOfType<Grid>();
        tileMapRange = grid.transform.Find("SelectionRange").GetComponent<Tilemap>();
        tileMapAoe = grid.transform.Find("SelectionAoe").GetComponent<Tilemap>();
        chao = grid.transform.Find("Chao").GetComponent<Tilemap>();
        paredes = grid.transform.Find("Paredes").GetComponent<Tilemap>();
        buracos = grid.transform.Find("Buracos").GetComponent<Tilemap>();
        EffectsMap = grid.transform.Find("TerrainEffects").GetComponent<Tilemap>();
        tintTile = Resources.Load<TileBase>("Tiles/Grid/whiteblock");
    }
}


