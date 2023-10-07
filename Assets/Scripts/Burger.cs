using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using static BurgerManager;

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

    public void RenderBurger() 
    {
        foreach (Transform child in transform) //Clear Burger
        {
            Destroy(child.gameObject);
        }

        int index = 0;
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
            else //All Burger Parts
            {
                part = Instantiate(burgerPart.model, transform);
                part.transform.localScale *= burgerPart.scale;
                if(burgerPart.name != "Bun")
                {
                    part.tag = "BurgerPart";
                    part.AddComponent<BurgerPartCollider>();
                    var partCollider = part.GetComponent<BurgerPartCollider>();
                    partCollider.burger = this;
                    partCollider.index = index;                    
                }
            }
            part.name = burgerPart.name;
            part.transform.position = burgerPart.offset + Vector3.up * nextPartZOffset;
            nextPartZOffset += burgerPart.modelHeight;

            index++;
        }

    }

    private void CreateRandomBurger(int size)
    {
        burgerParts.Clear();
        burgerParts.Add(instanceBurgerManager.burgerPartPrefabs[1]);
        for (int i = 0; i < size; i++)
        {
            int index = UnityEngine.Random.Range(2, instanceBurgerManager.burgerPartPrefabs.Count);
            burgerParts.Add(instanceBurgerManager.burgerPartPrefabs[index]);
        }
        burgerParts.Add(instanceBurgerManager.burgerPartPrefabs[0]);
    }

    private void CreateDebugBurger() //Include all burgerPartPrefabs
    {
        burgerParts.Clear();
        BurgerPart debugPlane = new BurgerPart();
        debugPlane.name = "Debug Plane";
        foreach (BurgerPart burgerPart in instanceBurgerManager.burgerPartPrefabs.Skip(1))
        {
            burgerParts.Add(burgerPart);
            burgerParts.Add(debugPlane);
        }
        burgerParts.Add(instanceBurgerManager.burgerPartPrefabs[0]);
    }

    //Funny cheese count
    //int count;
    //private void CheckCheese()
    //{
    //    count++;
    //    foreach (var burgerPart in burgerParts)
    //    {
    //        if (burgerPart.name != "Cheese" && burgerPart.name != "Bun") return;
    //    }
    //    Debug.Log("Count: " + count);
    //}

    public void AddBurgerPart(BurgerPart burgerPart)
    {
        burgerParts.Add(burgerPart);
    }

    public void RemoveBurgerPart()
    {

    }
}
