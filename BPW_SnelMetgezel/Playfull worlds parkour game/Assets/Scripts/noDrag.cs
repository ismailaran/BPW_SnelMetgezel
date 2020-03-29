using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noDrag : MonoBehaviour
{
    float inDrag;
    Rigidbody rb;

    void onCollisionEnter(Collision collision)
    {
        rb = collision.gameObject.GetComponent<Rigidbody>();
        inDrag = rb.drag;
        rb.drag = 0f;
    }

    void onCollisionStay(Collision collision)
    {
        rb = collision.gameObject.GetComponent<Rigidbody>();
        rb.drag = 0f;
    }

    void onCollisionExit(Collision collision)
    {
        rb = collision.gameObject.GetComponent<Rigidbody>();
        rb.drag = inDrag;
    }
}
