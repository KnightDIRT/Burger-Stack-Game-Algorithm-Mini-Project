using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
using static BurgerManager;

public class BurgerMenuManager : MonoBehaviour
{
    [SerializeField] Burger targetBurger;

    private TMP_Text text_PartCount;

    private void Start()
    {
        foreach (BurgerPart burgerPart in instanceBurgerManager.burgerPartPrefabs.Skip(2))
        {
            var iconUI = new GameObject();
            iconUI.name = burgerPart.name;
            var rawImage = iconUI.AddComponent<RawImage>();
            rawImage.texture = burgerPart.icon;
            var iconTransform = iconUI.GetComponent<RectTransform>();
            iconTransform.SetParent(transform.Find("Scroll View").Find("Viewport").Find("Content"));
            iconTransform.anchoredPosition3D = Vector3.zero;
            iconTransform.localRotation = Quaternion.identity;
            iconTransform.localScale = Vector3.one;

            var iconCode = iconUI.AddComponent<BurgerMenuIcon>();
            iconCode.burger = targetBurger;
            iconCode.burgerPartPrefab = burgerPart;
            iconCode.button = iconUI.AddComponent<Button>();
        }

        text_PartCount = transform.Find("Part Count Text").GetComponent<TMP_Text>();
    }

    private void LateUpdate()
    {
        text_PartCount.text = (targetBurger.burgerParts.Count - 2).ToString();
    }
}
