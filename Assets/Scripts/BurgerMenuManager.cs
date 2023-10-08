using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BurgerManager;

public class BurgerMenuManager : MonoBehaviour
{
    [SerializeField] Burger targetBurger;

    private void Awake()
    {
        foreach (BurgerPart burgerPart in instanceBurgerManager.burgerPartPrefabs.Skip(2))
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
