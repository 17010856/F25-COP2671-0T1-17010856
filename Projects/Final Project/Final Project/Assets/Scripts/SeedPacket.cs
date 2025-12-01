using UnityEngine;

[CreateAssetMenu(fileName = "NewSeedPacket", menuName = "Farm/SeedPacket")]
public class SeedPacket : ScriptableObject
{
    public string cropName;
    public Sprite[] growthSprites; // 4 sprites for growth stages
    public Sprite coverImage;      // toolbar or inventory
    public GameObject harvestablePrefab; // optional prefab to spawn on harvest
}
