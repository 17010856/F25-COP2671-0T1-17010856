using UnityEngine;
using UnityEngine.Tilemaps;

public class CropManager : MonoBehaviour
{
    [Header("Tilemap Reference")]
    public Tilemap farmTilemap;

    [Header("Test Seed")]
    public SeedPacket testSeedPacket;

    [Header("Soil Sprites")]
    public Sprite tilledSoilSprite;
    public Sprite wateredSoilSprite;

    public CropBlock[,] cropGrid;

    void Start()
    {
        if (farmTilemap == null)
        {
            Debug.LogError("Farm Tilemap not assigned!");
            return;
        }

        CreateGridUsingTilemap(farmTilemap);
    }

    void Update()
    {
        if (cropGrid == null) return;

        foreach (var block in cropGrid)
        {
            if (block != null)
            {
                block.Grow(Time.deltaTime);
            }
        }
    }

    void CreateGridUsingTilemap(Tilemap tilemap)
    {
        BoundsInt bounds = tilemap.cellBounds;
        cropGrid = new CropBlock[bounds.size.x, bounds.size.y];

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                Vector3Int cellPos = new Vector3Int(x + bounds.xMin, y + bounds.yMin, 0);

                if (tilemap.HasTile(cellPos))
                {
                    CropBlock block = new CropBlock();

                    // Create GameObject to hold soil and crop
                    Vector3 worldPos = tilemap.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f, 0);
                    GameObject tileGO = new GameObject($"Tile_{x}_{y}");
                    tileGO.transform.position = worldPos;
                    tileGO.transform.parent = this.transform;

                    // Soil renderer
                    GameObject soilGO = new GameObject("Soil");
                    soilGO.transform.parent = tileGO.transform;
                    soilGO.transform.localPosition = Vector3.zero;
                    SpriteRenderer soilSR = soilGO.AddComponent<SpriteRenderer>();
                    soilSR.sortingOrder = 0; // BEHIND PLAYER
                    soilSR.sortingLayerName = "Default";
                    block.soilRenderer = soilSR;
                    block.soilSprite = tilledSoilSprite;
                    block.waterSprite = wateredSoilSprite;

                    // Crop renderer
                    GameObject cropGO = new GameObject("Crop");
                    cropGO.transform.parent = tileGO.transform;
                    cropGO.transform.localPosition = Vector3.zero;
                    SpriteRenderer cropSR = cropGO.AddComponent<SpriteRenderer>();
                    cropSR.sortingOrder = 1; // BEHIND PLAYER
                    cropSR.sortingLayerName = "Default";
                    block.cropRenderer = cropSR;

                    cropGrid[x, y] = block;
                }
                else
                {
                    cropGrid[x, y] = null;
                }
            }
        }

        Debug.Log("Crop grid initialized for painted tiles only.");
    }
}
