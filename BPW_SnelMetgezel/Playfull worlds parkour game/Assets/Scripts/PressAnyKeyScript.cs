using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAnyKeyScript : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            RigidbodyPlayerController.canControlPlayer = true;
            TimeScript.canCount = true;
            Destroy(this);
        }
    }
}
