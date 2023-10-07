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
        public string name;
        public GameObject model;
        public Vector3 offset = Vector3.zero;
        public float modelHeight;
    }

    public BurgerPart[] burgerPartPrefabs { get; private set; } //0 is top bun, 1 is bottom bun
    [SerializeField] private BurgerPart[] inspectorAssignedBurgerPartPrefabs;

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

        burgerPartPrefabs = new BurgerPart[2];
        burgerPartPrefabs[0].name = "Top Bun";
        burgerPartPrefabs[0].model = (GameObject)Resources.Load("Prefabs/Buns/topbun");
        burgerPartPrefabs[0].modelHeight = 0f;
        burgerPartPrefabs[1].name = "Bottom Bun";
        burgerPartPrefabs[1].model = (GameObject)Resources.Load("Prefabs/Buns/bottombun");
        burgerPartPrefabs[1].modelHeight = 5f;
        burgerPartPrefabs.Concat(inspectorAssignedBurgerPartPrefabs);
    }
}
