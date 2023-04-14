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
        traitList.Add("knockdown", Resources.Load<GameObject>("Traits/KnockdownTrait"));
        traitList.Add("barrel", Resources.Load<GameObject>("Traits/ExplosiveBarrelTrait"));
        traitList.Add("burning", Resources.Load<GameObject>("Traits/BurningTrait"));
        traitList.Add("distracted", Resources.Load<GameObject>("Traits/EasilyDistractedTrait"));


    }

    public static GameObject GetTrait(string newTrait)
    {

        GameObject trait;
        traitList.TryGetValue(newTrait, out trait);
        return trait;

    }

}
