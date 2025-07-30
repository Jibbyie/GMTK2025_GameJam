using UnityEngine;

public class PlayerDetectable : DetectableObject
{
    private float boostSpeed = 10f;
    private bool isBoosted;
    private PlayerMovement playerMovement;
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    public override void OnDetected()
    {
        if (playerMovement != null)
        {
            if(!isBoosted)
            {
                playerMovement.SetWalkSpeed(boostSpeed);
                isBoosted = true;
                Debug.Log("Boosted!");
            }
            else if(isBoosted)
            {
                playerMovement.SetWalkSpeed(5f);
                isBoosted = false;
                Debug.Log("Not boosted!");
            }
        }
    }
}
