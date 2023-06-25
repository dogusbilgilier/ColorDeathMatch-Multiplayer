using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool startShoot;
    //[SerializeField] ParticleSystem particle;
    [HideInInspector] public Player player;
    [HideInInspector] public GunColor gunColor;
    [SerializeField] Transform defParent;
    [SerializeField] MeshRenderer meshRenderer;
    Vector3 startPos,startRotation;
    float speed;
    float timer;
    private void Start()
    {
        player = GetComponent<Player>();
        startPos = transform.localPosition;
        startRotation = transform.localEulerAngles;
        defParent=transform.parent;
        
    }
    private void SetColor()
    {
        switch (gunColor)
        {
            case GunColor.Red:
                meshRenderer.material.color = Color.red;
                break;
            case GunColor.Green:
                meshRenderer.material.color= Color.green;
                break;
            case GunColor.Blue:
                meshRenderer.material.color = Color.blue;
                break;
            case GunColor.Purple:
                meshRenderer.material.color = Color.blue + Color.red;
                break;
            default:
                break;
        }
    }
    public void Shoot(Vector3 targetPoint,float speed,GunColor gunColor)
    {
        //particle.Play();
        gameObject.SetActive(true);
        this.gunColor = gunColor;
        SetColor();
        this.speed = speed;
        transform.LookAt(targetPoint);
        transform.DOScale(Vector3.one * 3f, 0.15f).SetId("BulletScale");
        transform.SetParent(null);
        startShoot = true;
        
        
    }
    private void Update()
    {
        if(startShoot)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
            timer += Time.deltaTime;
        }
        if (timer > 1)
        {
            ResetShooting();
            startShoot=false;
            timer = 0;
        }
        
    }
    public void ResetShooting()
    {
        //particle.Stop();
        DOTween.Kill("BulletScale");

        gameObject.SetActive (false);
        startShoot=false;
        transform.SetParent(defParent);
        transform.localScale = Vector3.one;
        transform.SetAsLastSibling();
        transform.localEulerAngles = startRotation;
        transform.localPosition = startPos;
    }



}
