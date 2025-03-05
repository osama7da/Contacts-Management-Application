using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ContactsBusinessLayer;


namespace ContactsProj
{
    public partial class frmListContacts : Form
    {
        
        public frmListContacts()
        {
            InitializeComponent();
        }
        private void _RefreshContactsList()
        {
            dataGridView1.DataSource = clsContact.GetAllContact(); 
           
        }
        private void _RefreshContacts()
        {
            dataGridView1.DataSource = clsContact.GetAllContact(); 
        }

        private void frmListContacts_Load(object sender, EventArgs e)
        {
            _RefreshContacts(); 

        }

        private void btnAddNewContact_Click(object sender, EventArgs e)
        {
            AddEditForm frm2 = new AddEditForm(-1);
            frm2.ShowDialog();
            _RefreshContactsList();
        }
        

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEditForm form2 = new AddEditForm((int)dataGridView1.CurrentRow.Cells[0].Value);
            form2.ShowDialog();
            _RefreshContacts() ;

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the Contact [" + dataGridView1.CurrentRow.Cells[0].Value +"]","Confirm Delete",MessageBoxButtons.OKCancel)==DialogResult.OK)
            {
                if (clsContact.DeleteContact((int)dataGridView1.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Contact Deleted Succefully");
                    _RefreshContacts(); 
                }else
                {
                    MessageBox.Show("Contact is not deleted"); 
                }

            }

        }
    }
}
