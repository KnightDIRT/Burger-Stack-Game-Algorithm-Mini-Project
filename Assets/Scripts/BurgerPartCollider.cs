using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BurgerPartCollider : MonoBehaviour
{
    public Burger burger;
    public int index;

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
                var tempIndex = index;
                index = swapIndex;
                swapIndex = tempIndex;
                burger.ReRenderBurger();
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            burger.burgerParts.RemoveAt(index);
            burger.RegenerateBurger();
        }
    }
}
