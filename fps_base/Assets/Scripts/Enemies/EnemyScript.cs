using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    public int maxHealth;
    private int health;
    private enum State { searching, locked, };
    private State state;
    private float timeToShoot;
    private GameObject target;
    public GameObject _enemyBullet;

    // Start is called before the first frame update
    void Start()
    {
        LoadStartValues();
        state = State.searching;
    }

    void LoadStartValues()
    {
        health = maxHealth;
        timeToShoot = 0f;
    }



    // Update is called once per frame
    void Update()
    {
        Behaviour();   
    }

    private void Behaviour()
    {
        switch (state)
        {
            case State.searching:
                FindTarget();
                break;
            case State.locked:
                Shoot();
                break;
        }
    }

    private void FindTarget()
    {
        transform.Rotate(Vector3.up);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f))
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                target = hit.transform.gameObject;
                state = State.locked;
            }
        }
    }

    private void Shoot()
    {
        transform.LookAt(target.transform.position + Vector3.up);
        if (timeToShoot <= Time.time)
        {
            Instantiate(_enemyBullet, transform.position, transform.rotation);
            timeToShoot = Time.time + 3f;
        }
    }

    public void ApplyDamage(int receivedDmg)
    {
        if (health - receivedDmg > 0)
            health -= receivedDmg;
        else
        {
            health = 0;
            Destroy(this.gameObject);
        }
    }

}
