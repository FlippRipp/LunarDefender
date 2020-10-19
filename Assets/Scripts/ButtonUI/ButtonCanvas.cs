using UnityEngine;
using UnityEngine.UI;

public class ButtonCanvas : MonoBehaviour
{
    public Button button;

    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }


}
