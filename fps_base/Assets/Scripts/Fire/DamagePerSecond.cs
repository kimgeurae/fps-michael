using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePerSecond : MonoBehaviour
{
    [Tooltip("The damage that the fire does per second.")]
    public int dmgPerSecond;
    private float nextTimeToDamage;

    // Start is called before the first frame update
    void Start()
    {
        nextTimeToDamage = Time.time + 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (nextTimeToDamage < Time.time)
            {
                other.transform.GetChild(1).gameObject.GetComponent<PlayerBehaviourScript>().ReceivedDmg(dmgPerSecond);
                nextTimeToDamage = Time.time + 1;
            }
        }
    }
}
