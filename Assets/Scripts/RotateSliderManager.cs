using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateSliderManager : MonoBehaviour
{
    private Slider rotateSlider;
    private Slider distSlider;
    public float rotMinVal;
    public float rotMaxVal;
    public float distMinVal;
    public float distMaxVal;
    void Start()
    {
        rotateSlider = GameObject.Find("rotateSlider").GetComponent<Slider>();
        rotateSlider.minValue = rotMinVal;
        rotateSlider.maxValue = rotMaxVal;

        rotateSlider.onValueChanged.AddListener(RotateSliderUpdate);

        distSlider = GameObject.Find("distSlider").GetComponent<Slider>();
        distSlider.minValue = distMinVal;
        distSlider.maxValue = distMaxVal;

        distSlider.onValueChanged.AddListener(DistSliderUpdate);
    }

    void RotateSliderUpdate(float value)
    {
        transform.localEulerAngles = new Vector3(transform.rotation.x, value, transform.rotation.z);
    }

    void DistSliderUpdate(float value)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, value);
    }
}
