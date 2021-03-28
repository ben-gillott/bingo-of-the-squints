using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SoundManagerScript : MonoBehaviour {


	public static AudioClip attackWhooshSound, landingSound, unlockSound, jumpSound, jumpPadSound, menuScrollSound, menuSelectSound;
	static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start() {
        
        attackWhooshSound = Resources.Load<AudioClip> ("attackWhooshSound");
        landingSound = Resources.Load<AudioClip> ("landingSound");
        unlockSound = Resources.Load<AudioClip> ("unlockSound");
        jumpSound = Resources.Load<AudioClip> ("jumpSound");
        jumpPadSound = Resources.Load<AudioClip> ("jumpPadSound");
        menuScrollSound = Resources.Load<AudioClip> ("menuScrollSound");
        menuSelectSound = Resources.Load<AudioClip> ("menuSelectSound");


        audioSrc = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    public static SoundManagerScript Instance;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public static void PlaySound (string clip) {
    	switch (clip) {
    		case "attackWhooshSound":
    		audioSrc.PlayOneShot (attackWhooshSound);
    		break;
    		case "landingSound":
    		audioSrc.PlayOneShot (landingSound);
    		break;
    		case "unlockSound":
    		audioSrc.PlayOneShot (unlockSound);
    		break;
    		case "jumpSound":
    		audioSrc.PlayOneShot (jumpSound);
    		break;
    		case "jumpPadSound":
    		audioSrc.PlayOneShot (jumpPadSound);
    		break;
    		case "menuScrollSound":
    		audioSrc.PlayOneShot (menuScrollSound);
    		break;
    		case "menuSelectSound":
    		audioSrc.PlayOneShot (menuSelectSound);
    		break;
    	}
    }
}
