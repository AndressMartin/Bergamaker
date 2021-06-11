using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridGlobal : GridManager
{

    void Awake()
    {
        grid = GetComponent<Grid>();
        tileMapRange = grid.transform.Find("SelectionRange").GetComponent<Tilemap>();
        tileMapAoe = grid.transform.Find("SelectionAoe").GetComponent<Tilemap>();
        chao = grid.transform.Find("Chao").GetComponent<Tilemap>();
        paredes = grid.transform.Find("Paredes").GetComponent<Tilemap>();
        buracos = grid.transform.Find("Buracos").GetComponent<Tilemap>();
        EffectsMap = grid.transform.Find("TerrainEffects").GetComponent<Tilemap>();
        tintTile = Resources.Load<TileBase>("Tiles/Grid/whiteblock");
    }

    public void CleanArea(List<Vector3> _tiles, Tilemap tilemap)
    {
        _tiles.Clear();
         tilemap.ClearAllTiles();
    }
    public void CleanAllTileMaps(List<Vector3> tilesRange, List<Vector3> tilesAoe)
    {
        foreach(var tile in tilesRange)
        {
            tileMapRange.SetTile((Vector3Int.FloorToInt(tile)), null); // Remove tile at 0,0,0
        }
        //tileMapRange.ClearAllTiles();
        foreach (var tile in tilesAoe)
        {
            tileMapAoe.SetTile((Vector3Int.FloorToInt(tile)), null); // Remove tile at 0,0,0
        }
        //tileMapAoe.ClearAllTiles();
    }
    public void FindWalls(List<Vector3> _tiles, List<Vector3> tilesIgnore)
    {
        foreach (Vector3 tile in _tiles)
        {
            Tile.ColliderType tileType = paredes.GetColliderType(Vector3Int.FloorToInt(tile));
            if (tileType != Tile.ColliderType.None)
            {
                tilesIgnore.Add(tile);
            }
            Tile.ColliderType tileType2 = buracos.GetColliderType(Vector3Int.FloorToInt(tile));
            if (tileType2 != Tile.ColliderType.None)
            {
                tilesIgnore.Add(tile);
            }
        }
    }



    private void FindObjects()
    {
        throw new NotImplementedException();
    }
    private void FindTerrain()
    {
        throw new NotImplementedException();
    }

    public void FillGrid(List<Vector3> _tiles, Tilemap _tilemap, TileBase tileBase)
    {
        bool continuePaintingTile = true;
        for (var i = 0; i < _tiles.Count; i++)
        {
            //TODO LATER: IT DOES NOT FIND THE IGNORED TILES NOR DOES IT HAVE A REFERENCE TO IT. CALL FINDWALLS HERE.
            //foreach (Vector3 tile in tilesIgnore)
            //{
            //    if (_tiles[i] == tile && continuePaintingTile == true)
            //    {
            //        continuePaintingTile = false;
            //    }
            //}
            if (continuePaintingTile)
                _tilemap.SetTile(Vector3Int.FloorToInt(_tiles[i]), tileBase);
            else
                continuePaintingTile = true;
        }
    }
    public void FillGrid(List<Vector3> _tiles, Tilemap _tilemap, List<Vector3> tilesIgnore)
    {
        bool continuePaintingTile = true;
        for (var i = 0; i < _tiles.Count; i++)
        {
            foreach (Vector3 tile in tilesIgnore)
            {
                if (_tiles[i] == tile && continuePaintingTile == true)
                {
                    continuePaintingTile = false;
                }
            }
            if (continuePaintingTile)
            {
                 _tilemap.SetTile(Vector3Int.FloorToInt(_tiles[i]), tintTile);
            }
            else
                continuePaintingTile = true;
        }
    }
    public void SetTileColor(bool boo, Color color, Vector3Int position, Tilemap tilemap)
    {
        // Flag the tile, inidicating that it can change colour.
        // By default it's set to "Lock Color".
        if (boo)
            tilemap.SetTileFlags(position, TileFlags.None);
        else
            tilemap.SetTileFlags(position, TileFlags.LockColor);

        // Set the color.
        tilemap.SetColor(position, color);
    }

    public void PaintGridForFoundEntities(List<Transform> entities, List<Vector3> tiles)
    {
        if (entities != null)
        {
            for (var i = 0; i < tiles.Count; i++)
            {
                foreach (Transform entity in entities)
                {
                    BoxCollider2D childCollider = entity.transform.GetChild(0).GetComponent<BoxCollider2D>();
                    var V2EntityPos = childCollider.ClosestPoint(tiles[i]);
                    if (grid.LocalToCell(V2EntityPos) == tiles[i] && tileMapRange.GetTileFlags(Vector3Int.FloorToInt(tiles[i])) == TileFlags.LockColor)
                    {
                        SetTileColor(true, Color.yellow, chao.LocalToCell(V2EntityPos), tileMapRange);
                    }
                }
            }
        }
    }

    public void PaintGridForAOE(List<Vector3> _tiles)
    {
        foreach (Vector3 tile in _tiles)
        {
            SetTileColor(true, Color.red, Vector3Int.FloorToInt(tile), tileMapAoe);
        }
    }

    //For quick unsearch


    //Single Search

    //Multiple Targets??
    //TODO: Not implemented

    public void ApplyEffectsOnTiles(List<Vector3> area, TerrainEffects effectType)
    {
        FillGrid(area, EffectsMap, FindEffectsTileBaseOfName(effectType));
        ApplyCollider(area, EffectsMap);
        EffectsMap.GetComponent<TerrainEffectsManagement>().GetEffectParams(area, effectType);
    }

    private void ApplyCollider(List<Vector3> area, Tilemap effectsMap)
    {
        foreach (Vector3 tile in area)
        {
            effectsMap.SetColliderType(Vector3Int.FloorToInt(tile), Tile.ColliderType.Grid);
        }
    }

    private TileBase FindEffectsTileBaseOfName(TerrainEffects effectType)
    {
        TileBase tileBaseName = Resources.Load<TileBase>("Tiles/Animated Tiles/" + effectType + "AnimTile");
        return tileBaseName;
    }
}


