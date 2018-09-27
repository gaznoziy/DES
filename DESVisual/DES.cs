using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESVisual
{
    class DES
    {
        private static int[] IPTable =
        {
            58, 50, 42, 34, 26, 18, 10, 2,
            60, 52, 44, 36, 28, 20, 12, 4,
            62, 54, 46, 38, 30, 22, 14, 6,
            64, 56, 48, 40, 32, 24, 16, 8,
            57, 49, 41, 33, 25, 17, 9, 1,
            59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5,
            63, 55, 47, 39, 31, 23, 15, 7
        };

        private static int[] keyIPTable =
        {
            57, 49, 41, 33, 25, 17, 9,
            1, 58, 50, 42, 34, 26, 18,
            10, 2, 59, 51, 43, 35, 27,
            19, 11, 3, 60, 52, 44, 36,
            63, 55, 47, 39, 31, 23, 15,
            7, 62, 54, 46, 38, 30, 22,
            14, 6, 61, 53, 45, 37, 29,
            21, 13, 5, 28, 20, 12, 4
        };

        private static int[] ithKeyTable =
        {
            14, 17, 11, 24, 1, 5,
            3, 28, 15, 6, 21, 10,
            23, 19, 12, 4, 26, 8,
            16, 7, 27, 20, 13, 2,
            41, 52, 31, 37, 47, 55,
            30, 40, 51, 45, 33, 48,
            44, 49, 39, 56, 34, 53,
            46, 42, 50, 36, 29, 32
        };

        // Array to store the number of rotations that are to be done on each round
        private static int[] keyShifts =
        {
            1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1
        };

        // Expansion table
        private static int[] Etable =
        {
            32, 1, 2, 3, 4, 5,
            4, 5, 6, 7, 8, 9,
            8, 9, 10, 11, 12, 13,
            12, 13, 14, 15, 16, 17,
            16, 17, 18, 19, 20, 21,
            20, 21, 22, 23, 24, 25,
            24, 25, 26, 27, 28, 29,
            28, 29, 30, 31, 32, 1
        };

        // S-boxes (i.e. Substitution boxes)
        private static int[][][] Stables =
        {
            new int[][]
            {
                new int[] { 14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7 },
                new int[] { 0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8 },
                new int[] { 4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0 },
                new int[] { 15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13 }
            },
            new int[][]
            {
                new int[] {15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10 },
                new int[] {3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5 },
                new int[] {0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15 },
                new int[] {13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9 }
            },
            new int[][]
            {
                new int[] {10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8 },
                new int[] {13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1 },
                new int[] {13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7 },
                new int[] {1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12 }
            },
            new int[][]
            {
                new int[] {7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15 },
                new int[] {13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9 },
                new int[] {10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4 },
                new int[] {3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14 }
            },
            new int[][]
            {
                new int[] {2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9 },
                new int[] {14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6 },
                new int[] {4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14 },
                new int[] {11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 }
            },
            new int[][]
            {
                new int[] {12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11 },
                new int[] {10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8 },
                new int[] {9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6 },
                new int[] {4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13 }
            },
            new int[][]
            {
                new int[] {4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1 },
                new int[] {13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6 },
                new int[] {1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2 },
                new int[] {6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 }
            },
            new int[][]
            {
                new int[] {13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7 },
                new int[] {1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2 },
                new int[] {7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8 },
                new int[] {2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11 }
            }
        };

        // Permutation table
        private static int[] Ptable =
        {
            16, 7, 20, 21,
            29, 12, 28, 17,
            1, 15, 23, 26,
            5, 18, 31, 10,
            2, 8, 24, 14,
            32, 27, 3, 9,
            19, 13, 30, 6,
            22, 11, 4, 25
        };

        // Final permutation (aka Inverse permutation) table
        private static int[] IPReversedTable =
        {
            40, 8, 48, 16, 56, 24, 64, 32,
            39, 7, 47, 15, 55, 23, 63, 31,
            38, 6, 46, 14, 54, 22, 62, 30,
            37, 5, 45, 13, 53, 21, 61, 29,
            36, 4, 44, 12, 52, 20, 60, 28,
            35, 3, 43, 11, 51, 19, 59, 27,
            34, 2, 42, 10, 50, 18, 58, 26,
            33, 1, 41, 9, 49, 17, 57, 25
        };

        // in bits
        private const int charSize = 16; // 2 байта
        private const int blockSize = 64; // 8 байт

        private string[] TextBlocks;
        private string[] Keys = new string[16];
        private List<char> crypted;
        private List<double> entropies;

        public DES()
        {
            crypted = new List<char>();
            entropies = new List<double>();
        }

        private double CoutnEntropy(string block)
        {
            var onesCount = block.Count(x => x.Equals('1'));

            double onesProbability = (double)onesCount / block.Length;
            double zeroProbability = 1 - onesProbability;

            var entropy = -1 * (onesProbability * Math.Log(onesProbability, 2) + zeroProbability * Math.Log(zeroProbability, 2));

            return Math.Round(entropy, 4);
        }

        public string Encrypt(string text, string key)
        {
            text = CorrectTextToRigthLength(text);

            CutTextToBlocks(text);

            key = StringToBinaryFormat(key);
            key = ExpandKey(key, 56);
            key = ExtendKeyUnevenBits(key);

            GenerateKeys(key);
            for (int i = 0; i < TextBlocks.Length; ++i)
            {
                var block = EncryptBlock(TextBlocks[i]);

                for (int j = 0; j < 4; ++j)
                {
                    var subBlock = block.Substring(j * 16, 16);

                    var cast = (char)Convert.ToInt32(subBlock, 2);

                    crypted.Add(cast);
                }
            }

            return String.Join(" ", crypted.ToArray());
        }

        public string Decrypt(string encrypted, string key)
        {
            key = StringToBinaryFormat(key);
            key = ExpandKey(key, 56);
            key = ExtendKeyUnevenBits(key);

            GenerateKeys(key);

            crypted.Clear();
            crypted = encrypted.Select(x => x).Where(x => x != 32).ToList();

            var blocks = new string[crypted.Count / 4];

            var counter = 0;
            for (var i = 0; i < blocks.Length; ++i)
            {
                string res = String.Empty;

                for (int j = 0; j < 4; ++j)
                {
                    res += AddMissingZeroes(Convert.ToString(Convert.ToInt32(crypted[counter]), 2), 16);
                    ++counter;
                }

                blocks[i] = res;
            }

            string result = String.Empty;

            for (int i = 0; i < blocks.Length; ++i)
            {
                var decryptedBlock = DecryptBlock(blocks[i]);

                for (int j = 0; j < 4; ++j)
                {
                    var subBlock = decryptedBlock.Substring(j * 16, 16);
                    var cast = (char)Convert.ToInt32(subBlock, 2);

                    if (cast != '*')
                        result += cast;
                }
            }

            return result;
        }

        private string DecryptBlock(string block)
        {
            block = Permut(block, IPTable);

            var left = block.Substring(0, 32);
            var right = block.Substring(32);

            for (int i = 15; i >= 0; --i)
            {
                var nextRight = left;
                var nextLext = XOR(right, f(left, Keys[i]));

                left = nextLext;
                right = nextRight;
            }

            var result = left + right;
            result = Permut(result, IPReversedTable);

            return result;
        }

        private string EncryptBlock(string block)
        {
            entropies.Clear();
            block = Permut(block, IPTable);

            var left = block.Substring(0, 32);
            var right = block.Substring(32);

            for (int i = 0; i < 16; ++i)
            {
                entropies.Add(CoutnEntropy(left + right));
                var nextRight = XOR(left, f(right, Keys[i]));
                left = right;
                right = nextRight;
            }

            var result = left + right;
            result = Permut(result, IPReversedTable);

            entropies.Add(CoutnEntropy(result));

            Console.WriteLine("Entropy measures:");
            for (int i = 0; i < 17; ++i)
            {
                Console.WriteLine($"Iteration number {i + 1}: {entropies[i]}");
            }

            return result;
        }

        private string Permut(string text, int[] table)
        {
            var result = String.Empty;

            for (var i = 0; i < table.Length; ++i)
            {
                result += text[table[i] - 1].ToString();
            }

            return result;
        }

        private string f(string vector, string key)
        {
            var extendedVector = Permut(vector, Etable);

            var xored = XOR(extendedVector, key);

            var xoredBlocks = new string[8];

            for (int i = 0; i < 8; ++i)
            {
                xoredBlocks[i] = xored.Substring(i * 6, 6);
            }

            var resultVector = String.Empty;

            for (int i = 0; i < xoredBlocks.Length; ++i)
            {
                string twoBorderBits = xoredBlocks[i][0].ToString() + xoredBlocks[i][5].ToString();
                string fourCentralBits = xoredBlocks[i].Substring(1, 4);

                int rowIndex = Convert.ToInt32(twoBorderBits, 2);
                int columnIndex = Convert.ToInt32(fourCentralBits, 2);

                var value = Stables[i][rowIndex][columnIndex];

                resultVector += AddMissingZeroes(Convert.ToString(value, 2), 4);
            }

            var permuttedResult = Permut(resultVector, Ptable);

            return permuttedResult;
        }

        private string XOR(string left, string right)
        {
            var result = String.Empty;

            for (var i = 0; i < left.Length; ++i)
            {
                result += left[i] == right[i] ? "0" : "1";
            }

            return result;
        }

        private void GenerateKeys(string initialKey)
        {
            var permuttedKey = Permut(initialKey, keyIPTable);

            var c = permuttedKey.Substring(0, 28);
            var d = permuttedKey.Substring(28, 28);

            for (int i = 0; i < 16; ++i)
            {
                c = ShiftLeft(c, keyShifts[i]);
                d = ShiftLeft(d, keyShifts[i]);

                var concattedKey = c + d;

                var roundKey = Permut(concattedKey, ithKeyTable);

                Keys[i] = roundKey;
            }
        }

        private string ShiftLeft(string bits, int value)
        {
            var result = String.Empty;

            var bitsCut = bits.Substring(0, value);
            result += bits.Substring(value) + bitsCut;

            return result;
        }

        private string ExtendKeyUnevenBits(string key)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < 8; ++i)
            {
                var block = key.Substring(i * 7, 7);

                var onesCount = block.Count(c => c.Equals('1'));

                result.Append(block + (onesCount % 2 == 0 ? "1" : "0"));
            }

            return result.ToString();
        }

        private string CorrectTextToRigthLength(string text)
        {
            var result = new StringBuilder(text);

            while (((result.Length * charSize) % blockSize) != 0) result.Append("*");

            return result.ToString();
        }

        private void CutTextToBlocks(string text)
        {
            TextBlocks = new string[(text.Length * charSize) / blockSize];

            var blockLength = text.Length / TextBlocks.Length; // always == 4

            for (var i = 0; i < TextBlocks.Length; ++i)
            {
                TextBlocks[i] = StringToBinaryFormat(text.Substring(i * blockLength, blockLength));
            }
        }

        private string ExpandKey(string key, int length)
        {
            if (key.Length > length)
            {
                return key.Substring(0, length);
            }

            return AddMissingZeroes(key, length);
        }

        private string StringToBinaryFormat(string str)
        {
            string result = String.Empty;

            for (var i = 0; i < str.Length; ++i)
            {
                var binaryChar = Convert.ToString(str[i], 2);

                result += AddMissingZeroes(binaryChar, charSize);
            }

            return result;
        }

        private string AddMissingZeroes(string str, int neededLength)
        {
            while (str.Length < neededLength)
            {
                str = "0" + str;
            }

            return str;
        }
    }
}
