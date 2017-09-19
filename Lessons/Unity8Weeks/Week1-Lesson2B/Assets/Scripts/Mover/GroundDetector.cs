using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [Tooltip ("These are the layers we collide with for the sake of standing on the ground or not.")]
    public LayerMask onGroundLayerMask;

    protected List< Collider2D > colliders;

    //we need to track whether we're on the ground or not to allow jumping. a "bool" is a boolean
    //value - true or false. So this will either be yes or no to whether we are on the ground.
    public bool isOnGround
    {
        protected set { }
        get { return colliders.Count > 0; }
    }

    public float collisionRadiusY
    {
        get;
        protected set;
    }

    //we also need to know the center of our collider so we know where to raycast from
    public Vector2 colliderCenter
    {
        get;
        protected set;
    }

    public virtual void Start()
    {
        colliders = new List<Collider2D>();

        BoxCollider2D myCollider = GetComponent<BoxCollider2D>();
        //store the radius of our collider so we know how far to raycast.
        collisionRadiusY = myCollider.size.y / 2.0f;
        //store the center of our collider, and we'll raycast from there.
        colliderCenter = myCollider.offset;
    }

    //Unity will automatically call this on a MonoBehaviour on the frame that a collision starts
    //between 2 colliders. note that occasionally this doesn't get called - thanks Unity!
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // handle multiple collisions.
        if ( ( ( 1 << collision.collider.gameObject.layer ) ^ onGroundLayerMask.value ) == 0 )
        {
            for( int i = 0; i < collision.contacts.Length; i++ )
            {
                // Check if the contact point normal is pointing up.
                // There is floating point error in the comparison between two float values
                // so we must check against the margin of error.
                ContactPoint2D contactPoint = collision.contacts[ i ];
                float tempX = Mathf.Abs( contactPoint.normal.x - Vector2.up.x );
                float tempY = Mathf.Abs( contactPoint.normal.y - Vector2.up.y );
                if( ( tempX > 0.01f ) ||
                    ( tempY > 0.01f ) )
                {
                    // Continue if we are not an up vector.
                    continue;
                }

                colliders.Add( collision.collider );
                // Break out of the loop here, we don't want to add anymore after we found
                // who we are colliding with.
                break;
            }
        }
    }
    
    //Unity will automatically call this on a MonoBehaviour on the frame that a collision ends
    public void OnCollisionExit2D(Collision2D collision)
    {
        //we're not colliding anymore with one of the colliders so let's remove one from the ground colliders num.
        if ( ( ( 1 << collision.collider.gameObject.layer ) ^ onGroundLayerMask.value ) == 0 )
        {
            colliders.Remove( collision.collider );
        }
    }
}
