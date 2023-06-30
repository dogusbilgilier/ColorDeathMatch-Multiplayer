
using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayer : MonoBehaviourPun
{
    public static GameObject LocalPlayerInstance;

    [Range(0, 100)] public float health;
    public PlayerColor playerColor;
    public GunColor gunColor;

    [SerializeField] SkinnedMeshRenderer meshRenderer;
    [SerializeField] Bullet[] myBulelts;

    [SerializeField] Image healthBar,bulletColor;
    Material[] materials;

    public bool isInGame;
    [SerializeField] Image healthBarOnHead, bulletColorOnHead;


    private void Awake()
    {
        if (photonView.IsMine)
        {
            LocalPlayerInstance = this.gameObject;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private IEnumerator Start()
    {

        if (photonView.IsMine)
        {
            yield return new WaitForSeconds(0.1f);
            photonView.RPC("Initialize", RpcTarget.All);

            healthBar = GameObject.FindGameObjectWithTag("MyColorIndicator").GetComponent<Image>();
            healthBar.fillAmount = health / 100f;
            healthBar.color = FindMyColor();

            bulletColor = GameObject.FindGameObjectWithTag("MyBulletIndicator").GetComponent<Image>();
            bulletColor.color = FindMyGunColor();
        }
       
    }

    [PunRPC]
    void Initialize()
    {
        materials = meshRenderer.materials;

        materials[0].color = FindMyColor();
        materials[1].color = FindMyGunColor();

        meshRenderer.materials = materials;

        foreach (var item in myBulelts)
        {
            item.gameObject.SetActive(true);
            item.SetColor(gunColor);
            item.gameObject.SetActive(false);
        }
    }

    public void OnStart()
    {

    }

    #region GetHit
    public void GetHit(float damage)
    {
        photonView.RPC("GetHitRPC", RpcTarget.All, damage);
        if(photonView.IsMine)
        healthBar.fillAmount = health / 100f;
    }
    [PunRPC]
    void GetHitRPC(float damage)
    {
        health -= damage;
       
    }
    #endregion

    #region ChangeColors
    public void ChangeMyColor(PlayerColor color)
    {
        photonView.RPC("ChangeMyColorRPC", RpcTarget.All, color);
    }
    public void ChangeMyGunColor(GunColor color)
    {
        photonView.RPC("ChangeMyGunColorRPC", RpcTarget.All, color);
    }
    [PunRPC]
    void ChangeMyColorRPC(PlayerColor color)
    {
       
        playerColor = color;
        materials[0].color = FindMyColor();
        if (photonView.IsMine == false) return;
        healthBar.color = FindMyColor();
    }
    [PunRPC]
    void ChangeMyGunColorRPC(GunColor color)
    {
       
        gunColor = color;
        materials[1].color = FindMyGunColor();

        if (photonView.IsMine == false) return;

        bulletColor.color = FindMyGunColor();
        foreach (var item in myBulelts)
        {
            item.gameObject.SetActive(true);
            item.SetColor(gunColor);
            item.gameObject.SetActive(false);
        }
    }
    #endregion

    #region ColorHelper
    public Color32 FindMyColor()
    {
        Color32 myColor = Color.blue;

        switch (playerColor)
        {
            case PlayerColor.Red: myColor = Color.red; break;
            case PlayerColor.Green: myColor = Color.green; break;
            case PlayerColor.Blue: myColor = Color.blue; break;
        }
        return myColor;
    }
    Color32 FindMyGunColor()
    {
        Color32 myGunColor = Color.blue;

        switch (gunColor)
        {
            case GunColor.Red: myGunColor = Color.red; break;
            case GunColor.Green: myGunColor = Color.green; break;
            case GunColor.Blue: myGunColor = Color.blue; break;
            case GunColor.Purple: myGunColor = Color.red / 2; break;
        }
        return myGunColor;
    }
    #endregion
}
