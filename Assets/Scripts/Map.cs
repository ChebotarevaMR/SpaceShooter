using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{    
    public LevelMarker PassLevel;
    public LevelMarker OpenLevel;
    public LevelMarker CloseLevel;
    public Image ContentImage;

    private List<LevelMarker> _levels = new List<LevelMarker>();

    public void GenerateMap(List<ILevel> levels)
    {
        var rectTransform = ContentImage.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, GetHeight(levels.Count));

        foreach (var level in levels)
        {
            switch (level.State)
            {
                case LevelState.Pass:
                    CreateLevelMarker(PassLevel);
                    break;
                case LevelState.Open:
                    CreateLevelMarker(OpenLevel);
                    break;
                case LevelState.Close:
                    CreateLevelMarker(CloseLevel);
                    break;
            }
        }
    }

    public void ChandgeLevel(int index, LevelState state)
    {
        var old = _levels[index];
        var siblingIndex = old.transform.GetSiblingIndex();
        var prefab = state == LevelState.Open ? OpenLevel : state == LevelState.Pass ? PassLevel : CloseLevel;

        _levels[index] = Instantiate(prefab, ContentImage.gameObject.transform);
        _levels[index].transform.SetSiblingIndex(siblingIndex);
        _levels[index].SetNumber(index + 1);
        Destroy(old.gameObject);
    }

    private void CreateLevelMarker(LevelMarker levelMarker)
    {
        _levels.Add(Instantiate(levelMarker, ContentImage.gameObject.transform));
        _levels[_levels.Count - 1].SetNumber(_levels.Count);
    }

    private float GetHeight(int levelsCount)
    {
        var mapRect = GetComponentInParent<RectTransform>();
        var minHeight = mapRect.rect.height;

        var vertGroup = ContentImage.GetComponent<VerticalLayoutGroup>();

        var top = vertGroup.padding.top;
        var bottom = vertGroup.padding.bottom;
        var spacing = vertGroup.spacing;
        var prefabHeight = PassLevel.GetComponent<RectTransform>().rect.height;
        var needHeight = top + bottom + prefabHeight * levelsCount + spacing * (levelsCount - 1);

        if (needHeight > minHeight) minHeight = needHeight;

        return minHeight;
    }
}
