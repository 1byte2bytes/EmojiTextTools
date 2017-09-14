using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
                bs.SetLength(1024*10);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var bits = new BitArray(line.Length*8);

                    int bitcount = 0;
                    
                    foreach (char emoji in line)
                    {
                        if (emoji.ToString() == "😂")
                        {
                            bits.Set(bitcount, true);
                        }
                        else
                        {
                            bits.Set(bitcount, false);
                        }
                        
                        bitcount++;
                    }
                    
                    using (var stream = new FileStream(args[1], FileMode.Append))
                    {
                        byte[] newData = BitArrayToByteArray(bits);
                        char[] result = Encoding.UTF8.GetString(newData).ToCharArray();
                        stream.Write(Encoding.Default.GetBytes(result), 0, result.Length);
                    }
                }
            }
        }
        
        public static byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }
    }
}