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

    private bool _canChange = true;

    public void ChangeBgColor(Color color)
    {
        if(_canChange)
            _sprite.color = color;
    }

    public void SetCostsTexts(int fCost, int hCost, int gCost)
    {
        if (_canChange == false)
            return;

        if(fCost > _maxShowingCost)
            _costTextsObject.SetActive(false);
        else
        {
            _costTextsObject.SetActive(true);
            _fCostText.text = fCost.ToString();
            _hCostText.text = hCost.ToString();
            _gCostText.text = gCost.ToString();
        }
    }

    public void Lock(Color color)
    {
        _canChange = true;
        SetCostsTexts(_maxShowingCost + 1, 0, 0);
        ChangeBgColor(color);
        _canChange = false;
    }

    public void Unlock(Color color)
    {
        _canChange = true;
        ChangeBgColor(color);
    }
}
