
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
       static Dictionary<char, string> specialCharacters = new Dictionary<char, string>{

            {'\\', "is a shell builtin"},
            {'$', "is a shell builtin"},
            {'"', "is a shell builtin"},
            {'\n', "is a shell builtin"},
            
            


       };
       

        static void Main(string[] args)
        {
           
            
          
              
        
            for (;;)
                {
                Console.Write("$ ");
                 //command = Console.ReadLine();
                 command = autocomplete.autoCompletado();
                 //Console.WriteLine(command);
                 
                 

             
        
                // Reemplazar todas las secuencias de doble barra invertida por una sola barra invertida
                
                
                wordToList( command);
                //Console.WriteLine(words_command.Count);

                
                
                if (words_command.Count >1)
                {
                    arguments= string.Join(" ", words_command[1..]);
                
                }
               
                
                

             
                if(words_command.Count<1){ //en caso de que no se ingrese texto
                    continue;
                }

                if(words_command[0]=="exit" && words_command[1]=="0") break;
                else if(words_command.Count>=3 && (words_command.Contains(">") || words_command.Contains("1>") || words_command.Contains("1>>") || words_command.Contains(">>"))) stdout();
                else if(words_command.Count>=3 && (words_command.Contains("2>") || words_command.Contains("2>>")) ) stderr();
               else if(words_command[0]=="echo")Console.WriteLine( echo(words_command[1..]));
                else if(words_command[0]=="type") type();
                else if(words_command[0]=="pwd") pwd();
                else if(words_command[0]=="cd") cd();
                else if(words_command[0]=="cat") cat();
                else runProgram(words_command[0], arguments);

                arguments = ""; // vacia la string de argumentos



                //debug
              

               
                
                }


         
        }

    
    static void wordToList( string input ){  // convierte el texto a lista de words
        words_command.Clear();
         
       bool singleQuoting = false;
        bool doubleQuoting = false;
        bool escapeNext = false; // Controla si el siguiente carácter debe ser un carácter literal

        string wordfinal = "";

        for (int i = 0; i < input.Length; i++)
        {
            char current = input[i];

            

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


            // Agregar el carácter al resultado final sin comillas
            if (i<input.Length && singleQuoting==false && doubleQuoting==false )
            {
                if (current=='\\')
                {
                    wordfinal += input[i+1];
                    i += 1;
                    continue;
                }
                else wordfinal += current;
            }
            //agregar letras con doble comillas
            else if (i<input.Length && doubleQuoting == true)
            {
                if (current == '\\' && specialCharacters.ContainsKey(input[i+1]))
                {
                    wordfinal += input[i+1];
                    i +=1;
                    continue;
                }
                else wordfinal += current;
            }
            

            else wordfinal += current;
        }

        // Agregar la última palabra si no está vacía
        if (wordfinal.Length > 0)
        {
            words_command.Add(wordfinal);
        }
       
            
        
           

            
       
    
         
         
              
           //final de la funcion

        
    }
       

static int stdout(){

    List<string> argumentos2 = new List<string>();
    string textoStdout = "";
    string exe = words_command[0];
    string archivo = words_command[words_command.Count -1];

   if (!File.Exists(archivo))
        {
            // Si no existe, crea el archivo
           File.WriteAllText(archivo, "");
            
        }

    foreach (var item in words_command[1..])
    {
        if(item=="1>" || item ==">" || item ==">>" || item =="1>>" ) break;
        argumentos2.Add(item);
    }

    if (words_command[0]=="echo")
    {
        textoStdout = echo(argumentos2);
       

    }

    

    else{

        try
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = exe,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true ,
                    RedirectStandardError = true


                    
                };

                if (argumentos2.Count >=1)
                {
                     foreach (var i in argumentos2)
                        {
                            startInfo.ArgumentList.Add(i);
                            
                        }
                }

                using(Process process = Process.Start(startInfo)){

                    textoStdout = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    

                    process.WaitForExit();

                    if(!string.IsNullOrEmpty(error) ){

                        Console.Write(error);
                        
                        
                        
                        
                    }
                }
        }
        catch (Exception ex )
        {
           
            
        }

    }

      

   
    if ((!string.IsNullOrEmpty(textoStdout) && words_command.Contains(">>") || words_command.Contains("1>>")))
    {
         if (textoStdout[textoStdout.Length-1]!='\n')
        {
            textoStdout += "\n";
        }

        
        try
        {
            File.AppendAllText(archivo, textoStdout );
        }
        catch (Exception ex)
        {
           
            
        }
    }
  

    else if (!string.IsNullOrEmpty(textoStdout))
    {

        if (textoStdout[textoStdout.Length-1]!='\n')
        {
            textoStdout += "\n";
        }
        
       try
       {
         File.WriteAllText(archivo, textoStdout);
       }
       catch (Exception ex)
       {
        
        
       }
        
    }

    return 0;


}

