using OL.Extensions;
using System;
using UMKBNeedStuff;

namespace API.Models
{
	public class FormFile
		: FormData
	{
		private string _fileName;

		public string FileName { get { return (_fileName); } }
		public string LocalFileName { get; private set; }

		public FormFile(string name, string filename, string localFileName)
			: base(name)
		{
			_fileName = filename;
			LocalFileName = localFileName;
		}

		public override int GetHashCode()
		{
			return (HashCodeHelper.Combine(base.GetHashCode(), _fileName.GetHashCode()));
		}

		public override bool Equals(object obj)
		{
			if (!base.Equals(obj))
				return (false);

			FormFile ff = obj as FormFile;
			return ((ff != null) && (ff._fileName == _fileName));
		}

		public override string ToString()
		{
			return (String.Format("{0} = {1}", Name, _fileName));
		}
	}
}