using UnityEngine;

public class MoveSineWave : MonoBehaviour
{
    public float MoveAmountFactor = 50f;
    public bool YAxis;

    Vector2 startingPos;

    private void Awake()
    {
        startingPos = transform.localPosition;
    }

    void Update()
    {
        if (YAxis)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, startingPos.y + (Mathf.Sin(Time.time) * MoveAmountFactor));
        }
        else
        {
            transform.localPosition = new Vector2(startingPos.x + (Mathf.Sin(Time.time) * MoveAmountFactor), transform.localPosition.y);
        }
    }
}
