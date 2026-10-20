using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OL.Extensions
{
	public static class DataSetExtensions
	{
		public static DataTableColumnsIndex BuildColumnsIndex(this DataTable tbl)
		{
			return(new DataTableColumnsIndex(tbl.Columns
				.Cast<DataColumn>()
				.ToDictionary(c => c.ColumnName
					.Replace("\r", String.Empty)
					.Replace('\n', ' '))));
		}
	}

	public class DataTableColumnsIndex
	{
		private Dictionary<string, DataColumn> _index;

		internal DataTableColumnsIndex(Dictionary<string, DataColumn> index)
		{
			_index = index;
		}

		public Dictionary<string, DataColumn> Index
		{
			get
			{
				return (_index);
			}
		}

		public object TryGetValue(DataRow row, string column)
		{
			DataColumn col;
			if (_index.TryGetValue(column, out col))
				return (row[col]);
			else
				return (null);
		}

	}
}
