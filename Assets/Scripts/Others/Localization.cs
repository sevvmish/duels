using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization
{
    private Translation translation;
    private Localization(string lang) 
    {
        /*
        switch(lang)
        {
            case "ru":
                translation = Resources.Load<Translation>("languages/russian");                
                break;

            case "en":
                translation = Resources.Load<Translation>("languages/english");
                break;

            default:
                translation = Resources.Load<Translation>("languages/russian");
                break;
        }*/

        switch (lang)
        {
            case "ru":

                translation = Resources.Load<Translation>("languages/russian");
                //translation = Resources.Load<Translation>("languages/english");
                //translation = Resources.Load<Translation>("languages/deutch");
                //translation = Resources.Load<Translation>("languages/spain");
                break;

            case "be":

                translation = Resources.Load<Translation>("languages/russian");
                break;

            case "uk":

                translation = Resources.Load<Translation>("languages/russian");
                break;

            case "kk":

                translation = Resources.Load<Translation>("languages/russian");
                break;

            case "uz":

                translation = Resources.Load<Translation>("languages/russian");
                break;

            case "en":
                
                translation = Resources.Load<Translation>("languages/english");
                break;

            case "de":

                translation = Resources.Load<Translation>("languages/deutch");
                break;

            case "es":

                translation = Resources.Load<Translation>("languages/spain");
                break;

            default:
                
                translation = Resources.Load<Translation>("languages/english");
                break;
        }
    }

    private static Localization instance;
    public static Localization GetInstanse(string lang)
    {        
        if (instance == null)
        {            
            instance = new Localization(lang);
        }

        return instance;
    }

    public Translation GetCurrentTranslation() => translation;

}
