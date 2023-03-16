using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.IO;

namespace CP_Project2
{
    public class Functions
    {


        public void PrintMenu(int yearStart, int yearEnd)
        {
            Console.Write(
                "\n            MainMenu\n" +
                "---------------------------------------------------------------\n" +
                "1. Select range of years (Max 5) to report on (Current years: "  + yearStart + " - " + yearEnd + ")\n" +
                "2. Generate report for selected region\n" +
                "3. Generate report for all regions\n" +
                "0. EXIT");
        }//end of PrintMenu

        public int GetValidInput()
        {
            int validInput = 0;
            bool getData = true;

            while (getData)
            {
                Console.Write("\n> ");
                bool goodInput = int.TryParse(Console.ReadLine(), out validInput);
                if (goodInput)
                {
                    getData = false;
                }
                else
                {
                    Console.Write("Error: Entry not an Integer.");
                }
            }

            return validInput;

        }//end of GetValidInput 

        public int GetValidYear()
        {
            int validYear = 0;
            int input = 0;
            bool getYear = true;

            while (getYear)
            {
                input = GetValidInput();

                if (input >= 1970 && input <= 2021)
                {
                    validYear = input;
                    getYear = false;
                }//end of get years
                else
                {
                    Console.WriteLine("Error: Input was not a valid year. Please select a year between 1970 and 2021.");
                }
            }

            return validYear;

        }//end of GetValidYear

        //========================================================================================================================================
        // === COUNTRY FUNCTIONS ============================================================================= COUNTRY FUNCTIONS =================
        //========================================================================================================================================

        public List<string> getMetrics(XPathNavigator nav)//(XmlDocument doc)
        {
            List<string> metrics = new List<string>();
            int i = 1;
           // XmlNode node = doc.GetElementById("//labels/inflation/@consumer_prices_percent");// .Select("//labels/inflation/@consumer_prices_percent");

            XPathNodeIterator  nodeIt = nav.Select("//labels/inflation/@consumer_prices_percent");
            nodeIt.MoveNext();
            metrics.Add(nodeIt.Current.Value);

            nodeIt = nav.Select("//labels/inflation/@gdp_deflator_percent");
            nodeIt.MoveNext();
            metrics.Add(nodeIt.Current.Value);

            nodeIt = nav.Select("//labels/interest/@real");
            nodeIt.MoveNext();
            metrics.Add(nodeIt.Current.Value);

            nodeIt = nav.Select("//labels/interest/@lending");
            nodeIt.MoveNext();
            metrics.Add(nodeIt.Current.Value);

            nodeIt = nav.Select("//labels/interest/@deposit");
            nodeIt.MoveNext();
            metrics.Add(nodeIt.Current.Value);

            nodeIt = nav.Select("//labels/unemployment/@national_estimate");
            nodeIt.MoveNext();
            metrics.Add(nodeIt.Current.Value);

            nodeIt = nav.Select("//labels/unemployment/@modeled_ILO_estimate");
            nodeIt.MoveNext();
            metrics.Add(nodeIt.Current.Value);

            return metrics;
        }//end of getMetrics


        public void listAllCountries(XPathNavigator nav)
        {
            XPathNodeIterator nodeIt = nav.Select("//region/@rname");
            int i = 1;

            while (nodeIt.MoveNext() && nodeIt.Current is not null)
            {
                Console.WriteLine(i + ". " +  nodeIt.Current.Value);
                i++;
            }
            
        }

        public int getLongestName(XPathNavigator nav)
        {
            int length = 0;
            string temp = "";
            XPathNodeIterator nodeIt = nav.Select("//region/@rname");
            int i = 1;

            while (nodeIt.MoveNext() && nodeIt.Current is not null)
            {
                temp = nodeIt.Current.Value;
                if(temp.Length > length)
                {
                    length = temp.Length;
                }    
            }

            return length;
        }

