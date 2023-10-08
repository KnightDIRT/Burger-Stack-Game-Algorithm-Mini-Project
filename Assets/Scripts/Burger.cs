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
    [SerializeField] float extraOffset;

    public List<BurgerPart> burgerParts;

    void Update()
    {
        if (update)
        {
            //CreateDebugBurger();
            CreateRandomBurger(burgerSize);

            RegenerateBurger(extraOffset);
            update = false;
        }
    }

    public void RegenerateBurger(float extraOffset = 0f) 
    {
        foreach (Transform child in transform) //Clear Burger
        {
            Destroy(child.gameObject);
        }

        int index = 0;
        float nextPartZOffset = 0f;
        foreach (BurgerPart burgerPart in burgerParts)
        {
            int randPartI = UnityEngine.Random.Range(0, burgerPart.models.Count);
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
                part = Instantiate(burgerPart.models[randPartI].model, transform);
                part.transform.localScale *= burgerPart.models[randPartI].scale;
                if(burgerPart.name != "Bun")
                {
                    part.tag = "BurgerPart";
                    part.AddComponent<BurgerPartCollider>();
                    var partCollider = part.GetComponent<BurgerPartCollider>();
                    partCollider.burger = this;
                    partCollider.index = index;
                    partCollider.extraOffset = extraOffset;
                }
            }
            part.name = burgerPart.name;
            part.transform.position = burgerPart.models[randPartI].offset + Vector3.up * nextPartZOffset;
            nextPartZOffset += burgerPart.models[randPartI].modelHeight + burgerPart.models[randPartI].offset.y + extraOffset;

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
}
