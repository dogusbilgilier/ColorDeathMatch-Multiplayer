
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class MyPlayer : MonoBehaviourPun
{
    #region Properties

    public static GameObject LocalPlayerInstance;

    //Public------------
    public bool isInGame;
    [Range(0, 100)] public float health;
    public PlayerColor playerColor;
    public GunColor gunColor = GunColor.Red;

    //Private------------
    [SerializeField] SkinnedMeshRenderer meshRenderer;
    [SerializeField] Bullet[] myBulelts;

    //IU-----
    [SerializeField] Image healthBar, bulletColor;
    [SerializeField] Image healthBarOnHead, bulletColorOnHead;

    Material[] materials;

    #endregion



    private void Awake()
    {
        materials = meshRenderer.materials;
        if (photonView.IsMine)
        {
            LocalPlayerInstance = this.gameObject;
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
        materials[0].color = FindMyColor();
        materials[1].color = FindMyGunColor();

        meshRenderer.materials = materials;

        foreach (var item in myBulelts)
        {
            item.gameObject.SetActive(true);
            item.SetColor(gunColor);
            item.gameObject.SetActive(false);
        }

        if (!photonView.IsMine) return;

        healthBar = GameObject.FindGameObjectWithTag("MyColorIndicator").GetComponent<Image>();
        healthBar.fillAmount = health / 100f;
        healthBar.color = FindMyColor();

        bulletColor = GameObject.FindGameObjectWithTag("MyBulletIndicator").GetComponent<Image>();
        bulletColor.color = FindMyGunColor();
       
    }

    public void OnStart()
    {
        isInGame = true;
    }

    #region GetHit
    public void GetHit(float damage)
    {
        photonView.RPC("GetHitRPC", RpcTarget.AllBuffered, damage);

        if (photonView.IsMine)
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
        photonView.RPC("ChangeMyColorRPC", RpcTarget.AllBuffered, color);
    }
    public void ChangeMyGunColor(GunColor color)
    {
        photonView.RPC("ChangeMyGunColorRPC", RpcTarget.AllBuffered, color);
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
        switch (playerColor)
        {
            case PlayerColor.Red:return Color.red; 
            case PlayerColor.Green: return Color.green; 
            default: return Color.blue; 
        }

    }
    Color32 FindMyGunColor()
    {
        switch (gunColor)
        {
            case GunColor.Red: return Color.red;
            case GunColor.Green: return Color.green;
            case GunColor.Blue: return Color.blue;
            default: return Color.red / 2;
        }
        
    }
    public void GetColors()
    {
        materials[0].color = FindMyColor();
        materials[1].color = FindMyGunColor();
        foreach (var item in myBulelts)
        {
            item.gameObject.SetActive(true);
            item.SetColor(gunColor);
            item.gameObject.SetActive(false);
        }
    }


    #endregion

}
