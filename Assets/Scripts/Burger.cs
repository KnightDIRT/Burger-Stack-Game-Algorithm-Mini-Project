using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
                RegenerateBurger();
                update = false;
            }
        }
    }

    public void AddBurgerPart(BurgerPart burgerPart)
    {
        burgerPart.chosenModel = UnityEngine.Random.Range(0, burgerPart.models.Count);
        burgerParts.Insert(burgerParts.Count - 1, burgerPart.Clone());
    }

    private void CreateRandomBurger(int size)
    {
        burgerParts.Clear();
        burgerParts.Add(instanceBurgerManager.burgerPartPrefabs[1]);
        for (int i = 0; i < size; i++)
        {
            int index = UnityEngine.Random.Range(2, instanceBurgerManager.burgerPartPrefabs.Count);
            var part = instanceBurgerManager.burgerPartPrefabs[index];
            part.chosenModel = UnityEngine.Random.Range(0, part.models.Count);
            burgerParts.Add(part.Clone());
        }
        burgerParts.Add(instanceBurgerManager.burgerPartPrefabs[0]);
    }

    public void RegenerateBurger() 
    {
        foreach (Transform child in transform) //Clear Burger
        {
            Destroy(child.gameObject);
        }

        int index = 0;
        float nextPartZOffset = 0f;
        foreach (BurgerPart burgerPart in burgerParts)
        {
            var modelData = burgerPart.models[burgerPart.chosenModel];
            ref var part = ref burgerPart.physical;

            //Create part as empty
            part = new GameObject();
            part.name = burgerPart.name;
            part.transform.parent = transform;
            part.transform.position = modelData.offset + Vector3.up * nextPartZOffset;

            //Create model as child of part
            var model = Instantiate(modelData.model, part.transform);
            model.name = burgerPart.name + "_model";
            model.transform.localPosition = Vector3.zero;
            model.transform.localScale *= modelData.scale;

            //Add collider for dynamic parts
            if (burgerPart.name != "Bun")
            {
                var collider = part.AddComponent<BoxCollider>();
                collider.isTrigger = true;
                collider.center = new Vector3(-modelData.offset.x, modelData.modelHeight / 2, -modelData.offset.z);
                var bounds = new Bounds();
                Renderer[] renderers = part.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                {
                    bounds.Encapsulate(renderer.bounds);
                }
                collider.size = new Vector3(bounds.size.x, modelData.modelHeight + modelData.offset.y + extraOffset / 2, bounds.size.z);
                var colliderCode = part.AddComponent<BurgerPartCollider>();
                colliderCode.burger = this;
                colliderCode.index = index;
            }

            nextPartZOffset += modelData.modelHeight + modelData.offset.y + extraOffset;
            index++;
        }
    }

    public void ReRenderBurger()
    {
        float nextPartZOffset = 0f;
        foreach (BurgerPart burgerPart in burgerParts)
        {
            burgerPart.physical.transform.position = burgerPart.models[burgerPart.chosenModel].offset + Vector3.up * nextPartZOffset;
            nextPartZOffset += burgerPart.models[burgerPart.chosenModel].modelHeight + burgerPart.models[burgerPart.chosenModel].offset.y + extraOffset;
        }
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
                part.transform.position = Vector3.up * (nextPartZOffset + burgerPart.models[randPartI].offset.z);
                nextPartZOffset += extraOffset;

                index++;
            }
        }
    }
}
