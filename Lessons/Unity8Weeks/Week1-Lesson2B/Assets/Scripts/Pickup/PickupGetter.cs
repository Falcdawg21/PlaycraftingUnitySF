using System.Collections.Generic;
using UnityEngine;

//this is something that can get Pickups. Pretty clear, right?
//if something without one of these passes over a Pickup, nothing happens.
//should the trigger handler have been here instead of in the Pickup?
public class PickupGetter : MonoBehaviour
{
    //we'll keep track of every Pickup we've gotten, unless they're consumable
    //protected List<Pickup> pickups;
    protected Dictionary< string, List< Pickup > > pickups;

    public virtual void Awake()
    {
        pickups = new Dictionary< string, List< Pickup > >();
    }

    public virtual void PickUp( Pickup pickup )
    {
        if ( !pickup.isConsumable )
        {
            List< Pickup > pickupList;
            pickups.TryGetValue( pickup.id, out pickupList );
            if( pickupList != null )
            {
                pickupList.Add( pickup );
            } 
            else
            {
                pickups.Add( pickup.id, new List< Pickup >() { pickup } );
            }
        }
    }

    //this is a very poor way of writing this function. Imagine if we had 10,000
    //items we had picked up – we would have to go through every single one each
    //and every time. Instead, we would be better served storing the count as it
    //changes, and just accessing with we have stored.
    public virtual int FindPickupCount( string pickupId )
    {
        int count = 0;
        List< Pickup > pickupList;
        pickups.TryGetValue( pickupId, out pickupList );
        if( pickupList != null )
        {
            count = pickupList.Count;
        }

        return count;
    }
}
