using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private readonly PersonRepository _repository = new PersonRepository();
        BindingList<Person> people;
        public Form1()
        {
            InitializeComponent();
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            LoadData();
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }

        private void LoadData()
        {
            people = new BindingList<Person>(_repository.GetAll());
            dataGridView1.DataSource = people;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView1.ReadOnly = true;
            dataGridView1.CurrentCell = null;
            dataGridView1.ClearSelection();
            ClearInputs();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            bool rowSelected = dataGridView1.CurrentRow != null && dataGridView1.CurrentRow.DataBoundItem != null && dataGridView1.Focused;
            btnUpdate.Enabled = rowSelected;
            btnDelete.Enabled = rowSelected;

            if (rowSelected)
            {
                var selectedPerson = (Person)dataGridView1.CurrentRow.DataBoundItem;
                txtId.Text = selectedPerson.Id.ToString();
                txtName.Text = selectedPerson.Name;
                txtEmail.Text = selectedPerson.Email;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                var person = new Person
                {
                    Name = txtName.Text,
                    Email = txtEmail.Text
                };

                _repository.Add(person);
                LoadData();
                ClearInputs();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("please select a row to update");
                return;
            }
            if (!ValidateInputs())
                return;
            try
            {
                var selectedPerson = (Person)dataGridView1.CurrentRow.DataBoundItem;
                selectedPerson.Name = txtName.Text;
                selectedPerson.Email = txtEmail.Text;

                _repository.Update(selectedPerson);
                LoadData();
                ClearInputs();
                MessageBox.Show("Record updated successfully");
            }catch(Exception ex)
            {
                MessageBox.Show($"Error updating record: {ex.Message}");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("please select a row to delete");
                return;
            }
            if (dataGridView1.CurrentRow != null)
            {
                var selectedPerson = (Person)dataGridView1.CurrentRow.DataBoundItem;
                _repository.Delete(selectedPerson.Id);
                LoadData();
                ClearInputs();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a name!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Please enter an email!");
                return false;
            }

            return true;
        }

        private void ClearInputs()
        {
            txtId.Text = "";
            txtName.Text = "";
            txtEmail.Text = "";
        }
    }
}
