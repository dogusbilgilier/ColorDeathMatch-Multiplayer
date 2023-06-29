
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using UnityEngine;

public class MyPlayer : MonoBehaviourPun
{

    public static GameObject LocalPlayerInstance;

    [Range(0, 100)] public float health;
    public PlayerColor playerColor;
    public GunColor gunColor;
    [SerializeField] SkinnedMeshRenderer meshRenderer;
    [SerializeField] Bullet[] myBulelts;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        photonView.RPC("Initialize", RpcTarget.All);
    }

    [PunRPC]
    void Initialize()
    {
        Material[] materials = meshRenderer.materials;
        switch (playerColor)
        {
            case PlayerColor.Red:
                materials[0].color = Color.red;
                break;
            case PlayerColor.Green:
                materials[0].color = Color.green;
                break;
            case PlayerColor.Blue:
                materials[0].color = Color.blue;
                break;
        }
        switch (gunColor)
        {
            case GunColor.Red:
                materials[1].color = Color.red;
                break;
            case GunColor.Green:
                materials[1].color = Color.green;
                break;
            case GunColor.Blue:
                materials[1].color = Color.blue;
                break;
            case GunColor.Purple:
                materials[1].color = Color.red / 2;
                break;
        }
        meshRenderer.materials = materials;

        foreach (var item in myBulelts)
        {
            item.SetColor(gunColor);
        }
    }

    public void GetHit(float damage)
    {
        photonView.RPC("GetHitRPC", RpcTarget.All, damage);
    }
    [PunRPC]
    void GetHitRPC(float damage)
    {
        health -= damage;
    }
}
