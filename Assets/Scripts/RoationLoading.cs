using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoationLoading : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("RoationZ", 0,0.05f);
    }


    public void RoationZ()
    {
        transform.Rotate(-Vector3.forward * 5);
        if (transform.rotation.z<=-360)
        {
            transform.Rotate(Vector3.forward * 360) ;
        }
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
}
