using DG.Tweening;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviourPun,IPunObservable
{
    public bool startShoot;
    //[SerializeField] ParticleSystem particle;
    [HideInInspector] public MyPlayer player;
     public GunColor gunColor;
    public Transform defParent;
    [SerializeField] MeshRenderer meshRenderer;
    public Vector3 startPos,startRotation;
    float speed;
    float timer;

    private void Start()
    {
        player = GetComponent<MyPlayer>();
    }
    public void SetColor(GunColor gunColor)
    {
        photonView.RPC("SetColorRPC", RpcTarget.All, gunColor);
    }

    [PunRPC]
    void SetColorRPC(GunColor gunColor)
    {
        Debug.Log(name);
        this.gunColor = gunColor;
        switch (gunColor)
        {
            case GunColor.Red:
                meshRenderer.material.color = Color.red;
                break;
            case GunColor.Green:
                meshRenderer.material.color = Color.green;
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
        DOTween.Kill("BulletScale");

        gameObject.SetActive(false);
        GetComponentInChildren<Collider>().enabled = true;
        startShoot = false;
        transform.SetParent(defParent);
        transform.localEulerAngles = startRotation;
        transform.localPosition = startPos;
        transform.localScale = Vector3.one;
        transform.SetAsLastSibling();
       // photonView.RPC("ResetShootingRPC", RpcTarget.All);
    }
 
    void ResetShootingRPC()
    {
        DOTween.Kill("BulletScale");

        gameObject.SetActive(false);
        GetComponentInChildren<Collider>().enabled = true;
        startShoot = false;
        transform.SetParent(defParent);
        transform.localEulerAngles = startRotation;
        transform.localPosition = startPos;
        transform.localScale = Vector3.one;
        transform.SetAsLastSibling();
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
