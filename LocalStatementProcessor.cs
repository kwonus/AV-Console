﻿using Blueprint.Blue;
using Pinshot.PEG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AVConsole
{
    internal class LocalStatementProcessor
    {
        public LocalStatementProcessor() { }

        public void ProcessStatement(QStatement stmt, string result)
        {
            if (stmt != null)
            {
                if (stmt.Singleton != null)
                {
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        Console.WriteLine(result);
                    }
                    if (stmt.Singleton.GetType() == typeof(QAbsorb))
                    {

                    }
                    else if (stmt.Singleton.GetType() == typeof(QDelete))
                    {

                    }
                    else if (stmt.Singleton.GetType() == typeof(QExit))
                    {

                    }
                    else if (stmt.Singleton.GetType() == typeof(QExpand))
                    {

                    }
                    else if (stmt.Singleton.GetType() == typeof(QGet))
                    {

                    }
                    else if (stmt.Singleton.GetType() == typeof(QHelp))
                    {

                    }
                    else if (stmt.Singleton.GetType() == typeof(QInit))
                    {

                    }
                    else if (stmt.Singleton.GetType() == typeof(QReset))
                    {

                    }
                    else if (stmt.Singleton.GetType() == typeof(QReview))
                    {

                    }
                    else if (stmt.Singleton.GetType() == typeof(QVersion))
                    {

                    }
                    else
                    {
                        Console.Error.WriteLine("Unexpected statement type encountered.");
                    }
                }
                else if (stmt.Commands != null)
                {
                    foreach (var segment in stmt.Commands.Assignments)
                    {
                        if (segment.Persistent)
                        {
                            Console.Error.WriteLine("TO DO: variable assignment login goes here.");
                        }
                    }
                }
            }
        }
    }
}
