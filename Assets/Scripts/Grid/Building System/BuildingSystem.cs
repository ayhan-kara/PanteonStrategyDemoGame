using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem instance;

    private PlaceableObject placeableObject;
    public GridLayout gridLayout;
    private Grid grid;
    public GameObject barrackPrefab;
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase whiteTile;
    [SerializeField] Pooler barrackPooler;
    [SerializeField] Pooler powerPlantPooler;
    [SerializeField] SpriteRenderer backGround;
    public float width, height;

    #region Monobehaviour
    private void Awake()
    {
        instance = this;
        grid = gridLayout.GetComponent<Grid>();
    }
    private void Start()
    {
        height = Camera.main.orthographicSize - 1f;
        width = Camera.main.aspect * height;
        backGround.size = new Vector2(width * 2, height * 2);
    }

    #endregion


    public static Vector3 MousePosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mousePosition;
    }

    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPosition = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterLocal(cellPosition);
        return position;
    }

    #region Get Buildings
    public void FirstBarrackBuild()
    {
        Barrack barrack = GetBarrack();
        placeableObject = barrack.GetComponent<PlaceableObject>();
        barrack.gameObject.SetActive(true);
        barrack.tag = "Selected";
    }

    private Barrack GetBarrack()
    {
        return barrackPooler.GetGo<Barrack>();
    }

    public void FirstPowerPlantBuild()
    {
        PowerPlant plant = GetPowerPlant();
        placeableObject = plant.GetComponent<PlaceableObject>();
        plant.gameObject.SetActive(true);
        plant.tag = "Selected";
    }

    private PowerPlant GetPowerPlant()
    {
        return powerPlantPooler.GetGo<PowerPlant>();
    }

    #endregion

    public virtual void Builded()
    {
        placeableObject.Place();
    }
}
