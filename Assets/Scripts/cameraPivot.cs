using Unity.VisualScripting;
using UnityEngine;

public class cameraPivot : MonoBehaviour
{
    [SerializeField] private float minFOV = 15.0f;
    [SerializeField] private float maxFOV = 90.0f;
    [SerializeField] private float zoomSensitivity = 10.0f;
    [SerializeField] private float rotateSensitivity = 20.0f;

    float xRotation = 30.0f;
    float yRotation = 90.0f;

    void Start()
    {
        
    }

    void Update()
    {
        //Code for zooming
        float FOV = Camera.main.fieldOfView;
        FOV += Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
        FOV = Mathf.Clamp(FOV, minFOV, maxFOV);
        Camera.main.fieldOfView = FOV;

        //Code for rotation

        if (Input.GetKey("right"))
            yRotation += rotateSensitivity * Time.deltaTime;
        if (Input.GetKey("left"))
            yRotation -= rotateSensitivity * Time.deltaTime;
        if (Input.GetKey("down"))
            xRotation -= rotateSensitivity * Time.deltaTime;
        if (Input.GetKey("up"))
            xRotation += rotateSensitivity * Time.deltaTime;

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
