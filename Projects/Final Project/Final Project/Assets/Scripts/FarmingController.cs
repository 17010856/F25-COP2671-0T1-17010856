using UnityEngine;

public class FarmingController : MonoBehaviour
{
    [Header("References")]
    public CropManager cropManager;
    public Transform playerTransform;

    [Header("Settings")]
    public float interactRange = 1.5f; // how close the player must be to a tile

    // Find the closest farm tile within range
    private CropBlock GetClosestTile()
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

    // Button Methods
    public void OnHoe()
    {
        // ALWAYS trigger player animation
        PlayerController player = playerTransform.GetComponent<PlayerController>();
        if (player != null)
        {
            player.StartSwinging();
        }
        
        // Only do farming logic if near a tile
        CropBlock block = GetClosestTile();
        if (block != null)
        {
            block.TillSoil();
        }
        else
        {
            Debug.Log("No nearby tile to hoe");
        }
    }

    public void OnWater()
    {
        // ALWAYS trigger player animation
        PlayerController player = playerTransform.GetComponent<PlayerController>();
        if (player != null)
        {
            player.StartWatering();
        }
        
        // Only do farming logic if near a tile
        CropBlock block = GetClosestTile();
        if (block != null)
        {
            block.WaterSoil();
            Debug.Log("Water button pressed on nearby tile");
        }
    }

    public void OnSeed()
    {
        // ALWAYS trigger player animation
        PlayerController player = playerTransform.GetComponent<PlayerController>();
        if (player != null)
        {
            player.StartPlanting();
        }
        
        // Only do farming logic if near a tile
        CropBlock block = GetClosestTile();
        if (block != null && cropManager.testSeedPacket != null)
        {
            block.PlantSeed(cropManager.testSeedPacket);
        }
    }

    public void OnHarvest()
    {
        // ALWAYS trigger player animation
        PlayerController player = playerTransform.GetComponent<PlayerController>();
        if (player != null)
        {
            player.StartSwinging();  // Use same swinging animation for harvest
        }
        
        // Only do farming logic if near a tile
        CropBlock block = GetClosestTile();
        if (block != null)
        {
            block.HarvestPlants();
            Debug.Log("Harvest button pressed on nearby tile");
        }
        else
        {
            Debug.Log("No nearby tile to harvest");
        }
    }
}