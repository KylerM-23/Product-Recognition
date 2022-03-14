using Firebase.Firestore;

public class MusicData: FireStoreData
{
	[FirestoreProperty]
	public string Artist { get; set; }

	[FirestoreProperty]
	public string path { get; set; }

	[FirestoreProperty]
	public string type { get; set; }


}