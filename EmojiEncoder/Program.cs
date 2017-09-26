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
            {
                FileStream outfs = File.Open(args[1], FileMode.Create, FileAccess.Write, FileShare.None);
                BufferedStream outbs = new BufferedStream(outfs);

                byte[] onebit = Encoding.UTF8.GetBytes("😂");
                byte[] zerobit = Encoding.UTF8.GetBytes("😎");
                int emojilen = onebit.Length;
                
                byte[] bytes = new byte[1];
                
                while (bs.Read(bytes, 0, 1) != 0)
                {
                    BitArray bits = new BitArray(bytes);

                    for (int i = 0; i < bits.Length; i++)
                    {
                        // Write onebit if bit is 1, otherwise zerobit if bit is zero
                        outbs.Write(bits[i] ? onebit : zerobit, 0, emojilen);
                    }
                }
                
                outbs.Close();
                outfs.Close();
            }
        }
    }
}