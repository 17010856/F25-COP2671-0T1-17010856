using UnityEngine;

[System.Serializable]
public class CropBlock
{
    public int currentGrowthStage = 0;
    public SpriteRenderer soilRenderer;   // visual for tilled/watered soil
    public SpriteRenderer cropRenderer;   // visual for crop
    public SeedPacket seedData;

    public bool isPlanted = false;
    public bool isWatered = false;
    public float growthTimer = 0f;
    public float timePerStage = 5f; // seconds per stage

    public Sprite soilSprite;   // optional: tilled soil
    public Sprite waterSprite;  // optional: watered soil

    // Update crop sprite
    public void UpdateSprite()
    {
        if (seedData != null && cropRenderer != null)
        {
            int stage = Mathf.Clamp(currentGrowthStage, 0, seedData.growthSprites.Length - 1);
            cropRenderer.sprite = seedData.growthSprites[stage];
        }
    }

    public void TillSoil()
    {
        if (soilRenderer != null)
        {
            soilRenderer.sprite = soilSprite;
        }
        Debug.Log("Soil tilled");
    }

    public void WaterSoil()
    {
        if (isPlanted)
        {
            isWatered = true;
            if (soilRenderer != null) soilRenderer.sprite = waterSprite;
            Debug.Log("Soil watered — crop can now grow");
        }
        else
        {
            Debug.Log("Nothing planted here to water");
        }
    }

    public void PlantSeed(SeedPacket seed)
    {
        seedData = seed;
        currentGrowthStage = 0;
        isPlanted = true;
        isWatered = false;
        growthTimer = 0f;
        UpdateSprite();
        Debug.Log($"Planted {seed.cropName}");
    }

    public void HarvestPlants()
    {
        if (seedData != null && currentGrowthStage == seedData.growthSprites.Length - 1)
        {
            Debug.Log($"Harvested {seedData.cropName}");
            seedData = null;
            currentGrowthStage = 0;
            isPlanted = false;
            isWatered = false;
            growthTimer = 0f;
            if (cropRenderer != null) cropRenderer.sprite = null;
            if (soilRenderer != null) soilRenderer.sprite = null;
        }
        else
        {
            Debug.Log("Crop not ready to harvest");
        }
    }

    public void Grow(float deltaTime)
    {
        if (isPlanted && isWatered && currentGrowthStage < seedData.growthSprites.Length - 1)
        {
            growthTimer += deltaTime;
            if (growthTimer >= timePerStage)
            {
                growthTimer = 0f;
                currentGrowthStage++;
                UpdateSprite();
                Debug.Log($"Crop advanced to stage {currentGrowthStage}");
            }
        }
    }
}
