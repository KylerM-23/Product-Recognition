/*
Intro:
    This file is for old code that is deleted and is here in case it is to be
    used in the future.

Rules:
    The file name will be on top with ~~~~ seperating files
 */

/*
~~~~~~~~~~~~~
KIMAGE TARGET

Description: The following code was for the screen object and changing its text.
            The object is no longer being used.

    public void load_AROB(string message = "")
    {
        loaded = true;
        CloneARObj = Instantiate(ARobject, new Vector3(-0.5f,-7,0), Quaternion.identity);
        CloneARObj.transform.parent = IT.transform;
        CloneARObj.transform.Rotate(new Vector3(0, 90, 0));
        var display = CloneARObj.GetComponent<Display>();
        display.SetText(message);
    }

    public void updateScreen(string message = "")
    {
        var display = CloneARObj.GetComponent<Display>();
        display.SetText(message);
    }

~~~~~~~~~
var firestore = FirebaseFirestore.DefaultInstance;
        CollectionReference storeCollection = firestore.Collection("Stores");
        Query possibleStores = storeCollection.WhereArrayContains("AllProducts", ID);
        possibleStores.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot storeSnapshots = task.Result;
            
            foreach (DocumentSnapshot documentSnapshot in storeSnapshots.Documents)
            {
                //Debug.Log(String.Format("Document data for {0} document:", documentSnapshot.Id));
                storeNames.Add(documentSnapshot.Id);
                /*Dictionary<string, object> stores = documentSnapshot.ToDictionary();
                var x = ()
                foreach (KeyValuePair<string, object> pair in stores)
                {
                    var x = (List<string>)pair.Value;
                    Debug.Log(x);
                    Debug.Log(String.Format("{0}: {1}", pair.Key, pair.Value));
                }
            }
            //PopUpPipe.LoadPopUp();
        });
* 
 * 
 * 
 */