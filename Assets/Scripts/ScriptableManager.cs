using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableManager : MonoBehaviour
{
    public static ScriptableManager Instance;

    public DamageData damageData;

    private void Awake()
    {
        Instance = this;
    }

}