        public void getCountryData(XPathNavigator nav, int country, int yearStart, int yearEnd)
        {
            int i =1;
            string countryString = "";
            int currentYear = yearStart;
            List<string> infCPI     = new List<string>();
            List<string> infGDP     = new List<string>();
            List<string> lendInt    = new List<string>();
            List<string> realInt    = new List<string>();
            List<string> depInt     = new List<string>();
            List<string> unempNTL   = new List<string>();
            List<string> unempIPO   = new List<string>();

            XPathNodeIterator nodeIt = nav.Select("//region/@rname");
            XPathNodeIterator yearIterator;
            while (nodeIt.MoveNext() && i<country)
            {
                i++;
            }

            countryString = nodeIt.Current.Value;
            string query = $"//region[@rname = \"{countryString}\"]/year[@yid={currentYear}]";// $"//region[@rname = '{ countryString}']/year/@yid";
            nodeIt = nav.Select(query);

            i = 0;
            while (nodeIt.MoveNext() && nodeIt.Current is not null)
            {
                //currentYear = Int16.Parse(nodeIt.Current.Value);
                for (currentYear = yearStart; currentYear <= yearEnd; currentYear++) //if (currentYear >= yearStart)
                {
                    query = $"//region[@rname = \"{countryString}\"]/year[@yid={currentYear}]/inflation/@consumer_prices_percent";
                    //        //region[@rname = 'Canada']/year[@yid=2000]/inflation/@consumer_prices_percent
                    yearIterator = nav.Select(query);
                    yearIterator.MoveNext();
                    infCPI.Add(yearIterator.Current.Value);

                    query = $"//region[@rname = \"{countryString}\"]/year[@yid={currentYear}]/inflation/@gdp_deflator_percent";
                    yearIterator = nav.Select(query);
                    yearIterator.MoveNext();
                    infGDP.Add(yearIterator.Current.Value);

                    query = $"//region[@rname = \"{countryString}\"]/year[@yid={currentYear}]/interest_rates/@lending";
                    yearIterator = nav.Select(query);
                    yearIterator.MoveNext();
                    lendInt.Add(yearIterator.Current.Value);

                    query = $"//region[@rname = \"{countryString}\"]/year[@yid={currentYear}]/interest_rates/@real";
                    yearIterator = nav.Select(query);
                    yearIterator.MoveNext();
                    realInt.Add(yearIterator.Current.Value);

                    query = $"//region[@rname = \"{countryString}\"]/year[@yid={currentYear}]/interest_rates/@deposit";
                    yearIterator = nav.Select(query);
                    yearIterator.MoveNext();
                    depInt.Add(yearIterator.Current.Value);

                    query = $"//region[@rname = \"{countryString}\"]/year[@yid={currentYear}]/unemployment_rates/@national_estimate";
                    yearIterator = nav.Select(query);
                    yearIterator.MoveNext();
                    unempNTL.Add(yearIterator.Current.Value);

                    query = $"//region[@rname = \"{countryString}\"]/year[@yid={currentYear}]/unemployment_rates/@modeled_ILO_estimate";
                    yearIterator = nav.Select(query);
                    yearIterator.MoveNext();
                    unempIPO.Add(yearIterator.Current.Value);


                    //}
                    //if (currentYear == yearEnd)
                    //{
                    //    break;
                    //}
                    //i++;
                }

                string InflationCPI = "1. Inflation CPI      \t";
                string InflationGDP = "2. Inflation GDP      \t";
                string RealInterest = "3. Real Interest %    \t";
                string LendingInterest = "4. Lending Interest % \t";
                string DepositInterest = "5. Deposit Interest % \t";
                string UnemploymentNTL = "6. Unemployment NTL % \t";
                string UnemploymentIPO = "7. Unemployment IPO % \t";

                //Console.WriteLine("1. Inflation CPI " +      infCPI   [0] + infCPI   [0] );
                //Console.WriteLine("2. Inflation GDP " +      infGDP   [0] + infGDP   [0] );
                //Console.WriteLine("3. Real Interest % " +    lendInt  [0] + lendInt  [0] );
                //Console.WriteLine("4. Lending Interest % " + realInt  [0] + realInt  [0] );
                //Console.WriteLine("5. Deposit Interest % " + depInt   [0] + depInt   [0] );
                //Console.WriteLine("6. Unemployment NTL % " + unempNTL [0] + unempNTL [0] );
                //Console.WriteLine("Unemployment IPO % " +    unempIPO [0] + unempIPO [0] );

                for (int j = 0; j <= (yearEnd - yearStart); j++)
                {
                    if (infCPI[j] == "RegionRegion") { InflationCPI += ("-\t"); }
                    else { InflationCPI += (infCPI[j] + "\t"); }
                }

                for (int j = 0; j <= (yearEnd - yearStart); j++)
                {
                    if (infGDP[j] == "RegionRegion") { InflationGDP += ("-\t"); }
                    else { InflationGDP += (infGDP[j] + "\t"); }
                }

                for (int j = 0; j <= (yearEnd - yearStart); j++)
                {
                    if (lendInt[j] == "RegionRegion") { LendingInterest += ("-\t"); }
                    else { LendingInterest += (lendInt[j] + "\t"); }
                }

                for (int j = 0; j <= (yearEnd - yearStart); j++)
                {
                    if (realInt[j] == "RegionRegion") { RealInterest += ("-\t"); }
                    else { RealInterest += (realInt[j] + "\t"); }
                }

                for (int j = 0; j <= (yearEnd - yearStart); j++)
                {
                    if (depInt[j] == "RegionRegion") { DepositInterest += ("-\t"); }
                    else { DepositInterest += (depInt[j] + "\t"); }
                }

                for (int j = 0; j <= (yearEnd - yearStart); j++)
                {
                    if (unempNTL[j] == "RegionRegion") { UnemploymentNTL += ("-\t"); }
                    else { UnemploymentNTL += (unempNTL[j] + "\t"); }
                }

                for (int j = 0; j <= (yearEnd - yearStart); j++)
                {
                    if (infCPI[j] == "RegionRegion") { UnemploymentIPO += ("-\t"); }
                    else { UnemploymentIPO += (unempIPO[j] + "\t"); }
                }
                Console.Write("\t\t\t");
                int temp = yearStart;
                for(int j = 0; j<=(yearEnd-yearStart); j++)
                {
                    Console.Write(temp + "\t");
                    temp++;
                }
                Console.WriteLine("\n" + InflationCPI);
                Console.WriteLine(InflationGDP);
                Console.WriteLine(RealInterest);
                Console.WriteLine(LendingInterest);
                Console.WriteLine(DepositInterest);
                Console.WriteLine(UnemploymentNTL);
                Console.WriteLine(UnemploymentIPO);


                //XmlNodeList regionList = doc.GetElementsByTagName("rname");
                //XmlNodeList regionList = doc.GetElementsByTagName("region");
                // XmlNodeList regionList = null;


                //XmlNodeList infoByYear = doc.GetElementsByTagName("year");
                //Console.WriteLine("Number of years: " + regionList.Count );

                //foreach (XmlNode node in regionList)
                //{
                //    if (node.InnerText == country)
                //    {
                //        Console.WriteLine(country + " found. Node name: " + node.Name);
                //        XmlAttributeCollection infoByYear = regionList.Item(i).Attributes;

                //        foreach (XmlAttribute attr in infoByYear)
                //        {
                //            XmlNode attrValue = node.Attributes[attr.Name];
                //        }

                //        break;
                //    }
                //    i++;
                //}

                //for (i = 0; i < (yearEnd - yearStart); i++)
                //{
                //    XmlAttributeCollection infoByYear = regionList.Item(i).Attributes;

                //}
            }
        }//end of GetCountryData


