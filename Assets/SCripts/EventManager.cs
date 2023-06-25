using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager 
{
    //-----------------INPUT EVENTS----------------
    public static Func<Vector2> MovementInputDelta;
    public static Func<Vector3> MouseInputDelta;
    public static Func<bool> SpeedBoost;
    public static Func<bool> Jump;
    public static Func<bool> Aim;

    //-----------------------------------------
    public static Action StartAim;
    public static Action EndAim;
    public static Action ShootAttempt;
    public static Action StopShoot;
}
