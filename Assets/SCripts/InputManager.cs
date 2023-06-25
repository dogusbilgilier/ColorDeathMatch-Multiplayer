using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]Vector2 movementInputDelta;
    [SerializeField] float ySens,xSens;
    [SerializeField] bool speedBoost;
    [SerializeField] bool jump;
    [SerializeField] bool aim;
    Vector2 mouseInputDelta;

    private void OnEnable()
    {
        EventManager.MovementInputDelta = () => movementInputDelta;
        EventManager.MouseInputDelta = () => mouseInputDelta;
        EventManager.SpeedBoost = () => speedBoost;
        EventManager.Jump = () => jump;
        EventManager.Aim = () => aim;
    }


    private void Update()
    {
        movementInputDelta = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        mouseInputDelta = new Vector2(Input.GetAxis("Mouse X") * xSens, Input.GetAxis("Mouse Y")) * ySens;
        speedBoost = Input.GetKey(KeyCode.LeftShift);
        jump = Input.GetKey(KeyCode.Space);
        aim = Input.GetMouseButton(1);
        if (aim && Input.GetMouseButton(0))
        {
            EventManager.ShootAttempt.Invoke();
        }
        if (!aim || Input.GetMouseButton(0)==false)
        {
            EventManager.StopShoot.Invoke();
        }

    }


}
