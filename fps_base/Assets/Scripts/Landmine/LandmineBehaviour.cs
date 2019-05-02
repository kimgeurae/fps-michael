using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmineBehaviour : MonoBehaviour
{

    private Light blinkingLight;
    private ParticleSystem ps;
    private GameObject visual;

    void Start()
    {
        LoadReferencesAndSetValues();
        StartCoroutine("BlinkLight");
    }

    void LoadReferencesAndSetValues()
    {
        visual = transform.GetChild(0).gameObject;
        ps = transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        blinkingLight = transform.GetChild(2).gameObject.GetComponent<Light>();
    }

    void Update()
    {

    }

    IEnumerator BlinkLight()
    {
        while (true)
        {
            blinkingLight.enabled = !blinkingLight.enabled;
            yield return new WaitForSeconds(1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ps.Play(true);
            visual.SetActive(false);
            other.transform.GetChild(1).gameObject.GetComponent<PlayerBehaviourScript>().ReceivedDmg(101);
            Destroy(this.gameObject, 2);
        }
    }

}
