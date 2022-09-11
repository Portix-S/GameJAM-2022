using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StarterAssets;
public class GunScript : MonoBehaviour
{
    //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-//
    //PROJETIL, BALA, BULLET
    [SerializeField] public GameObject Bullet;
    public LayerMask PlayerLayerMask;
    public Vector3 targetPoint;

    //FORCA DA BALA, BULLET FORCE
    public float shootForce;

    //CONSTITUICAO DA ARMA, GUN STATS
    [Header("Gun Stats")]
    public float shootingRate; //tempo entre as disparadas
    public float fireRate; //tempo entre os tiros
    public float spread; //dispersao do tiro
    public float reloadTime; //tempo de recarregat
    public int magazineSize; //tamanho do pente
    public int totalShotgunAmmo;
    public int totalPistolAmmo;
    public int bulletPerTap; //quantas balas saem por clique
    public bool allowHold; //auto / semiauto
    int bulletsLeft, bulletsShot; //quantas balas tem
    public int damage = 10; // Muni��o atual
    public int smgDamage = 10;
    public int shotgunDamage = 8;
    public int pistolDamage = 20;
    public bool isOnPistol = false;
    public bool isOnShotgun = true;
    public int shotgunBullets;
    public int pistolBullets;
    
    //Bools CHECKS
    public bool shooting, readyToShoot, reloading;

    //REFERENCES
    [Header("References")]
    public Camera fpsCam;
    public Transform attackPoint;
    public FirstPersonController fpsControl;
    private GunAnimations gunAnimation;

    //GRAFICO
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammoDisplay;  // talvez usar depois

    //BUG FIXING
    public bool allowInvoke = true;
    public Vector3 mousePos;
    //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-//

    // https://answers.unity.com/questions/129715/collision-detection-if-raycast-source-is-inside-a.html

    private void Awake()
    {
        //TER CERTEZA SE O PENTE TA FULL
        shotgunBullets = 12;
        pistolBullets = 15;
        bulletsLeft = shotgunBullets;
        readyToShoot = true;
        totalShotgunAmmo = shotgunBullets * 3;
        totalPistolAmmo = pistolBullets * 3;
        gunAnimation = gameObject.GetComponent<GunAnimations>();
        ShotGun();
    }

