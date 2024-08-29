using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public void OnClickExitButton()
    {
        Debug.Log("Close");
        Application.Quit();
    }
}
