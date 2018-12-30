using POSLibrary.Abstract;
using POSLibrary.DataAccess;
using POSLibrary.Infrastructure;
using POSLibrary.Models;
using POSLibrary.Validations;
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

namespace POSUI.FormControls

{

    
    public partial class frmAddProduct : Form
    {
        private CategoryData category = new CategoryData();//variable to perform database operations
        private IEnumerable<CategoryModel> allCategories; //Variable to hold all categories
        private ProductSettings productSettings = new ProductSettings();//variable for product settings
        private ProductData product = new ProductData(); //variable to perform database operations
        List<int> _quantity = new List<int>();//Holds a quantity value 
        private List<ProductModel> allProducts = new List<ProductModel>(); //Holds all products in a list
        int currentRecord = 0;//holds the record for navigating through the product list.
        private bool clearQuantity = true;//determines if we are to clear the quantity list.
        private ProductModel selectedProduct;//holds the selected product.

        public frmAddProduct()
        {
            InitializeComponent();
            LoadCategories();
            LoadAllProducts();
        }

        #region Private Methods
        //***********Private methods for frmAddProducts********
        /// <summary>
        /// Loads all categories into the category combo box
        /// </summary>
        private void LoadCategories()
        {
            allCategories = category.GetAllRecords();
            if (allCategories.Count() > 0)
            {
                cboCategory.DataSource = allCategories;
                cboCategory.DisplayMember = "Name";
                cboCategory.ValueMember = "Id";
                cboCategory.SelectedIndex = -1;
            }
            lblCategories.Text = $"{allCategories.Count()} Registered Brands";
        }

        /// <summary>
        /// Set/unset the product number for the product number textbox
        /// </summary>
        private async Task SetProductNumber()
        {
            //First read scanner value
            bool scannerValue = productSettings.ReadFromScanner();
            if (scannerValue)
            {
                //Clear the textbox,Set the focus to the textbox for scanner reading
                txtProductNumber.Text = "";
                txtProductNumber.Enabled = true;
                switchScanner.Value = true;
                txtProductNumber.Focus();

            }
            else
            {
                //Calculates the next product number
                int nextProductId = await product.GenerateNextProductNumber();
                txtProductNumber.Text = nextProductId.ToString();
                txtProductNumber.Enabled = false;
                switchScanner.Value = false;
                txtName.Focus();
            }


        }

        /// <summary>
        /// Receives new product from the user.
        /// </summary> 
        private ProductModel NewProduct(ProductModel oProduct = null)
        {
            if (oProduct == null)
                oProduct = new ProductModel();
            oProduct.Number = txtProductNumber.Text;
            oProduct.Name = txtName.Text;
            int.TryParse(txtQuantity.Text, out int quantity);
            oProduct.Quantity = quantity;

            decimal.TryParse(txtUnitSellingPrice.Text, out decimal unitSellingPrice);
            oProduct.UnitSellingPrice = unitSellingPrice;

            decimal.TryParse(txtUnitCost.Text, out decimal unitCostPrice);
            oProduct.UnitCostPrice = unitCostPrice;

            oProduct.Instock = chkInstock.Checked == true;

            if (txtDescription.Text.Trim() != null)
                oProduct.Description = txtDescription.Text;
            if (cboCategory.SelectedItem != null)
                oProduct.Category = (CategoryModel)cboCategory.SelectedItem;
            if (chkExpiry.Checked)
                oProduct.ExpiryDate = dtpExpiryDate.Value;

            return oProduct;
        }


        /// <summary>
        /// Validates the form control
        /// </summary>        
        private bool IsValidForm()
        {
            bool valid = true;
            errorProvider1.Clear();
            ProductModel oProduct = NewProduct();
            ProductValidator validator = new ProductValidator();

            var result = validator.Validate(oProduct);
            if (!result.IsValid)
            {
                frmError frmErr = new frmError("Invalid Inputs", "One or more errors occured.\n Please input valid data.");
                frmErr.ShowDialog();

                foreach (var error in result.Errors)
                {
                    if (error.PropertyName == "Name")
                        errorProvider1.SetError(txtName, error.ErrorMessage);
                    if (error.PropertyName == "Number")
                        errorProvider1.SetError(txtProductNumber, error.ErrorMessage);
                    if (error.PropertyName == "Quantity")
                        errorProvider1.SetError(txtQuantity, error.ErrorMessage);
                    if (error.PropertyName == "UnitCostPrice")
                        errorProvider1.SetError(txtUnitCost, error.ErrorMessage);
                    if (error.PropertyName == "UnitSellingPrice")
                        errorProvider1.SetError(txtUnitSellingPrice, error.ErrorMessage);
                }

                valid = false;
            }
            return valid;
        }

