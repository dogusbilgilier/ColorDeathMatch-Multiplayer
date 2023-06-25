using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerCamController : MonoBehaviour
{
    [SerializeField] GameObject camPos;
    [SerializeField][Range(-90,90)] float minX, maxX;
    [SerializeField] float lerpSpeed;

    float xRot;

    private void Start()
    {
        transform.rotation = Quaternion.identity;
    }
    private void LateUpdate()
    {

        transform.SetPositionAndRotation(Vector3.Lerp(transform.position, camPos.transform.position, lerpSpeed * Time.deltaTime),
            Quaternion.Euler(new Vector3(camPos.transform.eulerAngles.x, camPos.transform.eulerAngles.y, 0)));
            
    }
}
