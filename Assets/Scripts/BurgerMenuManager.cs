using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static BurgerManager;

public class BurgerMenuManager : MonoBehaviour
{
    [SerializeField] Burger targetBurger;

    private void Start()
    {
        foreach (BurgerPart burgerPart in instanceBurgerManager.burgerPartPrefabs.Skip(2))
        {
            var iconUI = new GameObject();
            iconUI.name = burgerPart.name;
            iconUI.AddComponent<RawImage>();
            iconUI.GetComponent<RawImage>().texture = burgerPart.icon;
            iconUI.transform.parent = transform.Find("Scroll View").Find("Viewport").Find("Content");
            var iconTransform = iconUI.GetComponent<RectTransform>();
            iconTransform.anchoredPosition3D = Vector3.zero;
            iconTransform.localRotation = Quaternion.identity;
            iconTransform.localScale = Vector3.one;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
