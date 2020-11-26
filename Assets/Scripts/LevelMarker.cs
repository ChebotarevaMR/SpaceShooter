using UnityEngine;
using UnityEngine.UI;

public class LevelMarker : MonoBehaviour
{
    public Text Text;
    public int Number { get; private set; }

    public void SetNumber(int number)
    {
        Number = number;
        Text.text = number.ToString();
    }
}
