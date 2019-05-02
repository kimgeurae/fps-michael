using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmineBehaviour : MonoBehaviour
{

    private Light blinkingLight;

    void Start()
    {
        LoadReferencesAndSetValues();
        StartCoroutine("BlinkLight");

    }

    void LoadReferencesAndSetValues()
    {
        blinkingLight = transform.GetChild(2).gameObject.GetComponent<Light>();
    }

    void Update()
    {

    }

    IEnumerator BlinkLight()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            blinkingLight.enabled = !blinkingLight.enabled;
        }
    }

}
