using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyPlayerController : MonoBehaviour
{
    Rigidbody rb;

    //inputs
    float z, x, mouseX, mouseY;

    //movement
    public float speedZ, speedX, maxSpeed, jumpSpeed, wallRunJumpForce, jumpMoveSpeedMultiplier, walkDrag, jumpDrag, wallrunForce, wallrunForceUp, JumpXMagnitude, jumpCrouchSpeedBoostTimer, jumpCrouchSpeedBoost, normalDownForce;
    bool canJump = true;
    bool isJumping = false;
    bool jump;
    float jumpTimer = 1000;
    //float jumpTime = 0;
    bool wallrun = false;
    Vector3 jumpNormal = Vector3.up;

    //camera
    public new GameObject camera;
    public float camSpeedX, camSpeedY, camSensitivity, camRotX, camRotY;

    //crouch
    Vector3 crouchScale = new Vector3(1f, 0.5f, 1f);
    Vector3 playerScale = new Vector3(1f, 1f, 1f);
    public float crouchDrag, crouchDownForce;
    bool isCrouching = false;
    bool crouching, lastFrameCrouching;

    //layerMasks
    public LayerMask m_layerMask;

    // respawn variables
    Vector3 respawnLoc;
    Quaternion respawnRot;

    // if the player can be controlled or not
    public static bool canControlPlayer = false;
    public bool control;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        respawnLoc = rb.transform.position;
        respawnRot = rb.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (canControlPlayer == true || control == true)
        {
            inputs();
            movement();
            cameraMove();           
            if (crouching == true && lastFrameCrouching == false) startCrouch();

            // check for objects above head before unchrouch and jump
            Collider[] hitColliders = Physics.OverlapBox(rb.transform.position + new Vector3(0, 1.1f, 0), rb.transform.localScale * 0.9f, Quaternion.identity, m_layerMask);
            if (jump == true && canJump == true && hitColliders.Length < 2) jumping();
            if (crouching == false) if (hitColliders.Length < 2) stopCrouch();
        }
        Debug.Log(rb.drag);
    }

    private void movement()
    {
        //checkformaxspeed
            if (rb.velocity.z > maxSpeed || rb.velocity.z < -maxSpeed) z = 0;
            if (rb.velocity.x < -maxSpeed || rb.velocity.x > maxSpeed) x = 0;

        if (isCrouching == false)
        {
            //speed when walking
            if (isJumping == false)
            {
                rb.AddForce(transform.forward * z * speedZ * Time.deltaTime);
                rb.AddForce(transform.right * x * speedX * Time.deltaTime);
            }
            //speed when jumping
            else if (isJumping == true)
            {
                rb.AddForce(transform.forward * z * speedZ * Time.deltaTime * jumpMoveSpeedMultiplier);
                rb.AddForce(transform.right * x * speedX * Time.deltaTime * jumpMoveSpeedMultiplier);
            }
        }
        //extra speed when going down a ramp
        if (isCrouching == true)
        {
            if (rb.velocity.y < 0)
            {
                rb.AddForce(Vector3.down * Time.deltaTime * crouchDownForce);
            }
            else if (rb.velocity.y > 0)
            {
                if (rb.velocity.magnitude > maxSpeed)
                {
                    rb.AddForce(Vector3.down * Time.deltaTime * crouchDownForce / 3);
                }
                else
                {
                    rb.AddForce(Vector3.down * Time.deltaTime * crouchDownForce);
                }
            }
        }
        else
        {
            if (wallrun == false)rb.AddForce(Vector3.down * Time.deltaTime * normalDownForce);
        }
        //wallrun grav
        if (wallrun == true)
        {
            rb.AddForce(-jumpNormal * Time.deltaTime * wallrunForce);
            rb.AddForce(Vector3.up * Time.deltaTime * wallrunForceUp);
            rb.AddForce(transform.forward * z * speedZ * Time.deltaTime * jumpMoveSpeedMultiplier);
        }

        if (isJumping == true)
        {
            jumpTimer += 1 * Time.deltaTime;
        }
    }

    private void inputs()
    {
        z = Input.GetAxis("Vertical");
        x = Input.GetAxis("Horizontal");
        jump = Input.GetKeyDown(KeyCode.Space);
        lastFrameCrouching = crouching;
        crouching = Input.GetKey(KeyCode.LeftControl);
    }

    private void jumping()
    {
        //jumping against floor and wall, jump force increase with speed
        Vector3 jumpForce = (((jumpNormal * wallRunJumpForce) + (Vector3.up * jumpSpeed)) * ((30 + rb.velocity.magnitude) / JumpXMagnitude));
        jumpForce = new Vector3(jumpForce.x, jumpForce.y / 2, jumpForce.z);
        rb.AddForce(jumpForce, ForceMode.Impulse);
        canJump = false;
        isJumping = true;
        rb.drag = jumpDrag;
        stopCrouch();
        //Debug.Log("jump"+ rb.drag);
        if (isCrouching == false)
        {
            jumpTimer = 0;
        }
        else
        {
            jumpTimer = jumpCrouchSpeedBoostTimer + 1f;
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "jumpable")
        {
            canJump = true;
            isJumping = false;
            if (crouching == false) rb.drag = walkDrag;
            for (int i = 0; i < collision.contactCount; i++) jumpNormal = collision.contacts[i].normal;
            if (jumpNormal.normalized.y < 0.05)
            {
                wallrun = true;
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            }

            //Debug.Log("wallrun" + wallrun);
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "jumpable")
        {
            for (int i = 0; i < collision.contactCount; i++) jumpNormal = collision.contacts[i].normal;
        }
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.gameObject.tag == "spawnpoint")
        {
            respawnLoc = trigger.gameObject.transform.position;
            respawnRot = trigger.gameObject.transform.rotation;
        }

        if (trigger.gameObject.tag == "deathFloor")
        {
            rb.position = respawnLoc;
            rb.rotation = respawnRot;
            rb.velocity = new Vector3(0, 0, 0);
        }

        if(trigger.gameObject.tag == "Finish")
        {
            TimeScript.canCount = false;
            TimeScript.finished = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "jumpable")
        {
            wallrun = false;
            if (isCrouching == false && isJumping == false) rb.drag = jumpDrag;
        }
    }

    private void cameraMove()
    {
        //get mouse input
        mouseX = Input.GetAxis("Mouse X") * Time.fixedDeltaTime * camSensitivity;
        mouseY = Input.GetAxis("Mouse Y") * Time.fixedDeltaTime * camSensitivity;

        //look up/down
        camRotX += mouseY * camSpeedX;
        camRotX = Mathf.Clamp(camRotX, -90f, 90f);

        //set new rot
        rb.transform.Rotate(0, mouseX * camSpeedY, 0);
        camera.transform.localRotation = Quaternion.Euler(camRotX, 0, 0);
    }

    private void startCrouch()
    {
        transform.localScale = crouchScale;
        rb.drag = crouchDrag;
        isCrouching = true;

        // speed boost when crouching after jumping
        if (jumpTimer < jumpCrouchSpeedBoostTimer)
        {
            rb.AddForce(new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized * jumpCrouchSpeedBoost);
        }
    }

        void stopCrouch()
        {
            transform.localScale = playerScale;
            rb.drag = walkDrag;
            isCrouching = false;
        }
    
}
