using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class GunAnimations : MonoBehaviour
{
    public GameObject gunOnADS;
    public GameObject gunIdle;
    public GameObject pistolIdle;
    public GameObject mira;

    public Transform adsFirePoint;
    public Transform standardFirePoint;
    public Transform pistolFirePoint;

    float animationSpeed = 1f;
    float elapsedTime = 0f;
    float amplitude = 0.1f;

    public float x;
    public float y;

    public FirstPersonController fpsController;

    public Vector3 cameraToFirePoint;
    public Vector3 cameraToAdsFirePoint;
    public Vector3 cameraToPistolFirePoint;
    public GameObject standardFirePointRect;
    public GameObject adsFirePointRect;
    public GameObject pistolFirePointRect;

    public GameObject pistolMuzzleFlash;
    public GameObject shotgunMuzzleFlash;
    public GameObject shotgunADSMuzzleFlash;

    bool stoppedMoving = false;

    GunScript gunScript;
    // Update is called once per frame

    private void Start()
    {
        gunScript = GetComponent<GunScript>();
    }
    void Update()
    {
        if (!fpsController.isOnHUD)
        {                                            // && isOnShotgun
            if (Input.GetKeyDown(KeyCode.Mouse1) && gunScript.isOnShotgun)    // Fazer animação para quando mirar -> Mudar o localScale e positon até chegar no outro, e -1 para voltar
            {
                gunIdle.SetActive(!gunIdle.activeSelf);
                mira.SetActive(gunIdle.activeSelf);
                gunOnADS.SetActive(!gunOnADS.activeSelf);
                if (gunOnADS.activeSelf)
                {
                    gunScript.attackPoint = adsFirePoint.transform;
                    gunScript.muzzleFlash = shotgunADSMuzzleFlash;
                }
                else
                {
                    gunScript.attackPoint = standardFirePoint.transform;
                    gunScript.muzzleFlash = shotgunMuzzleFlash;
                }
            }

            x = amplitude * Mathf.Sin(animationSpeed * elapsedTime);
            y = amplitude / 1.5f * Mathf.Sin(animationSpeed * elapsedTime);
            RectTransform gunRect = gunIdle.GetComponent<RectTransform>();
            RectTransform gunAdsRect = gunOnADS.GetComponent<RectTransform>();
            RectTransform pistolRect = pistolIdle.GetComponent<RectTransform>();
            gunRect.localPosition += new Vector3(x, y, 0);
            gunAdsRect.localPosition += new Vector3(x / 10, y, 0);
            pistolRect.localPosition += new Vector3(x / 10, y, 0);
            elapsedTime += Time.deltaTime;

            if (fpsController.isSprinting)
            {
                animationSpeed = fpsController.SprintSpeed/1.5f;
                amplitude = fpsController.SprintSpeed / 6f;
                stoppedMoving = true;
            }
            else if (fpsController.isWalking)
            {
                animationSpeed = fpsController.MoveSpeed/1.5f;
                amplitude = fpsController.MoveSpeed / 4.5f;
                stoppedMoving = true;
            }
            else
            {
                if (stoppedMoving) // Reset sprites to initial position (so player can shoot accurately)
                {
                    gunRect.localPosition = new Vector3(0, 0, 0);   // Maybe Smooth it out
                    gunAdsRect.localPosition = new Vector3(0, 0, 0);
                    pistolRect.localPosition = new Vector3(0, 0, 0);
                    stoppedMoving = false;
                }
                animationSpeed = 1f;
                amplitude = 0.01f;
            }

            cameraToFirePoint = Camera.main.ScreenToWorldPoint(standardFirePointRect.transform.position);
            cameraToAdsFirePoint = Camera.main.ScreenToWorldPoint(adsFirePointRect.transform.position);
            cameraToPistolFirePoint = Camera.main.ScreenToWorldPoint(pistolFirePointRect.transform.position);

            standardFirePoint.transform.position = cameraToFirePoint;
            adsFirePoint.transform.position = cameraToAdsFirePoint;
            pistolFirePoint.transform.position = cameraToPistolFirePoint;
        }
    }
}
