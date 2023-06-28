using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviourPun
{
    [SerializeField]public Vector2 movementInputDelta;
    [SerializeField] public float ySens,xSens;
    [SerializeField] public bool speedBoost;
    [SerializeField] public bool jump;
    [SerializeField] public bool aim;
    ShootingManager shootingManager;
    public Vector2 mouseInputDelta;


    private void OnEnable()
    {
        shootingManager = GetComponent<ShootingManager>();
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if (Cursor.visible == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        movementInputDelta = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        mouseInputDelta = new Vector2(Input.GetAxis("Mouse X") * xSens, Input.GetAxis("Mouse Y")) * ySens;
        speedBoost = Input.GetKey(KeyCode.LeftShift);
        jump = Input.GetKey(KeyCode.Space);
        aim = Input.GetMouseButton(1);
        if (aim && Input.GetMouseButton(0))
        {
            shootingManager.ShootAttempt();
        }
        if (!aim || Input.GetMouseButton(0)==false)
        {
            shootingManager.StopShooting();
        }

    }


}
