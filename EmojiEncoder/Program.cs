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
                
                char[] bytes = new char[1024];
                int read;
                int current = 0;
                sr.Read();
                
                while ((read = sr.Read(bytes, 0, 1024)) != 0)
                {
                    byte[] inputBytes = Encoding.Default.GetBytes(bytes);
                    var bits = new BitArray(inputBytes);
                    read = read * 8;

                    for (int i = 0; i < bits.Length; i++)
                    {
                        if (read == current)
                        {
                            return;
                        }
                        current++;
                        
                        if (bits[i])
                        {
                            outfile.Write(onebit, 0, emojilen);
                        }
                        else
                        {
                            outfile.Write(zerobit, 0, emojilen);
                        }
                    }

                    current = 0;
                }
                
                outfile.Close();
            }
        }
    }
}