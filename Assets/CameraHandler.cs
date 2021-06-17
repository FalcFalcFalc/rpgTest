using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{

    public static CameraHandler Instance;
    public static Vector3 ogPosition;

    CinemachineVirtualCamera virtualCamera;

    private void Start() {
        Instance = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    float shakingTime = 0;
    float startinIntensity = 0;
    float startingTime = 0;

    public static void ScreenShake(float strength, float time){
        Instance.virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = strength;
        Instance.shakingTime = time;
        Instance.startingTime = time;
        Instance.startinIntensity = strength;
    }

    public static void Focus(Transform target){
        Vector3 pos = new Vector3(target.position.x, target.position.y, -10);
        LeanTween.value(Instance.gameObject,Instance.virtualCamera.m_Lens.OrthographicSize,5,.6f).setOnUpdate(Instance.UpdateOrthographicSize);
        LeanTween.move(Instance.gameObject,pos,.6f).setEaseOutBack();
    }

    public static void Defocus(){
        LeanTween.move(Instance.gameObject,10 * Vector3.back,.5f).setEaseInBack();
        LeanTween.value(Instance.gameObject,Instance.virtualCamera.m_Lens.OrthographicSize,6,.4f).setOnUpdate(Instance.UpdateOrthographicSize);

    }

    private void UpdateOrthographicSize(float v) {
        virtualCamera.m_Lens.OrthographicSize = v;
    }

    private void Update() {
        if(shakingTime > 0){
            shakingTime -= Time.deltaTime;
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = Mathf.Lerp(0,startinIntensity,shakingTime / startingTime);
        }
    }
}
