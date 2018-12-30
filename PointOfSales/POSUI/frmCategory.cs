using Autofac;
using POSLibrary.Abstract;
using POSLibrary.DataAccess;
using POSLibrary.Models;
using POSUI.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSUI
{
    public partial class frmCategory : Form
    {
        private ICategoryData _category = new CategoryData();

       

        public frmCategory()
        {
            InitializeComponent();
            LoadCategories();
        }

        private  void LoadCategories()
        {
            listCategories.Items.Clear();
            var categories =  _category.GetAllRecords();

            foreach (var cat in categories)
            {
                ListViewItem item = new ListViewItem(cat.Id.ToString());
                item.SubItems.Add(cat.Name);
                listCategories.Items.Add(item);

            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {


            if (IsValidForm())
            {
                try
                {
                    CategoryModel oCategory = new CategoryModel()
                    {
                        Name = txtName.Text.Trim().ToLower()
                    };

                    oCategory = await _category.CreateCategoryAsync(oCategory);
                    frmSuccess messageBox = new frmSuccess("Save", $"{oCategory.Name} has been successfully saved");
                    messageBox.ShowDialog();
                     LoadCategories();
                    ResetControls();
                }
                catch (Exception ex)
                {

                    MessageBox.Show($"Sorry an error occured while saving records.\n {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool IsValidForm()
        {
            errorProvider1.Clear();
            bool isValid = true;
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                errorProvider1.SetError(txtName, "Category Name is required");
                txtName.Focus();
                isValid = false;
            }
            return isValid;
        }


        private void ResetControls()
        {
            errorProvider1.Clear();
            txtName.Text = "";
            txtName.Focus();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetControls();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (listCategories.Items.Count > 0)
            {
                if (listCategories.SelectedItems.Count > 0)
                {
                    frmConfirm confirm = new frmConfirm("Confirm Delete", "Are you sure you want to delete record?");
                    if (confirm.ShowDialog() == DialogResult.OK)
                    {
                        int categoryId = int.Parse(listCategories.SelectedItems[0].Text);
                        bool result = await _category.DeleteRecordAsync(categoryId);
                         LoadCategories();                        
                        DialogControl.ShowNotification("Delete", "Record has been successfully deleted");
                    }

                }
                else
                {
                    frmInfo info = new frmInfo("Delete", "Select a record to delete");
                    info.ShowDialog();
                   
                }
            }




        }

        private async void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (listCategories.Items.Count > 0)
            {
                frmConfirm confirm = new frmConfirm("Delete All", "Are you sure you want to delete all records?");
                if (confirm.ShowDialog()==DialogResult.OK)
                {
                    try
                    {
                       await _category.DeleteAllRecordsAsync();
                        DialogControl.ShowNotification("Delete All", "All Categories has been successfully deleted");
                         LoadCategories();
                    }
                    catch (Exception ex)
                    {
                        frmError error = new frmError("Error", $"Sorry an error occured while deleting records \n.{ex.Message} ");
                        error.ShowDialog();
                    }
                }
            }
        }
    }
}
