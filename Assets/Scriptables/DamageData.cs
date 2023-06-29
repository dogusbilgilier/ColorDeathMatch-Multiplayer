using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage Data", menuName = "ScriptableObjects/Damage Data")]
public class DamageData : ScriptableObject
{
    public float regularHit;
    public float headShotAdditive;
    public float colorMatchedHeadShot;
    public float colorMatchMultiplier;
}
