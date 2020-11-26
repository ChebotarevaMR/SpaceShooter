using System;
using UnityEngine;

public class UIGameOver : MonoBehaviour
{
    public event Action Restart;
    public event Action Exit;
    public void OnClickRestart()
    {
        gameObject.SetActive(false);
        Restart?.Invoke();
    }

    public void OnClickExit()
    {
        gameObject.SetActive(false);
        Exit?.Invoke();
    }
}
