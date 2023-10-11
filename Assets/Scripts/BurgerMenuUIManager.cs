using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static BurgerManager;
using static UnityEditor.SceneView;

public class BurgerMenuUIManager : MonoBehaviour
{
    private CameraControlBurger cameraController;
    private Burger targetBurger;
    private Burger lastBurger;
    private Transform contentUI; //Scroll view contents
    private TMP_Text text_IsReadOnly;

    private bool isReadOnly;

    private TMP_Text text_PartCount;
    private Slider viewSlider;
    private Slider offsetSlider;

    private void Start()
    {
        cameraController = transform.parent.GetComponent<CameraControlBurger>();
        contentUI = transform.Find("Scroll View").Find("Viewport").Find("Content");
        text_IsReadOnly = contentUI.Find("IsReadOnly Text").GetComponent<TMP_Text>();

        text_PartCount = transform.Find("Part Count Text").GetComponent<TMP_Text>();
        viewSlider = transform.Find("View Slider").GetComponent<Slider>();
        offsetSlider = transform.Find("Offset Slider").GetComponent<Slider>();

        viewSlider.onValueChanged.AddListener(delegate { UpdateSliderValue(); });
        offsetSlider.onValueChanged.AddListener(delegate { UpdateOffsetValue(); }); 
    }

    private void LateUpdate()
    {
        targetBurger = cameraController.targetBurger;
        if (targetBurger != lastBurger)
        {
            lastBurger = targetBurger;
            isReadOnly = targetBurger.isReadOnly;
            UpdateContent();
        }

        text_PartCount.text = (targetBurger.burgerParts.Count - 2).ToString();
        viewSlider.value = cameraController.viewSliderValue;
    }

    private void UpdateContent()
    {
        foreach (Transform child in contentUI.Find("Icons")) Destroy(child.gameObject);

        if (isReadOnly)
        {
            text_IsReadOnly.gameObject.SetActive(true);
            UpdateIsReadOnlyText();
            return;
        }

        text_IsReadOnly.gameObject.SetActive(false);
        foreach (BurgerPart burgerPart in BurgerManagerInstance.burgerPartPrefabs.Skip(2))
        {
            var iconUI = new GameObject();
            iconUI.name = burgerPart.name;
            var rawImage = iconUI.AddComponent<RawImage>();
            rawImage.texture = burgerPart.icon;
            var iconTransform = iconUI.GetComponent<RectTransform>();
            iconTransform.SetParent(contentUI.Find("Icons"));
            iconTransform.anchoredPosition3D = Vector3.zero;
            iconTransform.localRotation = Quaternion.identity;
            iconTransform.localScale = Vector3.one;

            var iconCode = iconUI.AddComponent<BurgerMenuIcon>();
            iconCode.burger = targetBurger;
            iconCode.burgerPartPrefab = burgerPart;
            iconCode.button = iconUI.AddComponent<Button>();
        }

        void UpdateIsReadOnlyText()
        {
            var text = "ORDER";

            Dictionary<string, int> burgerCount = BurgerManagerInstance.burgerCountDict.ToDictionary(entry => entry.Key, entry => entry.Value);

            foreach (BurgerPart part in targetBurger.burgerParts)
            {
                burgerCount[part.name]++;
            }

            foreach (string key in burgerCount.Keys)
            {
                if (key == "Bun" || burgerCount[key] == 0) continue;
                text += string.Format("\n{0}: {1}", key, burgerCount[key]);
            }
            
            text_IsReadOnly.text = text;
        }
    }

    private void UpdateSliderValue()
    {
        cameraController.viewSliderValue = viewSlider.value;
    }

    private void UpdateOffsetValue()
    {
        targetBurger.extraOffset = offsetSlider.value;
        targetBurger.RegenerateBurger();
    }
}
