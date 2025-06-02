using UnityEngine;

[System.Serializable]
public class IrisData
{
    public float sepalLength;
    public float sepalWidth;
    public float petalLength;
    public float petalWidth;
    public string species;

    public override string ToString()
    {
        return $"SL: {sepalLength}, SW: {sepalWidth}, PL: {petalLength}, PW: {petalWidth}, Species: {species}";
    }
}