using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using DG.Tweening;

public class AimMamager : MonoBehaviour
{
    [SerializeField] Transform aimTarget;
    [SerializeField] float lookSpeed;
    [SerializeField] Transform spine;

    [SerializeField] TwoBoneIKConstraint leftHandAim;
    [SerializeField] TwoBoneIKConstraint rightHandAim;
    float targetY, targetZ;
    private void OnEnable()
    {
        EventManager.StartAim += StartAim;
        EventManager.EndAim += EndAim;
    }
    private void OnDisable()
    {
        EventManager.StartAim -= StartAim;
        EventManager.EndAim -= EndAim;
    }
    private void Start()
    {
        leftHandAim.weight = 0f;
        rightHandAim.weight = 0f;
        aimTarget.transform.localPosition = new Vector3(0, 0, 3);
        aimTarget.position = new Vector3(aimTarget.position.x, spine.position.y, aimTarget.position.z);
    }

    private void Update()
    {
        aimTarget.position += Vector3.up * EventManager.MouseInputDelta().y * Time.deltaTime * lookSpeed;

        aimTarget.position = new Vector3(aimTarget.position.x,
            Mathf.Clamp(aimTarget.position.y, spine.position.y - 3, spine.position.y + 3),
            aimTarget.position.z);
    }

    private void StartAim()
    {
        DOTween.Kill("Down0");
        DOTween.Kill("Down1");
        DOTween.To(() => leftHandAim.weight, x => leftHandAim.weight = x, 1, 0.5f).SetId("Up0"); 
        DOTween.To(() => rightHandAim.weight, x => rightHandAim.weight = x, 1, 0.5f).SetId("Up1"); 
    }
    private void EndAim()
    {
        DOTween.Kill("Up0");
        DOTween.Kill("Up1");
        DOTween.To(() => leftHandAim.weight, x => leftHandAim.weight = x, 0, 0.5f).SetId("Down0");
        DOTween.To(() => rightHandAim.weight, x => rightHandAim.weight = x, 0, 0.5f).SetId("Down1");
    }
}
