using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

[System.Serializable]
public class ForageSpot
{
    public SpriteRenderer cropRenderer;
    public ForageableCrop cropData;
    public int currentGrowthStage = 0;
    public float growthTimer = 0f;
    public bool isGrowing = false;

    public void StartGrowing(ForageableCrop crop)
    {
        cropData = crop;
        currentGrowthStage = 0;
        growthTimer = 0f;
        isGrowing = true;
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        if (cropData != null && cropRenderer != null && currentGrowthStage < cropData.growthSprites.Length)
        {
            cropRenderer.sprite = cropData.growthSprites[currentGrowthStage];
        }
    }

    public void Grow(float deltaTime, float currentTime)
    {
        bool isDaytime = currentTime >= 6f && currentTime <= 20f;

        if (isGrowing && isDaytime && currentGrowthStage < cropData.growthSprites.Length - 1)
        {
            growthTimer += deltaTime;
            if (growthTimer >= cropData.timePerStage)
            {
                growthTimer = 0f;
                currentGrowthStage++;
                UpdateSprite();
            }
        }
    }

    public bool CanHarvest()
    {
        return isGrowing && cropData != null && currentGrowthStage == cropData.growthSprites.Length - 1;
    }

    public void Harvest(InventorySystem inventory)
    {
        if (CanHarvest() && inventory != null)
        {
            inventory.AddItem(cropData.harvestItem, cropData.harvestAmount);
            Reset();
        }
    }

    public void Reset()
    {
        cropData = null;
        currentGrowthStage = 0;
        growthTimer = 0f;
        isGrowing = false;
        if (cropRenderer != null)
        {
            cropRenderer.sprite = null;
        }
    }
}

public class ForagingManager : MonoBehaviour
{
    [Header("Tilemap Reference")]
    public Tilemap foragingTilemap;

    [Header("Forageable Crops")]
    public ForageableCrop[] forageableCrops;

    [Header("Spawn Settings")]
    [Range(0f, 1f)]
    public float spawnChance = 0.3f;
    public float respawnTime = 30f;

    [Header("Time Manager")]
    public TimeManager timeManager;

    public List<ForageSpot> forageSpots = new List<ForageSpot>();
    private float respawnTimer = 0f;

    void Start()
    {
        if (foragingTilemap == null)
        {
            Debug.LogError("Foraging Tilemap not assigned!");
            return;
        }

        CreateForageSpots();
        SpawnInitialCrops();
    }

    void Update()
    {
        if (timeManager == null) return;

        foreach (var spot in forageSpots)
        {
            if (spot.isGrowing)
            {
                spot.Grow(Time.deltaTime, timeManager.time);
            }
        }

        respawnTimer += Time.deltaTime;
        if (respawnTimer >= respawnTime)
        {
            respawnTimer = 0f;
            TryRespawnCrops();
        }
    }

    void CreateForageSpots()
    {
        BoundsInt bounds = foragingTilemap.cellBounds;

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                Vector3Int cellPos = new Vector3Int(x + bounds.xMin, y + bounds.yMin, 0);

                if (foragingTilemap.HasTile(cellPos))
                {
                    ForageSpot spot = new ForageSpot();

                    Vector3 worldPos = foragingTilemap.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f, 0);
                    GameObject spotGO = new GameObject($"ForageSpot_{x}_{y}");
                    spotGO.transform.position = worldPos;
                    spotGO.transform.parent = this.transform;

                    GameObject cropGO = new GameObject("Crop");
                    cropGO.transform.parent = spotGO.transform;
                    cropGO.transform.localPosition = Vector3.zero;
                    SpriteRenderer cropSR = cropGO.AddComponent<SpriteRenderer>();
                    cropSR.sortingOrder = 2;
                    cropSR.sortingLayerName = "Default";
                    spot.cropRenderer = cropSR;

                    forageSpots.Add(spot);
                }
            }
        }

        Debug.Log($"Created {forageSpots.Count} forage spots");
    }

    void SpawnInitialCrops()
    {
        foreach (var spot in forageSpots)
        {
            if (Random.value < spawnChance && forageableCrops.Length > 0)
            {
                ForageableCrop randomCrop = forageableCrops[Random.Range(0, forageableCrops.Length)];
                spot.StartGrowing(randomCrop);
            }
        }
    }

    void TryRespawnCrops()
    {
        foreach (var spot in forageSpots)
        {
            if (!spot.isGrowing && Random.value < spawnChance && forageableCrops.Length > 0)
            {
                ForageableCrop randomCrop = forageableCrops[Random.Range(0, forageableCrops.Length)];
                spot.StartGrowing(randomCrop);
            }
        }
    }

    public ForageSpot GetClosestForageSpot(Vector3 position, float maxDistance)
    {
        ForageSpot closest = null;
        float minDist = Mathf.Infinity;

        foreach (var spot in forageSpots)
        {
            if (spot.CanHarvest() && spot.cropRenderer != null)
            {
                float dist = Vector2.Distance(position, spot.cropRenderer.transform.position);
                if (dist < minDist && dist <= maxDistance)
                {
                    minDist = dist;
                    closest = spot;
                }
            }
        }

        return closest;
    }
}