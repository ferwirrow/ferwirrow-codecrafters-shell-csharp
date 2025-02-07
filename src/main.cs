using System.Net;
using System.Net.Sockets;

string command;
bool invalid = false;

//Uncomment this line to pass the first stage
Console.Write("$ ");

// Wait for user input
command = Console.ReadLine();



if(invalid== false) Console.WriteLine($"{command}: command not found");
