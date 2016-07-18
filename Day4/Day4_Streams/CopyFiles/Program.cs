using System;
using System.IO;
using System.Net;
using System.Text;

namespace FileStreams
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Arguments: <source> <destination>");
                return;
            }

            string source = args[0];
            string destin = args[1];

            ByteCopy(source, destin);
            BlockCopy(source, destin);
            LineCopy(source, destin);
            MemoryBufferCopy(source, destin);
            WebClient();
        }

        public static void ByteCopy(string source, string dest)
        {
            int bytesCounter = 0;

            using (var sourceStream = new FileStream(source, FileMode.Open))
            {
                using (var destStream = new FileStream(dest, FileMode.Create))
                {
                    int b;
                    while((b = sourceStream.ReadByte()) != -1)
                    {
                        bytesCounter++;
                        destStream.WriteByte((byte)b);
                    }
                }
            }


            // TODO: Implement byte-copy here.
            /*
            using (var sourceStream = new FileStream(...))
            using (var destinStream = new FileStream(...))
            {
                int b;
                while ((b = sourceStream....()) != -1) // TODO: read byte
                {
                    bytesCounter++;
                    destinStream....((byte)b); // TODO: write byte
                }
            }
            */
            Console.WriteLine("ByteCopy() done. Total bytes: {0}", bytesCounter);
        }

        public static void BlockCopy(string source, string dest)
        {
            // TODO: Implement block copy via buffer.

            using (var sourceStream = new FileStream(source, FileMode.Open))
            {
                using (var destStream = new FileStream(dest, FileMode.Create))
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = 0;
                    int offset = 0;

                    do
                    {
                        bytesRead = sourceStream.Read(buffer, offset, buffer.Length);
                        Console.WriteLine("BlockCopy(): writing {0} bytes.", bytesRead);
                        offset += bytesRead;
                        destStream.Write(buffer, 0, bytesRead);
                    }
                    while (bytesRead == buffer.Length);
                }
            }
            
            /*using (var sourceStream = new FileStream(...))
            using (var destinStream = new FileStream(...))
            {
                byte[] buffer = new byte[1024];
                int bytesRead = 0;

                do
                {
                    bytesRead = sourceStream.Read(...); // TODO: read in buffer

                    Console.WriteLine("BlockCopy(): writing {0} bytes.", bytesRead);
                    destinStream.Write(...); // TODO: write to buffer
                }
                while (bytesRead == buffer.Length);
            }
            */
        }

        public static void LineCopy(string source, string dest)
        {
            int linesCount = 0;

            using (var sourceStream = new FileStream(source, FileMode.Open))
            using (var destStream = new FileStream(dest, FileMode.Create))
            {
                using (var streamReader = new StreamReader(sourceStream))
                using (var streamWriter = new StreamWriter(destStream))
                {
                    string line;
                    while(true)
                    {
                        linesCount++;
                        if ((line = streamReader.ReadLine()) == null)
                            break;
                        streamWriter.WriteLine(line);
                    }
                }
            }
            


                // TODO: implement copying lines using StreamReader/StreamWriter.
                /*
                using (var sourceStream = new FileStream(...))
                using (var destinStream = new FileStream(...))
                {
                    using (var streamReader = new StreamReader(...))
                    using (var streamWriter = new StreamWriter(...))
                    {

                        string line;
                        while (true)
                        {
                            linesCount++;
                            if ((line = streamReader....()) == null) // TODO: read line
                            {
                                break;
                            }
                            streamWriter....(line); // TODO: write line

                        }
                    }
                }
                */

                Console.WriteLine("LineCopy(): {0} lines.", linesCount);
        }

        public static void MemoryBufferCopy(string source, string dest)
        {
            
            var stringBuilder = new StringBuilder();

            string content;

            using (var textReader = (TextReader)new StreamReader(source)) // TODO: use StreamReader here
            {
                content = textReader.ReadToEnd();// TODO: read entire file
            }

            using (var sourceReader = new StringReader(content)) // TODO: Use StringReader here with content
            using (var sourceWriter = new StringWriter(stringBuilder)) // TODO: Use StringWriter here with stringBuilder
            {
                string line = null;

                do
                {
                    line = sourceReader.ReadLine(); // TODO: read line
                    if (line != null)
                    {
                        sourceWriter.WriteLine(line); // TODO: write line
                    }

                } while (line != null);
            }

            Console.WriteLine("MemoryBufferCopy(): chars in StringBuilder {0}", stringBuilder.Length);

            const int blockSize = 1024;

            using (var stringReader = new StringReader(stringBuilder.ToString())) // TODO: Use StringReader to read from stringBuilder.
            using (var memoryStream = new MemoryStream(blockSize))
            using (var streamWriter = new StreamWriter(memoryStream)) // TODO: Compose StreamWriter with memory stream.
            using (var destStream = new FileStream(dest, FileMode.Create)) // TODO: Use file stream.
            {
                char[] buffer = new char[blockSize];
                int bytesRead;

                do
                {
                    bytesRead = stringReader.Read(buffer, 0, buffer.Length); // TODO: Read block from stringReader to buffer.
                    streamWriter.Write(buffer); // TODO: Write buffer to streamWriter.

                    // TODO: After implementing everythin check the content of NewTextFile. What's wrong with it, and how this may be fixed?

                    destStream.Write(memoryStream.GetBuffer(), 0, memoryStream.GetBuffer().Length); // TODO: write memoryStream.GetBuffer() content to destination stream.
                }
                while (bytesRead == blockSize);
            }

            
        }

        public static void WebClient()
        {
            WebClient webClient = new WebClient();
            using (var stream = webClient.OpenRead("http://google.com"))
            {
                
                Console.WriteLine("WebClient(): CanRead = {0}", stream.CanRead); // TODO: print if it is possible to read from the stream
                Console.WriteLine("WebClient(): CanWrite = {0}", stream.CanWrite); // TODO: print if it is possible to write to the stream
                Console.WriteLine("WebClient(): CanSeek = {0}", stream.CanSeek); // TODO: print if it is possible to seek through the stream

                using (var streamWriter = File.Open("google_request.txt", FileMode.Create))
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = 0;
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        streamWriter.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }
    }
}