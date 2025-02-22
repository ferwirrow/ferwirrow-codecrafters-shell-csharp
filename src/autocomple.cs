using System.Security.Principal;

public static class autocomplete
    {
       
            public static string autoCompletado(){


                bool builtIn = false;
                List<string> autoCompleteString = new List<string>
            {
                "echo",
                "exit",
                "type"
            };

            string readLine = "";
            string actualWord = "";

            bool tabMultipleMatch = false;
            List<string> coincidences = new List<string>();

            ConsoleKeyInfo actualLetra;

            
          
            int lastIndex;
                    
            while (true)
            {
                builtIn = false;

                actualLetra = Console.ReadKey(true);
                
            
                if (actualLetra.Key == ConsoleKey.Tab && tabMultipleMatch==true) //manage multiples match in exes with double taps
                {
                    Console.WriteLine();
                    foreach (var item in coincidences)
                    {
                        Console.Write(item + "  ");

                    }
                    Console.WriteLine();

                    Console.Write("$ " + readLine);
                    tabMultipleMatch = false;
                    continue;
                }
                

                if (actualLetra.Key != ConsoleKey.Tab)
                {
                    Console.Write(actualLetra.KeyChar);
                    readLine += actualLetra.KeyChar;    //guardar cada letra en la string 
                    actualWord += actualLetra.KeyChar;
                    tabMultipleMatch = false;
                }

                if (actualLetra.Key == ConsoleKey.Enter)
                {
                    //Console.WriteLine(); //comment this for solve stage un3
                    
                    
                    break;
                }

                else if(actualLetra.Key == ConsoleKey.Spacebar){

                    
                    actualWord = "";
                    tabMultipleMatch = false;
                    continue;
                    

                }

                else if(  !string.IsNullOrEmpty(actualWord) && actualLetra.Key == ConsoleKey.Tab){             

                    

                    foreach (var word in autoCompleteString)
                    {
                        if (word.StartsWith(actualWord))
                        {
                            lastIndex = readLine.LastIndexOf(actualWord); 
                            if (lastIndex != -1)
                            {
                                readLine = readLine.Substring(0, lastIndex) + word ;
                            }
                            foreach (var letra in actualWord)
                            {
                                Console.Write('\b');
                            }
                            Console.Write(word);
                            Console.Write(" ");
                            readLine += " ";
                            actualWord = ""; // limpia la cadena actual para no se acumele
                            builtIn = true;
                            break;
                        } 
                    }
                        
                    if (builtIn)
                    {
                        continue;
                    } 
                         coincidences = searchCoincidencesInPath(actualWord);
                        if (coincidences.Count == 1)
                        {
                            string wordcoincidence = coincidences[0];
                            lastIndex = readLine.LastIndexOf(actualWord); 
                            if (lastIndex != -1)
                            {
                                readLine = readLine.Substring(0, lastIndex) + wordcoincidence ;
                            }
                            foreach (var letra in actualWord)
                            {
                                Console.Write('\b');
                            }
                            Console.Write(wordcoincidence);
                            Console.Write(" ");
                            readLine += " ";
                            actualWord = ""; // limpia la cadena actual para no se acumele
                            
                            break;


                        }
                        else if(coincidences.Count >1)
                        {
                            
                            if (coincidences[0].Length == coincidences[1].Length)
                            {
                            tabMultipleMatch = true;
                            Console.Write("\a"); 
                            }
                            else
                            {
                                int index = readLine.LastIndexOf(actualWord);
                                string palabracoincide = coincidences[0];
                                if (index != -1)
                                {
                                    readLine = readLine.Substring(0, index) + palabracoincide ;

                                }
                                foreach (var letra in actualWord)
                                {
                                    Console.Write('\b');
                                }
                                Console.Write(palabracoincide);
                                actualWord = palabracoincide;


                            }
                        }
                        
                         else Console.Write("\a");    
                         
                    // loop for built in funtions

                }
                
            }
        
        return readLine;
        }

        public static List<string> searchCoincidencesInPath(string word){

            string path = Environment.GetEnvironmentVariable("PATH");
            string[] directories = path.Split(':');
            List<string> coincidences = new List<string>();
            string nameFile;

            foreach (var directory in directories)
            {
                if (Directory.Exists(directory))
                {
                    string[] files = Directory.GetFiles(directory);
                    foreach (var file in files)
                    {
                        
                        nameFile = Path.GetFileName(file);
                        if(nameFile.StartsWith(word) && word != "" && !coincidences.Contains(nameFile))
                        {

                            coincidences.Add(nameFile);

                        }
                    }
                }

            }
            coincidences.Sort();
            return coincidences;




        }

    }