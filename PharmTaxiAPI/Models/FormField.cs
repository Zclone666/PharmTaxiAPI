using OL.Extensions;
using System;
using UMKBNeedStuff;

namespace API.Models
{
	public class FormField
		: FormData
	{
		private string _value;

		public string Value { get { return (_value); } }

		public FormField(string name, string value)
			: base(name)
		{
			_value = value;
		}

		public override int GetHashCode()
		{
			return(HashCodeHelper.Combine(base.GetHashCode(), _value.GetHashCode()));
		}

		public override bool Equals(object obj)
		{
			if (!base.Equals(obj))
				return (false);

			FormField ff = obj as FormField;
			return ((ff != null) && (ff._value == _value));
		}

		public override string ToString()
		{
			return (String.Format("{0} = {1}", Name, _value));
		}
	}
}