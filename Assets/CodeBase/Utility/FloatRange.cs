using UnityEngine;

[System.Serializable]
public struct FloatRange
{
    [SerializeField] private float _min, _max;

    public float Min => _min;
    public float Max => _max;
    public float Random => UnityEngine.Random.Range(Min, Max);

    public static FloatRange operator *(FloatRange range, float value) =>
        new(range._min * value, range._max * value);

    public FloatRange(float value) =>
        _min = _max = value;

    public FloatRange(float min, float max)
    {
        if(min <= max)
        {
            _min = min;
            _max = max;
        }
        else
        {
            _min = max;
            _max = min;
        }
    }

    public float FindPercentage(float value)
    {
        if (value >= _max) return 1f;
        if(value <= _min) return 0f;
        
        return (value - _min) / (_max - _min);
    }

    public float PercentageAt(float percentage)
    {
        if (percentage <= 0) return _min;
        if(percentage >= 1) return _max;

        return (_max - _min) * percentage + _min;
    }
}
