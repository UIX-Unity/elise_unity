#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("DtB264Ze9+zhRMwx/o/6hMWm8lvXrN+KijtHo2HROUnMMBzUnWEAk1eSIRrR5XTYz5L8PolM3wsjOfXjAU/3wb+QGo5E7Lqgf57Jpx/rmLUjHXg/KepAh/kRd3Z4X4gDNAq5n0CH+RF3dnhfiAM0CrmfAU/3wb+QGo5E7Lqgf57Jpx/rmLW8XVBHvzgzODCwMzMyivk+uZ/5g9es34qKOzSSuw1bHXM2RgkZp/yAIx14PynqvQocVbab6QlU3jqJXLe7Rk7L8TdHo2HROUnMMBzUnWEAk70KHFW2mzQ7GLR6tMU/MzMzNzIxsDM9MgKwFSxEAK/xp+VbsZS21sZXkiEa0eU5q8nziQMteHoXZ6on0KEoCxTa5vfs4UTMMf6P+oTFpvJbuBCv3r2dtml4nYBzgdhZRsI59e+NHFLr11MCsDMQAj80Oxi0erTFPzMzMzcyMY0cUuvXUzSSuw1bHXM2RgkZp/yAWaZatxKnaa/GoONk5kowWlwjlUwsxoAUALWylhgMbWKJEOqzk8oD1LgQr969nbZpeJ2Ac4HYWUbCOfXvvF1QR784CX3lMDEzMjMCsDMQAj+wMz0yArAzODCwMzMyivk+uZ/5g+kJVN46iVy3u0ZOy/E37Hn6R1NzoSgLFNrmWaZatxKnaa/GoONk5kqylhgMbWKJEOqzk8oD1A7QduuGXux5+kdTczmryfOJAy14ehdnqifQdNjPkvw+iUzfCyM59eMsxoAUALUwWlwjlUwVLEQAr/Gn5VuxlLbWxgl95TAxMzIz");
        private static int[] order = new int[] { 9,2,7,13,12,27,28,16,26,13,17,15,21,19,24,25,19,25,20,26,25,24,26,25,27,25,27,27,28,29 };
        private static int key = 50;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
