using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BurgerPrefabs;

public class Burger : MonoBehaviour
{
    [SerializeField] bool update;
    [SerializeField] int burgerSize;
    
    
    public List<BurgerPart> burgerParts;

    private void Start()
    {
    }

    void Update()
    {
        if (update)
        {
            CreateRandomBurger(burgerSize);
            RenderBurger();
            update = false;
        } 
    }

    private void RenderBurger() 
    {
        foreach (Transform child in transform) //Clear Burger
        {
            Destroy(child.gameObject);
        }

        float nextPartZOffset = 0f;
        foreach (BurgerPart burgerPart in burgerParts)
        {
            GameObject part;
            if (burgerPart.name == "Debug Plane")
            {
                part = GameObject.CreatePrimitive(PrimitiveType.Quad);
                part.transform.parent = transform;
                part.transform.rotation = Quaternion.Euler(90f,0f,0f);
                part.transform.localScale = Vector3.one * 0.5f;
            }
            else
            {
                part = Instantiate(burgerPart.model, transform);
                part.transform.localScale *= burgerPart.scale;
            }
            part.name = burgerPart.name;
            part.transform.position = burgerPart.offset + Vector3.up * nextPartZOffset;
            nextPartZOffset += burgerPart.modelHeight;
        }
    }

    private void CreateRandomBurger(int size)
    {
        burgerParts.Clear();
        burgerParts.Add(instance.burgerPartPrefabs[1]);
        for (int i = 0; i < size; i++)
        {
            int index = UnityEngine.Random.Range(2, burgerParts.Count);
            burgerParts.Add(instance.burgerPartPrefabs[index]);
        }
        burgerParts.Add(instance.burgerPartPrefabs[0]);
    }

    private void CreateDebugBurger() //Include all burgerPartPrefabs
    {
        burgerParts.Clear();
        BurgerPart debugPlane = new BurgerPart();
        debugPlane.name = "Debug Plane";
        foreach (BurgerPart burgerPart in instance.burgerPartPrefabs.Skip(1))
        {
            burgerParts.Add(burgerPart);
            burgerParts.Add(debugPlane);
        }
        burgerParts.Add(instance.burgerPartPrefabs[0]);
    }
}
