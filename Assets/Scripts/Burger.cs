using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Burger : MonoBehaviour
{
    public BurgerPrefabs.BurgerPart[] burgerParts;

    private void Start()
    {
        CreateDebugBurger();
        RenderBurger();
    }

    void Update()
    {
        
    }

    private void RenderBurger() 
    {
        foreach (Transform child in transform) //Clear Burger
        {
            Destroy(child.gameObject);
        }

        float nextPartZOffset = 0f;
        foreach (BurgerPrefabs.BurgerPart burgerPart in burgerParts)
        {
            GameObject part;
            if (burgerPart.name == "Debug Plane")
            {
                part = GameObject.CreatePrimitive(PrimitiveType.Quad);
                part.transform.parent = transform;
                part.transform.localScale = Vector3.one * 3f;
            }
            else
            {
                part = Instantiate(burgerPart.model, transform);
            }
            part.name = burgerPart.name;
            part.transform.position = burgerPart.offset + Vector3.up * nextPartZOffset;
            nextPartZOffset += burgerPart.modelHeight;
        }
    }

    private void CreateRandomBurger(int size)
    {

    }

    private void CreateDebugBurger() //Include all burgerPartPrefabs
    {
        BurgerPrefabs.BurgerPart debugPlane = new BurgerPrefabs.BurgerPart();
        debugPlane.name = "Debug Plane";
        foreach (BurgerPrefabs.BurgerPart burgerPart in BurgerPrefabs.instance.burgerPartPrefabs)
        {
            burgerParts.Append(burgerPart);
            burgerParts.Append(debugPlane);
        }
    }
}
