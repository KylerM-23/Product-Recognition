using Firebase.Firestore;

[FirestoreData]
public struct Album
{
	[FirestoreProperty]
	public string Name { get; set; }

	[FirestoreProperty]
	public string Artist { get; set; }

	[FirestoreProperty]
	public int Count { get; set; }

	[FirestoreProperty]
	public double Length { get; set; }

	/*
	[FirestoreProperty]
	public dataType date { get; set; }
	*/
}