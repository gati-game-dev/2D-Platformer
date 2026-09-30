using UnityEngine;

public class LeverPulled : MonoBehaviour
{
    public Transform player;
    private bool isLeverPulled = false;
    public bool IsLeverPulled { get { return isLeverPulled; } } // Other scripts can access (but not modify) the lever's state
    private SpriteRenderer leverSpriteRenderer; 
    public Sprite pulledLeverSprite; 
    void Start()
    {
        leverSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Vector3.Distance(player.position, transform.position) < 3f)
        {
            isLeverPulled = true;
            leverSpriteRenderer.sprite = pulledLeverSprite;
        }
    }
}