using System.Net;
using System.Net.Sockets;

string command = "";
bool invalid = false;




while (command != "quit()")
{
    Console.Write("$ ");
    command = Console.ReadLine();
    if(invalid== false) Console.WriteLine($"{command}: command not found");
}
