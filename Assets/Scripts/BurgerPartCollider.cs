using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BurgerPartCollider : MonoBehaviour
{
    private float dragOutThreshold = 300f;

    public Burger burger;
    public int index;
    public float extraOffset;

    private Vector3 initialMousePos;

    private void Awake()
    {
     
    }

    private void OnMouseDown()
    {
        initialMousePos = Input.mousePosition;
    }
    
    private void OnMouseDrag()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform != transform)
            {
                ref int swapIndex = ref hit.transform.GetComponent<BurgerPartCollider>().index;
                var temp = burger.burgerParts[index];
                burger.burgerParts[index] = burger.burgerParts[swapIndex];
                burger.burgerParts[swapIndex] = temp;
                burger.RegenerateBurger();
            }
        }

        if(Vector3.Distance(initialMousePos, Input.mousePosition) >= dragOutThreshold)
        {
            burger.burgerParts.RemoveAt(index);
            burger.RegenerateBurger();
        }
    }
}
