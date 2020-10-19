using UnityEngine;

public class PlayerResourceManager : MonoBehaviour
{
    public static PlayerResourceManager Instance;

    public float resourceCrystals;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void IncreaseResource(float amountPerSecond)
    {
        resourceCrystals += amountPerSecond * Time.deltaTime;
    }

    public void AddResourceInstant(float value)
    {
        resourceCrystals += value;
    }
}
