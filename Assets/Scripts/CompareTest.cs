using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BurgerManager;

public class CompareTest : MonoBehaviour
{
    [SerializeField] bool compare;

    [Header("Burgers to compare")]
    [SerializeField] Burger burger1;
    [SerializeField] Burger burger2;

    void Update()
    {
        if (compare)
        {
            ShowCompareResult();

            compare = false;
        }
    }

    public void ShowCompareResult()
    {
        var compareOutput = BurgerManagerInstance.CompareBurger(burger1, burger2);
        Debug.Log(string.Format("Score: {0}\nIncorrect: {1}", compareOutput[0].ToString(), compareOutput[1].ToString()));
    }
}
