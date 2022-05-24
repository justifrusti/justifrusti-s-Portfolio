using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedSlider : MonoBehaviour
{
    public Submarine controller;

    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider.minValue = 1;
    }

    // Update is called once per frame
    void Update()
    {
        controller.currentSpeed = slider.value;
    }

    public void SpeedChanged()
    {
        controller.maxSpeed = slider.value;
        controller.currentSpeed = slider.value;
    }
}
