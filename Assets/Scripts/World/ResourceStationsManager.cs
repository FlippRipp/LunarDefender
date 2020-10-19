using System.Collections.Generic;
using UnityEngine;

public class ResourceStationsManager : MonoBehaviour
{
    public Vector2 minMaxSectorCooldown = new Vector2(5, 30);

    private LandingPlatform[][] resourcePlatforms;
    private float[] sectorCoolDowns;
    private bool[] isSectorActive;

    private void Awake()
    {
        FindAllResourcePlatforms();
    }

    private void Update()
    {
        UpdateResourceSectors();
    }

    private void UpdateResourceSectors()
    {
        for (int i = 0; i < sectorCoolDowns.Length; i++)
        {
            bool hasAvailableResource = false;
            isSectorActive[i] = false;
            for (int j = 0; j < resourcePlatforms[i].Length; j++)
            {
                if (resourcePlatforms[i][j].HasActiveResource)
                {
                    hasAvailableResource = true;
                    isSectorActive[i] = true;
                }
            }

            if (!hasAvailableResource)
            {
                sectorCoolDowns[i] -= Time.deltaTime;
                sectorCoolDowns[i] = Mathf.Max(sectorCoolDowns[i], 0);

                if (sectorCoolDowns[i] <= 0)
                {
                    EnableRandomResourceSpot(i);
                    SetRandomSectorCooldown(i);
                }
            }
        }
    }

    public bool IsSectorActive(int sectorID)
    {
        return isSectorActive[sectorID];
    }

    private void EnableRandomResourceSpot(int sectorID)
    {
        resourcePlatforms[sectorID][Random.Range(0, resourcePlatforms[sectorID].Length)].ActivateResource();      
    }

    private void DisableAllResourcesInSector(int sectorID)
    {
        for (int i = 0; i < resourcePlatforms[sectorID].Length; i++)
        {
            resourcePlatforms[sectorID][i].DeActivateResource();
        }
    }

    private void SetRandomSectorCooldown(int sectorID)
    {
        sectorCoolDowns[sectorID] = Random.Range(minMaxSectorCooldown.x, minMaxSectorCooldown.y);
    }

    private void FindAllResourcePlatforms()
    {
        LandingPlatform[] allResourcePlatforms = FindObjectsOfType<LandingPlatform>();

        // Get the total number of sectors.
        int numberOfSectors = 0;
        for (int i = 0; i < allResourcePlatforms.Length; i++)
        {
            if (allResourcePlatforms[i].sectorID > numberOfSectors)
            {
                numberOfSectors = allResourcePlatforms[i].sectorID;
            }
        }

        numberOfSectors += 1; // Add one to accomidate for 0 index.

        List<LandingPlatform>[] validResourcePlatforms = new List<LandingPlatform>[numberOfSectors];

        // Add each resource platform to its respective place in the array (based on sectorID).
        for (int i = 0; i < allResourcePlatforms.Length; i++)
        {
            if (allResourcePlatforms[i].PlatformType != PlatformType.BasePlatform)
            {
                if (validResourcePlatforms[allResourcePlatforms[i].sectorID] == null)
                {
                    validResourcePlatforms[allResourcePlatforms[i].sectorID] = new List<LandingPlatform>();
                }

                validResourcePlatforms[allResourcePlatforms[i].sectorID].Add(allResourcePlatforms[i]);
            }
        }

        sectorCoolDowns = new float[numberOfSectors];
        isSectorActive = new bool[numberOfSectors];

        resourcePlatforms = new LandingPlatform[numberOfSectors][];
        for (int i = 0; i < numberOfSectors; i++)
        {
            resourcePlatforms[i] = validResourcePlatforms[i].ToArray();
            DisableAllResourcesInSector(i);
            EnableRandomResourceSpot(i);
            SetRandomSectorCooldown(i);
        }
    }
}
