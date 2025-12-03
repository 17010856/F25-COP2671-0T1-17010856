using UnityEngine;

[CreateAssetMenu(fileName = "NewForageableCrop", menuName = "Farm/ForageableCrop")]
public class ForageableCrop : ScriptableObject
{
    public string cropName;
    public Sprite[] growthSprites;
    public HarvestableItem harvestItem;
    public int harvestAmount = 1;
    public float timePerStage = 10f;
}