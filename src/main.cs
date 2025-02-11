
using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.Text;




namespace HolaMundo
{
    class Program
    {
     
       static string pattern = @"'(.*?)'|\S+";
       
    enum ParseState 
{
    Normal,         // Fuera de cualquier comilla
    Escaped,        // Escapando en modo normal (fuera de comillas)
    InSingleQuote,  // Dentro de comillas simples
    InDoubleQuote,  // Dentro de comillas dobles
    EscapedInDouble // Escapando dentro de comillas dobles
}
       

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
         
    
        var token = new StringBuilder();
    ParseState state = ParseState.Normal;
    
    for (int i = 0; i < input.Length; i++)
    {
        char c = input[i];
        switch (state)
        {
            case ParseState.Normal:
                if (char.IsWhiteSpace(c))
                {
                    // Fin del token si se encontró espacio y se tiene contenido
                    if (token.Length > 0)
                    {
                        words_command.Add(token.ToString());
                        token.Clear();
                    }
                }
                else if (c == '\\')
                {
                    // Activa el modo escapado
                    state = ParseState.Escaped;
                }
                else if (c == '\'')
                {
                    // Entra en comillas simples
                    state = ParseState.InSingleQuote;
                }
                else if (c == '"')
                {
                    // Entra en comillas dobles
                    state = ParseState.InDoubleQuote;
                }
                else
                {
                    token.Append(c);
                }
                break;

            case ParseState.Escaped:
                // Fuera de comillas: el carácter siguiente se añade sin la barra
                token.Append(c);
                state = ParseState.Normal;
                break;

            case ParseState.InSingleQuote:
                if (c == '\'')
                {
                    // Finaliza comillas simples
                    state = ParseState.Normal;
                }
                else
                {
                    // Dentro de comillas simples se toma todo literal
                    token.Append(c);
                }
                break;

            case ParseState.InDoubleQuote:
                if (c == '"')
                {
                    // Finaliza comillas dobles
                    state = ParseState.Normal;
                }
                else if (c == '\\')
                {
                    // En comillas dobles se activa el escape pero con comportamiento especial
                    state = ParseState.EscapedInDouble;
                }
                else
                {
                    token.Append(c);
                }
                break;

            case ParseState.EscapedInDouble:
                // En comillas dobles, solo se "desescapan" ciertos caracteres
                if (c == '"' || c == '$' || c == '\\')
                {
                    // Se añade solo el carácter especial sin la barra
                    token.Append(c);
                }
                else
                {
                    // Si no es uno de los especiales, se conserva la barra
                    token.Append('\\');
                    token.Append(c);
                }
                state = ParseState.InDoubleQuote;
                break;
        }
    }
    
    // Agregar el último token si no está vacío
    if (token.Length > 0)
    {
        words_command.Add(token.ToString());
    }
    

           
       
   
     

 


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
        
        
        
        
        
        
        
        
        
       

    











