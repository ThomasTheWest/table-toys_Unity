using UnityEngine;

public class fxIslandCrumble : MonoBehaviour
{
    private bool doOnce = false;
    private float speed = 0;
    private float maxSpeed = 30.0f;
    private float acceleration = 0.9f;

    void Update()
    {
        if (Input.GetKeyDown("r") && !doOnce)
            doOnce = true;

        if (speed < maxSpeed && doOnce)
            speed += acceleration * Time.deltaTime;

        transform.position = new Vector3(0f, transform.position.y - speed * Time.deltaTime, 0f);
    }
}
