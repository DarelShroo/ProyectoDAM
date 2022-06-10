using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public class gpsLocation : MonoBehaviour
{ 
    //Esta clase se encarga de comrobar los permisos del gps
    void Update()
    {
        StartCoroutine(GPSLoc());
    }

    IEnumerator GPSLoc()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }
        
        if (!Input.location.isEnabledByUser)
        {
            yield break;
        }
        
        Input.location.Start();

        int maxWait = 20;

        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            yield break;
        }
    }
}
