using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UsersFromLocation : MonoBehaviour
{
    public Text GetLocation;
    private int counter = 0;
    //public Text TempLat;
    //public Text TempLong;

   // public Text serverResponse;
    public static string message;
    private void Update()
    {
      //  serverResponse.text = message;
    }

    void Start()
    {
        LoadingManager.instance.loading.SetActive(true);
        Invoke(nameof(GetUserByLocations), 1);
    }
  
    public void GetUserByLocations()
    {
        LoadingManager.instance.loading.SetActive(true);
      //  Debug.Log("Gettttttttttt location");
        StartCoroutine(GetLocations());
    }

    public void CalculateRange()
    {

        Invoke(nameof(CalculateRange), 5);
    }

    IEnumerator GetLocations()
    {
        WWWForm form = new WWWForm();
        form.AddField("lat", Input.location.lastData.latitude.ToString());
        form.AddField("lng", Input.location.lastData.longitude.ToString());
        form.AddField("radius", "60");


        string requestName = "/api/v1/locations/get_location";

        using UnityWebRequest www = UnityWebRequest.Post(AuthManager.BASE_URL + requestName, form);
        www.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
           // serverResponse.text = www.downloadHandler.text;
            ConsoleManager.instance.ShowMessage("Error "+ www.error);
            Debug.Log(AuthManager.BASE_URL + requestName + form);
            Debug.Log(Auth0Manager.AccessToken);
            Debug.Log(www.error);
          //  serverResponse.text = www.error;
            LoadingManager.instance.loading.SetActive(false);
        }
        else
        {
            try
            {
                Debug.Log(www.downloadHandler.text);
                Debug.Log(" 3D pins placement function!");
                LocationDataManager.instance.PlacePoints(www.downloadHandler.text);
            }
            catch (Exception e)
            {
        
            }
            LoadingManager.instance.loading.SetActive(false);
            ConsoleManager.instance.ShowMessage("Location Found!");
        }
    }
}
