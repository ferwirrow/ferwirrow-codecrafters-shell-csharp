
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
                 
        
                // Reemplazar todas las secuencias de doble barra invertida por una sola barra invertida
                
                
                wordToList( command);
                
                if (words_command.Count >1)
                {
                    arguments= string.Join(" ", words_command[1..]);
                
                }
                
                

             
                if(words_command.Count<1){ //en caso de que no se ingrese texto
                    continue;
                }

                if(command == "exit 0") break;
                else if(words_command[0]=="echo") echo();
                else if(words_command[0]=="type") type();
                else if(words_command[0]=="pwd") pwd();
                else if(words_command[0]=="cd") cd();
                else if(words_command[0]=="cat") cat();
                else runProgram(words_command[0], arguments);

               
                
                }


         
        }

    
    static void wordToList( string input ){  // convierte el texto a lista de words
        words_command.Clear();
          bool singleQuoting = false;
        bool doubleQuoting = false;
        bool escapeNext = false;

        string wordfinal = "";

        for (int i = 0; i < input.Length; i++)
        {
            char current = input[i];

            // Si estamos manejando un carácter de escape
            if (escapeNext)
            {
                wordfinal += current;  // Agregar el carácter tal cual, sin interpretarlo
                escapeNext = false;
                continue;
            }

            // Si encontramos una barra invertida (\), indicamos que el siguiente carácter es un escape
            if (current == '\\')
            {
                escapeNext = true;
                continue;
            }

            // Manejo de comillas simples
            if (current == '\'' && !doubleQuoting)
            {
                singleQuoting = !singleQuoting;
                continue;
            }

            // Manejo de comillas dobles
            if (current == '"' && !singleQuoting)
            {
                doubleQuoting = !doubleQuoting;
                continue;
            }

            // Manejo de espacios (división de palabras fuera de comillas)
            if (!singleQuoting && !doubleQuoting && char.IsWhiteSpace(current))
            {
                if (wordfinal.Length > 0)
                {
                    words_command.Add(wordfinal);
                    wordfinal = "";
                }
                continue;
            }

            // Agregar el carácter al resultado final
            wordfinal += current;
        }

        // Agregar la última palabra si no está vacía
        if (wordfinal.Length > 0)
        {
            words_command.Add(wordfinal);
        }

        // Simulamos la ejecución del comando, como si fuera un "cat" o "echo"
        
    }
       
          

         
















       
    
    
    static void echo(){

            NoInvalid = true;

            

            foreach (string word in words_command[1..])
            {
                Console.Write(word + " ");

            }

            Console.WriteLine();

            
            
        }    

   

    static void wrongCommand(){

        Console.WriteLine($"{words_command[0]}: command not found");
        


    } 

    static void type(){

        NoInvalid = true;
        

        foreach (var tipo in words_command[1..])
        {
             
            try
        {
            if(types.ContainsKey(tipo) ){
            Console.WriteLine($"{tipo} is a shell builtin");
            

            }
        }
            catch (System.Exception ex)
            {
            
            
             }

             if (!types.ContainsKey(tipo))
             {
                Console.WriteLine(searchExeInPath(tipo)); 
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

    static void cat(){

        



        foreach (var exe in words_command[1..])
        {
            
            
            
           runProgramWithCat(exe); 

        }

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

    static void runProgramWithCat(string exe){
        try
        {
           

           
            ProcessStartInfo startinfo = new ProcessStartInfo();
            startinfo.FileName = "cat";
            startinfo.Arguments = $"\"{exe}\"";
            

            Process process = Process.Start(startinfo);
            process.WaitForExit();
        }
        catch (Exception )
        {
            
            Console.WriteLine($"{exe}: not found");
            
        }
    }


    }
}
        
        
        
        
        
        
        
        
        
       

    











