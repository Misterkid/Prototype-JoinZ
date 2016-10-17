using UnityEngine;
using System.Collections;
using System.Timers;

public class OptionsWindow : MonoBehaviour 
{
    public bool activated = false;
    public Vector2 sWidthHeight = new Vector2(250, 25);
    public Vector2 bWidthHeight = new Vector2(100, 25);
    public Rect optionWindowRect = new Rect(0,0, 550, 550);
    public Color32 headingColor = new Color32(255, 255, 255, 255);
    private Vector2 optionScrollPosition = Vector2.zero;
   // private Vector2 resScrollPosition = Vector2.zero;
    private float maxYPosScroll = 0;
    //Resolution
    private bool fullScreen;
    //Rendering
    private float antiAliasing = 0;
    private float maxAntiAliasing = 8;
    private AnisotropicFiltering anisotropicFilt;
    private float textureQuality = 0;
    private float maxTextureQuality = 10;
    private float pixelLightCount = 0;
    private float maxPixelLightCount = 4;
    //Shadows
    private ShadowProjection shadowProjection;
    private float shadowDistance = 0;
    private float maxShadowDistance = 250;
    private float shadowCascade = 0;
    private float maxShadowCascade = 4;
    //Other
    private float vSync = 0;
    private float maxvSync = 2;
    private float particleRaycastBudget = 0;
    private float maxParticleRaycastBudget = 4096;

