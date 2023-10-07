using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BurgerPartCollider : MonoBehaviour
{
    public Burger burger;
    public int index;

    private void Awake()
    {
     
    }

    private void OnMouseDown()
    {
        burger.burgerParts.RemoveAt(index);
        burger.RenderBurger();
    }
}
