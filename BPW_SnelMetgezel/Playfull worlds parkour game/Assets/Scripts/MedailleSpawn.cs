using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedailleSpawn : MonoBehaviour
{
    public GameObject medailleGoud, medailleZilver, medailleBrons;
    bool coinspawned = false
    // Update is called once per frame
    void Update()
    {
        if (TimeScript.finishedPlace > 0 && coinspawned == false)
        {
            if (TimeScript.finishedPlace == 1)
            {
                Instantiate(medailleGoud, transform.position, transform.rotation);
            }
            else if(TimeScript.finishedPlace == 2)
            {
                Instantiate(medailleZilver, transform.position, transform.rotation);
            }
            else if(TimeScript.finishedPlace == 3)
            {
                Instantiate(medailleBrons, transform.position, transform.rotation);
            }
            else if (TimeScript.finishedPlace == 4)
            {

            }
            coinspawned = true;
        }
    }
}
