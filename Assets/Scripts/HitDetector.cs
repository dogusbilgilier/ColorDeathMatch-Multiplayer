using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetector : MonoBehaviourPun
{
    [SerializeField] MyPlayer player;

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Bullet":
                photonView.RPC("Hit", RpcTarget.All, other);
                break;
        }  
    }

    [PunRPC]
    void Hit(Collider other)
    {
        other.enabled = false;
        Bullet bullet = other.GetComponent<Bullet>();

        if (bullet.gunColor.ToString().Equals(player.playerColor) || bullet.gunColor == GunColor.Purple)
        {

            if (name.Contains("Head"))
            {
                //MegaHit

            }
            else
            {
                //PerfectHit

            }

        }
        else
        {
            if (name.Contains("Head"))
            {
                //HeadShot

            }
        }
        bullet.ResetShooting();
    }
}

