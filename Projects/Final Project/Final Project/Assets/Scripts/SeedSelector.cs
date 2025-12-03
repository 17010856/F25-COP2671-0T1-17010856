using UnityEngine;
using UnityEngine.UI;

public class SeedSelector : MonoBehaviour
{
    public SeedPacket[] availableSeeds;
    public GameObject seedButtonPrefab;
    public Transform seedButtonContainer;
    public Color selectedColor = Color.green;
    public Color normalColor = Color.white;

    private SeedPacket currentlySelectedSeed;
    private Button[] seedButtons;

    void Start()
    {
        CreateSeedButtons();
        
        if (availableSeeds.Length > 0)
        {
            SelectSeed(availableSeeds[0], 0);
        }
    }

    void CreateSeedButtons()
    {
        seedButtons = new Button[availableSeeds.Length];

        for (int i = 0; i < availableSeeds.Length; i++)
        {
            SeedPacket seed = availableSeeds[i];
            GameObject buttonObj = Instantiate(seedButtonPrefab, seedButtonContainer);
            Button button = buttonObj.GetComponent<Button>();
            seedButtons[i] = button;

            Image iconImage = buttonObj.transform.Find("SeedIcon").GetComponent<Image>();
            if (iconImage != null && seed.coverImage != null)
            {
                iconImage.sprite = seed.coverImage;
            }

            int index = i;
            button.onClick.AddListener(() => SelectSeed(seed, index));
        }
    }

    void SelectSeed(SeedPacket seed, int index)
    {
        currentlySelectedSeed = seed;

        for (int i = 0; i < seedButtons.Length; i++)
        {
            ColorBlock colors = seedButtons[i].colors;
            colors.normalColor = (i == index) ? selectedColor : normalColor;
            seedButtons[i].colors = colors;
        }

        Debug.Log($"Selected seed: {seed.cropName}");
    }

    public SeedPacket GetSelectedSeed()
    {
        return currentlySelectedSeed;
    }
}