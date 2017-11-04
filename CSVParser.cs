using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace CSVParser
{
    public class CSVParser
    {
        private FileInfo data = null, metadata = null;

        public void OpenStockData(string company)
        {
            Close();
            string filePathData = String.Format(@"..\{0}-data.csv", company);
            string filePathMetaData = String.Format(@"..\{0}-metadata.csv", company);
            string csvQueryData = String.Format("https://www.quandl.com/api/v3/datasets/WIKI/{0}/data.csv?api_key=-cgVuGXX5HzSwHStGPPc", company);
            string csvQueryMetaData = String.Format("https://www.quandl.com/api/v3/datasets/WIKI/{0}/metadata.csv?api_key=-cgVuGXX5HzSwHStGPPc", company);

            var web = new WebClient();
            
            try
            {
                web.DownloadFile(csvQueryData, filePathData);
                web.DownloadFile(csvQueryMetaData, filePathMetaData);
                Console.WriteLine(filePathData + " : download success");
                Console.WriteLine(filePathMetaData + " : download success");
                data = new FileInfo(filePathData);
                metadata = new FileInfo(filePathMetaData);
            }
            catch (Exception ex)
            {
                Console.WriteLine("File Download Error: " + ex.Message);
            }

            
        }
        public void OpenStockData(string company, string start_date, string end_date)
        {
            Close();
            string filePathData = String.Format(@"C:\Users\JoltA\Documents\Visual Studio 2017\Projects\csvtest\{0}-data.csv", company);
            string filePathMetaData = String.Format(@"C:\Users\JoltA\Documents\Visual Studio 2017\Projects\csvtest\{0}-metadata.csv", company);
            string csvQueryData = String.Format("https://www.quandl.com/api/v3/datasets/WIKI/{0}/data.csv?start_date={1}&end_date={2}&api_key=-cgVuGXX5HzSwHStGPPc", company, start_date, end_date);
            string csvQueryMetaData = String.Format("https://www.quandl.com/api/v3/datasets/WIKI/{0}/metadata.csv?&start_date={1}&end_date={2}&api_key=-cgVuGXX5HzSwHStGPPc", company, start_date, end_date);


            var web = new WebClient();
            try
            {

                web.DownloadFile(csvQueryData, filePathData);
                web.DownloadFile(csvQueryMetaData, filePathMetaData);
                Console.WriteLine(filePathData + " : download success");
                Console.WriteLine(filePathMetaData + " : download success");
                data = new FileInfo(filePathData);
                metadata = new FileInfo(filePathMetaData);
            }
            catch (Exception ex)
            {
                Console.WriteLine("File Download Error: " + ex.Message);
            }
        }
        public void Close()
        {
            data = null;
            metadata = null;
        }
        public double GetMaxValue(int colNum)
        {
            double max = 0;
            string[] fLines = File.ReadAllLines(data.ToString());
            for (int i = 1; i < fLines.Length; i++)
            {
                string[] line = fLines[i].Split(',');
                double parsed = Double.Parse(line[colNum]);
                if (max < parsed)
                {
                    max = parsed;
                }
            }
            return max;
        }
        public double GetPrice(string date, int type)
        {
            string[] fLines = File.ReadAllLines(data.ToString());
            for (int i = 1; i < fLines.Length; i++)
            {
                string[] line = fLines[i].Split(',');
                if (line[0].Equals(date))
                {
                    return Double.Parse(line[type]);
                }
            }
            return -1;
        }
        public ArrayList GetPriceRange(int type, string start_date, string end_date)
        {
            ArrayList selected = new ArrayList();
            string[] fLines = File.ReadAllLines(data.ToString());

            for (int i = 1; i < fLines.Length; i++)
            {
                string[] line = fLines[i].Split(',');
                if (DateTime.Parse(line[0]) > DateTime.Parse(start_date) && DateTime.Parse(line[0]) < DateTime.Parse(end_date))
                {
                    selected.Add(line[type]);
                }
            }
            return selected;
        }
   
        public string GetMetaData(int type)
        {
            string[] fLines = File.ReadAllLines(metadata.ToString());
            string[] meta = fLines[1].Split(',');

            return meta[type];

        }

        public bool IsOpen
        {
            get { return !(data == null); }
        }
        public int Length
        {
            get
            {
                if (this.IsOpen)
                {
                    string[] fLines = File.ReadAllLines(data.ToString());
                    return fLines.Length - 1;
                }
                else
                {
                    return 0;
                }
            }
        }
        
    }
}







