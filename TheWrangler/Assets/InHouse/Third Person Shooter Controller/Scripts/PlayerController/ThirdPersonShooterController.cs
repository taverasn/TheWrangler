using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Animations.Rigging;
using Unity.Cinemachine;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private Rig aimRig;
    [SerializeField] private CinemachineCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float aimSpeed = 10f;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform mouseWorldPositionTransform;
    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform pfBulletProjectileRayCast;
    [SerializeField] private Transform spawnBulletPosition;

    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;
    [SerializeField] private Animator animator;
    [SerializeField] private bool raycastHit;
    [SerializeField] private bool raycastHitBullet;
    [SerializeField] private GameObject currentWeapon;
    [SerializeField] private List<WeaponInfoSO> weapons;
    private List<WeaponRanged> weaponsRanged = new List<WeaponRanged>();
    private WeaponRanged currentWeaponRanged;
    private GameObject currentWeaponLight;
    private float shootTimer;

    private ThirdPersonController thirdPersonController;
    private Transform hitTransform;
    private float aimRigWeight;
    private bool isShooting;
    private bool isAiming;
    private bool isReloading;
    private bool isWeaponSwapping;
    private bool isShootTimerDone = true;
    private PlayerControls inputActions;
    [SerializeField] private List<GameObject> pistolIkList;
    [SerializeField] private List<GameObject> rifleIkList;
    [SerializeField] private List<GameObject> weaponIkList;
    private void Awake()
    {
        foreach (WeaponInfoSO weapon in weapons)
        {
            weaponsRanged.Add(new WeaponRanged(weapon));
        }
    }

    private void OnEnable()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();

        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.Player.Aim.performed += OnAimStarted;
            inputActions.Player.Aim.canceled += OnAimStopped;
            inputActions.Player.Shoot.performed += OnShootStarted;
            inputActions.Player.Shoot.canceled += OnShootStopped;
            inputActions.Player.Weapon1.performed += OnEquipWeapon1;
            inputActions.Player.Weapon2.performed += OnEquipWeapon2;
            inputActions.Player.Weapon3.performed += OnEquipWeapon3;
            inputActions.Player.Weapon4.performed += OnEquipWeapon4;
            inputActions.Player.Reload.performed += OnReload;
        }
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Aim.performed -= OnAimStarted;
        inputActions.Player.Aim.canceled -= OnAimStopped;
        inputActions.Player.Shoot.performed -= OnShootStarted;
        inputActions.Player.Shoot.canceled -= OnShootStopped;
        inputActions.Player.Weapon1.performed -= OnEquipWeapon1;
        inputActions.Player.Weapon2.performed -= OnEquipWeapon2;
        inputActions.Player.Weapon3.performed -= OnEquipWeapon3;
        inputActions.Player.Weapon4.performed -= OnEquipWeapon4;
        inputActions.Player.Reload.performed -= OnReload;
        inputActions.Disable();
    }

    private void Start()
    {
        currentWeaponRanged = weaponsRanged[0];
        GameEventsManager.Instance.weaponInputEvents.AmmoUpdate(currentWeaponRanged.currentAmmo, currentWeaponRanged.currentAmmoReserve);
        GameObject weapon = Instantiate(currentWeaponRanged.weaponInfo.pfWeapon, currentWeapon.transform.parent);
        Destroy(currentWeapon);
        spawnBulletPosition = weapon.transform.GetChild(0);
        currentWeaponLight = weapon.transform.GetChild(1).gameObject;
        currentWeapon = weapon;
    }

    private void Update()
    {
        if (!isWeaponSwapping)
        {
            currentWeaponLight.SetActive(false);
            HandleMouseWorldPosition();
            HandleAiming();
            HandleShooting();
            aimRig.weight = Mathf.Lerp(aimRig.weight, aimRigWeight, Time.deltaTime * rotationSpeed);
        }
    }

    private void SetWeapon(WeaponRanged weaponRanged)
    {
        if (!isShooting && !isReloading && !isWeaponSwapping && currentWeaponRanged != weaponRanged)
        {
            isWeaponSwapping = true;
            animator.SetTrigger("isWeaponSwapping");
            currentWeaponLight.SetActive(false);
            currentWeaponRanged = weaponRanged;
            StartCoroutine(WeaponSwap());
        }
    }

    private IEnumerator WeaponSwap()
    {
        yield return new WaitForSeconds(1.2f);
        if (currentWeaponRanged.weaponInfo.weaponType == WeaponType.Rifle)
        {
            for (int i = 0; i < weaponIkList.Count; i++)
            {
                weaponIkList[i].transform.position = rifleIkList[i].transform.position;
                weaponIkList[i].transform.rotation = rifleIkList[i].transform.rotation;
            }

            animator.SetBool("isRifle", true);
        }
        else if (currentWeaponRanged.weaponInfo.weaponType == WeaponType.Pistol)
        {
            for (int i = 0; i < weaponIkList.Count; i++)
            {
                weaponIkList[i].transform.position = pistolIkList[i].transform.position;
                weaponIkList[i].transform.rotation = pistolIkList[i].transform.rotation;
            }

            animator.SetBool("isRifle", false);
        }
        GameEventsManager.Instance.weaponInputEvents.AmmoUpdate(currentWeaponRanged.currentAmmo, currentWeaponRanged.currentAmmoReserve);
        GameObject weapon = Instantiate(currentWeaponRanged.weaponInfo.pfWeapon, currentWeapon.transform.parent);
        Destroy(currentWeapon);
        spawnBulletPosition = weapon.transform.GetChild(0);
        currentWeaponLight = weapon.transform.GetChild(1).gameObject;
        currentWeapon = weapon;

        yield return new WaitForSeconds(.5f);
        isWeaponSwapping = false;
    }

    private void OnShootStarted(InputAction.CallbackContext context)
    {
        isShooting = true;
    }

    private void OnShootStopped(InputAction.CallbackContext context)
    {
        animator.SetBool("isShooting", false);
        isShooting = false;
    }

    private void OnEquipWeapon1(InputAction.CallbackContext context)
    {
        SetWeapon(weaponsRanged[0]);
    }

    private void OnEquipWeapon2(InputAction.CallbackContext context)
    {
        SetWeapon(weaponsRanged[1]);
    }

    private void OnEquipWeapon3(InputAction.CallbackContext context)
    {
        SetWeapon(weaponsRanged[2]);
    }

    private void OnEquipWeapon4(InputAction.CallbackContext context)
    {
        SetWeapon(weaponsRanged[3]);
    }

    private void HandleShooting()
    {
        if (isReloading)
        {
            Reload();
        }
        else if (isShooting)
        {
            if (isShootTimerDone && currentWeaponRanged.CanShoot())
            {
                StartCoroutine(ShootTimer());
            }
            else if (currentWeaponRanged.CanReload() && !currentWeaponRanged.CanShoot())
            {
                isReloading = true;
                animator.SetBool("isShooting", false);
                animator.SetBool("isReloading", true);
            }
        }
        else
        {
            animator.SetBool("isShooting", false);
        }
    }

    private IEnumerator ShootTimer()
    {
        currentWeaponLight.SetActive(true);
        isShootTimerDone = false;
        if (isShooting)
        {
            Shoot();
        }
        yield return new WaitForSeconds(currentWeaponRanged.weaponInfo.fireRate);
        isShootTimerDone = true;
    }

    private void Shoot()
    {
        currentWeaponRanged.Shoot();
        GameEventsManager.Instance.weaponInputEvents.AmmoUpdate(currentWeaponRanged.currentAmmo, currentWeaponRanged.currentAmmoReserve);
        if (raycastHitBullet)
        {
            Vector3 aimDir = (mouseWorldPositionTransform.position - spawnBulletPosition.position).normalized;
            if (hitTransform != null)
            {
                // Hit Something
                Character_Base target = hitTransform.GetComponent<Character_Base>();
                if (target != null)
                {
                    // Hit Target
                    if (target.targetType != AITargetType.NPC_Friendly)
                    {
                        target.health.HealthChanged(-currentWeaponRanged.damage, AITargetType.Player);
                        Instantiate(pfBulletProjectileRayCast, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up)).GetComponent<BulletProjectileRaycast>().SetUp(mouseWorldPositionTransform.position, true);
                    }
                }
                else
                {
                    // Hit Something Else
                    Instantiate(pfBulletProjectileRayCast, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up)).GetComponent<BulletProjectileRaycast>().SetUp(mouseWorldPositionTransform.position, false);
                }
            }
        }
        else if (raycastHit)
        {
            if (hitTransform != null)
            {
                // Hit Something
                Character_Base target = hitTransform.GetComponent<Character_Base>();
                if (target != null)
                {
                    // Hit Target
                    if(target.targetType != AITargetType.NPC_Friendly)
                    {
                        target.health.HealthChanged(-currentWeaponRanged.damage, AITargetType.Player);
                        Instantiate(vfxHitGreen, mouseWorldPositionTransform.position, Quaternion.identity);
                    }
                }
                else
                {
                    // Hit Something Else
                    Instantiate(vfxHitRed, mouseWorldPositionTransform.position, Quaternion.identity);
                }
            }
        }
        else
        {
            Vector3 aimDir = (mouseWorldPositionTransform.position - spawnBulletPosition.position).normalized;
            Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        }
        animator.SetBool("isShooting", true);
    }

    private void Reload()
    {
        currentWeaponLight.SetActive(false);
        if (1.3f > shootTimer)
        {
            shootTimer += Time.deltaTime;
        }
        else
        {
            shootTimer = 0;
            isReloading = false;
            animator.SetBool("isReloading", false);
            currentWeaponRanged.Reload();
            GameEventsManager.Instance.weaponInputEvents.AmmoUpdate(currentWeaponRanged.currentAmmo, currentWeaponRanged.currentAmmoReserve);
        }
    }

    private void OnReload(InputAction.CallbackContext context)
    {
        if (currentWeaponRanged.CanReload() && !isShooting)
        {
            isReloading = true;
            animator.SetBool("isReloading", true);
        }
    }

    private void OnAimStarted(InputAction.CallbackContext context)
    {
        aimVirtualCamera.gameObject.SetActive(true);
        thirdPersonController.SetSensitivity(aimSensitivity);
        thirdPersonController.SetRotateOnMove(false);
        isAiming = true;
    }

    private void OnAimStopped(InputAction.CallbackContext context)
    {
        aimVirtualCamera.gameObject.SetActive(false);
        thirdPersonController.SetSensitivity(normalSensitivity);
        thirdPersonController.SetRotateOnMove(true);
        isAiming = false;
    }

    private void HandleMouseWorldPosition()
    {
        mouseWorldPositionTransform.position = Vector3.Lerp(mouseWorldPositionTransform.position, GetMouseWorldPosition(), Time.deltaTime * rotationSpeed);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            hitTransform = raycastHit.transform;
        }
        return raycastHit.point;
    }

    private void HandleAiming()
    {
        if (!isReloading && !isWeaponSwapping)
        {
            if (isAiming || (currentWeaponRanged.CanShoot() && isShooting))
            {
                animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * aimSpeed));

                if (isAiming)
                {
                    animator.SetBool("isAiming", true);
                }
                else
                {
                    animator.SetBool("isAiming", false);
                }
                Vector3 worldAimTarget = mouseWorldPositionTransform.position;
                worldAimTarget.y = transform.position.y;
                Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotationSpeed);
                aimRigWeight = 1f;
            }
            else
            {
                animator.SetBool("isAiming", false);
                animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0, Time.deltaTime * aimSpeed));
                aimRigWeight = 0f;
            }
        }
    }
}
