using UnityEngine;
using UnityEngine.InputSystem;

public class cameraScript : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] Vector3 cameraOffset;
    
    void Start()
    {
        cameraOffset = new Vector3(-5, 3, 3);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.position + cameraOffset;
        transform.LookAt(playerTransform.position);
    }
}


    