        /// <summary>
        /// Calculates the total cost of the product
        /// </summary>
        private void CalculateTotalCost()
        {
            int.TryParse(txtQuantity.Text, out int quantity);
            decimal.TryParse(txtUnitCost.Text, out decimal unitCost);
            decimal totalCost = unitCost * quantity;
            txtTotalCost.Text = totalCost.ToString("c");
        }

        /// <summary>
        /// Automatically Calculates the total selling price 
        /// </summary>
        private void CalculateTotalSell()
        {
            int.TryParse(txtQuantity.Text, out int quantity);
            decimal.TryParse(txtUnitSellingPrice.Text, out decimal unitSell);
            decimal totalSell = unitSell * quantity;
            txtTotalSellingPrice.Text = totalSell.ToString("c");
        }

        /// <summary>
        /// Reset all controls on the form
        /// </summary>
        /// <returns></returns>
        private async Task ResetControls()
        {
            errorProvider1.Clear();
            LoadCategories();
            LoadAllProducts();
            selectedProduct = null;
            await SetProductNumber();
            txtName.Text = "";
            txtQuantity.Text = "0";
            txtUnitCost.Text = "";
            txtTotalCost.Text = "";
            txtUnitSellingPrice.Text = "";
            txtTotalSellingPrice.Text = "";
            txtDescription.Text = "";
            currentRecord = 0;
            chkExpiry.Checked = false;
            chkInstock.Checked = true;
            dtpExpiryDate.Value = DateTime.Now;
            txtSearchCategory.Text = "";
            txtSearchProduct.Text = "";
            txtName.Focus();
            ResetNavigations();
            btnForward.Enabled = true;
            btnBackward.Enabled = true;
            ToggleButtons();
        }

        /// <summary>
        /// Loads all products into a list
        /// </summary>
        private void LoadAllProducts()
        {
            allProducts = product.GetAllRecords().ToList();
            if (allProducts.Count() > 0)
            {
                lblTotalQuantity.Text = allProducts.Sum(p => p.Quantity).ToString();
                lblTotalRegistered.Text = allProducts.Count().ToString();
            }
        }

