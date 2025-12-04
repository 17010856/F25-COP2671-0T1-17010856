using UnityEngine;
using System.Collections;

// controls merchant boat arrival and departure
public class MerchantBoatManager : MonoBehaviour
{
    [Header("Boat Settings")]
    public GameObject boatPrefab;
    public Transform spawnPosition; // where boat starts offscreen
    public Transform dockPosition; // where boat stops
    public float arrivalTime = 20f; 
    public float departureTime = 22f; 
    public float moveSpeed = 2f;

    [Header("References")]
    public TimeManager timeManager;
    public GameObject sellUI;

    private GameObject currentBoat;
    private bool boatIsPresent = false;
    private bool hasArrivedToday = false;

    void Update()
    {
        if (timeManager == null) return;

        float currentTime = timeManager.time;

        if (currentTime >= arrivalTime && currentTime < departureTime && !boatIsPresent && !hasArrivedToday)
        {
            SpawnBoat();
        }

        if (currentTime >= departureTime && boatIsPresent)
        {
            DespawnBoat();
        }

        if (currentTime < arrivalTime) // reset for next day
        {
            hasArrivedToday = false;
        }
    }

    void SpawnBoat()
    {
        if (boatPrefab == null || spawnPosition == null || dockPosition == null) return;

        currentBoat = Instantiate(boatPrefab, spawnPosition.position, Quaternion.identity);
        boatIsPresent = true;
        hasArrivedToday = true;

        StartCoroutine(MoveBoatToDock());
    }

    IEnumerator MoveBoatToDock() // animates boat sailing in
    {
        while (currentBoat != null && Vector3.Distance(currentBoat.transform.position, dockPosition.position) > 0.1f)
        {
            currentBoat.transform.position = Vector3.MoveTowards(
                currentBoat.transform.position,
                dockPosition.position,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        if (sellUI != null)
        {
            sellUI.SetActive(true);
        }
    }

    void DespawnBoat()
    {
        if (currentBoat != null)
        {
            if (sellUI != null)
            {
                sellUI.SetActive(false);
            }

            StartCoroutine(MoveBoatAway());
        }

        boatIsPresent = false;
    }

    IEnumerator MoveBoatAway() // animates boat sailing away
    {
        Vector3 exitPosition = spawnPosition.position;

        while (currentBoat != null && Vector3.Distance(currentBoat.transform.position, exitPosition) > 0.1f)
        {
            currentBoat.transform.position = Vector3.MoveTowards(
                currentBoat.transform.position,
                exitPosition,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        if (currentBoat != null)
        {
            Destroy(currentBoat);
        }
    }

    public GameObject GetCurrentBoat()
    {
        return currentBoat;
    }
}