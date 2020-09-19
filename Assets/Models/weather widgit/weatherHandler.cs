using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class weatherHandler : MonoBehaviour {
    public GameObject Cloud1, Cloud2, Cloud3, lightning, weatherText;
    public GameObject rain, snow, sun;
    string url = "http://api.openweathermap.org/data/2.5/weather?lat=41.88&lon=-87.6&APPID=285c4fa1813be8efc39220903a9af06f&units=imperial";

    int current;

    void Start() {
        // wait a couple seconds to start and then refresh every 900 seconds

        InvokeRepeating("GetDataFromWeb", 0f, 900f);
    }

    private void Update() {
        if (Input.GetKeyDown("right")) {
            nextState();
        }
        else if (Input.GetKeyDown("left")) {
            lastState();
        }
        else if (Input.GetKeyDown("up")) {
            GetDataFromWeb();
        }
        else if(Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
                nextState();
        }
    }

    void nextState() {
        if(current < 300) {
            current = 300;
        }
        else if(current < 520) {
            current = 520;
        }
        else if(current < 600) {
            current = 600;
        }
        else if(current < 700) {
            current = 700;
        }
        else if(current < 800) {
            current = 800;
        }
        else if(current == 800) {
            current = 801;
        }
        else if (current == 801) {
            current = 802;
        }
        else if (current == 802) {
            current = 803;
        }
        else if (current > 802) {
            current = 200;
        }
        displayWeather(current);
    }

    void lastState() {
        if (current < 300) {
            current = 803;
        }
        else if (current < 520) {
            current = 200;
        }
        else if (current < 600) {
            current = 300;
        }
        else if (current < 700) {
            current = 520;
        }
        else if (current < 800) {
            current = 600;
        }
        else if (current == 800) {
            current = 700;
        }
        else if (current == 801) {
            current = 800;
        }
        else if (current == 802) {
            current = 801;
        }
        else if (current > 802) {
            current = 802;
        }
        displayWeather(current);
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
                Debug.Log(":\nWeather Widget Received: " + json);

                //parse weather id from json response
                int weatherID = int.Parse(json.Substring(json.IndexOf("id") + 4, 3));
                current = weatherID;

                displayWeather(current);
            }
        }
    }

    void displayWeather(int weather) {
        var main = rain.GetComponent<ParticleSystem>().main;
        if(weather < 300) {
            Cloud1.SetActive(true);
            Cloud2.SetActive(true);
            Cloud3.SetActive(true);
            rain.SetActive(false);
            main.maxParticles = 15;
            snow.SetActive(false);
            sun.SetActive(false);
            lightning.SetActive(true);
            weatherText.GetComponent<TextMeshPro>().text = "Thunderstorm";
            return;
        }
        if(weather < 520) {
            Cloud1.SetActive(true);
            Cloud2.SetActive(true);
            Cloud3.SetActive(true);
            rain.SetActive(true);
            main.maxParticles = 7;
            snow.SetActive(false);
            sun.SetActive(false);
            lightning.SetActive(false);
            weatherText.GetComponent<TextMeshPro>().text = "Rain";
            return;
        }
        if(weather < 600) {
            Cloud1.SetActive(true);
            Cloud2.SetActive(true);
            Cloud3.SetActive(true);
            rain.SetActive(true);
            main.maxParticles = 15;
            snow.SetActive(false);
            sun.SetActive(false);
            lightning.SetActive(false);
            weatherText.GetComponent<TextMeshPro>().text = "Showers";
            return;
        }
        if(weather < 700) {
            Cloud1.SetActive(true);
            Cloud2.SetActive(true);
            Cloud3.SetActive(true);
            rain.SetActive(false);
            snow.SetActive(true);
            sun.SetActive(false);
            lightning.SetActive(false);
            weatherText.GetComponent<TextMeshPro>().text = "Snow";
            return;
        }
        if(weather < 800) {
            Cloud1.SetActive(false);
            Cloud2.SetActive(false);
            Cloud3.SetActive(false);
            rain.SetActive(true);
            main.maxParticles = 7;
            snow.SetActive(false);
            sun.SetActive(false);
            lightning.SetActive(false);
            weatherText.GetComponent<TextMeshPro>().text = "Mist";
            return;
        }
        if(weather == 800) {
            Cloud1.SetActive(false);
            Cloud2.SetActive(false);
            Cloud3.SetActive(false);
            rain.SetActive(false);
            snow.SetActive(false);
            lightning.SetActive(false);
            sun.SetActive(true);
            weatherText.GetComponent<TextMeshPro>().text = "Clear";
            return;
        }
        if(weather == 801) {
            Cloud1.SetActive(false);
            Cloud2.SetActive(false);
            Cloud3.SetActive(true);
            rain.SetActive(false);
            snow.SetActive(false);
            sun.SetActive(false);
            lightning.SetActive(false);
            weatherText.GetComponent<TextMeshPro>().text = "Few Clouds";
            return;
        }
        if (weather == 802) {
            Cloud1.SetActive(true);
            Cloud2.SetActive(false);
            Cloud3.SetActive(true);
            rain.SetActive(false);
            snow.SetActive(false);
            sun.SetActive(false);
            lightning.SetActive(false);
            weatherText.GetComponent<TextMeshPro>().text = "Scattered Clouds";
            return;
        }
        if (weather == 803 || weather == 805) {
            Cloud1.SetActive(true);
            Cloud2.SetActive(true);
            Cloud3.SetActive(true);
            rain.SetActive(false);
            snow.SetActive(false);
            sun.SetActive(false);
            lightning.SetActive(false);
            weatherText.GetComponent<TextMeshPro>().text = "Overcast";
            return;
        }
    }
}
