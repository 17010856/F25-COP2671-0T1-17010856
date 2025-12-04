using UnityEngine;

// connects button presses to farming actions
public class FarmingController : MonoBehaviour
{
    [Header("References")]
    public CropManager cropManager;
    public Transform playerTransform;
    public SeedSelector seedSelector;
    public ForagingManager foragingManager;

    [Header("Particle Effects")]
    public GameObject harvestParticles;

    [Header("Settings")]
    public float interactRange = 1.5f; // how close player needs to be

    private CropBlock GetClosestTile() // finds nearest farm tile
    {
        CropBlock closest = null;
        float minDist = Mathf.Infinity;

        for (int x = 0; x < cropManager.cropGrid.GetLength(0); x++)
        {
            for (int y = 0; y < cropManager.cropGrid.GetLength(1); y++)
            {
                CropBlock block = cropManager.cropGrid[x, y];
                if (block != null && block.cropRenderer != null)
                {
                    float dist = Vector2.Distance(playerTransform.position, block.cropRenderer.transform.position);
                    if (dist < minDist && dist <= interactRange)
                    {
                        minDist = dist;
                        closest = block;
                    }
                }
            }
        }

        return closest;
    }

    public void OnHoe()
    {
        PlayerController player = playerTransform.GetComponent<PlayerController>();
        if (player != null)
        {
            player.StartSwinging(); // play animation
        }
        
        CropBlock block = GetClosestTile();
        if (block != null)
        {
            block.TillSoil();
        }
    }

    public void OnWater()
    {
        PlayerController player = playerTransform.GetComponent<PlayerController>();
        if (player != null)
        {
            player.StartWatering();
        }
        
        CropBlock block = GetClosestTile();
        if (block != null)
        {
            block.WaterSoil();
        }
    }

    public void OnSeed()
    {
        PlayerController player = playerTransform.GetComponent<PlayerController>();
        if (player != null)
        {
            player.StartPlanting();
        }
        
        CropBlock block = GetClosestTile();
        if (block != null && seedSelector != null)
        {
            SeedPacket selectedSeed = seedSelector.GetSelectedSeed();
            if (selectedSeed != null)
            {
                block.PlantSeed(selectedSeed);
            }
        }
    }

    public void OnHarvest()
    {
        PlayerController player = playerTransform.GetComponent<PlayerController>();
        if (player != null)
        {
            player.StartSwinging();
        }
        
        CropBlock block = GetClosestTile();
        if (block != null) // check farm tiles first
        {
            InventorySystem inventory = playerTransform.GetComponent<InventorySystem>();
            block.HarvestPlants(inventory, harvestParticles);
        }
        else if (foragingManager != null) // then check foraging tiles
        {
            ForageSpot forageSpot = foragingManager.GetClosestForageSpot(playerTransform.position, interactRange);
            if (forageSpot != null)
            {
                InventorySystem inventory = playerTransform.GetComponent<InventorySystem>();
                forageSpot.Harvest(inventory);
            }
        }
    }
}