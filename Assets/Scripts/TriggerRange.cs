using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRange : MonoBehaviour
{

    public float rate = 0f;
    public float high = 60f;
    public float time = 1f;

    private void OnTriggerStay(Collider other)
    {
        //if (other.CompareTag("Enemy"))
        //{
            
        //    time -= Time.deltaTime;

        //    if(time <= 0)
        //    {
        //        gameObject.GetComponentInParent<PlayerController>().EnterRange(other);
        //        time = 1f;
        //    }
        //}
        //Debug.Log(time);
    }
}
