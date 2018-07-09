using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour {

    public GameObject enemy;
    public Transform wp;
    private int i = 0;

    void OnTriggerEnter(Collider other)
    {
        if (i == 0)
        {
            if (other.CompareTag("Player"))
            {
                enemy.GetComponent<EnemyStateMachine>().waypoint.Add(wp);
                i++;
            }
        }
    }
}
