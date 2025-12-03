using UnityEngine;

[System.Serializable]
public class CropBlock
{
    public int currentGrowthStage = 0;
    public SpriteRenderer soilRenderer;   // visual for tilled/watered soil
    public SpriteRenderer cropRenderer;   // visual for crop
    public SeedPacket seedData;

    public bool isTilled = false;  // NEW: tracks if soil has been hoed
    public bool isPlanted = false;
    public bool isWatered = false;
    public float growthTimer = 0f;
    public float timePerStage = 2f; // seconds per stage

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
        isTilled = true;  // Mark as tilled
        if (soilRenderer != null)
        {
            soilRenderer.sprite = soilSprite;
        }
        Debug.Log("Soil tilled!");
    }

    public void WaterSoil()
    {
        // Can only water if soil is tilled AND planted
        if (!isTilled)
        {
            Debug.Log("Need to hoe the soil first!");
            return;
        }
        
        if (!isPlanted)
        {
            Debug.Log("Nothing planted here to water");
            return;
        }
        
        isWatered = true;
        if (soilRenderer != null) soilRenderer.sprite = waterSprite;
        Debug.Log("Soil watered!");
    }

    public void PlantSeed(SeedPacket seed)
    {
        // Can only plant if soil is tilled
        if (!isTilled)
        {
            Debug.Log("Need to hoe the soil first!");
            return;
        }
        
        seedData = seed;
        currentGrowthStage = 0;
        isPlanted = true;
        isWatered = false;
        growthTimer = 0f;
        UpdateSprite();
        Debug.Log("Seed planted!");
    }

    public void HarvestPlants()
    {
        if (seedData != null && currentGrowthStage == seedData.growthSprites.Length - 1)
        {
            Debug.Log("Harvested!");
            seedData = null;
            currentGrowthStage = 0;
            isPlanted = false;
            isWatered = false;
            isTilled = false;  // Reset tilled state after harvest
            growthTimer = 0f;
            if (cropRenderer != null) cropRenderer.sprite = null;
            if (soilRenderer != null) soilRenderer.sprite = null;
        }
        else
        {
            Debug.Log("Crop not ready to harvest yet");
        }
    }

    public void Grow(float deltaTime, float currentTime)
    {
        // Only grow during daytime (6am to 8pm)
        bool isDaytime = currentTime >= 6f && currentTime <= 20f;
        
        if (isPlanted && isWatered && isDaytime && currentGrowthStage < seedData.growthSprites.Length - 1)
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