using UnityEngine;

public class LeverPoint : MonoBehaviour
{
    public LeverOuterTire leverOuterTire;
    public PutOuterTireBack putOuterTireBack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Lever"))
        {
            if (leverOuterTire.isActiveScript) 
            {
                leverOuterTire.OnLeverTouchPoint(gameObject);
            }
        } 
        else if (other.gameObject.name == "Left Controller" || other.gameObject.name == "Right Controller")
        {
            if (putOuterTireBack.isActiveScript) 
            {
                putOuterTireBack.OnTireTouchPoint(gameObject);
            }
        }
    }
}
