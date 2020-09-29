using DataBaseExampla.Data;
using DataBaseExampla.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataBaseExampla
{
    public partial class Form1 : Form
    {
        Customer model = new Customer();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            GetInput();
            Save();
            Clear();
            PopulateGrid();
            MessageBox.Show("Saved Successfully");
        }

        void Save()
        {
            using (var ctx = new ApplicationDbContext())
            {
                if (model.Id == 0)
                {
                    ctx.Customers.Add(model);
                }
                else
                {
                    ctx.Entry(model).State = EntityState.Modified;
                }
                ctx.SaveChanges();
            }
        }

        void Clear()
        {
            txtAddress.Text = txtFirstName.Text = txtLastName.Text = txtPhoneNumber.Text = "";
        }

        void GetInput()
        {
            model.FirstName = txtFirstName.Text.Trim();
            model.LastName = txtLastName.Text.Trim();
            model.PhoneNumber = txtPhoneNumber.Text.Trim();
            model.Address = txtAddress.Text.Trim();
        }

        void PopulateGrid()
        {
            dgvCustomer.AutoGenerateColumns = false;
            using (var ctx = new ApplicationDbContext())
            {
                dgvCustomer.DataSource = ctx.Customers.ToList<Customer>();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            PopulateGrid();
        }

        private void dgvCustomer_DoubleClick(object sender, EventArgs e)
        {
            if (dgvCustomer.CurrentRow.Index != -1)
            {
                model.Id = Convert.ToInt32(dgvCustomer.CurrentRow.Cells["Id"].Value);
                using (var ctx = new ApplicationDbContext())
                {
                    model = ctx.Customers.Where(x => x.Id == model.Id).FirstOrDefault();
                    txtFirstName.Text = model.FirstName;
                    txtLastName.Text = model.LastName;
                    txtPhoneNumber.Text = model.PhoneNumber;
                    txtAddress.Text = model.Address;
                }

                btnDelete.Enabled = true;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            GetInput();
            Save();
            PopulateGrid();
            MessageBox.Show("Updated.");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this entry?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Delete();
                PopulateGrid();
                Clear();
                MessageBox.Show("User entry has been Deleted.");
            }
        }

        void Delete()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entry = ctx.Entry(model);
                if (entry.State == EntityState.Detached)
                    ctx.Customers.Attach(model);
                ctx.Customers.Remove(model);
                ctx.SaveChanges();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }
}
