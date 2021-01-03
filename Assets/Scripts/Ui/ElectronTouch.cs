using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronTouch : MonoBehaviour
{

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
 
    private void OnMouseUp()
    {
        rb.isKinematic = false;
    
    }

    private void OnMouseDown()
    {

   
    }

    private void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z + transform.position.z);

        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        transform.position = objPosition;

        print(objPosition);

        rb.isKinematic = true;
    }
}
