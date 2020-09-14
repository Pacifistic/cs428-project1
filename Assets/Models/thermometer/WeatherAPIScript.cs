using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class WeatherAPIScript : MonoBehaviour {
    public GameObject temperature;
    public GameObject humidity;
    public GameObject thermometer;
    string url = "http://api.openweathermap.org/data/2.5/weather?lat=41.88&lon=-87.6&APPID=285c4fa1813be8efc39220903a9af06f&units=imperial";


    void Start() {

        // wait a couple seconds to start and then refresh every 900 seconds

        InvokeRepeating("GetDataFromWeb", 2f, 900f);
    }

    void GetDataFromWeb() {

        StartCoroutine(GetRequest(url));
    }

    IEnumerator GetRequest(string uri) {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri)) {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();


            if (webRequest.isNetworkError) {
                Debug.Log(": Error: " + webRequest.error);
            }
            else {
                string json = webRequest.downloadHandler.text;
                // print out the weather data to make sure it makes sense
                Debug.Log(":\nReceived: " + json);
                
                //parse temperature and humidity from json
                int temp = int.Parse(json.Substring(json.IndexOf("temp") + 6, 2));
                int hum = int.Parse(json.Substring(json.IndexOf("humidity") + 10, 2));

                //set temperuture and humidity text
                temperature.GetComponent<TextMeshPro>().text = temp + " F";
                humidity.GetComponent<TextMeshPro>().text = hum + "%";

                //change scale of thermometer based on temperature
                float scaleY = (float)15.75 * (float)(temp / 100.0);
                thermometer.transform.localScale = new Vector3(5, scaleY, 150);
            }
        }
    }
}
