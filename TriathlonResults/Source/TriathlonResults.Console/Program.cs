using System;
using System.Collections.Generic;
using System.Text;
using sys = System;
using System.Configuration;
using System.IO;
using TriathlonResults.Console.Properties;
using TriathlonResults.Central.ServiceRequests;
using TriathlonResults.Central.ServiceRequests.Entities;
using entities = TriathlonResults.Entities;

namespace TriathlonResults.Console
{
    /// <summary>
    /// Console app submitting sector result to flat file
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                ResetScreen();
                sys.Console.WriteLine("1. To enter athlete's sector time");
                sys.Console.WriteLine(string.Empty);
                sys.Console.WriteLine("2. Exit");
                sys.Console.WriteLine(string.Empty);
                string selection = sys.Console.ReadLine();
                switch (selection)
                {
                    case (Selection.EnterResult):
                        GetResult();
                        break;
                    case (Selection.Exit):
                        exit = true;
                        break;                       
                }
            }
        }

        private static void ResetScreen()
        {
            sys.Console.Clear();
            sys.Console.WriteLine(string.Empty);
            sys.Console.WriteLine("* Triathlon Swim Sector Timer *");
            sys.Console.WriteLine(string.Empty);
            sys.Console.WriteLine(string.Empty);
        }

        private static void GetResult()
        {
            //initialise result entity:
            SectorTime result = new SectorTime();
            result.SectorId = entities.Sector.Swim;

            //capture & validate values:
            ResetScreen();
            sys.Console.WriteLine("Enter Race Id:");
            result.RaceId = ValidateInt(sys.Console.ReadLine());
            sys.Console.WriteLine("Enter Athlete Id:");
            result.AthleteId = ValidateInt(sys.Console.ReadLine());
            sys.Console.WriteLine("Enter Sector Time (format mm.ss):");
            result.Duration = ValidateDuration(sys.Console.ReadLine());

            //output result to flat file or ESB:
            if (Settings.Default.UseESB)
            {
                SubmitResult(result);
            }
            else
            {
                WriteOutput(result);
            }
        }

        private static void SubmitResult(SectorTime time)
        {
            SectorTime esbResult = new SectorTime();
            esbResult.AthleteId = time.AthleteId;
            esbResult.RaceId = time.RaceId;
            esbResult.SectorId = time.SectorId;
            esbResult.StartTime = time.StartTime;
            esbResult.EndTime = time.EndTime;

            RecordResult serviceRequest = new RecordResult();
            serviceRequest.Request.result = esbResult;
            bool sucess = serviceRequest.Response.RecordResultResult;
        }

        private static void WriteOutput(SectorTime result)
        {
            StringBuilder contentBuilder = new StringBuilder();
            contentBuilder.AppendFormat("{0}{1}", result.RaceId, OutputFileConfiguration.Current.FieldDelimiter);
            contentBuilder.AppendFormat("{0}{1}", result.SectorId, OutputFileConfiguration.Current.FieldDelimiter);
            contentBuilder.AppendFormat("{0}{1}", result.AthleteId, OutputFileConfiguration.Current.FieldDelimiter);
            //times not captured:
            contentBuilder.AppendFormat("{0}{1}", DateTime.MinValue.ToString("yyyy-MM-ddTHH:mm:ss"), OutputFileConfiguration.Current.FieldDelimiter);
            contentBuilder.AppendFormat("{0}{1}", DateTime.MinValue.ToString("yyyy-MM-ddTHH:mm:ss"), OutputFileConfiguration.Current.FieldDelimiter);
            contentBuilder.AppendFormat("{0}{1}", result.Duration, OutputFileConfiguration.Current.FieldDelimiter);
            
            string fileName = string.Format(OutputFileConfiguration.Current.FileNameFormat, result.RaceId, result.SectorId, result.AthleteId);
            string filePath = Path.Combine(OutputFileConfiguration.Current.Path, fileName);
            File.WriteAllText(filePath, contentBuilder.ToString().TrimEnd(OutputFileConfiguration.Current.FieldDelimiter.ToCharArray()));
        }

        private static int ValidateDuration(string input)
        {
            int output = -1;
            //strip minutes & seconds:
            string[] inputParts = input.Split('.');
            if (inputParts.Length != 2)
            {
                sys.Console.WriteLine("Invalid format, expected mm.ss (e.g. 20.40): {0}", input);
            }
            else
            {
                int minutes = ValidateInt(inputParts[0]);
                int seconds = ValidateInt(inputParts[1]);
                if (minutes != -1 && seconds != -1)
                {
                    output = (minutes * 60) + seconds;
                }
            }
            return output;
        }

        private static int ValidateInt(string input)
        {
            int output = -1;
            if (!int.TryParse(input, out output))
            {
                sys.Console.WriteLine("Invalid number: {0}", input);
            }
            return output;
        }

        private struct Selection
        {
            public const string EnterResult = "1";
            public const string Exit = "2";
        }
    }
}
