using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursorFollower : MonoBehaviour
{
    Vector3 lastMousePos;

    // Start is called before the first frame update
    void Start()
    {
        lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 movementVector = mousePos - lastMousePos;

        gameObject.transform.position += movementVector;
        lastMousePos = mousePos;
//        if (Input.GetMouseButtonDown(0)) {
//            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .2f);
//        } else if (Input.GetMouseButtonUp(0)) {
//            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
//        }
    }
}
