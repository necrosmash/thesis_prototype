using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TraitManager
{

    private static Dictionary<string, GameObject> traitList;

    public static void Initialise()
    {
        traitList = new Dictionary<string, GameObject>();
        traitList.Add("drunk", Resources.Load<GameObject>("Traits/DrunkTrait"));

    }

    public static GameObject GetTrait(string newTrait)
    {

        GameObject trait;
        traitList.TryGetValue(newTrait, out trait);
        return trait;

    }

}
