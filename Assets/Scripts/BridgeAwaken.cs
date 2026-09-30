using UnityEngine;

public class BridgeAwaken : MonoBehaviour
{

    public Transform bridge;
    public float bridgeSpeed = 5f;
    public Vector3 targetPosition; 
    private bool isBridgeActivated = false;
    public LeverPulled lever; // Reference to the LeverPulled script
    void Start()
    {
        
    }

    void Update()
    {
        if (lever.IsLeverPulled && bridge.position != targetPosition)
        {
            isBridgeActivated = true;
        }
        if (isBridgeActivated)
        {
            bridge.position = Vector3.MoveTowards(bridge.position, targetPosition, bridgeSpeed * Time.deltaTime);
            if (bridge.position == targetPosition)
            {
                isBridgeActivated = false;            }
        }
    }
}
