using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogOfWar : MonoBehaviour
{
    public Grid grid;
    public Tilemap fog;
    public List<Vector3> tilesVision = new List<Vector3>();
    public List<Vector3> tilesFog = new List<Vector3>();
    public bool isFog;
    int vision = 5;
    int linhas = 19;
    int colunas = 26;
    int indice = 0;
    public GameObject gridHolder;
    private Player caster;
    public TileBase tintTile;
    private Vector3 previousCasterPosition;

    // Start is called before the first frame update
    void Start()
    {
        caster = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFog)
        {
            if (grid.LocalToCell(previousCasterPosition) != grid.LocalToCell(caster.transform.position))
            {
                CleanAreas();
                GetVisionArea(vision, caster.transform.GetChild(0).position, tilesVision);
                GetFogGrid();
                PaintFogGrid();
                previousCasterPosition = caster.transform.position;
            }
        }
    }

    private void CleanAreas()
    {
        tilesVision.Clear();
        tilesFog.Clear();
        fog.ClearAllTiles();
    }

    Vector3 Coordinates(int iteration, int i, Vector3 center)
    {
        if (iteration == 1) return new Vector3(center.x - (i + 1), center.y, center.z);
        if (iteration == 2) return new Vector3(center.x + (i + 1), center.y, center.z);
        if (iteration == 3) return new Vector3(center.x, center.y - (i + 1), center.z);
        if (iteration == 4) return new Vector3(center.x, center.y + (i + 1), center.z);
        else return new Vector3(0, 0, 0);
    }
    Vector3 Coordinates(int iteration, int i, int f, Vector3 center)
    {
        if (iteration == 1) return new Vector3(center.x + (i + 1) - (f), center.y - (f), center.z);
        if (iteration == 2) return new Vector3(center.x + (i + 1) - (f), center.y + (f), center.z);
        if (iteration == 3) return new Vector3(center.x - (i + 1) + (f), center.y - (f), center.z);
        if (iteration == 4) return new Vector3(center.x - (i + 1) + (f), center.y + (f), center.z);
        else return new Vector3(0, 0, 0);
    }
    void GetVisionArea(int range, Vector3 position, List<Vector3> _tiles)
    {
        Vector3 center = grid.LocalToCell(position);
        _tiles.Add(center);
        for (int i = 0; i < range; i++)
        {
            for (int iteration = 1; iteration < 5; iteration++)
            {
                if (!tilesVision.Contains(Coordinates(iteration, i, center)))
                    _tiles.Add(Coordinates(iteration, i, center));
            }
            if (i > 0 && i < range)
            {
                for (int f = 1; f <= i; f++)
                {
                    for (int iteration = 1; iteration < 5; iteration++)
                    {
                        if (!tilesVision.Contains(Coordinates(iteration, i, f, center)))
                            _tiles.Add(Coordinates(iteration, i, f, center));
                    }
                }
            }
        }
    }

    void GetFogGrid()
    {
        Vector2 casterPosition = new Vector2(caster.transform.GetChild(0).position.x, caster.transform.GetChild(0).position.y);
        for (int linha = 0; linha < linhas; linha++)
        {
            for (int coluna = 0; coluna < colunas; coluna++)
            {
                float posX = (coluna * 1) + 0.5f;
                float posY = (linha * -1) + 0.5f;
                var tile = new Vector2(posX - 13 + (int)casterPosition.x, posY + 8 + (int)casterPosition.y);
                if (tilesVision.Contains(grid.LocalToCell(tile)) == false && tilesFog.Contains(grid.LocalToCell(tile)) == false)
                {
                    tilesFog.Add(tile);
                }
            }
        }
    }

    void PaintFogGrid()
    {
        bool continuePaintingTile = true;
        for (var i = 0; i < tilesFog.Count; i++)
        {
            foreach (Vector3 tile in tilesVision)
            {
                if (tilesFog[i] == tile && continuePaintingTile == true)
                {
                    continuePaintingTile = false;
                }
            }
            if (continuePaintingTile)
                fog.SetTile(Vector3Int.FloorToInt(tilesFog[i]), tintTile);
            else
                continuePaintingTile = true;
        }
    }

    private void GenerateGrid() //For DEBUGGING ONLY
    {

        //GameObject GridTile = new GameObject("GridTile" + indice);
        for (int linha = 0; linha < linhas; linha++)
        {
            for (int coluna = 0; coluna < colunas; coluna++)
            {
                GameObject GridTile = new GameObject("GridTile" + indice);
                GridIndice ThisIndice = GridTile.AddComponent<GridIndice>();
                ThisIndice.thisIndice = indice;
                BoxCollider2D thisBoxCollider2d = GridTile.AddComponent<BoxCollider2D>();
                //thisBoxCollider2d.offset = new Vector2(0.15f, -0.15f);
                thisBoxCollider2d.size = new Vector2(0.25f, 0.25f);
                thisBoxCollider2d.isTrigger = true;
                GridTile.transform.SetParent(gridHolder.transform);

                float posX = (coluna * 1) + 0.5f;
                float posY = (linha * -1) + 0.5f;

                GridTile.transform.position = new Vector2(posX - 11, posY + 5);
                //DrawIcon(GridTile, 2);
                indice++;
            }
        }
    }
}
