using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TraitManager
{

    private static Dictionary<string, GameObject> traitList;

    private static List<string> spawnTraits;

    public static void Initialise()
    {
        traitList = new Dictionary<string, GameObject>();
        traitList.Add("drunk", Resources.Load<GameObject>("Traits/DrunkTrait"));
        traitList.Add("sober", Resources.Load<GameObject>("Traits/SoberTrait"));
        traitList.Add("knockdown", Resources.Load<GameObject>("Traits/KnockdownTrait"));
        traitList.Add("explosivebarrel", Resources.Load<GameObject>("Traits/ExplosiveBarrelTrait"));
        traitList.Add("burning", Resources.Load<GameObject>("Traits/BurningTrait"));
        traitList.Add("easilydistracted", Resources.Load<GameObject>("Traits/EasilyDistractedTrait"));
        traitList.Add("hungover", Resources.Load<GameObject>("Traits/HungoverTrait"));
        traitList.Add("onearmed", Resources.Load<GameObject>("Traits/OneArmedTrait"));
        traitList.Add("shortsighted", Resources.Load<GameObject>("Traits/ShortSightedTrait"));


        spawnTraits = new List<string>();
        spawnTraits.Add("drunk");
        spawnTraits.Add("sober");
        spawnTraits.Add("easilydistracted");
        spawnTraits.Add("hungover");
        spawnTraits.Add("onearmed");
        spawnTraits.Add("shortsighted");


    }

    public static GameObject GetTrait(string newTrait)
    {

        GameObject trait;
        traitList.TryGetValue(newTrait, out trait);
        return trait;

    }

    public static string GetRandomSpawnTrait()
    {

        int range = spawnTraits.Count;
        string randomTraitKey = spawnTraits[Random.Range(0, range)];

        return randomTraitKey;

    }
}
