using Firebase.Firestore;

[FirestoreData]
public class Album: FireStoreData
{
	[FirestoreProperty]
	public string Artist { get; set; }

	[FirestoreProperty]
	public int Count { get; set; }

	[FirestoreProperty]
	public double Length { get; set; }

	[FirestoreProperty]
	public string Category { get; set; }

}