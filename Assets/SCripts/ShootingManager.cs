
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] float cooldown,timer;
    [SerializeField] GameObject[] bulletHolders;
    [SerializeField] Transform playerCam;
    [SerializeField] Vector3 camRaycastPosition;
    [SerializeField] GameObject targetObject;
    [SerializeField] GameObject bulletTargetObject;
    [SerializeField] Transform muzzleLeft, muzzleRight; 
    bool canShoot;
    bool shootAttempt;
    [SerializeField] LayerMask bulletLayerMask;
    RaycastHit camHit,leftMuzzleHit,rightMuzzleHit;


    int shootCount;

    private void OnEnable()
    {
        EventManager.ShootAttempt += ShootAttempt;
        EventManager.StopShoot += StopShooting;
    }

    private void OnDisable()
    {
        EventManager.ShootAttempt -= ShootAttempt;
        EventManager.StopShoot -= StopShooting;
    }

    private void Start()
    {
        foreach (Bullet item in transform.GetComponentsInChildren<Bullet>())
            item.gameObject.SetActive(false);
    }

    private void Update()
    {
       
        if (timer >= cooldown && shootAttempt)
        {
            timer = 0;
            Shoot();
        }
        timer += Time.deltaTime;
    }
    private void FixedUpdate()
    {
        FindTargetForShooting();
    }

    void Shoot()
    {
        shootCount++;
        bulletHolders[shootCount%2].transform.GetChild(0).GetComponent<Bullet>().Shoot(bulletTargetObject.transform.position,bulletSpeed,GetComponent<Player>().gunColor);
        FindObjectOfType<AimMamager>().Recoil(shootCount % 2!=0);
    }

    void FindTargetForShooting()
    {
        Debug.DrawRay(playerCam.position, playerCam.forward * 500, Color.red);

        Physics.Raycast(playerCam.position, playerCam.forward, out camHit, 500, bulletLayerMask);

        if (camHit.collider)
        {
            camRaycastPosition = camHit.point;
        }
        else
        {
            camRaycastPosition = playerCam.forward * 500;
        }

        targetObject.transform.position = camRaycastPosition;

        Debug.DrawRay(muzzleLeft.position, targetObject.transform.position - muzzleLeft.position, Color.red);
        Debug.DrawRay(muzzleLeft.position, targetObject.transform.position - muzzleLeft.position, Color.red);

        if (shootCount % 2 == 0)
        {
            Physics.Raycast(muzzleLeft.position, targetObject.transform.position - muzzleLeft.position, out leftMuzzleHit, 500, bulletLayerMask);
            if (leftMuzzleHit.collider)
            {
                bulletTargetObject.transform.position = leftMuzzleHit.point;
            }
            else
            {
                bulletTargetObject.transform.position = targetObject.transform.position;
            }
        }

        else
        {
            Physics.Raycast(muzzleRight.position, targetObject.transform.position - muzzleRight.position, out rightMuzzleHit, 500, bulletLayerMask);
            if (rightMuzzleHit.collider)
            {
                bulletTargetObject.transform.position = rightMuzzleHit.point;
            }
            else
            {
                bulletTargetObject.transform.position = targetObject.transform.position;
            }

        }
    }

    private void ShootAttempt() => shootAttempt = true;
    private void StopShooting() => shootAttempt = false;
}
