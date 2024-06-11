using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WeaponSpawner : MonoBehaviour
{
    public ObjectPool<Weapon> pool;

    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();

        //pool = new ObjectPool<Weapon>(CreateWeapon, OnTakeWeaponFromPool, OnReturnWeaponToPool, OnDestroyWeapon, true, 1000, 3000);
    }

    //private Weapon CreateWeapon()
    //{
    //    Weapon weapon = Instantiate(playerController.pfWeapon, playerController.weaponPoint.position, playerController.transform.rotation);

    //    return weapon;
    //}

    private void OnTakeWeaponFromPool(Weapon weapon)
    {
        weapon.transform.position = playerController.transform.position;
        weapon.transform.right = playerController.transform.right;

        weapon.gameObject.SetActive(true);
    }

    private void OnReturnWeaponToPool(Weapon weapon)
    {
        weapon.gameObject.SetActive(false);
    }

    private void OnDestroyWeapon(Weapon weapon)
    {
        Destroy(weapon.gameObject);
    }
}
