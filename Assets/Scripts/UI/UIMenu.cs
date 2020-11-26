using System;
using UnityEngine;

public class UIMenu : MonoBehaviour
{
    public event Action Start;
    public event Action Exit;
    public void OnStartClick()
    {
        gameObject.SetActive(false);
        Start?.Invoke();
    }

    public void OnExitClick()
    {
        Exit?.Invoke();
    }
}
