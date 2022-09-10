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


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1))    // Fazer animação para quando mirar -> Mudar o localScale e positon até chegar no outro, e -1 para voltar
        {
            GunScript gunScript = gameObject.GetComponent<GunScript>();
            gunIdle.SetActive(!gunIdle.activeSelf);
            mira.SetActive(gunIdle.activeSelf);
            gunOnADS.SetActive(!gunOnADS.activeSelf);
            if(gunOnADS.activeSelf)
                gunScript.attackPoint = adsFirePoint.transform;
            else
                gunScript.attackPoint = standardFirePoint.transform;
        }

        x = amplitude * Mathf.Sin(animationSpeed * elapsedTime);
        y = amplitude/1.5f * Mathf.Sin(animationSpeed * elapsedTime);
        RectTransform gunRect = gunIdle.GetComponent<RectTransform>();
        gunRect.localPosition += new Vector3(x,y,0);
        elapsedTime += Time.deltaTime;

        if(fpsController.isSprinting)
        {
            animationSpeed = 5f;
            amplitude = 1.5f;
        }
        else if(fpsController.isWalking)
        {
            animationSpeed = 3f;
            amplitude = 0.7f;
        }
        else
        {
            animationSpeed = 1f;
            amplitude = 0.1f;
        }
    

    }
}
