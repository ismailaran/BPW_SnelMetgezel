using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedboost : MonoBehaviour
{
    public float speedboostSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay(Collider trigger)
    {
        trigger.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * Time.deltaTime * speedboostSpeed);
    }
}
