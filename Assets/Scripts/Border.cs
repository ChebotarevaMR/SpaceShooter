using UnityEngine;

public class Border : MonoBehaviour
{
    public float YTop { get; private set; }
    public float YBottom { get; private set; }
    public float XLeft { get; private set; }
    public float XRight { get; private set; }

    private void Start()
    {
        var bounds = GetComponent<BoxCollider2D>().bounds;

        var yHalfExtents = bounds.extents.y;
        var yCenter = bounds.center.y;
        YTop = transform.position.y + (yCenter + yHalfExtents);
        YBottom = transform.position.y + (yCenter - yHalfExtents);

        var xHalfExtents = bounds.extents.x;
        var xCenter = bounds.center.x;
        XRight = transform.position.x + (xCenter + xHalfExtents);
        XLeft = transform.position.x + (xCenter - xHalfExtents);

    }
}
