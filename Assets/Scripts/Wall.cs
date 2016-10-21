using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

    public Sprite dmgSprite;                //Alternate sprite to display after wall has been attacked by player
    public int hp = 4;                      //hit points for the wall
    public AudioClip wallChopSound1;        //The first sound for the player chop animation
    public AudioClip wallChopSound2;        //The second sound for the player chop animation    

    private SpriteRenderer spriteRender;    //Store a component reference to the attached SpriteRenderer

	// Use this for initialization
	void Awake () {
        //Get a component reference to the SpriteRenderer
        spriteRender = GetComponent<SpriteRenderer>();
	}
	
    public void DamageWall(int loss)
    {
        //Play the chop sound animation
        SoundManager.instance.RandomizeSfx(wallChopSound1, wallChopSound2);

        //Set the sprite renderer to the damaged wall sprite
        spriteRender.sprite = dmgSprite;

        //Subtract loss from hit point total
        hp -= loss;

        //If hit point are less than or equal to zero:
        if(hp <= 0)
        {
            //Disable the GameObject
            gameObject.SetActive(false);
        }
    }
}
