using UnityEngine;
using System.Collections;

public class MaterialChange : MonoBehaviour {

    public Material attention;
    public Material idle;
    public Material follow;
    public MeshRenderer rend;
    public FieldOfView iSeeYou;
    public EnemyStateMachine follower;

	// Use this for initialization
	void Start () {
        rend = GetComponent<MeshRenderer>();
        iSeeYou = GetComponent<FieldOfView>(); // added 24.01.2017 Script befindet sich nun am gleichen Object
        rend.enabled = true;
    }
	
	// Update is called once per frame
	void Update () {
	if(iSeeYou.dogLookAtPlayer == true || follower.dogFollow == true)
        {
            if (iSeeYou.dogLookAtPlayer == true)
            {
                rend.material = attention;
            }
            if (follower.dogFollow == true)
            {
                rend.material = follow;
            }
            //rend.material = attention;
        }
    else
        {
            rend.material = idle;
        }
	}
}
