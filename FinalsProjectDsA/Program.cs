using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FinalsProjectDsA
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] sourceLett = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            char[] otherChars = new char[] { ' ', ',', '.', '/', ';', '\'', '[', ']', '<', '>', '?', ':', '"', '{', '}', '|', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '+', '=', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' }; 
            string[] shiftLett = new string[26];
            string[,] mainLett = new string[26, 26];

            string choice = "";
            string keyword = "";
            string keymessage = "";
            string message = ""; 
            bool pass = false;
            bool finish = false; 


            for (int x = 0; x < mainLett.GetLength(0); x++)
            {
                //shifting part
                if (x >= 1)
                {
                    int temp = shiftLett.Length - x;
                    for (int z = 0; z < x; z++)
                    {
                        shiftLett[temp] = sourceLett[z];
                        temp++;
                    }


                    for (int w = 0; w < shiftLett.Length - x; w++)
                    {
                        shiftLett[w] = sourceLett[w + x];
                    }
                }
                else
                {
                    for (int a = 0; a < sourceLett.Length; a++)
                    {
                        shiftLett[a] = sourceLett[a];
                    }
                }

                //Table part
                for (int y = 0; y < mainLett.GetLength(1); y++)
                {
                    mainLett[x, y] = shiftLett[y];
                    Console.Write(mainLett[x,y] + " ");
                    
                }
                Console.Write("\n");
                Console.ReadKey();
            }

            while (!pass)
            {
                Console.WriteLine("Welcome! Will you Encrypt(E) or De-crypt(D)");
                choice = Console.ReadLine();

                //ENCRYPTION PART
                if (choice == "e" || choice == "E")
                {
                    Console.WriteLine("Please input your keyword:");
                    keyword = Console.ReadLine().ToUpper();
                    Console.WriteLine("Please input your message:");
                    message = Console.ReadLine().ToUpper();

                    //to remove unnecessary characters from the key
                    string[] enkeyword = new string[keyword.Length];
                    using (StreamWriter sw = new StreamWriter("Key.txt"))
                    {
                        for(int i = 0; i < keyword.Length; i++)
                        {
                            for(int j = 0; j < sourceLett.Length; j++)
                            {
                                if (keyword[i].ToString() == sourceLett[j])
                                {
                                    enkeyword[i] = keyword[i].ToString();
                                }
                               
                            }
                        }
                        foreach (string k in enkeyword)
                            sw.Write(k);
                    }

                    //to remove unnecessary characters from the message
                    string[] bmessage = new string[message.Length];
                    using (StreamWriter sw = new StreamWriter("Keymessage.txt"))
                    {
                        for (int i = 0; i < message.Length; i++)
                        {
                            for (int j = 0; j < sourceLett.Length; j++)
                            {
                                if (message[i].ToString() == sourceLett[j])
                                {
                                    bmessage[i] = message[i].ToString();
                                }

                            }
                        }
                        foreach (string k in bmessage)
                            sw.Write(k);
                    }


                    using (StreamReader sr = new StreamReader("Key.txt"))
                    {
                        string scanned = "";          
                        {
                            while ((scanned = sr.ReadLine()) != null)
                            {
                                keyword = scanned;
                            }
                        }
                    }

                    using (StreamReader sr = new StreamReader("Keymessage.txt"))
                    {
                        string scanned = "";
                        {
                            while ((scanned = sr.ReadLine()) != null)
                            {
                                keymessage = scanned;
                            }
                        }
                    }
                    string[] enmessage = new string[keymessage.Length]; 
                    string[] keymask = new string[keymessage.Length];

                    int b = 0;

                    for (int a = 0; a < keymessage.Length; a++)
                    {
                        if (b == keyword.Length)
                        {
                            b = 0;
                        }
                        keymask[a] = keyword[b].ToString();

                        b++;
                    }

                    //keymask to x 
                    //message to y
                    int xside = 0;
                    int yside = 0;
                    for (int z = 0; z < keymessage.Length; z++)
                    {
                        for (int e = 0; e < sourceLett.Length; e++)
                        {
                            if (keymask[z] == sourceLett[e])
                            {
                                xside = e;
                            }
                            if (keymessage[z].ToString() == sourceLett[e])
                            {
                                yside = e;
                            }
                        }      
                        enmessage[z] = mainLett[xside, yside];
                    }

                    using (StreamWriter sw = new StreamWriter("EncryptedMessage.txt"))
                    {
                        foreach (string m in enmessage)
                            sw.Write(m);
                    }
                    Console.Write("Message encrypted");
                    Console.ReadKey();

                }

                //DE-ENCRYPTION PART
                else if (choice == "d" || choice == "D")
                {
                    if (File.Exists("EncryptedMessage.txt"))
                    {
                        using (StreamReader sr = new StreamReader("Key.txt"))
                        {
                            string scanned = "";
                            {
                                while ((scanned = sr.ReadLine()) != null)
                                {
                                    keyword = scanned;
                                }
                            }
                        }

                        using (StreamReader sr = new StreamReader("EncryptedMessage.txt"))
                        {
                            string scanned = "";
                            {
                                while ((scanned = sr.ReadLine()) != null)
                                {
                                    keymessage = scanned;
                                }
                            }
                        }

                        string[] enmessage = new string[keymessage.Length];
                        string[] keymask = new string[keymessage.Length];
                        int b = 0;

                        for (int a = 0; a < keymessage.Length; a++)
                        {
                            if (b == keyword.Length)
                            {
                                b = 0;
                            }
                            keymask[a] = keyword[b].ToString();

                            b++;
                        }

                        int xside = 0;
                        int yside = 0;
                        for (int z = 0; z < keymessage.Length; z++)
                        {
                            for (int e = 0; e < sourceLett.Length; e++)
                            {
                                if (keymask[z].ToString() == sourceLett[e])
                                {
                                    xside = e;
                                    break;
                                }
                            }
                            for (int f = 0; f < sourceLett.Length; f++)
                            {
                                if (keymessage[z].ToString() == mainLett[xside, f])
                                {
                                    yside = f;
                                    break;
                                }
                            }
                            enmessage[z] = mainLett[xside - xside, yside];

                        }
                        using (StreamWriter sw = new StreamWriter("DecryptedMessage.txt"))
                        {
                            foreach (string m in enmessage)
                                sw.Write(m);
                        }
                        Console.Write("Message Decrypted");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Please input the message to be decoded:");
                        message = Console.ReadLine().ToUpper();

                        using (StreamReader sr = new StreamReader("Key.txt"))
                        {
                            string scanned = "";
                            {
                                while ((scanned = sr.ReadLine()) != null)
                                {
                                    keyword = scanned;
                                }
                            }
                        }

                        string[] enmessage = new string[message.Length];
                        string[] keymask = new string[message.Length];
                        int b = 0;

                        for (int a = 0; a < message.Length; a++)
                        {
                            if (b == keyword.Length)
                            {
                                b = 0;
                            }
                            keymask[a] = keyword[b].ToString();

                            b++;
                        }

                        int xside = 0;
                        int yside = 0;
                        for (int z = 0; z < message.Length; z++)
                        {
                            for (int e = 0; e < sourceLett.Length; e++)
                            {
                                if (keymask[z].ToString() == sourceLett[e])
                                {
                                    xside = e;
                                    break;
                                }
                            }
                            for (int f = 0; f < sourceLett.Length; f++)
                            {
                                if (message[z].ToString() == mainLett[xside, f])
                                {
                                    yside = f;
                                    break;
                                }
                            }
                            enmessage[z] = mainLett[xside - xside, yside];

                        }
                        using (StreamWriter sw = new StreamWriter("DecryptedMessage.txt"))
                        {
                            foreach (string m in enmessage)
                                sw.Write(m);
                        }
                        Console.Write("Message Decrypted");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.Write("Please choose properly! Press any key.");
                    Console.ReadKey();
                    Console.Clear();
                }

                keyword = "";
                choice = "";
                message = "";
                Console.Clear();
            }              

        }
    }
}
