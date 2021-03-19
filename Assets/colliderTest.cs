using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class colliderTest : MonoBehaviour
{
    public List<string> grabbableObjects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "grabbable"){
            grabbableObjects.Add(other.gameObject.name);
            Debug.Log("grabbable object");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (grabbableObjects.Contains(other.gameObject.name)){
            grabbableObjects.Remove(other.gameObject.name);
            Debug.Log(other.gameObject.name + "is removed from grabbable object list");
        }
        Debug.Log("離脱");
    }

    public List<string> getGrabbableObjects(){
        return grabbableObjects;
    }
}
