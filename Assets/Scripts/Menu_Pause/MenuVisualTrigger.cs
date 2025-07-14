using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuVisualTrigger : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.Instance.IrParaMenuVisual();
        }
    }
}