    private float frameRate = 0;
    private float maxFrameRate = 300;
    //Get FPS
    public double refreshRateMS = 500;
    private float lastFrameRate = 0.0f;
    private bool setFrameRate = true;
    private System.Timers.Timer frameUpdateTimer;
    //Sound
    private float masterVolume = 0;
    private float maxMasterVolume = 100;
    // Use this for initialization
	void Start () 
    {
        //Resolution
        fullScreen = Screen.fullScreen;
        //Rendering
        antiAliasing = QualitySettings.antiAliasing;
        anisotropicFilt = QualitySettings.anisotropicFiltering;
        textureQuality = QualitySettings.masterTextureLimit;
        pixelLightCount = QualitySettings.pixelLightCount;
        //Shadow
        shadowProjection = QualitySettings.shadowProjection;
        shadowDistance = QualitySettings.shadowDistance;
        shadowCascade = QualitySettings.shadowCascades;
        //Other
        vSync = QualitySettings.vSyncCount;
        particleRaycastBudget = QualitySettings.particleRaycastBudget;
        frameRate = Application.targetFrameRate;
        //FPS
        frameUpdateTimer = new System.Timers.Timer(refreshRateMS);
        frameUpdateTimer.Elapsed += new ElapsedEventHandler(frameUpdateTimer_Elapsed);
        frameUpdateTimer.AutoReset = true;
        frameUpdateTimer.Start();
        //Volume
        masterVolume = AudioListener.volume * 100;
	}
    void frameUpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
        setFrameRate = true;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (setFrameRate)
        {
            lastFrameRate = (int)(1.0f / Time.deltaTime);
            setFrameRate = false;
        }
	}
    void OnGUI()
    {
        if (activated)
        {
            optionWindowRect = GUI.Window(0, optionWindowRect, OptionWindow, "Options");
        }
    }
    private void OptionWindow(int id)
    {
        optionScrollPosition = GUI.BeginScrollView(new Rect(-5, 20, optionWindowRect.width, optionWindowRect.height - 30), optionScrollPosition, new Rect(0, 0, optionWindowRect.width - 20, maxYPosScroll));
        float yPos = 0;
        GUI.Label(new Rect(25, 0, sWidthHeight.x, sWidthHeight.y), string.Format("Fps:{0} refresh Fps rate: {1}", lastFrameRate, refreshRateMS));
        yPos = ResolutionGroup(0, yPos);
        yPos = RenderingGroup(0, yPos);
        yPos = ShadowGroup(0, yPos);
        yPos = OtherGroup(0, yPos);
        yPos = SoundGroup(0, yPos);
        yPos = yPos + bWidthHeight.y;
        if (GUI.Button(new Rect(25, yPos, bWidthHeight.x, bWidthHeight.y), "Exit"))
            activated = false;

        if(maxYPosScroll == 0)
            maxYPosScroll = yPos + 100;

        GUI.EndScrollView();
        GUI.DragWindow();
    }
    float ResolutionGroup(float xStart, float yStart)
    {
        float xPos = xStart + 25;
        float yPos = yStart + 25;
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), EUtils.UnityColoredText("Resolutions", headingColor));
        yPos = yPos + sWidthHeight.y;
        fullScreen = GUI.Toggle(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), fullScreen, "Full screen");
        yPos = yPos + sWidthHeight.y;
        float maxHeight = yPos + (Mathf.Floor(Screen.resolutions.Length / 3) * (sWidthHeight.y));
        Resolution[] resolutions = Screen.resolutions;
        for (int i = 0; i < resolutions.Length; i++)
        {
            float newXPos = xPos + (i % 3) * (bWidthHeight.x);
            float newYPos = yPos + (Mathf.Floor(i / 3) * (bWidthHeight.y));
            if (GUI.Button(new Rect(newXPos, newYPos, bWidthHeight.x, bWidthHeight.y), resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRate))
            {
                Screen.SetResolution(resolutions[i].width, resolutions[i].height, fullScreen, resolutions[i].refreshRate);
#if UNITY_WEBPLAYER
            //var unity = document.getElementById('unityPlayer');
                Application.ExternalEval("config.width = " + resolutions[i].width + ";" +
                    "config.height = " + resolutions[i].height + ";" +
                    "document.getElementById('unityPlayer').style.width = '" + resolutions[i].width + "px';" +
                    "document.getElementById('unityPlayer').style.height = '" + resolutions[i].height + "px';"+
                    "document.getElementsByTagName('embed')[0].width = '"+ resolutions[i].width + "px';" +
                    "document.getElementsByTagName('embed')[0].height = '" + resolutions[i].height + "px';" +
                    "document.getElementsByTagName('embed')[0].style.width = '"+ resolutions[i].width + "px';" +
                    "document.getElementsByTagName('embed')[0].style.height = '" + resolutions[i].height + "px';" +
                    "document.getElementsByClassName('content')[0].style.width = '" + resolutions[i].width + "px';"
                    );
#endif
                optionWindowRect.x = 0;
                optionWindowRect.y = 0;
                
            }
        }
        return maxHeight;
    }
    float RenderingGroup(float xStart,float yStart)
    {
        float xPos = xStart + 25;
        float yPos = yStart + 25;
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), EUtils.UnityColoredText("Rendering", headingColor));
        yPos = yPos + sWidthHeight.y;
        //Anti Analysing
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Antialiasing: " + QualitySettings.antiAliasing);
        antiAliasing = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), antiAliasing, 0, maxAntiAliasing);
        if (antiAliasing != QualitySettings.antiAliasing)
        {
            antiAliasing = (int)Mathf.ClosestPowerOfTwo((int)antiAliasing);
            if (antiAliasing == 1)
                antiAliasing = 0;
            QualitySettings.antiAliasing = (int)Mathf.ClosestPowerOfTwo((int)antiAliasing);
        }
        //End anti Analysing
        //Anisotropic Filtering
        yPos = yPos + sWidthHeight.y;
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Anisotropic Filtering: " + QualitySettings.anisotropicFiltering.ToString());

        yPos = yPos + sWidthHeight.y;
        if (GUI.Button(new Rect(xPos, yPos, bWidthHeight.x, bWidthHeight.y), AnisotropicFiltering.Disable.ToString()))
        {
            anisotropicFilt = AnisotropicFiltering.Disable;
        }
        if (GUI.Button(new Rect(xPos + bWidthHeight.x, yPos, bWidthHeight.x, bWidthHeight.y), AnisotropicFiltering.Enable.ToString()))
        {
            anisotropicFilt = AnisotropicFiltering.Enable;
        }
        if (GUI.Button(new Rect(xPos + (bWidthHeight.x * 2), yPos, bWidthHeight.x, bWidthHeight.y), AnisotropicFiltering.ForceEnable.ToString()))
        {
            anisotropicFilt = AnisotropicFiltering.ForceEnable;
        }
        QualitySettings.anisotropicFiltering = anisotropicFilt;
        //End Anisotropic Filtering

        //Texture Quality
        yPos = yPos + bWidthHeight.y;
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Texture Quality(Beter to worse): " + QualitySettings.masterTextureLimit);
        textureQuality = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, 25), textureQuality, 0, maxTextureQuality);
        QualitySettings.masterTextureLimit = (int)textureQuality;
        //End Texture Quality

        //Pixel Light Count
        yPos = yPos + sWidthHeight.y;
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Pixel Light Count: " + QualitySettings.pixelLightCount);
        pixelLightCount = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), pixelLightCount, 0, maxPixelLightCount);
        QualitySettings.pixelLightCount = (int)pixelLightCount;
        //End Pixel Light Count
        return yPos;
    }
    float ShadowGroup(float xStart, float yStart)
    {
        float xPos = xStart + 25;
        float yPos = yStart + 25;

        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y),  EUtils.UnityColoredText("Shadow",headingColor));
        yPos = yPos + sWidthHeight.y;
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Shadow Projection: " + QualitySettings.shadowProjection.ToString());
        yPos = yPos + sWidthHeight.y;
        if (GUI.Button(new Rect(xPos, yPos, bWidthHeight.x, bWidthHeight.y), ShadowProjection.CloseFit.ToString()))
        {
            shadowProjection = ShadowProjection.CloseFit;
        }
        if (GUI.Button(new Rect(xPos + bWidthHeight.x, yPos, bWidthHeight.x, bWidthHeight.y), ShadowProjection.StableFit.ToString()))
        {
            shadowProjection = ShadowProjection.StableFit;
        }
        QualitySettings.shadowProjection = shadowProjection;
        yPos = yPos + bWidthHeight.y;
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Shadow Distance: " + QualitySettings.shadowDistance);
        shadowDistance = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), shadowDistance, 0, maxShadowDistance);
        QualitySettings.shadowDistance = shadowDistance;
        yPos = yPos + sWidthHeight.y;
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Shadow Cascade: " + QualitySettings.shadowCascades);
        shadowCascade = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), shadowCascade, 0, maxShadowCascade);
        shadowCascade = (int)Mathf.ClosestPowerOfTwo((int)shadowCascade);
        if (shadowCascade == 1)
            shadowCascade = 0;
        QualitySettings.shadowCascades = (int)Mathf.ClosestPowerOfTwo((int)shadowCascade);

        return yPos;
    }
    float OtherGroup(float xStart, float yStart)
    {
        float xPos = xStart + 25;
        float yPos = yStart + 25;

        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y),  EUtils.UnityColoredText("Other",headingColor));
        yPos = yPos + sWidthHeight.y;
        //vSync
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "vSync: " + QualitySettings.vSyncCount);
        vSync = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), vSync, 0, maxvSync);
        QualitySettings.vSyncCount = (int)vSync;
        yPos = yPos + sWidthHeight.y;
        //End vSync
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x + 100, sWidthHeight.y), "Limit frame rate only works when vSync is disabled(0)");
        yPos = yPos + sWidthHeight.y;
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Frame rate (0 = unlimited): " + Application.targetFrameRate);
        frameRate = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), frameRate, 0, maxFrameRate);
        Application.targetFrameRate = (int)frameRate;
        yPos = yPos + sWidthHeight.y;

        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Particle Raycast Budget: " + QualitySettings.particleRaycastBudget);
        particleRaycastBudget = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), particleRaycastBudget, 0, maxParticleRaycastBudget);
        particleRaycastBudget = (int)Mathf.ClosestPowerOfTwo((int)particleRaycastBudget);
        if (particleRaycastBudget < 4)
            particleRaycastBudget = 4;
        QualitySettings.particleRaycastBudget = (int)Mathf.ClosestPowerOfTwo((int)particleRaycastBudget);
        return yPos;
    }
    float SoundGroup(float xStart, float yStart)
    {
        float xPos = xStart + 25;
        float yPos = yStart + 25;
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y),  EUtils.UnityColoredText("Sound",headingColor));
        yPos = yPos + sWidthHeight.y;
        //masterVolume
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Master Volume: " + (int)(AudioListener.volume * 100));
        masterVolume = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), masterVolume, 0, maxMasterVolume);
        AudioListener.volume = masterVolume /100;

        return yPos;
    }
    private void OnApplicationQuit()
    {
        frameUpdateTimer.Elapsed -= new ElapsedEventHandler(frameUpdateTimer_Elapsed);
        frameUpdateTimer.Dispose();
        frameUpdateTimer = null;
    }
}
