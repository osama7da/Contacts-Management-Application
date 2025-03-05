using ContactsBusinessLayer;
using ContactsDataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContactsProj
{
    public partial class AddEditForm : Form
    {
       public enum enMode { AddNew=0, Edit=1};
        private enMode _Mode;
        int _ContactID;
        clsContact _Contact; 
       
        public AddEditForm()
        {
            InitializeComponent();
        }
        public AddEditForm(int ContactID)
        {
            InitializeComponent();

            _ContactID = ContactID;

            if (_ContactID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Edit;
        }
        private void _FillCountryComboBox()
        {
            DataTable dt = clsCountries.GetAllCountries(); 
            foreach(DataRow row in dt.Rows)
            {
                comboBox1.Items.Add(row["CountryName"]); 
            }
            
        }
        private void _LoadData()
        {
            _FillCountryComboBox();
            comboBox1.SelectedIndex = 0;
            if (_Mode == enMode.AddNew)
            {
                label9.Text = "Add new Contact"; 
                _Contact = new clsContact();
                return;
            }
            _Contact = clsContact.Find(_ContactID); 
            if (_Contact==null)
            {
                MessageBox.Show("Sorry there is no contact with this id");
                this.Close();
                return; 
            }
            label9.Text = "Edit Contact ID " + _ContactID;
            label10.Text = _ContactID.ToString();  
            textBox2.Text = _Contact.FirstName;  
            textBox3.Text = _Contact.LastName;   
            textBox4.Text = _Contact.Email;
            textBox5.Text = _Contact.Phone;
            dateTimePicker1.Value = _Contact.DateOfBirth;
            comboBox1.SelectedIndex = comboBox1.FindString(clsCountries.GetCountriesByID(_Contact.CountryID).CountryName); 
            if (_Contact.ImagePath!="")
            {
                pictureBox1.Load(_Contact.ImagePath); 
            }
            richTextBox1.Text = _Contact.Address.ToString();
            llRemoveImage.Visible = (_Contact.ImagePath !=""); 

        }

     
       

        private void AddEditForm_Load(object sender, EventArgs e)
        {
            _LoadData();
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int CountryID = clsCountries.Find(comboBox1.Text).CountryID;
            _Contact.FirstName = textBox2.Text;
            _Contact.LastName = textBox3.Text;
            _Contact.Email = textBox4.Text;
            _Contact.Phone = textBox5.Text;
            _Contact.DateOfBirth = dateTimePicker1.Value;
            _Contact.CountryID = CountryID;
            _Contact.Address= richTextBox1 .Text;
            if (pictureBox1.ImageLocation!=null)
            { 
                _Contact.ImagePath = pictureBox1.ImageLocation;
            }else
            {
                _Contact.ImagePath = "";
            }

            if (_Contact.Save())
            {
                MessageBox.Show("Contact saved Succefullly :)"); 
            }else
            {
                MessageBox.Show("Contact saved Failed :(");
            }
           
            _Mode = enMode.Edit; 
            label9.Text = "Edit Contact ID " +_ContactID;
            label10.Text = _Contact.ID.ToString();
         
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Process the selected file
                string selectedFilePath = openFileDialog.FileName;
                //MessageBox.Show("Selected Image is:" + selectedFilePath);

                pictureBox1.Load(selectedFilePath);
                // ...
            }
        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (pictureBox1.ImageLocation=="")
            {
                llRemoveImage.Visible = false;
            }
            pictureBox1.ImageLocation = null;
            llRemoveImage.Visible = false;
        }
    }
}
