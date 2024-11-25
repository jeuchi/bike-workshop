using UnityEngine;

public class LeverPoint : MonoBehaviour
{
    public LeverOuterTire leverOuterTire;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Lever"))
        {
            leverOuterTire.OnLeverTouchPoint(gameObject);
        }
    }
}
