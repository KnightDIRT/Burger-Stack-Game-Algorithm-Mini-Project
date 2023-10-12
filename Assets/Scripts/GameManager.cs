using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static BurgerManager;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float maxScore { get; private set; }
    private float maxScore_p = 1000f;
    public float inOrderMultiplier { get; private set; }
    private float inOrderMultiplier_p = 2f;

    [Header("Links")]
    [SerializeField] CameraControlBurger cameraControlBurger;
    [SerializeField] Burger orderBurger;
    [SerializeField] Burger inputBurger;
    [SerializeField] Button Button_NextState;
    [SerializeField] TMP_Text Text_Display;
    [SerializeField] TMP_Text Text_Add;

    [Header("Sounds")]
    [HideInInspector] public AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip deleteSound;

    [Header("Debug Display")]
    public float totalScore;
    public int currentLevel;

    private int partCount;

    private float lerpTime = 1f;
    private float currentlerpTime;
    private float displayScore;

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
        audioSource = GetComponent<AudioSource>();

        maxScore = maxScore_p;
        inOrderMultiplier = inOrderMultiplier_p;

        Button_NextState.onClick.AddListener(NextState);
    }

    private void Start()
    {
        currentState = state.ShowingInput;
        NextState();
        totalScore = 0;
        currentLevel = 1;
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0)) audioSource.PlayOneShot(clickSound);

        currentlerpTime -= Time.deltaTime;

        displayScore = Mathf.Lerp(displayScore, totalScore, 1 - currentlerpTime / lerpTime);
        Text_Display.text = string.Format("SCORE: {0:F0}\nLEVEL: {1}", displayScore, currentLevel);

        Color color = Text_Add.color;
        color.a = currentlerpTime / lerpTime;
        Text_Add.color = color;
    }

    public void NextState()
    {
        switch(currentState)
        {
            case state.ShowingOrder:
                currentState = state.ShowingInput;

                inputBurger.CreateRandomBurger(0);
                inputBurger.RegenerateBurger();
                cameraControlBurger.targetBurger = inputBurger;

                orderBurger.gameObject.SetActive(false);
                inputBurger.gameObject.SetActive(true);

                Button_NextState.transform.GetComponentInChildren<TMP_Text>().text = "DONE";
                break;
            case state.ShowingInput:
                currentState = state.ShowingOrder;
                currentLevel++;
                currentlerpTime = lerpTime;

                var compareOutput = BurgerManagerInstance.CompareBurger(orderBurger, inputBurger);
                totalScore += compareOutput[0];
                UpdateAddText(compareOutput[0]);

                BurgerManagerInstance.burgerPartPrefabs.Clear();
                int typeCount = 2 + (currentLevel < 4 ? currentLevel : Mathf.FloorToInt((currentLevel - 3) / 2.4f) + 3); //difficulty curve in green https://www.desmos.com/calculator/yxpydnaqui
                typeCount = Mathf.Min(typeCount, BurgerManagerInstance.burgerPartPrefabsAll.Count);
                for (int i = 0; i < typeCount; i++)
                {
                    BurgerManagerInstance.burgerPartPrefabs.Add(BurgerManagerInstance.burgerPartPrefabsAll[i]);
                }
                
                partCount = (int)(1.5f * Mathf.Sqrt(currentLevel)) + (currentLevel > 50 ? (currentLevel - 50) * (currentLevel - 50) : 0); //difficulty curve in blue https://www.desmos.com/calculator/yxpydnaqui
                orderBurger.CreateRandomBurger(partCount);
                orderBurger.RegenerateBurger();
                cameraControlBurger.targetBurger = orderBurger;

                orderBurger.gameObject.SetActive(true);
                inputBurger.gameObject.SetActive(false);

                Button_NextState.transform.GetComponentInChildren<TMP_Text>().text = "OK";
                break;
        }

        void UpdateAddText(float scoreOut)
        {  
            if (scoreOut == maxScore * inOrderMultiplier)
            {
                Text_Add.color = Color.yellow;
            }
            else
            {
                Text_Add.color = Color.Lerp(Color.red, Color.green, scoreOut / maxScore);
            }
            Text_Add.text = "+" + scoreOut.ToString();
        }
    }
}