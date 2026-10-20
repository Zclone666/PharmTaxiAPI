using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Models
{
    public partial class ModifyPriceItemData : Core.Data.ItemData
    {

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private long QuantityField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal SellingPriceField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public long Quantity
        {
            get
            {
                return this.QuantityField;
            }
            set
            {
                if ((this.QuantityField.Equals(value) != true))
                {
                    this.QuantityField = value;
                    this.RaisePropertyChanged("Quantity");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

    }

    public partial class RemovePriceItemData : Core.Data.ItemData
    {

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CommentField;

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Comment
        {
            get
            {
                return this.CommentField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CommentField, value) != true))
                {
                    this.CommentField = value;
                    this.RaisePropertyChanged("Comment");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

    }


}
