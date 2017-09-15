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
            
            using (FileStream fs = File.Open(args[0], FileMode.Open, FileAccess.Read))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                FileStream outfile = File.Open(args[1], FileMode.Create, FileAccess.Write, FileShare.None);

                byte[] onebit = Encoding.Default.GetBytes("😂");
                byte[] zerobit = Encoding.Default.GetBytes("😎");
                int emojilen = onebit.Length;
                
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    byte[] inputBytes = Encoding.Default.GetBytes(line);
                    var bits = new BitArray(inputBytes);

                    for (int i = 0; i < bits.Length; i++)
                    {
                        if (bits[i])
                        {
                            outfile.Write(onebit, 0, emojilen);
                        }
                        else
                        {
                            outfile.Write(zerobit, 0, emojilen);
                        }
                    }
                }
                
                outfile.Close();
            }
        }
    }
}