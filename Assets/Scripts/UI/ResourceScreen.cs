using TMPro;
using UnityEngine;

public class ResourceScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] string resourcePrefix = "Crystals: ";
    [SerializeField] string resourceSuffix = "";

    private void Update()
    {
        textMesh.text = resourcePrefix + Mathf.Floor(PlayerResourceManager.Instance.resourceCrystals).ToString("0000") + resourceSuffix;
    }
}
