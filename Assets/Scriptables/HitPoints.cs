using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HitData", menuName = "ScriptableObjects/Hit Data")]
public class HitPoints : ScriptableObject
{
    public int regularHit;
    public int headShotAdditive;
    public int colorMatchedHeadShot;
}
