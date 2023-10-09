using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlBurger : MonoBehaviour
{
    public Burger focusedBurger;

    float minHeight;

    private void Awake()
    {
        minHeight = transform.position.y;
    }

    void LateUpdate()
    {
        transform.position += Vector3.up * Input.mouseScrollDelta.y;
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, minHeight, focusedBurger.burgerHeight), transform.position.z);
    }
}
