using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class BoostSlider : MonoBehaviour
{
    private Slider slider;
    private TextMeshProUGUI text;
    private PlayerInput playerInput;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        playerInput = FindObjectOfType<PlayerInput>();
    }

    private void Update()
    {
        if (playerInput == null || playerInput.PlayerMovement == null)
        {
            slider.value = 0f;
            SetText("0%");
        }
        else
        {
            slider.value = playerInput.PlayerMovement.BoostFuel / playerInput.PlayerMovement.boostFuelMax;
            SetText(Mathf.RoundToInt(slider.value * 100f) + "%");
        }
    }

    private void SetText(string text)
    {
        if (this.text == null) return;

        this.text.text = text;
    }
}
