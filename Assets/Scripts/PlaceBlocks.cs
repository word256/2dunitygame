using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBlocks : MonoBehaviour
{
    public bool enable;
    public GameObject selectionGameObject;
    public GameObject placeableObject;
    private Vector2 selectionPoint;

    private void Update()
    {
        Vector2 mouse = Input.mousePosition;
        selectionPoint = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, Camera.main.nearClipPlane));
        Debug.Log(Mathf.Floor(selectionPoint.x)+ ", " + Mathf.Floor(selectionPoint.y));
        selectionGameObject.transform.position = new Vector2(Mathf.Floor(selectionPoint.x + 0.5f), Mathf.Floor(selectionPoint.y + 0.5f));

        if (Input.GetMouseButtonDown(0))
        {
            GameObject block = Instantiate(placeableObject);
            placeableObject.transform.position = new Vector2(Mathf.Floor(selectionPoint.x + 0.5f), Mathf.Floor(selectionPoint.y + 0.5f));
        }
    }
}
