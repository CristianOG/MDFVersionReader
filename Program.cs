using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFVersion1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter path to .mdf file: ");
            string path = Console.ReadLine();
            Console.WriteLine(string.Empty);
            Console.WriteLine(".mdf version: " + GetDbiVersion(path).ToString());
            Console.ReadKey();
        }

        /// <summary>
        /// Returns the dbi_version property of a SQL Server .mdf file,
        /// indicating the SQL Server version of the database file
        /// </summary>
        /// <param name="anMdfFilename">A SQL Server .mdf filename</param>
        /// <returns>
        /// e.g.
        /// 611 = SQL Server 2005
        /// 655 = SQL Server 2008
        /// 706 = SQL Server 2012
        /// 782 = SQL Server 2014
        /// 851 / 852 = SQL Server 2016
        /// or -1 on error
        /// </returns>
        public static int GetDbiVersion(string anMdfFilename)
        {
            int dbiVersion = -1;
            try
            {
                using (FileStream fs = File.OpenRead(anMdfFilename))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        // Skip pages 0-8 (8 KB each) of the .mdf file,
                        // plus the 96 byte header of page 9 and the
                        // first 4 bytes of the body of page 9,
                        // then read the next 2 bytes
                        br.ReadBytes(9 * 8192 + 96 + 4);
                        byte[] buffer = br.ReadBytes(2);
                        dbiVersion = buffer[0] + 256 * buffer[1];
                    }
                }
            }
            catch
            {
            }

            return dbiVersion;
        }
    }
}
