using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medicine : MonoBehaviour
{

    [SerializeField] VariableJoystick variableJoystick;
    public Vector2 directionVec;

    private Weapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        weapon = GetComponent<Weapon>();

        weapon.setWeapon(weapon.weaponDatas[0]);
    }

    // Update is called once per frame
    void Update()
    {
        directionVec = variableJoystick.Direction;
        if(directionVec != Vector2.zero)
        {
            transform.localRotation = Quaternion.FromToRotation(Vector3.up, directionVec);

            weapon.Fire();
        }
    }
}
