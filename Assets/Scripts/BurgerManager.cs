using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BurgerManager : MonoBehaviour
{
    public static BurgerManager instanceBurgerManager;

    [Serializable]
    public class BurgerPart
    {
        public string name = "";
        public GameObject model = null;
        public float modelHeight = 0f;
        public Vector3 offset = Vector3.zero;
        public float scale = 1f;

        public GameObject physical;
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
        BurgerPart topBun = new BurgerPart();
        topBun.name = "Bun";
        topBun.model = (GameObject)Resources.Load("Prefabs/Buns/topbun");
        topBun.offset = new Vector3(1f, 2.2f, 0.5f) / 15f;
        topBun.scale = 1 / 15f;
        burgerPartPrefabs.Add(topBun);
        BurgerPart bottomBun = new BurgerPart();
        bottomBun.name = "Bun";
        bottomBun.model = (GameObject)Resources.Load("Prefabs/Buns/bottombun");
        bottomBun.modelHeight = 1.1f / 15f;
        bottomBun.scale = 1 / 15f;
        burgerPartPrefabs.Add(bottomBun);
        burgerPartPrefabs.AddRange(inspectorAssignedBurgerPartPrefabs);
    }
}
