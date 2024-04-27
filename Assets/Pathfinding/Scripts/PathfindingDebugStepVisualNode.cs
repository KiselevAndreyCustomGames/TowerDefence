using TMPro;
using UnityEngine;

public class PathfindingDebugStepVisualNode : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private GameObject _costTextsObject;
    [SerializeField] private TextMeshPro _fCostText;
    [SerializeField] private TextMeshPro _hCostText;
    [SerializeField] private TextMeshPro _gCostText;
    [SerializeField, Min(0)] private int _maxShowingCost = 1000;

    public void ChangeBgColor(Color color)
    {
        _sprite.color = color;
    }

    public void SetCostsTexts(int fCost, int hCost, int gCost)
    {
        if(fCost > _maxShowingCost)
        {
            _costTextsObject.SetActive(false);
        }
        else
        {
            _costTextsObject.SetActive(true);
            _fCostText.text = fCost.ToString();
            _hCostText.text = hCost.ToString();
            _gCostText.text = gCost.ToString();
        }
    }
}
