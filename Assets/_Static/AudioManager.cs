using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioManager
{
    public enum SFX
    {
        NONE,
        ATTACK,
        BUTTON,
        DESTROY,
        SETCARDS,
        DRAWCARDS,
        MOVINGCARD,     //TODO
        SELECTCARD,
        DESSELECTCARD,
        CAMARAROTATION
    }

    public enum Music
    {
        NONE,
        BACKGROUND,
        WIN
    }

    public static void SetSFX(SFX sound)
    {
        AudioSource sfxAS = GameObject.Find("SFX_Source").GetComponent<AudioSource>();

        sfxAS.Stop();

        string audioName = null;

        switch (sound) 
        {
            case SFX.NONE:
                audioName = "";
                break;
            case SFX.ATTACK:
                int attack = Random.Range(0, 2) + 1;
                audioName = "Cards_fight_" + attack;
                break;
            case SFX.BUTTON:
                audioName = "Buttons";
                break;
            case SFX.DESTROY:
                audioName = "Hero_Card_Destroy";
                break;
            case SFX.SETCARDS:
                audioName = "Cards_placed";
                break;
            case SFX.DRAWCARDS:
                audioName = "shuffling-cards";
                break;
            case SFX.MOVINGCARD:
                audioName = "Cards_move";
                break;
            case SFX.SELECTCARD:
                audioName = "Cards_select_deselect";
                break;
            case SFX.DESSELECTCARD:
                audioName = "Cards_select_deselect";
                break;
            case SFX.CAMARAROTATION:
                audioName = "Camera";
                break;
        }

        sfxAS.clip = Resources.Load<AudioClip>("Audio/SFX/" + audioName);
        sfxAS.Play();
    }

    public static void SetMusic(Music sound)
    {
        AudioSource musicAS = GameObject.Find("Music_Source").GetComponent<AudioSource>();

        musicAS.Stop();

        string audioName = null;

        switch (sound)
        {
            case Music.NONE:
                audioName = "";
                break;
            case Music.BACKGROUND:
                audioName = "medieval-fantasy-142837";
                break;
            case Music.WIN:
                audioName = "kingdom-of-fantasy-version-60s-10817";
                break;
        }

        musicAS.clip = Resources.Load<AudioClip>("Audio/Music/" + audioName);
        musicAS.Play();
    }
}
