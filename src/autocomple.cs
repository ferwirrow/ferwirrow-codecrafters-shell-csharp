public static class autocomplete
    {
       
            public static string autoCompletado(){



                List<string> autoCompleteString = new List<string>
            {
                "echo",
                "exit",
                "type"
            };

            string readLine = "";
            string actualWord = "";

            ConsoleKeyInfo actualLetra;

            
          
            int lastIndex;
                    
            while (true)
            {
            

                actualLetra = Console.ReadKey(true);
                
            
                

                if (actualLetra.Key != ConsoleKey.Tab)
                {
                    Console.Write(actualLetra.KeyChar);
                    readLine += actualLetra.KeyChar;    //guardar cada letra en la string 
                    actualWord += actualLetra.KeyChar;
                }

                if (actualLetra.Key == ConsoleKey.Enter)
                {
                    //Console.WriteLine(); //comment this for solve stage un3
                    
                    
                    break;
                }

                else if(actualLetra.Key == ConsoleKey.Spacebar){

                    
                    actualWord = "";
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
                            
                            break;
                        } 
                        
                        var coincidences = searchCoincidencesInPath(actualWord);
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
                        
                         else Console.Write("\a");    
                         
                    }

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
                        if(nameFile.StartsWith(word))
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