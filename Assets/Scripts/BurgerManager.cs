using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class BurgerManager : MonoBehaviour
{
    public static BurgerManager BurgerManagerInstance;

    private GameObject highlightBox;
    private Renderer highlightBoxRenderer;
    private Material overPartMaterial;
    private Material dragPartMaterial;

    [Serializable]
    public class BurgerModel
    {
        public GameObject model = null;
        public float modelHeight = 0f;
        public Vector3 offset = Vector3.zero;
        public float scale = 1f;
    }

    [Serializable]
    public class BurgerPart
    {
        public string name = "";
        public Texture2D icon = null;
        public List<BurgerModel> models = new List<BurgerModel>();

        [HideInInspector] public GameObject physical;
        [HideInInspector] public int chosenModel;
        
        public BurgerPart Clone()
        {
            BurgerPart clone = new BurgerPart();
            clone.name = name;
            clone.icon = icon;
            clone.models = models;
            clone.chosenModel = chosenModel;
            return clone;
        }
    }

    [HideInInspector] public List<BurgerPart> burgerPartPrefabs = new List<BurgerPart>(); 
    [HideInInspector] public List<BurgerPart> burgerPartPrefabsAll = new List<BurgerPart>(); //0 is top bun, 1 is bottom bun
    [SerializeField] private List<BurgerPart> inspectorAssignedBurgerPartPrefabs;

    public Dictionary<string, int> burgerCountDict { get; private set; }
        
    private void Awake()
    {
        if (BurgerManagerInstance != null && BurgerManagerInstance != this)
        {
            Destroy(this);
        }
        else
        {
            BurgerManagerInstance = this;
        }
        
        highlightBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Destroy(highlightBox.GetComponent<BoxCollider>());
        highlightBox.name = "Highlighter";
        highlightBox.transform.parent = transform;
        highlightBox.transform.localScale = Vector3.one;
        highlightBoxRenderer = highlightBox.GetComponent<Renderer>();
        highlightBox.SetActive(false);
        overPartMaterial = (Material)Resources.Load("Materials/OverPart");
        dragPartMaterial = (Material)Resources.Load("Materials/DragPart");

        BurgerPart topBun = new BurgerPart();
        topBun.name = "Bun";
        BurgerModel topModel = new BurgerModel();
        topModel.model = (GameObject)Resources.Load("Prefabs/Buns/topbun");
        topModel.offset = new Vector3(1f, 2.2f, 0.5f) / 15f;
        topModel.scale = 1 / 15f;
        topBun.models.Add(topModel);
        burgerPartPrefabsAll.Add(topBun);
        BurgerPart bottomBun = new BurgerPart();
        bottomBun.name = "Bun";
        BurgerModel bottomModel = new BurgerModel();
        bottomModel.model = (GameObject)Resources.Load("Prefabs/Buns/bottombun");
        bottomModel.modelHeight = 1.1f / 15f;
        bottomModel.scale = 1 / 15f;
        bottomBun.models.Add(bottomModel);
        burgerPartPrefabsAll.Add(bottomBun);
        burgerPartPrefabsAll.AddRange(inspectorAssignedBurgerPartPrefabs);

        burgerCountDict = new Dictionary<string, int>();
        foreach (BurgerPart partPrefab in burgerPartPrefabsAll.Skip(1))
        {
            burgerCountDict.Add(partPrefab.name, 0);
        }
    }

    public void HighlightBurgerPart(BurgerPart part, int mode) //0 = hide, 1 = over, 2 = drag
    {
        switch (mode)
        {
            case 0:
                highlightBox.SetActive(false);
                return;
            case 1:
                highlightBox.SetActive(true);
                highlightBoxRenderer.material = overPartMaterial;
                break;
            case 2:
                highlightBox.SetActive(true);
                highlightBoxRenderer.material = dragPartMaterial;
                break;
        }
        var collider = part.physical.GetComponent<BoxCollider>();
        highlightBox.transform.position = part.physical.transform.position + collider.center;
        highlightBox.transform.localScale = collider.size;
    }

    public float[] CompareBurger(Burger burgerRef, Burger burger, float maxScore = -1, float inOrderMultiplier = -1) 
    {
        //[0] Score
        //[1] -1 is perfect match; >=0 is number different part
        if (maxScore == -1) maxScore = GameManager.Instance.maxScore;
        if (inOrderMultiplier == -1) inOrderMultiplier = GameManager.Instance.inOrderMultiplier;

        Dictionary<string, int> burgerCount = burgerCountDict.ToDictionary(entry => entry.Key, entry => entry.Value);

        bool isInOrder = true;

        List<BurgerPart>[] burgerPartsList = new List<BurgerPart>[2] { burgerRef.burgerParts, burger.burgerParts };
        int maxCount = Mathf.Max(burgerPartsList[0].Count, burgerPartsList[1].Count);
        for(int i = 0; i < maxCount; i++)
        {
            if (i < burgerPartsList[0].Count) burgerCount[burgerPartsList[0][i].name]++;
            else isInOrder = false;
            if (i < burgerPartsList[1].Count) burgerCount[burgerPartsList[1][i].name]--;
            else isInOrder = false;
            if (isInOrder && burgerPartsList[0][i].name != burgerPartsList[1][i].name) isInOrder = false;
        }

        if (isInOrder) return new float[2] { maxScore * inOrderMultiplier, -1 };
        float wrongCount = (burgerCount.Sum(x => Mathf.Abs(x.Value)) + Mathf.Abs(burgerPartsList[0].Count - burgerPartsList[1].Count)) / 2;
        return new float[2] { maxScore * Mathf.Max(1f - wrongCount / (burgerPartsList[0].Count - 2), 0), wrongCount};
    }
}