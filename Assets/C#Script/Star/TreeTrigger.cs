using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("EntityCheck"))
            other.GetComponent<TreeEntityChecker>().P = true;
    }
    void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("EntityCheck"))
            other.GetComponent<TreeEntityChecker>().P = false;
    }
}
