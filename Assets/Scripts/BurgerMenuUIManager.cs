using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
using static BurgerManager;
using static UnityEditor.SceneView;

public class BurgerMenuUIManager : MonoBehaviour
{
    private Burger targetBurger;
    private CameraControlBurger cameraController;

    private TMP_Text text_PartCount;
    private Slider viewSlider;
    private Slider offsetSlider;

    private void Start()
    {
        targetBurger = transform.parent.GetComponent<CameraControlBurger>().focusedBurger;
        cameraController = transform.parent.GetComponent<CameraControlBurger>();
        foreach (BurgerPart burgerPart in BurgerManagerInstance.burgerPartPrefabs.Skip(2))
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
        viewSlider = transform.Find("View Slider").GetComponent<Slider>();
        offsetSlider = transform.Find("Offset Slider").GetComponent<Slider>();

        viewSlider.onValueChanged.AddListener(delegate { UpdateSliderValue(); });
        offsetSlider.onValueChanged.AddListener(delegate { UpdateOffsetValue(); });
    }

    private void LateUpdate()
    { 
        text_PartCount.text = (targetBurger.burgerParts.Count - 2).ToString();
        viewSlider.value = cameraController.viewSliderValue;
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
