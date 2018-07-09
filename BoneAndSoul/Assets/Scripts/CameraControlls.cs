using UnityEngine;
using System.Collections;

public class CameraControlls : MonoBehaviour
{

    public GameObject player;

    private Vector3 offset;
    public float camDistanz;
    public float offsetXMin;
    public float offsetXMax;
    public float offsetZMin;
    public float offsetZMax;
    public float offsetY;


    // Use this for initialization
    void Start()
    {
        offset = player.transform.position;

    }

    void Update()
    {
        offset = player.transform.position;
        offset.z = offset.z - camDistanz;
        if (offset.x <= offsetXMin)
        {
            offset.x = offsetXMin;
        }
        if (offset.x >= offsetXMax)
        {
            offset.x = offsetXMax;
        }
        if (offset.z <= offsetZMin)
        {
            offset.z = offsetZMin;
        }
        if (offset.z >= offsetZMax)
        {
            offset.z = offsetZMax;
        }
        if (offset.y < offsetY || offset.y > offsetY)
        {
            offset.y = offsetY;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = offset;

    }
}