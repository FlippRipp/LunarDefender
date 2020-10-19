using System;
using  System.Collections;
using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    private PlayerInput playerInput;
    private Vector3 lastKnownPlayerPos;
    [SerializeField] private float respawnTime = 2f;
    [SerializeField] private PlayerMovement playerPrefab;
    [SerializeField] private float spawnHeight = 1.5f;
    [SerializeField] private float respawnCost = 2;
    private bool isRespawning;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        StartCoroutine(DoRespawn());
    }

    private void Update()
    {
        if (GameState.GameOver) return;

        if (playerInput.ControlledActor)
        {
            lastKnownPlayerPos = playerInput.ControlledActor.transform.position;
        }
        else if(!isRespawning)
        {
            StartCoroutine(DoRespawn());
        }
    }

    private IEnumerator DoRespawn()
    {
        isRespawning = true;
        yield return  new WaitForSeconds(respawnTime);
        PlayerResourceManager.Instance.resourceCrystals -= respawnCost;
        
        if (PlayerResourceManager.Instance.resourceCrystals < 0)
        {
            PlayerResourceManager.Instance.resourceCrystals = 0;
        }
        
        float closestDistance = float.PositiveInfinity;
        LandingPlatform closestPlatform = null;
        
        foreach (LandingPlatform landingPlatform in FindObjectsOfType<LandingPlatform>())
        {
            if (landingPlatform.PlatformType == PlatformType.BasePlatform)
            {
                float dist = Vector3.Distance(lastKnownPlayerPos, landingPlatform.transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closestPlatform = landingPlatform;
                }
            }
        }

        Vector2 spawnPosition = closestPlatform.transform.position + closestPlatform.transform.up * spawnHeight;

        PlayerMovement spawnedPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        playerInput.ControlledActor = spawnedPlayer.gameObject;
        FindObjectOfType<MiniMap>().player = spawnedPlayer.transform;
        FindObjectOfType<CameraMovement>().target = spawnedPlayer.transform;

            isRespawning = false;
    }
    
    
}
