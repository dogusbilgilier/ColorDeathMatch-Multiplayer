using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetector : MonoBehaviourPun
{
    [SerializeField] MyPlayer player;
    DamageData damageDatas;
    Bullet bullet;
    Collider collider;
    private void Start()
    {
        damageDatas = ScriptableManager.Instance.damageData;
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Bullet":
                bullet = other.GetComponentInParent<Bullet>();
                other.enabled = false;
                if (bullet.startShoot)
                    Hit();
                    //photonView.RPC("Hit", RpcTarget.All);
                break;
        }  
    }

 
    void Hit()
    {
        Debug.Log("Hit");
       
        float damage = 0;

        if (bullet.gunColor.ToString().Equals(player.playerColor) || bullet.gunColor == GunColor.Purple) //ColorMatchedHit
        {
            damage = damageDatas.colorMatchMultiplier * damageDatas.regularHit;
            Debug.Log("ColorMatchHit");
            if (name.Contains("Head"))
            {
                damage = damageDatas.colorMatchedHeadShot;

            }
        }
        else
        {
            damage = damageDatas.regularHit;
            if (name.Contains("Head"))
            {
                damage += damageDatas.regularHit * damageDatas.colorMatchMultiplier;

            }
        }
        bullet.ResetShooting();
        player.GetHit(damage);
    }
}

