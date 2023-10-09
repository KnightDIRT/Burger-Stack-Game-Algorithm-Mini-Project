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
            var output = instanceBurgerManager.CompareBurger(burger1, burger2);
            Debug.Log(string.Format("Mismatch: {0}\nCount Mismatch: {1}", output[0].ToString(), output[1].ToString()));

            compare = false;
        }
    }
}
