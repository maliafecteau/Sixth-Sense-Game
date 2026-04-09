using UnityEngine;
using UnityEngine.InputSystem;

public class cameraScript : MonoBehaviour
{
    //[SerializeField] Transform playerTransform;
    //Vector3 cameraOffset = new Vector3(-3,3,2);
    [SerializeField] Vector3 cameraTransform;
    [SerializeField] Quaternion cameraRotation;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cameraTransform = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        cameraRotation = transform.rotation;
        transform.position = cameraTransform;
        transform.rotation = cameraRotation;
        //transform.position = playerTransform.position + cameraOffset;
        //transform.rotation = Quaternion.Euler(25, 121, -0.8f);
    }
}


    
