using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BurgerManager;

public class CompareTest : MonoBehaviour
{
    [SerializeField] bool compare;
    [SerializeField] Burger burger1;
    [SerializeField] Burger burger2;

    void Update()
    {
        if (compare)
        {
            Debug.Log(string.Format("Mismatch: {0}", instanceBurgerManager.CompareBurger(burger1, burger2).ToString()));

            compare = false;
        }
    }
}
