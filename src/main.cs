
using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;




namespace HolaMundo
{
    class Program
    {
     
       static string pattern = @"'(.*?)'|\S+";
       

       

       static bool NoInvalid = false;
       static List<string> words_command = new List<string>();

       static string arguments;

       static string command;

       static Dictionary<string, string> types = new Dictionary<string, string>{

            {"echo", "is a shell builtin"},
            {"exit", "is a shell builtin"},
            {"type", "is a shell builtin"},
            {"pwd", "is a shell builtin"},
            {"cd", "is a shell builtin"},


       };
       

        static void Main(string[] args)
        {
           
            
          
              
        
            for (;;)
                {
                Console.Write("$ ");
                command = Console.ReadLine();
                wordToList( command);
                
                arguments= string.Join(" ", words_command[1..]);
                

             


                if(command == "exit 0") break;
                else if(words_command[0]=="echo") echo();
                else if(words_command[0]=="type") type();
                else if(words_command[0]=="pwd") pwd();
                else if(words_command[0]=="cd") cd();
                else runProgram(words_command[0], arguments);

               
                
                }


         
        }

    
    static void wordToList( string word ){  // convierte el texto a lista de words
         words_command.Clear();

         bool singlequoting = false;

         
         string wordfinal = "";
        
        for (int i = 0; i < word.Length; i++)
        {
            if(singlequoting==true && word[i]=='\''){
                
                singlequoting = false;
                

            }
            else if(singlequoting ==false && word[i]=='\''){
                singlequoting = true;
            }           
            else if(singlequoting==true)
            {
                if(word[i]=='\\'){
                   // wordfinal += '\\';
                    wordfinal += word[i];
                }
                else{
                    wordfinal += word[i];
                }
                
            }
            else if(singlequoting==false && word[i]!= ' '){

                wordfinal += word[i];
                

            }
             if(singlequoting ==false && (word[i]==' ' || i == word.Length -1  )&& wordfinal.Length>0){
                words_command.Add(wordfinal);
                
                wordfinal = "";

            }
            
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

        Console.WriteLine($"{words_command[0]}: command not found");
        


    } 

    static void type(){

        NoInvalid = true;

        try
        {
            if(types.ContainsKey(words_command[1]) ){
            Console.WriteLine($"{words_command[1]} is a shell builtin");

            }
        }
        catch (System.Exception)
        {
            
            try
            {
                Console.WriteLine(searchExeInPath(words_command[1]));  //regresa lo que se encontro
            }
            catch (System.Exception)
            {
            
            
        }
        }
        
         

        
            
        
        
       

    }

    static string searchExeInPath(string exe){  //busca el ejecutable el path
        
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

    static void runProgram(string name, string argument){


        
        try
        {
            
            ProcessStartInfo startinfo = new ProcessStartInfo();
            startinfo.FileName = name;
            
            startinfo.Arguments = argument;

            Process process = Process.Start(startinfo);
            process.WaitForExit();

        }
        catch (System.Exception)
        {
            
            wrongCommand();
        }

    }

    static void pwd(){

        
        Console.WriteLine(Directory.GetCurrentDirectory());
        

    }

    static int cd(){

        if(words_command.Count()==1)return 0;

        if(words_command[1]=="~"){

            Directory.SetCurrentDirectory(Environment.GetEnvironmentVariable("HOME"));
        }

        else
        {
            try
                {
                Directory.SetCurrentDirectory(words_command[1]);

                }
            catch (System.Exception)
                {
                
                Console.WriteLine($"cd: {words_command[1]}: No such file or directory");
                
                }
        
        }
        return 1;

    }


    }
}
        
        
        
        
        
        
        
        
        
       

    











