using PW;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class BurgerManager : MonoBehaviour
{
    public static BurgerManager instanceBurgerManager;

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

    [HideInInspector] public List<BurgerPart> burgerPartPrefabs = new List<BurgerPart>(); //0 is top bun, 1 is bottom bun
    [SerializeField] private List<BurgerPart> inspectorAssignedBurgerPartPrefabs; 

    private void Awake()
    {
        if (instanceBurgerManager != null && instanceBurgerManager != this)
        {
            Destroy(this);
        }
        else
        {
            instanceBurgerManager = this;
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
        burgerPartPrefabs.Add(topBun);
        BurgerPart bottomBun = new BurgerPart();
        bottomBun.name = "Bun";
        BurgerModel bottomModel = new BurgerModel();
        bottomModel.model = (GameObject)Resources.Load("Prefabs/Buns/bottombun");
        bottomModel.modelHeight = 1.1f / 15f;
        bottomModel.scale = 1 / 15f;
        bottomBun.models.Add(bottomModel);
        burgerPartPrefabs.Add(bottomBun);
        burgerPartPrefabs.AddRange(inspectorAssignedBurgerPartPrefabs);
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

    public int[] CompareBurger(Burger burger1, Burger burger2) 
    {
        //[0]: 0 is perfect match, >0 is number mismatch
        //[1]: 0 is same count, >0 is number count mismatch
        int missMatch = 0;
        Dictionary<string, int> burgerCount = new Dictionary<string, int>();

        int len = Mathf.Max(burger1.burgerParts.Count, burger2.burgerParts.Count);
        for(int i = 0; i < len; i++)
        {
            burgerCount[burger1.burgerParts[i].name]++;
            burgerCount[burger2.burgerParts[i].name]--;
            if (burger1.burgerParts[i].name != burger2.burgerParts[i].name) missMatch++;
        }
        return new int[2] { missMatch, burgerCount.Sum(x => Mathf.Abs(x.Value)) };
    }   
}
