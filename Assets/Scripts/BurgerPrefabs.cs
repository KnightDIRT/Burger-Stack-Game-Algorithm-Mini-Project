using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BurgerPrefabs : MonoBehaviour
{
    public static BurgerPrefabs instance;

    [Serializable]
    public class BurgerPart
    {
        public string name = "";
        public GameObject model = null;
        public float modelHeight = 0f;
        public Vector3 offset = Vector3.zero;
        public Vector3 scale = Vector3.one;
    }

    public List<BurgerPart> burgerPartPrefabs = new List<BurgerPart>(); //0 is top bun, 1 is bottom bun
    [SerializeField] private List<BurgerPart> inspectorAssignedBurgerPartPrefabs;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        BurgerPart topBun = new BurgerPart();
        topBun.name = "Top Bun";
        topBun.model = (GameObject)Resources.Load("Prefabs/Buns/topbun");
        topBun.offset = new Vector3(1f, 2.2f, 0.5f);
        burgerPartPrefabs.Add(topBun);
        BurgerPart bottomBun = new BurgerPart();
        bottomBun.name = "Bottom Bun";
        bottomBun.model = (GameObject)Resources.Load("Prefabs/Buns/bottombun");
        bottomBun.modelHeight = 1.1f;
        burgerPartPrefabs.Add(bottomBun);
        burgerPartPrefabs.AddRange(inspectorAssignedBurgerPartPrefabs);
    }
}
