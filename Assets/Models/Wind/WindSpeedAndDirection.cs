using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Text.RegularExpressions;

public class WindSpeedAndDirection : MonoBehaviour {
    public GameObject windSock;
    public GameObject pivot;
    public GameObject windText;
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
                Debug.Log(":\nWind Received: " + json);

                //parse wind speed and direction from json
                string degree = Regex.Replace(json.Substring(json.IndexOf("deg") + 5, 3), "[^0-9]+", "");
                Debug.Log("degree: " + degree);
                int direction = int.Parse(degree);
                string s = Regex.Replace(json.Substring(json.IndexOf("speed") + 7, 4), "[^0-9.]+", "");
                Debug.Log("speed: " + s);
                float speed = float.Parse(s);

                //set text of wind speed
                windText.GetComponent<TextMeshPro>().text = speed + " MPH";

                //get accurate direction and scale of wind sock
                direction = 360 - direction;
                float scaleY = (float)1.5 / (speed / 10);

                //set direction and scale of wind sock
                pivot.transform.Rotate(new Vector3(0, direction, 0));
                windSock.transform.localScale = new Vector3(1, scaleY, 1);
            }
        }
    }
}
