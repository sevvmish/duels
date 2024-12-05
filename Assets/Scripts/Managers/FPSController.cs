using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

public class FPSController : MonoBehaviour
{
    [Inject] private FOVControl fovControl;

    private List<float> fps = new List<float>();
    private float _timer, _firstTimer;
    private bool isFirst;

    public float GetAverage()
    {        
        if (fps.Count < 10) return 0;

        return fps.Average();
    }

    

    private void Update()
    {
        if (!Globals.IsInitiated || Globals.IsLowFPS) return;

        if (Globals.IsInitiated && !isFirst)
        {
            isFirst = true;
            _firstTimer = 2;
        }

        if (_firstTimer > 0)
        {
            _firstTimer-=Time.deltaTime;
            return;
        }
               
        if (_timer > 0.05f)
        {
            _timer = 0;
            if (EasyFpsCounter.EasyFps != null)
            {
                fps.Add(EasyFpsCounter.EasyFps.FPS);
                if (fps.Count > 50)
                {
                    fps.Remove(fps[0]);
                }

                float ave = GetAverage();

                if (fps.Count > 40 && ave > 5 && ave < 45)
                {             
                    
                    
                    Globals.IsLowFPS = true;
                    Globals.SetQualityLevel();
                    fovControl.SetFOV();
                    
                }
            }
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }

    
}
