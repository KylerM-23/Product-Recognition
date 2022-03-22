using Firebase.Firestore;

public class VideoGameData : FireStoreData
{
	[FirestoreProperty]
	public string Studio { get; set; }

	[FirestoreProperty]
	public string path { get; set; }

	[FirestoreProperty]
	public string type { get; set; }


}