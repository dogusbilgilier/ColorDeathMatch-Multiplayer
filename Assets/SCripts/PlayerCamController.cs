using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerCamController : MonoBehaviourPun
{
    [SerializeField] GameObject camPos;
    [SerializeField] float lerpSpeed;
    Transform camTransform;

    private void Start()
    {
        if (photonView.IsMine)
        {
            camTransform = Camera.main.transform;
            camTransform.rotation = Quaternion.identity;
        }
       
    }
    private void LateUpdate()
    {
        if (photonView.IsMine)
            camTransform.SetPositionAndRotation(camPos.transform.position,
            Quaternion.Euler(new Vector3(camPos.transform.eulerAngles.x, camPos.transform.eulerAngles.y, 0)));
            
    }
}
