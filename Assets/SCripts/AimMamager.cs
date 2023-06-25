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

    [SerializeField] GameObject leftHandIK, rightHandIK;
    Vector3 leftHandIKDefRot, rightHandIKDefRot;

    [SerializeField] Ease recoilEase;

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

        leftHandIKDefRot = leftHandIK.transform.localEulerAngles;
        rightHandIKDefRot =rightHandIK.transform.localEulerAngles;
    }

    private void Update()
    {
        aimTarget.position += Vector3.up * EventManager.MouseInputDelta().y * Time.deltaTime * lookSpeed;

        aimTarget.position = new Vector3(aimTarget.position.x,
            Mathf.Clamp(aimTarget.position.y, spine.position.y - 3, spine.position.y + 3),
            aimTarget.position.z);
    }
    public void Recoil(bool left)
    {
        if (left)
        {
            //DOTween.Complete("RecoilLeft");
            //DOTween.Complete("RecoilLeft2");
            leftHandIK.transform.DORotate(Vector3.back * 30, 0.05f, RotateMode.LocalAxisAdd).SetEase(recoilEase).OnComplete(() =>
            {
                leftHandIK.transform.DOLocalRotate(leftHandIKDefRot, 0.1f).SetId("RecoilLeft2");
            }).SetId("RecoilLeft");
        }

        else
        {
            //DOTween.Complete("RecoilRight");
            //DOTween.Complete("RecoilRight2");
            rightHandIK.transform.DORotate(Vector3.forward * 30, 0.05f, RotateMode.LocalAxisAdd).SetEase(recoilEase).OnComplete(() =>
            {
                rightHandIK.transform.DOLocalRotate(rightHandIKDefRot, 0.1f).SetId("RecoilRight2"); ;
            }).SetId("RecoilRight");

        }
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
