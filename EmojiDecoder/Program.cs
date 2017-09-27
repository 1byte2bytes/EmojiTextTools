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
                
                outbs.Write(ConvertToByte(bits), 0, 1);
                
                outbs.Flush();
                outbs.Close();
                outfs.Close();
            }
        }

        static byte[] ConvertToByte(BitArray bits)
        {
            if (bits.Count != 8)
            {
                throw new ArgumentException("illegal number of bits");
            }

            byte b = 0;
            if (bits.Get(7)) b++;
            if (bits.Get(6)) b += 2;
            if (bits.Get(5)) b += 4;
            if (bits.Get(4)) b += 8;
            if (bits.Get(3)) b += 16;
            if (bits.Get(2)) b += 32;
            if (bits.Get(1)) b += 64;
            if (bits.Get(0)) b += 128;
            byte[] b2 = new byte[1];
            b2[0] = b;
            return b2;
        }
    }
}