using UnityEditor;
using UnityEngine;

public class ObjectPlacingEditor : Editor
{
    [MenuItem("Lunar Defender/Rotate From Planet %g")]
    static void RotateUpFromZero()
    {
        //Selection.activeTransform.up = Selection.activeTransform.position.normalized;

        foreach (Transform transform in Selection.transforms)
        {
            transform.up = transform.position.normalized;
        }       
    }
}
