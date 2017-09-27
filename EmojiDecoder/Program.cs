using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EmojiDecoder
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
                FileStream outfs = File.Open(args[1], FileMode.Create, FileAccess.Write, FileShare.None);
                BufferedStream outbs = new BufferedStream(outfs);

                byte[] onebit = Encoding.UTF8.GetBytes("😂");
                byte[] zerobit = Encoding.UTF8.GetBytes("😎");
                int emojilen = onebit.Length;
                
                BitArray bits = new BitArray(8);
                byte[] bytes = new byte[emojilen];
                int bitpos = -1;
                
                while (bs.Read(bytes, 0, emojilen) != 0)
                {
                    if (bitpos < 7)
                    {
                        bitpos++;
                    }
                    else
                    {
                        foreach (bool bit in bits)
                        {
                            Console.Write(bit ? "1" : "0");
                        }
                        Console.Write(" ");
                        outbs.Write(ConvertToByte(bits), 0, 1);
                        bitpos = 0;
                        bits = new BitArray(8);
                    }

                    if (bytes.SequenceEqual(onebit)) // Read bit is 1
                    {
                        bits[bitpos] = true;
                    }
                    else
                    {
                        bits[bitpos] = false;
                    }
                }
                
                outbs.Close();
                outfs.Close();
            }
        }

        static byte[] ConvertToByte(BitArray bits)
        {
            if (bits.Count != 8)
            {
                throw new ArgumentException("bits");
            }
            byte[] bytes = new byte[1];
            bits.CopyTo(bytes, 0);
            return bytes;
        }
    }
}