using UnityEngine;

[CreateAssetMenu(fileName = "NewSeedPacket", menuName = "Farm/SeedPacket")]
public class SeedPacket : ScriptableObject
{
    public string cropName;
    public Sprite[] growthSprites;
    public Sprite coverImage;
    public HarvestableItem harvestItem;
    public int harvestAmount = 1;
}