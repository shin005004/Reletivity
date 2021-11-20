using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor : MonoBehaviour
{
    public float speed = 20f;
    public bool editMode = false;
    public int makeObjectid = 0;
    public float objectScale = 1.0f;
    public GameObject[] gameObjects = new GameObject[3];

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeMode();
            Debug.Log("E");
        }

        if(editMode)
        {
            if(Input.GetMouseButtonDown(0))
            {
                RaycastHit hit = GetClick();
                Vector3 position = hit.point;

                GameObject newObject = Instantiate(gameObjects[makeObjectid], position, Quaternion.identity);
                newObject.transform.localScale = Vector3.one * objectScale;
            }
            if(Input.GetMouseButtonDown(1))
            {
                RaycastHit hit = GetClick();
                if(hit.transform.tag == "Reletivable")
                    Destroy(hit.transform.gameObject);
            }
        }

    }

    RaycastHit GetClick()
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
        return hit;
    }

    void ChangeMode()
    {
        if(!editMode)
        {
            editMode = true;
        }
        if(editMode)
        {
            editMode = false;
        }
    }

    void ChangeObjectId(int id)
    {
        makeObjectid = id;
    }
}
