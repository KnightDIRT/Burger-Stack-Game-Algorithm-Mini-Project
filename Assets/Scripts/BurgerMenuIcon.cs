using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BurgerManager;

public class BurgerMenuIcon : MonoBehaviour
{
    public Burger burger;
    public BurgerPart burgerPartPrefab;

    private void OnMouseDown()
    {
        burger.AddBurgerPart(burgerPartPrefab);
        burger.RegenerateBurger();
    }
}
