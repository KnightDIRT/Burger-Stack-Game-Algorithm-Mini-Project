using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static BurgerManager;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] CameraControlBurger cameraControlBurger;
    [SerializeField] Burger orderBurger;
    [SerializeField] Burger inputBurger;
    [SerializeField] Button Button_NextState;
    [SerializeField] TMP_Text Text_Display;

    public float totalScore;
    public int currentLevel;

    private int partCount = 1;

    enum state
    {
        ShowingOrder,
        ShowingInput
    }
    private state currentState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        Button_NextState.onClick.AddListener(NextState);
    }

    private void Start()
    {
        currentState = state.ShowingInput;
        NextState();
        totalScore = 0;
        currentLevel = 1;
    }

    private void Update()
    {
        switch (currentState) 
        {
            case state.ShowingOrder:
                orderBurger.gameObject.SetActive(true);
                inputBurger.gameObject.SetActive(false);
                break;
            case state.ShowingInput:
                orderBurger.gameObject.SetActive(false);
                inputBurger.gameObject.SetActive(true);
                break;
        }
    }

    private void LateUpdate()
    {
        Text_Display.text = string.Format("SCORE: {0}\nLEVEL: {1}", totalScore, currentLevel);
    }

    public void NextState()
    {
        switch(currentState)
        {
            case state.ShowingOrder:
                inputBurger.CreateRandomBurger(0);
                inputBurger.RegenerateBurger();
                cameraControlBurger.targetBurger = inputBurger;

                currentState = state.ShowingInput;
                break;
            case state.ShowingInput:
                currentLevel++;

                var compareOutput = BurgerManagerInstance.CompareBurger(orderBurger, inputBurger);
                totalScore += compareOutput[0];

                partCount = (int)(1.5f * Mathf.Sqrt(currentLevel)) + (currentLevel > 50 ? (currentLevel - 50) * (currentLevel - 50) : 0); //difficulty curve in blue https://www.desmos.com/calculator/yxpydnaqui
                orderBurger.CreateRandomBurger(partCount);
                orderBurger.RegenerateBurger();
                cameraControlBurger.targetBurger = orderBurger;

                currentState = state.ShowingOrder;
                break;
        }
    }
}