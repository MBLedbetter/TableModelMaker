using ExcelLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TableModelMaker.Services
{
    public class TableRowService
    {
        #region Properties

        private ObservableCollection<KeyValuePair<string, string>> Results { get; set; }

        public string ReturnValue { get; set; }

        public WorkbookReader WorkbookReader { get; private set; }

        #endregion // Properties

        #region Public Methods

        /// <summary>
        ///     Adds the comment box above the property with the collumn name
        /// </summary>
        /// <param name="value">table collumn name</param>
        /// <returns></returns>
        public string AddComment(string value)
        {
            string result = "";
            result += Environment.NewLine;
            result += "/// <summary>";
            result += Environment.NewLine;
            result += "///     Gets or sets " + value;
            result += Environment.NewLine;
            result += "/// </summary>";
            result += Environment.NewLine;
            return result;
        }

        public string AddGetSet()
        {
            return " { get; set; }" + Environment.NewLine; ;
        }

        public string AddPublicType(string value)
        {
            string result = "public ";

            switch(value)
            {
                case "char":
                    result += "varchar ";
                    break;
                case "decimal":
                    result += "decimal ";
                    break;
                case "int":
                    result += "int ";
                    break;
                case "smallint":
                    result += "short ";
                    break;
                case "PSDATE":
                    result += "DateTime ";
                    break;
                case "PSDATETIME":
                    result += "DateTime ";
                    break;
                case "varchar":
                    result += "varchar ";
                    break;
            }

            return result;
        }

        /// <summary>
        ///     Creates a Property name form the collumn name by removing '_' and capitalizing
        /// </summary>
        /// <param name="value">collumn name</param>
        /// <returns></returns>
        public string CreatePropertyName(string value)
        {
            string temp = value.Replace("_", " ");
            temp = temp.ToLower();
            string result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(temp);
            result = result.Replace(" ", "");
            return result;
        }
        
        /// <summary>
        ///     Loads in rows from excel sheet and returns them as a keyvalue pair
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public void LoadRows(string fileName)
        {
            WorksheetData worksheetData = this.WorkbookReader.ReadWorksheet(fileName);
            for (int row = 0; row < worksheetData.CellData.Count; row++)
            {
                string Key = worksheetData.GetValue(row, "Type").Trim();
                string Value = worksheetData.GetValue(row, "Value").Trim();
                KeyValuePair<string, string> Line = new KeyValuePair<string, string>(Key,Value);
                Results.Add(Line);
            }
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string ProcessRows(string filename)
        {
            LoadRows(filename);
            foreach (KeyValuePair<string, string> row in Results)
            {
                string PropertyText = string.Empty;
                PropertyText += AddComment(row.Value);
                PropertyText += AddPublicType(row.Key);
                PropertyText += CreatePropertyName(row.Value);
                PropertyText += AddGetSet();
                ReturnValue += PropertyText;
            }
            return ReturnValue;
        }
        
        #endregion // Public Methods

        #region Constructor

        public TableRowService()
        {
            WorkbookReader = new WorkbookReader();
            Results = new ObservableCollection<KeyValuePair<string, string>>();
        }

        #endregion // Constructor
    }
}
