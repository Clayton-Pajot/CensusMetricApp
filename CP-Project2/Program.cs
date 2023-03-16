using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using CP_Project2;


class Program
{ 
    static void Main(String[] args)
    {
        //PROGRAM VARIBLES ========================================================================
        int yearStart = 2016;
        int yearEnd = 2020;
        int input = 0;
        int country;
        int metric;

        bool getInput = true;
        bool getYear = true;

        string XML_FILE = "global_economies.xml";
        XmlDocument doc = new XmlDocument();
        XPathNavigator nav = doc.CreateNavigator()!;

        Functions functions = new Functions();
      

        try
        {
            // Initialize the DOM object
            doc.Load(XML_FILE);
        }
        catch (IOException)
        {
            Console.WriteLine($"Error opening the file {XML_FILE}");
        }
        catch (XmlException ex)
        {
            Console.WriteLine($"XML Error: {ex.Message}");
        }

        

        //functions.getCountryData(doc, "Afghanistan", 2010, 2015);

        //BEGIN PROGRAM AND WRITE MENU ========================================================================
        Console.WriteLine("\n\n======= Census Data Display Program ========================");
        Console.WriteLine(functions.getLongestName(nav));
        List<string> metrics = functions.getMetrics(nav);

        while (getInput)
        {
            functions.PrintMenu(yearStart, yearEnd);
            input = functions.GetValidInput();
            if (input >= 0 && input <= 3)//Process account selection 
            {
                switch (input)
                {
                    case 0: //EXIT =================================================
                        getInput = false;
                        break;

                    case 1: //GET YEARS =================================================
                        getYear = true;
                        while (getYear)
                        {
                            Console.Write("Please enter the starting year: ");
                            yearStart = functions.GetValidYear();
                            Console.Write("Please enter the ending year: ");
                            yearEnd = functions.GetValidYear();
                            if (yearEnd - yearStart > 5)
                            {
                                Console.WriteLine("Start and End years cannot be more than 5 years apart.");
                                yearStart = 0;
                                yearEnd = 0;
                            }
                            else { getYear = false; }
                        }

                        break;

                    case 2:// PROCESS FOR SELECTED COUNTRY =================================================
                        Console.WriteLine("\nPlease select a country: ");
                        functions.listAllCountries(nav);

                        Console.Write("Please enter a region #: ");
                        country = functions.GetValidInput();

                        functions.getCountryData(nav, country, yearStart, yearEnd);

                        break;

                    case 3:// PROCESS FOR ALL COUNTRYS =================================================
                        
                        Console.WriteLine("\nPlease enter the metric to display: ");

                        for(int i = 0; i <metrics.Count; i++)
                        {
                            Console.WriteLine((i + 1) + ". " + metrics[i]);
                        }

                        metric = functions.GetValidInput();

                        functions.getAllData(nav,yearStart,yearEnd,metric);

                        break;

                    default://exit
                        getInput = false;
                        break;
                }
            }
            else
            {
                Console.WriteLine("\nERROR: Please enter an number corresponding to the menu options above.");
            }

            
        }//end of getInput While loop


        Console.WriteLine("\n\n================== END OF PROGRAM ==============================================================================\n");
    }//end of MAIN ===================================================================================================================================
}
