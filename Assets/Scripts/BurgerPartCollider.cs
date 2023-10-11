using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BurgerManager;

public class BurgerPartCollider : MonoBehaviour
{
    public Burger burger;
    public int index;

    private void OnMouseOver()
    {
        if(Input.GetMouseButton(0)) instanceBurgerManager.HighlightBurgerPart(burger.burgerParts[index], 2);
        else instanceBurgerManager.HighlightBurgerPart(burger.burgerParts[index], 1);

        if (Input.GetMouseButtonDown(1))
        {
            instanceBurgerManager.HighlightBurgerPart(burger.burgerParts[index], 0);
            burger.burgerParts.RemoveAt(index);
            burger.RegenerateBurger();
        }
    }

    private void OnMouseDrag()
    {
        instanceBurgerManager.HighlightBurgerPart(burger.burgerParts[index], 2);
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
    }

    private void OnMouseExit()
    {
        instanceBurgerManager.HighlightBurgerPart(burger.burgerParts[index], 0);
    }

    private void OnMouseUp()
    {
        instanceBurgerManager.HighlightBurgerPart(burger.burgerParts[index], 0);
    }
}