    private void Update()
    {
        if (!fpsControl.isOnHUD)
        {
            MyInput();
            ChooseWeapon();
        }
        //set ammo display
        if (ammoDisplay != null && isOnPistol)
            ammoDisplay.SetText(pistolBullets/bulletPerTap + "/" + totalPistolAmmo/bulletPerTap);
        else if(ammoDisplay != null && isOnShotgun)
            ammoDisplay.SetText(shotgunBullets / bulletPerTap + "/" + totalShotgunAmmo / bulletPerTap);


    }
    //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-//
    private void MyInput()
    {
        //CHECAR SE PODE SENTA A PUA
        if (allowHold)
            shooting = Input.GetKey(KeyCode.Mouse0);
        else
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
            //shooting = Input.GetButtonDown("Fire1");

        //RECARREGAR MANUAL
        if (Input.GetKeyDown(KeyCode.R) && pistolBullets < magazineSize && !reloading && totalPistolAmmo > 0 && isOnPistol)
            ReloadPistol();
        else if (Input.GetKeyDown(KeyCode.R) && shotgunBullets < magazineSize && !reloading && totalShotgunAmmo > 0 && isOnShotgun)
            Reload();
        //RECARGA AUTOMATICA
        if (readyToShoot && shooting && !reloading && shotgunBullets <= 0 && totalShotgunAmmo > 0 && isOnShotgun)
            Reload();
        else if (readyToShoot && shooting && !reloading && pistolBullets <= 0 && totalPistolAmmo > 0 && isOnPistol)
            ReloadPistol();
        //ATIRANDO, SHOOTING
        if (readyToShoot && shooting && !reloading && ((pistolBullets > 0 && isOnPistol) || (shotgunBullets > 0 && isOnShotgun)) )
        {
            //NAO ATIROU NENHUMA, ainda
            bulletsShot = 0;
            Shoot();
        }
        //MOUSE POSITION
        //Vector3  mousePos = Input.mousePosition;
    }
    //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-//
    private void Shoot()
    {
        //ja atirou
        readyToShoot = false;

        //ENCONTRAR A POSICAO ACERTADA DO CURSOR USANDO RAYCAST
        //origem do ray eh o meio do player
        //aponta para a posicao do cursor
        float rayLength = 10000f;//distancia infinita onde o z aponta
        Ray ray =  Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //CHECAR SE O RAY MIRA EM ALGO
        //Vector3 targetPoint;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, rayLength, PlayerLayerMask))
        {
            targetPoint = hit.point;
        }
        else 
        {
            ray.direction = -ray.direction;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, rayLength, PlayerLayerMask))
            {
                targetPoint = hit.point;
            }
        }
            //targetPoint = ray.GetPoint(100);//just away from the player
        //CALCULO DA DIRECAO DA NAVE ATE O ALVO
        Vector3 directionNOSpread = targetPoint - attackPoint.position;

        //CALCULO DO SPREAD
        float x = Random.Range(-spread, spread); // Escalar com distancia?
        float y = Random.Range(-spread, spread);
        float z = Random.Range(-spread, spread);
        //direcao com spread
        Vector3 directionSpread = directionNOSpread + new Vector3(x,y,z);

        //INSTANCIAR A BALA, INSTANCIATE BULLET
        GameObject currentBullet = Instantiate(Bullet, attackPoint.position, Quaternion.identity);
        //rodar a bala na direcao correta
        currentBullet.transform.forward = directionSpread.normalized;

        //ADD FORCES TO BULLET
        currentBullet.GetComponent<Rigidbody>().AddForce(directionSpread.normalized * shootForce, ForceMode.Impulse);

        //INSTANCIAR muzzleFlash
        StartCoroutine(MuzzleFlash());

        //DESCONTAR DAS BALAS E MARCAR Q ATIROU
        //bulletsLeft--;
        if (isOnPistol)
            pistolBullets--;
        else if (isOnShotgun)
            shotgunBullets--;
        bulletsShot++;

        //RESET DO SHOOT
        if (allowInvoke)
        {
            Invoke("ResetShot", shootingRate);
            allowInvoke = false;

        }

        //SE TEM MAIS DE UMA BALA POR CLICK
        if (bulletsShot < bulletPerTap && bulletsLeft > 0)
            Invoke("Shoot", shootingRate);
    }

    private void ResetShot()
    {
        //allow shooting and invoking again
        readyToShoot = true;
        allowInvoke = true;
    }
    //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-//

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadShotgunFinished", reloadTime);
    }

    private void ReloadPistol()
    {
        reloading = true;
        Invoke("ReloadPistolFinished", reloadTime);
    }
    private void ReloadShotgunFinished()
    {
        if(totalShotgunAmmo > magazineSize || totalShotgunAmmo + shotgunBullets > magazineSize)
        {
            Debug.Log("Reloading Shotgun");
            totalShotgunAmmo -= (magazineSize - shotgunBullets);
            //totalShotgunAmmo -= 10;
            shotgunBullets = magazineSize;
        }
        else
        {
            shotgunBullets += totalShotgunAmmo;
            totalShotgunAmmo = 0;
        }
        reloading = false;
    }

    private void ReloadPistolFinished()
    {
        if (totalPistolAmmo > magazineSize || totalPistolAmmo + pistolBullets > magazineSize)
        {
            Debug.Log("Reloading pistol");
            totalPistolAmmo -= (magazineSize - pistolBullets);
            pistolBullets = magazineSize;
        }
        else
        {
            pistolBullets += totalPistolAmmo;
            totalPistolAmmo = 0;
        }
        reloading = false;
    }

    IEnumerator MuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.SetActive(false);
    }

    private void ChooseWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Pistol();
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ShotGun();
    }

    private void SMG()
    {
        allowHold = true;
        shootForce = 20f;
        shootingRate = 0.1f;
        fireRate = 10f;
        spread = 0.2f;
        reloadTime = 2f;
        magazineSize = 100;
        bulletPerTap = 1;
        damage = smgDamage;
        fpsControl.MoveSpeed = 4f;
        fpsControl.SprintSpeed = 8f;
    }

    private void Pistol()
    {
        allowHold = false;
        shootForce = 18f;
        shootingRate = 0.3f;
        fireRate = 1f;
        spread = 0.1f;
        reloadTime = 1f;
        magazineSize = 10;
        bulletPerTap = 1;
        damage = pistolDamage;
        fpsControl.MoveSpeed = 6f;
        fpsControl.SprintSpeed = 10f;
        isOnPistol = true;
        isOnShotgun = false;
        gunAnimation.gunIdle.SetActive(false);
        gunAnimation.gunOnADS.SetActive(false);
        gunAnimation.pistolIdle.SetActive(true);
        attackPoint = gunAnimation.pistolFirePoint;
        muzzleFlash = gunAnimation.pistolMuzzleFlash;
    }

    private void ShotGun() // Arrumar para demonstrar quantidade corretamente
    {
        allowHold = false;
        shootForce = 30f;
        shootingRate = 0.01f;
        fireRate = 3f; // Verificar
        spread = 0.8f;
        reloadTime = 2f;
        magazineSize = 12;
        bulletPerTap = 4;
        damage = shotgunDamage;
        fpsControl.MoveSpeed = 4f;
        fpsControl.SprintSpeed = 8f;
        isOnPistol = false;
        isOnShotgun = true;
        gunAnimation.gunIdle.SetActive(true);
        gunAnimation.gunOnADS.SetActive(false);
        gunAnimation.pistolIdle.SetActive(false);
        attackPoint = gunAnimation.standardFirePoint;
        muzzleFlash = gunAnimation.shotgunMuzzleFlash;
    }

}
