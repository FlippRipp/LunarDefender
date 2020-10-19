using UnityEngine;

public class MaterialSwitch : MonoBehaviour
{

    [SerializeField] private MeshRenderer renderer;
    [SerializeField] private int materialIndex = 0;

    public void SwitchToMaterial(Material material)
    {
        Material[] mat = renderer.sharedMaterials;
        mat[materialIndex] = material;
        renderer.sharedMaterials = mat;
    }
}

