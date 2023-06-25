
using UnityEngine;

public class Player : MonoBehaviour
{
    [Range(0, 100)] public int health;
    public PlayerColor playerColor;
    public GunColor gunColor;
    [SerializeField] SkinnedMeshRenderer meshRenderer;

    private void Start()
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
                materials[1].color= Color.green;
                break;
            case GunColor.Blue:
                materials[1].color = Color.blue;
                break;
            case GunColor.Purple:
                materials[1].color = Color.red/2;
                break;
        }
        meshRenderer.materials = materials;
    }

    void Initialize()
    {

    }
}
