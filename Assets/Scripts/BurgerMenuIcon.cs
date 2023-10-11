using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static BurgerManager;

public class BurgerMenuIcon : MonoBehaviour
{
    public Burger burger;
    public BurgerPart burgerPartPrefab;
    public Button button;

    private void Start()
    {
        button.onClick.AddListener(AddPartToBurger);
    }

    private void AddPartToBurger()
    {
        burger.AddBurgerPart(burgerPartPrefab);
        burger.RegenerateBurger();
    }
}
