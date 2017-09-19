using UnityEngine;
using System.Collections;

//this is a Pickup that makes the player jump higher.
public class PickupJump : Pickup
{
    public float jumpIncreaseAmount = 5.0f;

    // Max Duration that the jump pickup will last.
    public float maxDuration = 1;

    public override void PickUp( PickupGetter getter )
    {
        //first, give health back, if applicable
        Jumper jumper = getter.GetComponent< Jumper >();
        if ( jumper != null )
        {
            jumper.jumpImpulse += jumpIncreaseAmount;
            StartCoroutine( EndEffect( jumper, maxDuration ) );
        }

        //then, do our default behavior
        base.PickUp( getter );
    }

    IEnumerator EndEffect( Jumper jumper, float delay )
    {
        yield return new WaitForSeconds( delay );
        jumper.jumpImpulse -= jumpIncreaseAmount;
    }
}
