using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData[] weaponDatas;

    float time;

    private WeaponData weaponData;
    private float weaponPower;
    private float weaponSpeed;
    private float weaponIntervalTime;

    public void setWeapon(WeaponData data)
    {
        weaponData = data;
        weaponPower = data.basePower;
        weaponSpeed = data.baseSpeed;
        weaponIntervalTime = data.baseIntervalTime;
    }

    private void Start()
    {
        time = 0f;
    }

    public void Fire()
    {

        if (time >= weaponIntervalTime)
        {
            time = 0f;

            int prefabId = 0;
            for (int i = 0; i < GameManager.instance.pool.prefabs.Length; i++)
            {
                var obj = GameManager.instance.pool.prefabs[i];
                if (obj == weaponData.projectile)
                {
                    prefabId = i;
                    break;
                }
            }

            GameObject bullet = GameManager.instance.createBullet(prefabId);
            Component component = bullet.GetComponent<Component>();
            component.speed = weaponSpeed;
            component.power = weaponPower;
            component.directionVec = (bullet.transform.localPosition - GameManager.instance.player.transform.localPosition).normalized;
        }

    }

    private void Update()
    {

        time += Time.deltaTime;
        if (time > weaponIntervalTime) time = weaponIntervalTime;

    }

}
