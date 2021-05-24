using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public GameObject player;
    public float cameraDistance;
    public float cameraHeight;
    private Rigidbody cameraRb;
    //Vector2 input;
    //public Transform camPivot;
    public float heading1;
    public float heading2;

    public float minY;
    public float maxY;

    // Start is called before the first frame update
    void Start()
    {
        cameraRb = GetComponent<Rigidbody>();
        cameraRb.constraints = RigidbodyConstraints.FreezeRotationZ;
        cameraRb.constraints = RigidbodyConstraints.FreezeRotationY;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position - player.transform.forward * cameraDistance;
        transform.position = new Vector3(transform.position.x, transform.position.y + cameraHeight, transform.position.z);
        heading1 += Input.GetAxis("Mouse X") * Time.deltaTime * 180;
        heading2 += Input.GetAxis("Mouse Y") * Time.deltaTime * 180;

        if(heading2 > maxY)
        {
            heading2 = maxY;
        }

        if(heading2 < minY)
        {
            heading2 = minY;
        }


        transform.rotation = Quaternion.Euler(-heading2, heading1, 0);
        //transform.localEulerAngles = new Vector3(heading2, heading1, 0);
    }
}