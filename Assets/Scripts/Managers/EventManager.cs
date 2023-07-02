using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager
{
    public static Action<bool> SetColorSelectorActivity { get; internal set; }
    public static Action<PlayerColor, GunColor> SetColorSelectorOutlines { get; internal set; }
}
