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
            BoostSpeed();
        }
    }

    private void BoostSpeed()
    {
        if (!isBoosted)
        {
            playerMovement.SetWalkSpeed(boostSpeed);
            isBoosted = true;
            Debug.Log("Player speed set to boosted.");
        }
        else if (isBoosted)
        {
            Debug.Log("Player speed set to normal.");
            playerMovement.SetWalkSpeed(5f);
            isBoosted = false;
        }
    }
}
