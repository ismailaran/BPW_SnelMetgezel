using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    CharacterController controller;

    // inputs
    float x, z, mouseX, mouseY;
    //movement
    float speedX, speedZ, jump;
    float lastZ, lastX;
    public float maxSpeedZ, maxSpeedX, accelerationRate, decelerationRate;
    public float gravity = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        getInput();

        if (controller.isGrounded)
        {
            if (z != 0)
            {
                speedZ = speedZ + accelerationRate * z * Time.deltaTime;
                if (speedZ > maxSpeedZ) speedZ = maxSpeedZ;
                lastZ = z;
            }
            else
            {
                speedZ = speedZ + decelerationRate * lastZ * Time.deltaTime;
                if (speedZ < 0) speedZ = 0;
            }
            if (x != 0)
            {
                speedX = speedX + accelerationRate * x * Time.deltaTime;
                if (speedX > maxSpeedX) speedX = maxSpeedX;
                lastX = x;
            }
            else
            {
                speedX = speedX + decelerationRate * lastX * Time.deltaTime;
                if (speedX < 0) speedX = 0;
            }
            Debug.Log(speedX + " , " + speedZ);
        }
        jump -= gravity * Time.deltaTime;
        controller.Move(new Vector3( speedX, jump, speedZ) * Time.deltaTime);
    }

    private void getInput()
    {
        z = Input.GetAxis("Vertical");
        x = Input.GetAxis("Horizontal");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
    }
}
