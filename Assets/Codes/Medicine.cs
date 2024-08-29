using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medicine : MonoBehaviour
{

    [SerializeField] VariableJoystick variableJoystick;
    public Vector2 inputVec;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inputVec = variableJoystick.Direction;
        if(inputVec != Vector2.zero)
            transform.localRotation = Quaternion.FromToRotation(Vector3.up, inputVec);
    }
}
