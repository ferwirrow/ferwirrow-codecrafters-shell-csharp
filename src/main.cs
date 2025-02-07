
using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;




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
                else runProgram(words_command[0]);

            
                
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

        if(types.ContainsKey(words_command[1]) ){
            Console.WriteLine($"{words_command[1]} is a shell builtin");

        }
        else Console.WriteLine(searchExeInPath(words_command[1]));

        
            
        
        
       

    }

    static string searchExeInPath(string exe){
        
        string resultado = $"{exe}: not found";
        string path = Environment.GetEnvironmentVariable("PATH");
        string [] patharray = path.Split(":");
        foreach (string path1 in patharray)
        {
            if(File.Exists($@"{path1}/{exe}")){
                resultado = $@"{path1}/{exe}";
            }  //confirma si existe el nombre del archivo en un nombre
        }
        
        NoInvalid = false;

        return resultado;



    }

    static void runProgram(string name){


        
        try
        {
            Process.Start(searchExeInPath(name));
            Console.WriteLine();
        }
        catch (System.Exception)
        {
            
            wrongCommand();
        }

    }


    }
}
        
        
        
        
        
        
        
        
        
       

    











