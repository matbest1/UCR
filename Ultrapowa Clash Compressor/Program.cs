using SevenZip.SDK;
using System;
using System.IO;

namespace UCC
{
    internal class Program
    {
        // This is the decompression function
        private static void Decompress(string[] args)
        {
            // We call the 7zip decoder
            SevenZip.SDK.Compress.LZMA.Decoder decoder = new SevenZip.SDK.Compress.LZMA.Decoder();
            // We get an array of file list ( All files in args[0] folder, where args[2] is the filter
            string[] filePaths = Directory.GetFiles(args[0], args[2]);
            // Directory exist or not, we create the folder
            Directory.CreateDirectory(args[1]);
            // For each files in the specified folder, we decode it
            foreach (string filePath in filePaths)
            {
                // We get the file properties ( Size, date, owner, etc.. )
                FileInfo f = new FileInfo(filePath);
                // We work on the input file
                using (FileStream input = new FileStream(filePath, FileMode.Open))
                {
                    // We work on the output file
                    using (FileStream output = new FileStream(Path.Combine(args[1], Path.GetFileName(filePath)), FileMode.Create))
                    {
                        // Read the decoder properties
                        byte[] properties = new byte[5];
                        // Take the 5 bytes after offset 0
                        input.Read(properties, 0, 5);
                        // Read in the decompress file size.
                        byte[] fileLengthBytes = new byte[4];
                        // Take 4 bytes after offset 0
                        input.Read(fileLengthBytes, 0, 4);
                        // Convert to Int32
                        int fileLength = BitConverter.ToInt32(fileLengthBytes, 0);
                        // Set the decoder parameters
                        decoder.SetDecoderProperties(properties);
                        // Write the decompressed file in output directory
                        decoder.Code(input, output, input.Length, fileLength, null);
                        // Free
                        output.Flush();
                        // Free
                        output.Close();
                    }
                    // Free
                    input.Close();
                }
            }
        }

        // This is the compression function
        private static void Compress(string[] args)
        {
            // We call the 7zip compressor
            SevenZip.SDK.Compress.LZMA.Encoder coder = new SevenZip.SDK.Compress.LZMA.Encoder();
            // We get all files in directory list, in an array
            string[] filePaths = Directory.GetFiles(args[0], args[2]);
            // Exist or not, we create the folder
            Directory.CreateDirectory(args[1]);
            // For each files in directory
            foreach (string filePath in filePaths)
            {
                // We get file property ( Size, Owner, Data, ... )
                FileInfo f = new FileInfo(filePath);
                // We work on the inputed files
                using (FileStream input = new FileStream(filePath, FileMode.Open))
                {
                    // We work on the outputed files
                    using (FileStream output = new FileStream(Path.Combine(args[1], Path.GetFileName(filePath)), FileMode.Create))
                    {
                        // byte[] properties = new byte[5] { 0x5d, 0x00, 0x00, 0x04, 0x00};

                        // We create the 7zip property manager array
                        CoderPropID[] propIDs =
                        {
                            CoderPropID.DictionarySize,
                            CoderPropID.PosStateBits,
                            CoderPropID.LitContextBits,
                            CoderPropID.LitPosBits,
                            CoderPropID.Algorithm,
                            CoderPropID.NumFastBytes,
                            CoderPropID.MatchFinder,
                            CoderPropID.EndMarker
                        };

                        Int32 dictionary = 1 << 18;
                        Int32 posStateBits = 2; // This is the value for skipping/state
                        Int32 litContextBits = 3; // This is the value for normal files
                        // UInt32 litContextBits = 0; // This is the value for 32-bits files
                        Int32 litPosBits = 0; // This is the value for offset
                        // UInt32 litPosBits = 2; // This is the value for 32-biys files
                        Int32 algorithm = 2; // This is the algorithm for compression/decompression, LZMA
                        Int32 numFastBytes = 32; // Unknown
                        string mf = "bt4";
                        bool eos = false;

                        // We store as an object
                        object[] properties =
                        {
                            (Int32)(dictionary),
                            (Int32)(posStateBits),
                            (Int32)(litContextBits),
                            (Int32)(litPosBits),
                            (Int32)(algorithm),
                            (Int32)(numFastBytes),
                            mf,
                            eos
                        };

                        // We set the properties in 7-zip properties manager
                        coder.SetCoderProperties(propIDs, properties);
                        // We write the properties in the outputed files
                        coder.WriteCoderProperties(output);
                        // We write the length at the start of files 
                        output.Write(BitConverter.GetBytes(input.Length), 0, 4);
                        // We write the compressed data in the files
                        coder.Code(input, output, input.Length, -1, null);
                        // Free
                        output.Flush();
                        // Free
                        output.Close();
                    }
                    // Free
                    input.Close();
                }
            }
        }

        private static void Main(string[] args)
        {
            // args[0] is the source directory, where ucc will take files
            // args[1] is the target directory, where ucc will output files
            // args[2] is the filename but can also be a filter, (ex: *.csv, *.sc)
            // args[3] is the caller, if not set, -> compression, if set to '-d', -> Decompression
            // We check if there is 4 arguments
            if (args.Length == 4)
            {
                // If the args[3] is -d, we decompress the files
                if (args[3] == "-d")
                    // We call the decompression function
                    Decompress(new string[] { args[0], args[1], args[2] });
            }
            // We check if there is 3 args
            else if (args.Length == 3)
                // We call the compression function
                Compress(new string[] { args[0], args[1], args[2] });
            else
                // Else, the job is done and program has nothing todo, we quit
                Environment.Exit(0x00);
        }
    }
}