        public void getAllData(XPathNavigator nav, int yearStart, int yearEnd, int metric)
        {
            int i = 0;
            int currentYear = yearStart;
            int yearCounter = 0;
            string query;
            string countryStr;
            string metricSelction = "";
            string output = "";

            string infCPI       = "inflation/@consumer_prices_percent";
            string infGDP       = "inflation/@gdp_deflator_percent";
            string lending      = "interest_rates/@lending";
            string real         = "interest_rates/@real";
            string deposit      = "interest_rates/@deposit";
            string unempNTL         = "unemployment_rates/@national_estimate";
            string unempIPO      = "unemployment_rates/@modeled_ILO_estimate";

            switch(metric)
            {
                case 1:
                    metricSelction = infCPI;
                    break;
                case 2:
                    metricSelction = infGDP  ;
                    break;
                case 3:
                    metricSelction = lending ;
                    break;
                case 4:
                    metricSelction = real    ;
                    break;
                case 5:
                    metricSelction = deposit ;
                    break;
                case 6:
                    metricSelction = unempNTL;
                    break;
                case 7:
                    metricSelction = unempIPO;
                    break;
            }

            List<string> Items = new List<string>();

            List<XmlNode> yearlyInfo = new List<XmlNode>();
            XPathNodeIterator nodeIt = nav.Select("//region/@rname");
            string region = " Region | ";
            string columns = String.Format("{0,55}", region);

            Console.Write(columns);
            yearCounter = yearStart;
            for(int j = 0;j <=(yearEnd-yearStart);j++)
            {
                Console.Write(yearCounter + "\t");
                yearCounter++;
            }
            Console.WriteLine();



            while (nodeIt.MoveNext() && nodeIt.Current is not null)
            {
                
                countryStr = nodeIt.Current.Value;
                output = String.Format("{0,55} | ", countryStr);
                //query = $"//region[@rname = \"{countryStr}\"]/year[@yid={currentYear}]";
                query = $"//region[@rname = \"{countryStr}\"]/year[@yid={currentYear}]";
                //query += metricSelction;
                XPathNodeIterator yearIterator = nav.Select(query);

                //Console.Write(countryStr + "\t\t");
                

                for (currentYear = yearStart; currentYear <= yearEnd;  currentYear++)
                {
                    yearIterator.MoveNext();

                    query = $"//region[@rname = \"{countryStr}\"]/year[@yid={currentYear}]/";
                    query += metricSelction;

                    XPathNodeIterator metricValue = nav.Select(query);
                    metricValue.MoveNext();
                    if (metricValue.Current.Value == "RegionRegion")
                    {
                        output += "-\t";
                    }
                    else { output += metricValue.Current.Value + "\t"; }

                    
                }

                Console.WriteLine(output);
            }


                //XmlNodeList regionList = doc.GetElementsByTagName("region");
                //XmlNodeList infoByYear = doc.GetElementsByTagName("year");
                //Console.WriteLine("Number of years: " + regionList.Count );

                //foreach (XmlNode node in regionList)
                //{
                //    XmlAttributeCollection infoByYear = regionList.Item(i).Attributes;

                //    i++;
                //}

                //for (i = 0; i < (yearEnd - yearStart); i++)
                //{
                //    XmlAttributeCollection infoByYear = regionList.Item(i).Attributes;

                //}
            }//end of GetAllData





    }//end of Fucntion Class
}
