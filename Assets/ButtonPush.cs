using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPush : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject objectToAdd;
    public void OnClickAddObject()
    {
        Touch touch = Input.GetTouch(0);
        Vector2 touchPos = touch.position;
        GameObject b = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        b.transform.position = touchPos;
        b.GetComponent<Renderer>().material.color = Color.green;
        b.transform.localScale = new Vector3(100f, 100f, 100f);
        //objectToAdd.SetActive(true);
        //Debug.Log("Adding button clicked");
    }
}
