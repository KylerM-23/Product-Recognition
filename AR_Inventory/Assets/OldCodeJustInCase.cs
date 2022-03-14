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

 * 
 * 
 * 
 */