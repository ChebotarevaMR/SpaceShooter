using System;
using UnityEngine;

public class UIMenu : MonoBehaviour
{
    public event Action StartClick;
    public event Action ExitClick;
    public void OnStartClick()
    {
        gameObject.SetActive(false);
        StartClick?.Invoke();
    }

    public void OnExitClick()
    {
        ExitClick?.Invoke();
    }
}
