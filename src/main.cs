
using System;
using System.Text.RegularExpressions;
using System.IO;




namespace HolaMundo
{
    class Program
    {
     
       static string pattern = @"\w+";
       static bool NoInvalid = false;
       static List<string> words_command = new List<string>();

       static string command;

       static Dictionary<string, string> types = new Dictionary<string, string>{

            {"echo", "is a shell builtin"},
            {"exit", "is a shell builtin"},
            {"type", "is a shell builtin"},


       };
       

        static void Main(string[] args)
        {
           
            
          
              
        
            for (;;)
                {
                Console.Write("$ ");
                command = Console.ReadLine();
                wordToList( command);

                if(command == "exit 0") break;
                else if(words_command[0]=="echo") echo();
                else if(words_command[0]=="type") type();
                else if(NoInvalid==false) wrongCommand();

                Console.WriteLine(Directory.GetCurrentDirectory());

                
                
                
                }


         
        }

    
    static void wordToList( string word ){  // convierte el texto a lista de words

        words_command.Clear();
        MatchCollection matches = Regex.Matches(word, pattern);
        foreach (Match match in matches)
        {
            words_command.Add(match.Value);
        }

    }

    
    static void echo(){

            NoInvalid = true;

            foreach (var word in words_command[1..])
            {
                Console.Write(word+ " ");

            }

            Console.WriteLine();

            
            
        }    

    static void wrongCommand(){

        Console.WriteLine($"{command}: command not found");
        


    } 

    static void type(){

        NoInvalid = true;
        try
        {
            Console.WriteLine(words_command[1] + " "+ types[words_command[1]]);
        }
        catch (System.Exception)
        {
            
            Console.WriteLine(words_command[1] + ": not found");
        }
       

    }


    }
}
        
        
        
        
        
        
        
        
        
       

    











