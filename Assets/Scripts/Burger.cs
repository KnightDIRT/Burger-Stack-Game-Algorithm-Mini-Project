using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using static BurgerManager;

public class Burger : MonoBehaviour
{
    [SerializeField] bool debug;
    [SerializeField] bool update;
    [SerializeField] int burgerSize;
    [SerializeField] float extraOffset;

    public List<BurgerPart> burgerParts;

    void Update()
    {
        if (debug)
        {
            CreateAndRenderDebugBurger();
        }
        else
        {
            if (update)
            {
                CreateRandomBurger(burgerSize);
                RegenerateBurger(extraOffset);
                update = false;
            }
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

    private void CreateAndRenderDebugBurger() //Include all burgerPartPrefabs
    {
        //CREATE
        burgerParts.Clear();
        foreach (BurgerPart burgerPart in instanceBurgerManager.burgerPartPrefabs.Skip(1))
        {
            burgerParts.Add(burgerPart);
        }
        burgerParts.Add(instanceBurgerManager.burgerPartPrefabs[0]);

        //RENDER
        foreach (Transform child in transform) //Clear Burger
        {
            Destroy(child.gameObject);
        }

        int index = 0;
        float nextPartZOffset = 0f;
        foreach (BurgerPart burgerPart in burgerParts)
        {
            for (int randPartI = 0; randPartI < burgerPart.models.Count; randPartI++)
            {
                GameObject part;
                part = Instantiate(burgerPart.models[randPartI].model, transform);
                part.transform.localScale *= burgerPart.models[randPartI].scale;
                part.name = burgerPart.name;
                part.transform.position = burgerPart.models[randPartI].offset + Vector3.up * nextPartZOffset;
                nextPartZOffset += burgerPart.models[randPartI].modelHeight + burgerPart.models[randPartI].offset.y;

                part = GameObject.CreatePrimitive(PrimitiveType.Quad);
                part.name = "Debug Plane";
                part.transform.parent = transform;
                part.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                part.transform.localScale = Vector3.one * 0.5f;
                part.transform.position = burgerPart.models[randPartI].offset + Vector3.up * nextPartZOffset;
                nextPartZOffset += extraOffset;

                index++;
            }
        }
    }

    public void AddBurgerPart(BurgerPart burgerPart)
    {
        burgerParts.Add(burgerPart);
    }
}
