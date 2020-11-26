using System;
using UnityEngine;
using UnityEngine.UI;

public class UIWin : MonoBehaviour
{
    public Text CongratulationsText;
    public event Action Next;
    public event Action Exit;

    public void Init(int level)
    {
        CongratulationsText.text = $"Level {level} complete!";
    }

    public void OnClickNext()
    {
        gameObject.SetActive(false);
        Next?.Invoke();        
    }

    public void OnClickExit()
    {
        gameObject.SetActive(false);
        Exit?.Invoke();        
    }
}
