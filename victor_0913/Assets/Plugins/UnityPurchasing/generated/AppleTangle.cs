#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class AppleTangle
    {
        private static byte[] data = System.Convert.FromBase64String("jeMFVOTu3Kv9k7yloPa+gKe7k7W1k7eloPanoLCu4tPTz8aD8czM14kl6yVUrqKipqajk8GSqJOqpaD2g+DikyGigZOupaqJJeslVK6ioqI2PdmvB+Qo+He1lJBoZ6zubbfKcvoEpqrftOP1sr3XcBQogJjkAHbMEpP7T/mnkS/LECy+fcbQXMT9xh/XysXKwMLXxoPB2oPCzdqD08LR14yTImClq4iloqampKGhkyIVuSIQ8cbPysLNwMaDzM2D18vK0IPAxtGPg8DG0dfKxcrAwtfGg9PMz8rA2mPAkNRUmaSP9Uh5rIKteRnQuuwW3OILO1pyacU/h8iycwAYR7iJYLzU1I3C09PPxo3AzM6MwtPTz8bAwqWg9r6tp7Wnt4hzyuQ31apdV8guvDJ4veTzSKZO/donjkiVAfTv9k/ELKsXg1RoD4+DzNMVnKKTLxTgbCGio6WqiSXrJVTAx6aikyJRk4mlarrRVv6tdtz8OFGGoBn2LO7+rlLag8LQ0NbOxtCDwsDAxtPXws3AxhaZDlesraMxqBKCtY3Xdp+ueMG108/Gg+DG0dfKxcrAwtfKzM2D4tYjt4hzyuQ31apdV8gujeMFVOTu3KRP3pogKPCDcJtnEhw57KnIXIhfkJX5k8GSqJOqpaD2p6WwofbwkrAdV9A4TXHHrGja7Jd7AZ1a21zIa5OypaD2p6mwqeLT08/Gg+rNwI2Sp6WwofbwkrCTsqWg9qepsKni09MIANIx5PD2YgyM4hBbWEDTbkUA7+p71TyQt8YC1DdqjqGgoqOiACGi5t2878jzNeIqZ9fBqLMg4iSQKSLTz8aD8czM14Pg4pO9tK6TlZOXkdHCwNfKwMaD0NfC18bOxs3X0I2Tg8zFg9fLxoPXy8bNg8LT08/KwMKsPp5QiOqLuWtdbRYarXr9v3VonsrFysDC18rMzYPi1tfLzNHK19qSz8aD6s3AjZKFk4eloPanqLC+4tODws3Hg8DG0dfKxcrAwtfKzM2D07wmICa4Op7klFEKOOMtj3cSM7F7C3/dgZZphnZ6rHXIdwGHgLJUAg+riKWipqakoaK1vcvX19PQmYyM1IWTh6Wg9qeosL7i09PPxoPgxtHXpqOgIaKso5MhoqmhIaKio0cyCqqHQUhyFNN8rOZChGlSzttORBa0tKWTrKWg9r6woqJcp6aToKKiXJO+kyGnGJMhoAADoKGioaGioZOuparBz8aD0NfCzcfC0ceD18bRztCDwpaRkpeTkJX5tK6QlpORk5qRkpeT18vM0crX2pK1k7eloPanoLCu4tOr/ZMhorKloPa+g6choquTIaKnkyi6Kn1a6M9WpAiBk6FLu51b86pwzceDwMzNx8rXyszN0IPMxYPW0MaVOu+O2xROLzh/UNQ4UdVx1JPsYq6lqokl6yVUrqKipqajoCGioqP/2ZMhotWTraWg9r6soqJcp6egoaIs0CLDZbj4qowxEVvn61PDmz22VhS4HjDhh7GJZKy+Fe4//cBr6CO0epXcYiT2egQ6GpHhWHt20j3dAvGehcSDKZDJVK4hbH1IAIxa8Mn4x8eWgLbotvq+EDdUVT89bPMZYvvz8wkpdnlHX3OqpJQT1taC");
        private static int[] order = new int[] { 31,12,12,13,18,52,26,39,37,38,44,57,17,36,30,15,21,34,19,40,23,40,32,51,37,50,38,34,37,34,50,38,46,41,46,57,36,45,49,54,57,44,50,53,46,49,50,54,52,50,57,53,52,54,57,55,59,58,59,59,60 };
        private static int key = 163;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
