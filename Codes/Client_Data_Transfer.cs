using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;
using MNF;
using System;
public class Client_Data_Transfer : MonoBehaviour
{
    public DATA_PARSING skript;
    public ButtonController MNF_Button_Contrl;
    public TextMesh connection_status,version_status;
    public GameObject Info_first;
    public GameObject Map,PFD;
    public GameObject low_vacuum;
    public GameObject oil_pressure;
    public GameObject attitude_roll;
    public GameObject compass;
    public GameObject pitch;
    public GameObject spd;
    public GameObject SPD;
    public GameObject ALT;
    public GameObject ALT_panel;
    public TextMesh degree;
    public TextMesh ALT_NUM;
    public TextMesh time;
    public TextMesh hdgdegree;
    public GameObject heading_bug;
    public TextMesh spdkts;
    private float deg;
    public GameObject SOFT_KEYS;
    public GameObject HSI_Indicator;
    public GameObject ASI_Indicator;
    public GameObject VSI_Indicator;
    public GameObject VSI;
    public GameObject green;
    public TextMesh RPM_engine;
    public GameObject RPM_idle;
    public float hsi_smooth_float;
    public float rpm = 524;
    private float volt_M, volt_E, amper_M, amper_S;
    public GameObject Engine_Disp;
    private bool search_completed;
    [SerializeField] private MFD_parameters MFD = new MFD_parameters();
    [System.Serializable]
    public class MFD_parameters                                   
    {
        public GameObject FFLOW_GPH;
        public GameObject OIL_PRES;
        public GameObject OIL_TEMP;
        public GameObject EGT;
        public GameObject VAC;
        public TextMesh Elect_Volt_M, Elect_Volt_E, Elect_Amper_M, Elect_Amper_S;

    }
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        version_status.text = "VERSION: "+ Application.version.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = (Camera.main.ScreenPointToRay(Input.mousePosition));

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit))
        {

            if (hit.collider.gameObject.name == "CONNECT")
            {
                MMVibrationManager.Haptic(HapticTypes.LightImpact);
                MNF_Button_Contrl.onClientButton_click();
                StartCoroutine(search());
            }

            if (hit.collider.gameObject.name == "TUTORIAL")
            {
                Application.OpenURL("https://bagor.co/?page_id=1346");
                MMVibrationManager.Haptic(HapticTypes.LightImpact);
            }

            if (hit.collider.gameObject.name == "BUT_1")
            {
                Engine_Disp.active = Engine_Disp.active == false ? true : false;
                MMVibrationManager.Haptic(HapticTypes.LightImpact);
            }


        }

            /////////////////////////////////////MULTI FUNCTIONAL DISPLAY/////////////////////////////////////////////////
            if (skript.maxEnginePower > 0)
            {
                MFD.Elect_Amper_M.text = string.Format("{0:0.0}", amper_M);
                MFD.Elect_Volt_M.text = string.Format("{0:0.0}", volt_M);
                MFD.FFLOW_GPH.transform.localPosition = Vector3.Lerp(MFD.FFLOW_GPH.transform.localPosition, new Vector3(-4.5f * skript.Throttle * skript.EnginePower / skript.maxEnginePower, 0, 0), Time.deltaTime * 2);
                MFD.OIL_PRES.transform.localPosition = Vector3.Lerp(MFD.OIL_PRES.transform.localPosition, new Vector3(-4.8f * skript.EnginePower / skript.maxEnginePower, 0, 0), Time.deltaTime * 2);
                MFD.EGT.transform.localPosition = Vector3.Lerp(MFD.EGT.transform.localPosition, new Vector3(-4.8f * skript.EnginePower / skript.maxEnginePower, 0, 0), Time.deltaTime * 2);
                MFD.OIL_TEMP.transform.localPosition = Vector3.Lerp(MFD.OIL_TEMP.transform.localPosition, new Vector3(-10f * skript.StarterEnginePower - skript.Throttle * skript.EnginePower / skript.maxEnginePower, 0, 0), Time.deltaTime * 0.1f);
                MFD.VAC.transform.localPosition = Vector3.Lerp(MFD.VAC.transform.localPosition, new Vector3(-64f * skript.StarterEnginePower - skript.Throttle * skript.EnginePower / skript.maxEnginePower, 0, 0), Time.deltaTime * 0.5f);
                RPM_engine.text = string.Format("{0:0000}", rpm + (rpm * 3.1f * skript.Throttle) + (skript.vertical_speed / -5));
                RPM_idle.transform.localRotation = Quaternion.Euler(0, rpm / -13 + (rpm * 3.1f * skript.Throttle) / -13 - (skript.vertical_speed / -40), 0);
                if (skript.StarterEnginePower == 0.0f)
                {
                    rpm = Mathf.Lerp(rpm, 0, Time.deltaTime);
                    /////////ELECTRICAL_DATA////////////////////
                    volt_M = Mathf.Lerp(volt_M, 23.8f, Time.deltaTime * 2);
                    amper_M = Mathf.Lerp(amper_M, -22, Time.deltaTime * 2);
                }
                if (skript.StarterEnginePower == 0.05f)
                {
                    rpm = Mathf.Lerp(rpm, 524, Time.deltaTime);
                    /////////ELECTRICAL_DATA////////////////////
                    volt_M = Mathf.Lerp(volt_M, 28, Time.deltaTime * 2);
                    amper_M = Mathf.Lerp(amper_M, 0.1f, Time.deltaTime * 2);



                }
            }
            skript.LOC_ROT_ANG_Y;
            compass.transform.localRotation = Quaternion.Euler(180.6f, skript.LOC_ROT_ANG_Y, 0);
            degree.text = string.Format("{0:000}", (int)skript.LOC_ROT_ANG_Y) + "°";
            time.text = string.Format("{0:00}", System.DateTime.Now.Hour) + ":" + string.Format("{0:00}", System.DateTime.Now.Minute) + ":" + string.Format("{0:00}", System.DateTime.Now.Second);
            //ALT_NUM.text=buttoncontrol.altdigit.ToString();
            hdgdegree.text = string.Format("{0:000}", (int)skript.hdgdeg) + "°";

            //"""""""""""""""""""""""""""""""VSI MAGENTA Level"""""""""""""""""""""""""""""""""""""""
            if (skript.vertical_speed > 0)
                VSI_Indicator.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Progress", skript.vertical_speed / 2000);//VSI Indicator POSITIVE
            if (skript.vertical_speed < 0)
                VSI_Indicator.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Progress", skript.vertical_speed / -2000);//VSI Indicator NEGATIVE

            //""""""""""""""""""""""""""""""""VSI BLACK Window"""""""""""""""""""""""""""""""""""""""""""""""""
            if ((int)skript.vertical_speed % 50 == 0f)
                VSI.transform.GetChild(0).transform.GetComponent<TextMesh>().text = string.Format("{0:0}", ((int)skript.vertical_speed));
            if (skript.vertical_speed < 100 && skript.vertical_speed > -100)
                VSI.transform.GetChild(0).transform.gameObject.SetActive(false);
            else VSI.transform.GetChild(0).transform.gameObject.SetActive(true);
            if (skript.vertical_speed < 2000 && skript.vertical_speed > -2000)//garminde gosterici 2000ft/m kimidir
                VSI.transform.localPosition = Vector3.Lerp(VSI.transform.localPosition, new Vector3(0, 0, skript.vertical_speed * 0.000008f), Time.deltaTime);

            //---------------------------HSI INDICATOR-------------------------------------------------------------------------------------------------------
            if (skript.angle_roll > 20)
                HSI_Indicator.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
            else HSI_Indicator.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);

            if (skript.angle_roll < -20)
                HSI_Indicator.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
            else HSI_Indicator.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(false);


            HSI_Indicator.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Progress", skript.angle_roll / 20);//HSI Indicator LEFT

            HSI_Indicator.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Progress", skript.angle_roll / -20);//HSI Indicator RIGHT

            //--------------------------ASI INDICATOR----------------------------------------------------------------------------------------------------------
            if (skript.acceleration < 0)
                ASI_Indicator.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Progress", -skript.acceleration / 1.5f);//HSI Indicator POSITIVE
            if (skript.acceleration > 0)
                ASI_Indicator.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Progress", skript.acceleration / 1.5f);//HSI Indicator NEGATIVE

            heading_bug.transform.localRotation = Quaternion.Euler (180,(int)skript.hdgdeg, 0);
            pitch.GetComponent<MeshRenderer>().GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, 0.35f - (skript.angle_pitch * 0.006f)));
            spd.GetComponent<MeshRenderer>().GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0.1f, skript.ForwardSpeed * 0.0025f));
            spdkts.text = ((int)(skript.ForwardSpeed * 2)).ToString();
            SPD.transform.GetChild(0).GetComponent<MeshRenderer>().GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, 1 + (0.1f * (skript.ForwardSpeed * 2f))));
            SPD.transform.GetChild(1).GetComponent<MeshRenderer>().GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, (0.01f * ((int)(skript.ForwardSpeed * 2f / 10)) * 10)));
            SPD.transform.GetChild(2).GetComponent<MeshRenderer>().GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, (0.001f * ((int)(skript.ForwardSpeed * 2f / 100)) * 100)));
            attitude_roll.transform.localRotation = Quaternion.Euler(0, -skript.LOC_ROT_ANG_Z, 0);
            ALT.transform.GetChild(0).GetComponent<MeshRenderer>().GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, 0.85F + (0.01f * ((int)skript.Altitude))));
            ALT.transform.GetChild(1).GetComponent<MeshRenderer>().GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, (0.001f * ((int)(skript.Altitude / 100)) * 100)));
            ALT.transform.GetChild(2).GetComponent<MeshRenderer>().GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, (0.0001f * ((int)(skript.Altitude / 1000)) * 1000)));
            ALT.transform.GetChild(3).GetComponent<MeshRenderer>().GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, (0.00001f * ((int)(skript.Altitude / 10000)) * 10000)));


        ALT_panel.transform.GetChild(0).GetComponent<MeshRenderer>().GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, -0.2F + (0.001f * ((int)skript.Altitude))));
        ALT_panel.transform.GetChild(1).GetComponent<MeshRenderer>().GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, -0.02F + (0.0001f * ((int)skript.Altitude))));
        ALT_panel.transform.GetChild(2).GetComponent<MeshRenderer>().GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(Mathf.Floor(((0.1f * skript.Altitude) / 10000) * 10) / 10, -0.2F + (0.001f * ((int)skript.Altitude))));
        ALT_panel.transform.GetChild(3).GetComponent<MeshRenderer>().GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, -0.2F + (0.001f * ((int)skript.Altitude))));

        if (skript.Altitude > 900)
        {
            ALT_panel.transform.GetChild(1).GetComponent<MeshRenderer>().GetComponent<Renderer>().material.SetColor("_Color", new Color32(255, 255, 255, 255));
            ALT_panel.transform.GetChild(0).gameObject.transform.localPosition = new Vector3(0.005f, ALT_panel.transform.GetChild(0).gameObject.transform.localPosition.y, ALT_panel.transform.GetChild(0).gameObject.transform.localPosition.z);
        }
        else
        {
            ALT_panel.transform.GetChild(1).GetComponent<MeshRenderer>().GetComponent<Renderer>().material.SetColor("_Color", new Color32(255, 255, 255, 0));
            ALT_panel.transform.GetChild(0).gameObject.transform.localPosition = new Vector3(0.008f, ALT_panel.transform.GetChild(0).gameObject.transform.localPosition.y, ALT_panel.transform.GetChild(0).gameObject.transform.localPosition.z);
        }

        if (skript.Altitude > 10000)
        {
            ALT_panel.transform.GetChild(2).GetComponent<MeshRenderer>().GetComponent<Renderer>().material.SetColor("_Color", new Color32(255, 255, 255, 255));

        }
        else
        {
            ALT_panel.transform.GetChild(2).GetComponent<MeshRenderer>().GetComponent<Renderer>().material.SetColor("_Color", new Color32(255, 255, 255, 0));

        }

        if (skript.StarterEnginePower > 0)
                oil_pressure.SetActive(false);
            if (skript.EnginePower < 0.01f)
                oil_pressure.SetActive(true);
            if (skript.EnginePower > 4)
                low_vacuum.SetActive(false);
            if (skript.EnginePower < 2)
                low_vacuum.SetActive(true);

    }


    public IEnumerator search()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
            connection_status.text = "Check WiFi connection and try again..";
        else
        {
            connection_status.text = "Searching other device. Please wait..";
            yield return new WaitForSeconds(4);



            if (MNF_ChatClient.Instance.init() == true)
            {
                green.GetComponent<MeshRenderer>().material.color = Color.green;
                Info_first.SetActive(false);
                PFD.SetActive(true);
                connection_status.text = "";
                MMVibrationManager.Haptic(HapticTypes.Success);
            }
            else
            {
                green.GetComponent<MeshRenderer>().material.color = Color.red;
                PFD.SetActive(false);
                connection_status.text = "No device found..";
                MMVibrationManager.Haptic(HapticTypes.Warning);
            }
        }
    }
    
}
