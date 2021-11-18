using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor : MonoBehaviour
{
    public float speed = 20f;
    public bool editMode = false;
    public int makeObjectid = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(x, 0, z);
        transform.transform.Translate(movement * Time.deltaTime * speed);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform != null)
                {
                    Debug.Log(hit.point);
                }
            }
        }
    }

    void ChangeMode()
    {
        if(!editMode)
        {
            editMode = 
        }
    }
}