        /// <summary>
        /// Populates product into the appropriate controls
        /// </summary>
        /// <param name="oproduct">
        /// The selected product you want to populate into controls
        /// </param>
        private void PopulateControls(ProductModel oproduct)
        {

            txtProductNumber.Text = oproduct.Number;
            txtName.Text = oproduct.Name;
            txtQuantity.Text = oproduct.Quantity.ToString();
            txtDescription.Text = oproduct?.Description;
            txtUnitCost.Text = oproduct.UnitCostPrice.ToString("n2");
            txtUnitSellingPrice.Text = oproduct.UnitSellingPrice.ToString("n2");
            if (oproduct.ExpiryDate != null)
            {
                dtpExpiryDate.Value = (DateTime)oproduct.ExpiryDate;
                chkExpiry.Checked = true;
            }
            else
            {
                chkExpiry.Checked = false;
            }
            chkInstock.Checked = oproduct.Instock == true;
            if (oproduct.Category != null)
            {
                cboCategory.SelectedValue = oproduct.Category.Id;
            }
            else
            {
                cboCategory.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Toggles the forward and next button.
        /// </summary>
        private void ResetNavigations()
        {
            if (currentRecord > allProducts.Count())
            {
                currentRecord--;
            }

            if (currentRecord < 0)
            {
                currentRecord++;
            }


            if (currentRecord <= 1)
            {
                //First element or lower. Disable backward
                btnBackward.Enabled = false;
                btnForward.Enabled = true;
            }

            if (currentRecord >= allProducts.Count())
            {
                btnForward.Enabled = false;
                btnBackward.Enabled = true;
            }


            if (currentRecord == 1 && allProducts.Count() == 1)
            {
                //Only one item to navigate
                btnForward.Enabled = false;
                btnBackward.Enabled = false;
            }
        }

        private void ToggleButtons()
        {
            if (selectedProduct != null)
            {
                btnSave.Enabled = false;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                btnSave.Enabled = true;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }
        }
        //*********End of Private Methods 
        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            frmCategory category = new frmCategory();
            category.ShowDialog();
            LoadCategories();
        }

        private void txtUnitCost_OnValueChanged(object sender, EventArgs e)
        {
            txtUnitCost.Text = AppSettings.TrimValue(txtUnitCost.Text.Trim());

            CalculateTotalCost();
        }

        private void txtProductNumber_OnValueChanged(object sender, EventArgs e)
        {

        }

        private void txtTotalCost_OnValueChanged(object sender, EventArgs e)
        {

        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (IsValidForm())
            {
                frmConfirm confirm = new frmConfirm("Update", "Are you sure you want to update product");
                if (confirm.ShowDialog() == DialogResult.OK)
                {
                    ProductModel oproduct = NewProduct(selectedProduct);
                    decimal unitProfit = oproduct.UnitSellingPrice - oproduct.UnitCostPrice;
                    if (unitProfit <= 0)
                    {
                        frmConfirm confirmLoss = new frmConfirm("At Loss", $"You will run at a loss of at least {-unitProfit:c2} if you sell this product. \n Do you want to save anyway?");
                        if (confirmLoss.ShowDialog() == DialogResult.Cancel)
                        {
                            DialogControl.ShowNotification("Cancelled", "Operation has been successfully cancelled");
                            return;
                        }

                    }

                    var result = await product.UpdateProduct(oproduct);
                    DialogControl.ShowNotification("Update", "Record has been successfully updated");
                    await ResetControls();
                }


            }

        }

        private void frmAddProduct_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;
            if (!(char.IsDigit(e.KeyChar) || char.IsPunctuation(e.KeyChar)))
                e.Handled = true;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValidForm())
            {

                ProductModel newProduct = NewProduct();
                decimal unitProfit = newProduct.UnitSellingPrice - newProduct.UnitCostPrice;
                if (unitProfit <= 0)
                {
                    frmConfirm confirm = new frmConfirm("Run at Loss", $"You will run at a loss of at least {-unitProfit:c2} if you sell this product. \n Do you want to save anyway?");
                    if (confirm.ShowDialog() == DialogResult.Cancel)
                    {
                        DialogControl.ShowNotification("Cancelled", "Operation has been successfully cancelled");
                        return;
                    }

                }

                newProduct = await product.CreateProduct(newProduct);
                frmSuccess success = new frmSuccess("Save", $"{newProduct.Name} has been successfully saved.");
                success.ShowDialog();
                await ResetControls();
            }
        }

        private async void frmAddProduct_Load(object sender, EventArgs e)
        {
            await SetProductNumber();
        }

        private async void switchScanner_Click(object sender, EventArgs e)
        {
            if (switchScanner.Value)
            {
                productSettings.WriteToScanner(true.ToString());
                await SetProductNumber();
            }
            else
            {
                productSettings.WriteToScanner(false.ToString());
                await SetProductNumber();
            }
        }

        private void txtSearchCategory_OnValueChanged(object sender, EventArgs e)
        {

            if (txtSearchCategory.Text.Trim() == string.Empty)
            {
                LoadCategories();

            }
            else
            {
                if (allCategories.Count() > 0)
                {
                    //Search for the result
                    var result = category.SearchForRecord(allCategories, txtSearchCategory.Text.Trim());
                    cboCategory.DataSource = null;
                    cboCategory.DataSource = result.ToList();
                    cboCategory.DisplayMember = "Name";
                    cboCategory.ValueMember = "Id";
                    lblCategories.Text = $"{result.Count()} Item(s) matched";
                }
                else
                {
                    lblCategories.Text = "No Items to search for.";
                }
            }
        }


