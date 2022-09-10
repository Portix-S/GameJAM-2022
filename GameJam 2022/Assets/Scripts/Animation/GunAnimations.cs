using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class GunAnimations : MonoBehaviour
{
    public GameObject gunOnADS;
    public GameObject gunIdle;
    public GameObject mira;

    public Transform adsFirePoint;
    public Transform standardFirePoint;

    float animationSpeed = 1f;
    float elapsedTime = 0f;
    float amplitude = 0.1f;

    public float x;
    public float y;

    public FirstPersonController fpsController;

    public Vector3 cameraToFirePoint;
    public Vector3 cameraToAdsFirePoint;
    public GameObject cube;
    public GameObject standardFirePointRect;
    public GameObject adsFirePointRect;

    bool stoppedMoving = false;

    GunScript gunScript;
    // Update is called once per frame
    void Update()
    {
        if (!fpsController.isOnHUD)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))    // Fazer animação para quando mirar -> Mudar o localScale e positon até chegar no outro, e -1 para voltar
            {
                gunScript = gameObject.GetComponent<GunScript>();
                gunIdle.SetActive(!gunIdle.activeSelf);
                mira.SetActive(gunIdle.activeSelf);
                gunOnADS.SetActive(!gunOnADS.activeSelf);
                if (gunOnADS.activeSelf)
                    gunScript.attackPoint = adsFirePoint.transform;
                else
                    gunScript.attackPoint = standardFirePoint.transform;
            }

            x = amplitude * Mathf.Sin(animationSpeed * elapsedTime);
            y = amplitude / 1.5f * Mathf.Sin(animationSpeed * elapsedTime);
            RectTransform gunRect = gunIdle.GetComponent<RectTransform>();
            RectTransform gunAdsRect = gunOnADS.GetComponent<RectTransform>();
            gunRect.localPosition += new Vector3(x, y, 0);
            gunAdsRect.localPosition += new Vector3(x / 10, y, 0);
            elapsedTime += Time.deltaTime;

            if (fpsController.isSprinting)
            {
                animationSpeed = 5f;
                amplitude = 1.5f;
                stoppedMoving = true;
            }
            else if (fpsController.isWalking)
            {
                animationSpeed = 3f;
                amplitude = 0.7f;
                stoppedMoving = true;
            }
            else
            {
                if (stoppedMoving) // Reset sprites to initial position (so player can shoot accurately)
                {
                    gunRect.localPosition = new Vector3(530, -448, 0);   // Maybe Smooth it out
                    gunAdsRect.localPosition = new Vector3(-19.45f, -817, 0);
                    stoppedMoving = false;
                }
                animationSpeed = 1f;
                amplitude = 0.01f;
            }

            cameraToFirePoint = Camera.main.ScreenToWorldPoint(standardFirePointRect.transform.position);
            cameraToAdsFirePoint = Camera.main.ScreenToWorldPoint(adsFirePointRect.transform.position);
            standardFirePoint.transform.position = cameraToFirePoint;
            adsFirePoint.transform.position = cameraToAdsFirePoint;

        }
    }
}
