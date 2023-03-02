using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem instance;

    public GridLayout gridLayout;
    private Grid grid;
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase whiteTile;

    public GameObject barrackPrefab;

    private PlaceableObject placeableObject;

    [SerializeField] Pooler barrackPooler;

    [SerializeField] SpriteRenderer backGround;

    public float width, height;


    private void Awake()
    {
        instance = this;
        grid = gridLayout.GetComponent<Grid>();

    }
    private void Start()
    {
        height = Camera.main.orthographicSize - 1f;
        width = Camera.main.aspect * height;

        Debug.Log("Width: " + width + " " + "Height: " + height);

        backGround.size = new Vector2(width * 2, height * 2);
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space)) 
        //{
        //    InitializeObject(barrackPrefab);
        //}

        //if(!placeableObject)
        //{
        //    return;
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    if (CanBePlacaed(placeableObject))
        //    {
        //        placeableObject.Place();
        //        Vector3Int start = gridLayout.WorldToCell(placeableObject.GetStartPosition());
        //        TakeArea(start, placeableObject.size);
        //    }
        //    else
        //    {
        //        Destroy(placeableObject.gameObject);
        //    }
        //}
    }

    public static Vector3 MousePosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
        return mousePosition;
    }

    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPosition = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterLocal(cellPosition);
        return position;
    }

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int count = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[count] = tilemap.GetTile(pos);
            count++;
        }

        return array;
    }

    public void FirstBuild()
    {
        Barrack barrack = GetBarrack();
        barrack.gameObject.SetActive(true);
        placeableObject = barrack.GetComponent<PlaceableObject>();
        //InitializeObject(barrack.gameObject);
    }

    private Barrack GetBarrack()
    {
        return barrackPooler.GetGo<Barrack>();
    }

    public virtual void Builded()
    {
        //if (!placeableObject)
        //{
        //    return;
        //}
        if (CanBePlacaed(placeableObject))
        {
            placeableObject.Place();
            Vector3Int start = gridLayout.WorldToCell(placeableObject.GetStartPosition());
            TakeArea(start, placeableObject.size);
        }
        else
        {
            Destroy(placeableObject.gameObject);
        }
    }

    public void InitializeObject(GameObject prefab) 
    {
        Vector3 position = SnapCoordinateToGrid(Vector3.zero);
        //GameObject obj = Instantiate(prefab, position, Quaternion.identity);

        //placeableObject = obj.GetComponent<PlaceableObject>();
        //obj.AddComponent<ObjectDrag>();
    }

    private bool CanBePlacaed(PlaceableObject placeableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(placeableObject.GetStartPosition());
        area.size = new Vector3Int(area.size.x + 1, area.size.y + 1, area.size.z);

        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);

        foreach (var b in baseArray)
        {
            if (b == whiteTile)
            {
                return false;
            }
        }

        return true;
    }

    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        MainTilemap.BoxFill(start, whiteTile, start.x, start.y, start.x + size.x, start.y + size.y);
    }
}
