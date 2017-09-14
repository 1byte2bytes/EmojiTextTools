using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EmojiEncoder
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Invalid arguments.");
                return;
            }
            
            if (File.Exists(args[0]) == false)
            {
                Console.WriteLine("Input file does not exist");
                return;
            }

            if (File.Exists(args[1]))
            {
                File.Delete(args[1]);
            }
            
            using (FileStream fs = File.Open(args[0], FileMode.Open, FileAccess.Read))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    byte[] inputBytes = Encoding.Default.GetBytes(line);
                    var bits = new BitArray(inputBytes);
            
                    string newText = string.Empty;
            
                    for (int i = 0; i < bits.Length; i++)
                    {
                        if (bits[i])
                        {
                            newText += "😂";
                        }
                        else
                        {
                            newText += "😎";
                        }
                    }
                    
                    using (var stream = new FileStream(args[1], FileMode.Append))
                    {
                        stream.Write(Encoding.Default.GetBytes(newText), 0, newText.Length);
                    }
                }
            }
        }
    }
}