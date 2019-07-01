using System;
using System.Collections.Generic;
using System.IO;

namespace satisfying_words
{
    class Program
    {
        static Dictionary<char, string> LetterNeighbors;
        static HashSet<char> LeftHand;
        static HashSet<char> RightHand;
    
        static void Main(string[] args)
        {
            LeftHand = new HashSet<char>();
            RightHand = new HashSet<char>();
            char[] left = { 'q', 'a', 'z', 'w', 's', 'x', 'e', 'd', 'c', 'r', 'f', 'v', 't', 'g', 'b' };
            char[] right = { 'y', 'h', 'n', 'u', 'j', 'm', 'i', 'k', 'o', 'l', 'p'};
            foreach (char ch in left)
                LeftHand.Add(ch);
            foreach (char ch in right)
                RightHand.Add(ch);

            LetterNeighbors = new Dictionary<char, string>();
            using (StreamReader stream = File.OpenText("./qwerty-layout.txt"))
            {
                string key, val;
                while((key = stream.ReadLine()) != null)
                {
                    val = stream.ReadLine();
                    LetterNeighbors.Add(key.ToCharArray()[0], val);
                }
            }

            var results = new List<Tuple<double, string>>();
            using (StreamReader stream = File.OpenText("./english-10k-long.txt"))
            {
                string line;
                double score;
                Tuple<double, string> temp;
                while ((line = stream.ReadLine()) != null)
                {
                    score = ScoreWord(line);
                    if (score != -1)
                    {
                        temp = new Tuple<double, string>(score, line);
                        results.Add(temp);
                    }
                }
            }

            results.Sort((x, y) => y.Item1.CompareTo(x.Item1));
            int i = 0;
            foreach (var tuple in results)
            {
                Console.WriteLine("{0}\t{1}", tuple.Item1, tuple.Item2);
                if (i++ > 500)
                    break;
            }
        }

        public static double ScoreWord(string input) // scores a word if it is valid, or returns 0 for invalid
        {
            double score = 0;
            char[] inputLetters = input.ToCharArray();

            if (input == "")
                return 0;

            for(int i = 1; i < inputLetters.Length; i++)
            {
                char prevLetter = inputLetters[i - 1];
                char currLetter = inputLetters[i];
                if (LetterNeighbors[prevLetter].Contains(currLetter))
                {
                    score += 1;
                }
                else if (RightHand.Contains(prevLetter) && LeftHand.Contains(currLetter)
                        || LeftHand.Contains(prevLetter) && RightHand.Contains(currLetter))
                    continue;
                else
                {
                    return -1;
                }
            }

            return score;
        }
    }
}
