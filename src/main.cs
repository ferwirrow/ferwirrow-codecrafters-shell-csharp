using System.Net;
using System.Net.Sockets;

string command = "";
bool invalid = false;






for (;;)
{
    Console.Write("$ ");
    command = Console.ReadLine();
    if(command == "exit 0") break;
    if(invalid== false) Console.WriteLine($"{command}: command not found");
}
