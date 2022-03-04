using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Firebase;
using Firebase.Extensions;
using Firebase.Analytics;

public class FireBaseInit : MonoBehaviour
{

    public UnityEvent OnFirebaseInitialized = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread( task =>
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            OnFirebaseInitialized.Invoke();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    } 
}
