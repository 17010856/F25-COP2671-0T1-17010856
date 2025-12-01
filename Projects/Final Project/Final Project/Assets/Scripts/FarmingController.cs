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
        CropBlock block = GetClosestTile();
        if (block != null)
        {
            block.TillSoil();
            Debug.Log("Hoe button pressed on nearby tile");
        }
        else
        {
            Debug.Log("No nearby tile to hoe");
        }
    }

    public void OnWater()
    {
        CropBlock block = GetClosestTile();
        if (block != null)
        {
            block.WaterSoil();
            Debug.Log("Water button pressed on nearby tile");
        }
        else
        {
            Debug.Log("No nearby tile to water");
        }
    }

    public void OnSeed()
    {
        CropBlock block = GetClosestTile();
        if (block != null && cropManager.testSeedPacket != null)
        {
            block.PlantSeed(cropManager.testSeedPacket);
            Debug.Log($"Plant button pressed on nearby tile: Planted {cropManager.testSeedPacket.cropName}");
        }
        else
        {
            Debug.Log("No nearby tile to plant");
        }
    }

    public void OnHarvest()
    {
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
