using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Range(0.1f, 10f)]
    public float bulletSpeed;
    public int bulletDmg;

    // Update is called once per frame
    void Update()
    {
        transform.Translate((new Vector3(0f, 0f, bulletSpeed * 2) * Time.deltaTime));
        Destroy(this.gameObject, 5f);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Oi tudo bom?");
            collision.transform.GetChild(1).gameObject.GetComponent<PlayerBehaviourScript>().ReceivedDmg(bulletDmg);
            Destroy(this.gameObject);
        }
    }
}
