using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelsManager : MonoBehaviour
{



    float speed = -0.01f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        speed -= 0.01f;

        gameObject.transform.localPosition = new Vector3((gameObject.transform.localPosition.x) + speed, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z); ;
        
    }
}
