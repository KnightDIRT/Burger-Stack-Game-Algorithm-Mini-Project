using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static BurgerManager;

public class BurgerMenuManager : MonoBehaviour
{
    [SerializeField] Burger targetBurger;

    private void Awake()
    {
        foreach (BurgerPart burgerPart in instanceBurgerManager.burgerPartPrefabs.Skip(2))
        {
            var iconUI = new GameObject();
            iconUI.name = burgerPart.name;
            iconUI.transform.parent = transform.Find("Scroll View").Find("Viewport").Find("Content");
            iconUI.AddComponent<RawImage>();
            iconUI.GetComponent<RawImage>().texture = burgerPart.icon;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
