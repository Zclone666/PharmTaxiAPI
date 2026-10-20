using System;
using System.Reflection;

namespace PharmTaxiAdm.Models.User
{
    public interface IUserRegUMKB
    {
        public string department { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string key { get; set; }
        public string lastName { get; set; }
        public string lpuName { get; set; }
        public string middleName { get; set; }
        public string parent_id { get; set; }
        public string password { get; set; }
        public string roleCode { get; set; }
        public string service { get; set; }

        public object GetVal(string ValN)
        {
            Type myType = this.GetType();
            try
            {
                foreach (MemberInfo mi in myType.GetMembers())
                {
                    if (mi.Name == ValN)
                    {
                        if (mi.MemberType == MemberTypes.Field)
                        {
                            FieldInfo field = myType.GetField(ValN);
                            return (object)field.GetValue(this);
                        }
                        if (mi.MemberType == MemberTypes.Property)
                        {
                            PropertyInfo property = myType.GetProperty(ValN);
                            return (object)property.GetValue(this);
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                return (object)e.Message;
            }
        }
    }
}