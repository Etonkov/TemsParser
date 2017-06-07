using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemsParser.Models.TemsFileInfo
{
    public class HeaderInfoModel
    {
        public HeaderInfoModel(long headerRowIndex, IEnumerable<ColumnInfoModel> columnInfoList)
        {
            this.ColumnInfoList = columnInfoList;
            this.HeaderRowIndex = headerRowIndex;

        }

        /// <summary>
        /// Index of row header.
        /// </summary>
        public long HeaderRowIndex { get; private set; }

        public IEnumerable<ColumnInfoModel> ColumnInfoList { get; private set; }

    }



}
