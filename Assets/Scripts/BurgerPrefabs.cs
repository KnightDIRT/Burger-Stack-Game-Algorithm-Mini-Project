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

        public BurgerPart(string name = default, GameObject model = default, float modelHeight = default, Vector3 offset = default, Vector3 scale = default)
        {
            this.name = name;
            this.model = model;
            this.modelHeight = modelHeight;
            this.offset = offset;
            this.scale = scale;
        }
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

        burgerPartPrefabs.Add(new BurgerPart("Top Bun", (GameObject)Resources.Load("Prefabs/Buns/topbun")));
        burgerPartPrefabs.Add(new BurgerPart("Bottom Bun", (GameObject)Resources.Load("Prefabs/Buns/bottombun"), 5f));
        burgerPartPrefabs.AddRange(inspectorAssignedBurgerPartPrefabs);
    }
}
