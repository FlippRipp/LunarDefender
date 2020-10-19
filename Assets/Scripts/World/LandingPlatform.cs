using System.Resources;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum PlatformType { BasePlatform, CrystalPlatform, CrystalPlatformWithTimer }

public class LandingPlatform : MonoBehaviour
{
    [SerializeField] PlatformType platformType;
    public int sectorID;

    [SerializeField] Slider resourceSlider;

    [Header("CrystalPlatform (per second)")]
    public float resourcePerSecond = 1f;

    [Header("CrystalPlatform (timer)")]
    public float resourcesAmount;
	public float waitForSeconds;

    public UnityEvent onShipLand;
    public UnityEvent onShipLandedUpdate;
    public UnityEvent onShipTakeOff;
    public UnityEvent onResourceSpawn;
    public UnityEvent onResourceUse;
    public UnityEvent mining;
    public UnityEvent collected;

    public bool HasLandedShip { get; private set; }
    public bool HasActiveResource { get; private set; }
    public float ResourceTimer { get; private set; }

    public PlatformType PlatformType
    {
        get { return platformType; }
    }

    private void Awake()
    {
        if (resourceSlider)
        {
            resourceSlider.gameObject.SetActive(false);
        }       
    }

    private void Update()
    {
        if (HasLandedShip)
        {
            onShipLandedUpdate.Invoke();
            
            UpdatePlatform();
		}
    }

    private void UpdatePlatform()
    {
        if (!HasActiveResource) return;

        if (platformType == PlatformType.CrystalPlatform)
        {
            PlayerResourceManager.Instance.IncreaseResource(resourcePerSecond);
        }
        else if (platformType == PlatformType.CrystalPlatformWithTimer)
        {
            IncreaseResource();
        }
    }

    private void IncreaseResource()
    {
        if (!HasActiveResource) return;

        ResourceTimer += Time.deltaTime;

        resourceSlider.gameObject.SetActive(true);
        resourceSlider.value = ResourceTimer / waitForSeconds;

        if (ResourceTimer > waitForSeconds)
        {
            PlayerResourceManager.Instance.AddResourceInstant(resourcesAmount);
            collected.Invoke();
            DeActivateResource();

            resourceSlider.gameObject.SetActive(false);
            ResourceTimer = 0;
        }
    }

    public void ActivateResource()
    {
        HasActiveResource = true;
        onResourceSpawn.Invoke();
    }

    public void DeActivateResource()
    {
        HasActiveResource = false;
        onResourceUse.Invoke();
    }

    public void Land()
    {
        onShipLand.Invoke();
        
        if (platformType != PlatformType.BasePlatform) 
        {
            mining.Invoke();
        }
        HasLandedShip = true;
    }

    public void TakeOff()
    {
        onShipTakeOff.Invoke();

        if (platformType == PlatformType.CrystalPlatformWithTimer)
        {
            resourceSlider.gameObject.SetActive(false);
            ResourceTimer = 0f;
        }

        HasLandedShip = false;
    }
}
