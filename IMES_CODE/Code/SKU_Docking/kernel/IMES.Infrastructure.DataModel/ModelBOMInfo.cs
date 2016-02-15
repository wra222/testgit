using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace IMES.DataModel
{
    [Serializable]
    public class ModelBOMInfo
    {
        private int id = 0;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private string material = "";
        private string plant = "";
        private string bom = "";
        private string material_group = "";
        private string item_text = "";
        private string component = "";
        private string valid_from_data = "";
        private string valid_to_data = "";
        private string base_qty = "";
        private string quantity = "";
        private string uom = "";
        private string change_number = "";
        private string alternative_item_group = "";
        private string priority = "";
        private string usabe_probability = "";
        private string item_number = "";
        private string sub_items = "";
        private string editor = "";
        private DateTime cdt = new DateTime();
        private DateTime udt = new DateTime();

        public string Material
        {
            get { return material; }
            set { this.material = value; }
        }

        public string Alternative_item_group
        {
            get { return alternative_item_group; }
            set { this.alternative_item_group = value; }
        }

        public string Base_qty
        {
            get { return base_qty; }
            set { this.base_qty = value; }
        }

        public string Bom
        {
            get { return bom; }
            set { this.bom = value; }
        }

        public DateTime Cdt
        {
            get { return cdt; }
            set { this.cdt = value; }
        }

        public DateTime Udt
        {
            get { return udt; }
            set { this.udt = value; }
        }

        public string Change_number
        {
            get { return change_number; }
            set { this.change_number = value; }
        }

        public string Component
        {
            get { return component; }
            set { this.component = value; }
        }

        public string Editor
        {
            get { return editor; }
            set { this.editor = value; }
        }

        public string Item_number
        {
            get { return item_number; }
            set { this.item_number = value; }
        }

        public string Item_text
        {
            get { return item_text; }
            set { this.item_text = value; }
        }

        public string Material_group
        {
            get { return material_group; }
            set { this.material_group = value; }
        }

        public string Plant
        {
            get { return plant; }
            set { this.plant = value; }
        }

        public string Priority
        {
            get { return priority; }
            set { this.priority = value; }
        }

        public string Quantity
        {
            get { return quantity; }
            set { this.quantity = value; }
        }

        public string Sub_items
        {
            get { return sub_items; }
            set { this.sub_items = value; }
        }

        public string Uom
        {
            get { return uom; }
            set { this.uom = value; }
        }

        public string Usabe_probability
        {
            get { return usabe_probability; }
            set { this.usabe_probability = value; }
        }

        public string Valid_from_data
        {
            get { return valid_from_data; }
            set { this.valid_from_data = value; }
        }

        public string Valid_to_data
        {
            get { return valid_to_data; }
            set { this.valid_to_data = value; }
        }
    }
}
