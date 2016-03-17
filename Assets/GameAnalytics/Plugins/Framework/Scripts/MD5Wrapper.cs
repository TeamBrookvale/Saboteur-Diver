using UnityEngine;
using System.Collections;

[NotConverted]

#if (UNITY_4_9 || UNITY_4_8 || UNITY_4_7 || UNITY_4_6 || UNITY_4_5 || UNITY_4_3 || UNITY_4_2 || UNITY_4_1 || UNITY_4_0_1 || UNITY_4_0 || UNITY_3_5 || UNITY_3_4 || UNITY_3_3 || UNITY_3_2 || UNITY_3_1 || UNITY_3_0_0 || UNITY_3_0 || UNITY_2_6_1 || UNITY_2_6)
[NotRenamed]
#endif

public static class MD5Wrapper
{
	#if !UNITY_WP8 && !UNITY_METRO

	#if (UNITY_4_9 || UNITY_4_8 || UNITY_4_7 || UNITY_4_6 || UNITY_4_5 || UNITY_4_3 || UNITY_4_2 || UNITY_4_1 || UNITY_4_0_1 || UNITY_4_0 || UNITY_3_5 || UNITY_3_4 || UNITY_3_3 || UNITY_3_2 || UNITY_3_1 || UNITY_3_0_0 || UNITY_3_0 || UNITY_2_6_1 || UNITY_2_6)
	[NotRenamed]
	#endif

	public static string Md5Sum (string strToEncrypt)
	{

		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding ();

		byte[] bytes = ue.GetBytes (strToEncrypt);

     

		// encrypt bytes

		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider ();

		byte[] hashBytes = md5.ComputeHash (bytes);

     

		// Convert the encrypted bytes back to a string (base 16)

		string hashString = "";

     

		for (int i = 0; i < hashBytes.Length; i++) {

			hashString += System.Convert.ToString (hashBytes [i], 16).PadLeft (2, '0');

		}

		return hashString.PadLeft (32, '0');
	}
	#endif
}