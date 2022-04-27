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
 * 
 *         /*
        public void createImageTarget(float delay = .1f)
        {
            if (!found)
            {
                for (int i = 0; i < n; i++)
                {
                    if (place < max)
                    {
                        var ARobjClone = Instantiate(ARobject, new Vector3(-0.5f, -7, 0), Quaternion.identity);
                        var TimerClone = Instantiate(TObj, new Vector3(-0.5f, -7, 0), Quaternion.identity);
                        KImageTarget kIT = new KImageTarget(baseStr + databaseName + fileExt, names[place], delay, ARobjClone, TimerClone);
                        kIT.addEvent(OnTargetStatusChanged);
                        kIT.addEvent(IncrementCount);
                        kImages.Push(kIT);
                        place++;
                        textBox.text = "Progress: " + place.ToString() + "/" + max.ToString() + " images.";
                    }
                }
            }
        }

        
        public void createMIT(float delay = .1f)
    {
        if (!found)
        {
            string artist = (string) databaseData["Artist"];
            debugBox.text = artist;
            for (int i = 0; i < n; i++)
            {
                if (place < max)
                {
                    var ARobjClone = CategoryManager.GetComponent<CategoryRetrival>().GetObject("Music");
                    var TimerClone = Instantiate(TObj, new Vector3(-0.5f, -7, 0), Quaternion.identity);
                    MusicImageTarget MIT = new MusicImageTarget(databaseName, names[place],(string) databaseData["Firestore"],  ARobjClone, TimerClone, delay);
                    MIT.addEvent(OnTargetStatusChanged);
                    MIT.addEvent(IncrementCount);
                    kImages.Push(MIT);
                    place++;
                    textBox.text = "Progress: " + place.ToString() + "/" + max.ToString() + " images.";
                }
            }
        }
    }

    public void createVGIT(float delay = .1f)
    {
        if (!found)
        {
            string studio = (string)databaseData["Studio"];
            for (int i = 0; i < n; i++)
            {
                if (place < max)
                {
                    var ARobjClone = CategoryManager.GetComponent<CategoryRetrival>().GetObject("Video_Games");
                    var TimerClone = Instantiate(TObj, new Vector3(-0.5f, -7, 0), Quaternion.identity);
                    VGImageTarget VGIT = new VGImageTarget(databaseName, names[place], (string)databaseData["Firestore"],  ARobjClone, TimerClone, delay);
                    VGIT.addEvent(OnTargetStatusChanged);
                    VGIT.addEvent(IncrementCount);
                    kImages.Push(VGIT);
                    place++;

                    textBox.text = "Progress: " + place.ToString() + "/" + max.ToString() + " images.";
                }
            }
        }
    }

    public void createBIT(float delay = .1f)
    {
        if (!found)
        {
            for (int i = 0; i < n; i++)
            {
                if (place < max)
                {
                    var ARobjClone = CategoryManager.GetComponent<CategoryRetrival>().GetObject("Books");
                    var TimerClone = Instantiate(TObj, new Vector3(-0.5f, -7, 0), Quaternion.identity);
                    BookImageTarget BIT = new BookImageTarget(databaseName, names[place], (string)databaseData["Firestore"], ARobjClone, TimerClone, delay);
                    BIT.addEvent(OnTargetStatusChanged);
                    BIT.addEvent(IncrementCount);
                    kImages.Push(BIT);
                    place++;

                    textBox.text = "Progress: " + place.ToString() + "/" + max.ToString() + " images.";
                }
            }
        }
    }

/*            if (targetType == "Image")
                switch (category)
                {
                    case "Music":
                        CIT = createMIT; break;
                    case "Video_Games":
                        CIT = createVGIT; break;
                    case "Books":
                        CIT = createBIT; break;
                    default:
                        CIT = createImageTarget; break;
                }
        }
    }
    /*
    public virtual void SetData(Dictionary<string, object> result)
    {
        Info["ID"] = (string)result["ID"];
        PopUpPipe.SetInfo("Default String", Info["ID"]);
        PopUpPipe.LoadPopUp();
        GetStores();
    }
*/