        private void txtQuantity_OnValueChanged(object sender, EventArgs e)
        {
            if (clearQuantity)
                _quantity.Clear();

            CalculateTotalCost();
            CalculateTotalSell();
            if (txtQuantity.Text.Trim() != string.Empty)
            {
                _quantity.Add(int.Parse(txtQuantity.Text));
                clearQuantity = true;
            }

        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;
            if (!char.IsNumber(e.KeyChar))
                e.Handled = true;
        }

        private void txtUnitSellingPrice_OnValueChanged(object sender, EventArgs e)
        {
            txtUnitSellingPrice.Text = AppSettings.TrimValue(txtUnitSellingPrice.Text);
            CalculateTotalSell();
        }

        private void chkExpiry_OnChange(object sender, EventArgs e)
        {
            if (chkExpiry.Checked)
            {
                dtpExpiryDate.Enabled = true;
            }
            else
            {
                dtpExpiryDate.Enabled = false;
            }
        }

        private void chkInstock_OnChange(object sender, EventArgs e)
        {
            clearQuantity = false;
            if (!chkInstock.Checked)
            {
                txtQuantity.Text = 0.ToString();

            }
            else
            {
                if (_quantity.Count > 0)
                {
                    txtQuantity.Text = _quantity[0].ToString();
                }
            }
        }

        private async void btnReset_Click(object sender, EventArgs e)
        {
            await ResetControls();
        }




        private void btnForward_Click(object sender, EventArgs e)
        {
            if (allProducts.Count() > 0)
            {
                if (currentRecord == 0)
                {
                    //The first navigating..
                    selectedProduct = allProducts.First();
                    PopulateControls(selectedProduct);
                    currentRecord++;
                }
                else
                {
                    //The user has already navigate
                    selectedProduct = allProducts.Skip(currentRecord).First();
                    PopulateControls(selectedProduct);
                    currentRecord++;
                }
            }
            ResetNavigations();
            ToggleButtons();
        }

        private void btnBackward_Click(object sender, EventArgs e)
        {
            if (allProducts.Count() > 0)
            {
                if (currentRecord == 0)
                {
                    //The first navigating..
                    selectedProduct = allProducts.Last();
                    PopulateControls(selectedProduct);
                    currentRecord = allProducts.Count();
                }
                else
                {
                    //The user has already navigate                  
                    selectedProduct = allProducts.Take(currentRecord - 1).Last();
                    PopulateControls(selectedProduct);
                    currentRecord--;
                }
            }
            ResetNavigations();
            ToggleButtons();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            frmConfirm confirm = new frmConfirm("Confirm Delete", $"Are you sure you want to delete {selectedProduct.Name}?");
            if (confirm.ShowDialog() == DialogResult.OK)
            {
                await product.DeleteRecordAsync((int)selectedProduct.Id);
                DialogControl.ShowNotification("Delete", $"{selectedProduct.Name} has been successfully deleted");
                await ResetControls();
            }
        }

        private void lnkAllRecords_Click(object sender, EventArgs e)
        {
            frmAllProducts allProducts = new frmAllProducts();
            allProducts.ShowDialog();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {

            if (allProducts.Count()>0)
            {
                if (txtSearchProduct.Text.Trim() != string.Empty)
                {
                    selectedProduct = allProducts.FirstOrDefault(p => p.Number.ToLower() == txtSearchProduct.Text.Trim().ToLower());
                    if (selectedProduct != null)
                    {
                        currentRecord = allProducts.IndexOf(selectedProduct) + 1;
                        PopulateControls(selectedProduct);
                        ToggleButtons();
                        ResetNavigations();
                    }
                    else
                    {
                        frmInfo info = new frmInfo("Search", $"No Item match the product number '{txtSearchProduct.Text.Trim()}'. \nPlease provide a valid product number.");
                        info.ShowDialog();
                    }
                }
                else
                {
                  await  ResetControls();
                }
            }
            else
            {
                frmInfo info = new frmInfo("Search", "No product to search for.\n Please register products");
                info.ShowDialog();
            }
        }
    }
}
