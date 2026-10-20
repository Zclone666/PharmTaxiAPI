
namespace API.Models
{
	public class FormData
	{
		private string _name;

		public string Name { get { return (_name); } }

		public FormData(string name)
		{
			_name = name;
		}

		public override int GetHashCode()
		{
			return (_name.GetHashCode());
		}

		public override bool Equals(object obj)
		{
			FormData fdata = obj as FormData;
			return((fdata != null) && (fdata._name == _name));
		}

		public override string ToString()
		{
			return (_name);
		}
	}
}