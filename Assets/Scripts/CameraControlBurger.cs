using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlBurger : MonoBehaviour
{
    [SerializeField] private float sensitivity = 0.1f;

    public Burger focusedBurger;

    [HideInInspector] public float viewSliderValue = 1f;

    private float minHeight;

    private void Awake()
    {
        minHeight = transform.position.y;
    }

    void LateUpdate()
    {
        viewSliderValue += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        viewSliderValue = Mathf.Clamp01(viewSliderValue);
        var maxHeight = Mathf.Max(minHeight, focusedBurger.burgerHeight - GetComponent<Camera>().orthographicSize/2);
        var currentViewHeight = viewSliderValue * (maxHeight - minHeight) + minHeight;
        transform.position = new Vector3(transform.position.x, currentViewHeight, transform.position.z);
    }
}
