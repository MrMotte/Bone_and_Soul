using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class markTest : MonoBehaviour {

    private EnemyStateMachine enemyStateMachine;

    public Canvas canvas;
    public GameObject markPrefab;

    public float markOffsety;
    public float markOffsetx;
    public float markOffsetz;
    public GameObject markPanel;

	// Use this for initialization
	void Start () {
        enemyStateMachine = GetComponent<EnemyStateMachine>();
        markPanel = Instantiate(markPrefab) as GameObject;
        markPanel.transform.SetParent(canvas.transform, false);


	}
	
	// Update is called once per frame
	void Update () {
        Vector3 worldPos = new Vector3(transform.position.x + markOffsetx, transform.position.y + markOffsety, transform.position.z + markOffsetz);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        markPanel.transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z);
	}
}