static int stderr(){

    List<string> argumentos = new List<string>();
    string textoStderr;
    string exe = words_command[0];
    string archivo = words_command[words_command.Count -1];
    string error = "";


   if (!File.Exists(archivo))
        {
            // Si no existe, crea el archivo
            File.WriteAllText(archivo, "");
            
        }
    

    foreach (var item in words_command[1..])
    {
        if(item=="2>" || item == "2>>") break;
        argumentos.Add(item);
        
    }

     if (words_command[0]=="echo")
    {
        error = echo(argumentos);

        Console.WriteLine(error);
        return 0;

        if (argumentos.Contains("2>>"))
        {
            File.AppendAllText(archivo, "" );
        }

        else File.WriteAllText(archivo, "");
        

       

    }


    ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = exe,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true ,
                    RedirectStandardError = true
                    
                };

                if (argumentos.Count >=1)
                {
                     foreach (var i in argumentos)
                        {
                            startInfo.ArgumentList.Add(i);
                            
                        }
                }

                using(Process process = Process.Start(startInfo)){

                    textoStderr = process.StandardOutput.ReadToEnd();
                    error = process.StandardError.ReadToEnd();
                    

                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(textoStderr))
                    {
                        Console.Write(textoStderr);
                    }

                   
                }


            


            if (!string.IsNullOrEmpty(error))
    {

        if (error[error.Length-1]!='\n' )
        {
            error += "\n";
        }
        
        if (words_command.Contains("2>>"))
        {
            File.AppendAllText(archivo, error);
            
            
        }

        else File.WriteAllText(archivo, error);
        
    }


    


return 0;

}
          

         
















       
    
    
    static string echo(List<string> lista){

            NoInvalid = true;
            string wordEcho = "";

            

            foreach (string word in lista)
            {
                
                wordEcho += word;
                wordEcho += " ";

            }

            
            return wordEcho;

            
            
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
        string pathEnv = Environment.GetEnvironmentVariable("PATH");
        if (string.IsNullOrEmpty(pathEnv))
            return resultado;

        string[] paths = pathEnv.Split(Path.PathSeparator);
        foreach (string path in paths)
        {
            string fullPath = Path.Combine(path, exe);
            if (File.Exists(fullPath))
                
                return fullPath;
        }

        return resultado;

    }

    static void cat(){

        



        
            
            
            
           runProgramWithCat(); 

        

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

    static void runProgramWithCat(){
        try
        {
           
         

           // string exeCorrected = $"\"{exe}\"";  // Escapamos las comillas correctamente para mantenerlas como literales

        ProcessStartInfo startinfo = new ProcessStartInfo();

        

        startinfo.FileName = "cat";  // Usamos 'sh' para ejecutar el comando
         foreach (var item in words_command[1..])
          {
                startinfo.ArgumentList.Add(item);
          }

          

          
            Process process = Process.Start(startinfo);

            


            process.WaitForExit();

           

           


            
            
            
        }
        catch (Exception ex)
        {
            
           Console.WriteLine(ex.Message);
            
            
        }
    }


    }
}
        
        
        