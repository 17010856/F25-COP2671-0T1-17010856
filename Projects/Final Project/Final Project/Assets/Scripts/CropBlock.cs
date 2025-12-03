using UnityEngine;

[System.Serializable]
public class CropBlock
{
    public int currentGrowthStage = 0;
    public SpriteRenderer soilRenderer;
    public SpriteRenderer cropRenderer;
    public SeedPacket seedData;

    public bool isTilled = false;
    public bool isPlanted = false;
    public bool isWatered = false;
    public float growthTimer = 0f;
    public float timePerStage = 5f;

    public Sprite soilSprite;
    public Sprite waterSprite;

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
        isTilled = true;
        if (soilRenderer != null)
        {
            soilRenderer.sprite = soilSprite;
        }
        Debug.Log("Soil tilled!");
    }

    public void WaterSoil()
    {
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

    public void HarvestPlants(InventorySystem inventory)
    {
        if (seedData != null && currentGrowthStage == seedData.growthSprites.Length - 1)
        {
            if (inventory != null && seedData.harvestItem != null)
            {
                inventory.AddItem(seedData.harvestItem, seedData.harvestAmount);
            }
            
            Debug.Log("Harvested!");
            seedData = null;
            currentGrowthStage = 0;
            isPlanted = false;
            isWatered = false;
            isTilled = false;
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