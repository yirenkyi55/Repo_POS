using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PagedList;
using POSLibrary;
using POSLibrary.Abstract;
using POSLibrary.DataAccess;
using POSLibrary.Models;
using POSUI.GridData;

namespace POSUI
{

    public partial class frmAllProducts : Form
    {
        private ProductData product = new ProductData();
        ProductsGrid grid = new ProductsGrid();
        private List<ProductModel> allProducts = new List<ProductModel>();
        int pageNumber = 1;//Holds the current page number
        IPagedList<ProductModel> list;
        List<ProductModel> searchedProducts = new List<ProductModel>();

        private void LoadCategories()
        {
            ICategoryData category = new CategoryData();
            IEnumerable<CategoryModel> allCategories = category.GetAllRecords();
            if (allCategories.Count() > 0)
            {
                cboCategory.DataSource = allCategories.ToList();
                cboCategory.DisplayMember = "Name";
                cboCategory.ValueMember = "Id";
                cboCategory.SelectedIndex = -1;
            }

        }

        private void ToggleComboSearch(SearchProductBy searched)
        {
            void SearchOnText()
            {
                pnlTextSearch.Visible = true;
                btnSearch.Left = pnlTextSearch.Right;
                pnlInstock.Visible = false;
                pnlExpiry.Visible = false;
                pnlCategory.Visible = false;
                txtSearch.Focus();
            }


            switch (searched)
            {
                case SearchProductBy.Number:
                    SearchOnText();
                    break;
                case SearchProductBy.Name:
                    SearchOnText();
                    break;
                case SearchProductBy.Category:
                    pnlCategory.Visible = true;
                    btnSearch.Left = pnlCategory.Right;
                    pnlExpiry.Visible = false;
                    pnlInstock.Visible = false;
                    pnlTextSearch.Visible = false;
                    cboCategory.Focus();
                    break;
                case SearchProductBy.ExpiryDate:
                    pnlExpiry.Visible = true;
                    pnlInstock.Visible = false;
                    pnlTextSearch.Visible = false;
                    pnlCategory.Visible = false;
                    btnSearch.Left = pnlExpiry.Right;

                    break;
                case SearchProductBy.InStock:
                    pnlExpiry.Visible = false;
                    pnlInstock.Visible = true;
                    pnlTextSearch.Visible = false;
                    pnlCategory.Visible = false;
                    cboInStock.SelectedIndex = 0;
                    btnSearch.Left = pnlInstock.Right;
                    break;

            }
            sep.Left = btnSearch.Right;
            btnRefresh.Left = sep.Right;
        }

        private void SearchProduct(SearchProductBy searchBy)
        {
            switch (searchBy)
            {
                case SearchProductBy.Number:
                    searchedProducts = product.SearchProduct(allProducts, searchBy, txtSearch.Text.Trim());
                    break;
                case SearchProductBy.Name:
                    searchedProducts = product.SearchProduct(allProducts, searchBy, txtSearch.Text.Trim());
                    break;
                case SearchProductBy.Category:
                    searchedProducts = product.SearchProduct(allProducts, searchBy, category: (CategoryModel)cboCategory.SelectedItem);
                    break;
                case SearchProductBy.ExpiryDate:
                    searchedProducts = product.SearchProduct(allProducts, searchBy, period1: dtmFrom.Value, period2: dtmTo.Value);
                    break;
                case SearchProductBy.InStock:
                    searchedProducts = product.SearchProduct(allProducts, searchBy, inStock: (ProductInStock)cboInStock.SelectedIndex);
                    break;
            }

            lblStatus.Text = $"{searchedProducts.Count} Products Matched";
            pageNumber = 1;
        }

        private bool ValidateControl()
        {
            errorProvider1.Clear();
            bool valid = true;
            if (pnlTextSearch.Visible)
            {
                if (txtSearch.Text.Trim() == string.Empty)
                {
                    errorProvider1.SetError(txtSearch, "Please enter item to search");
                    valid = false;
                }
            }

            if (pnlCategory.Visible)
            {
                if (cboCategory.Items.Count == 0)
                {
                    errorProvider1.SetError(cboCategory, "No Categories to search for");
                    valid = false;
                }
                else if (cboCategory.SelectedIndex == -1)
                {
                    errorProvider1.SetError(cboCategory, "Select category to search for");
                    valid = false;
                }
            }

            if (pnlInstock.Visible)
            {
                if (cboInStock.SelectedIndex == -1)
                {
                    errorProvider1.SetError(cboInStock, "Make a selection");
                    valid = false;
                }
            }
            return valid;
        }

        private async Task PopulateGridView()
        {
            list = await grid.GetPagedProductsAsync(pageNumber);
            gvProducts.DataSource = grid.FormatGridProduct(list.ToList());
            lblPageNumber.Text = $"Page {pageNumber} of {list.PageCount}";
            btnPrevious.Enabled = list.HasPreviousPage;
            btnNext.Enabled = list.HasNextPage;
            lblStatus.Text = "";
        }

        private async Task PopulateSearchGridView()
        {
            list = await grid.GetPagedProductsAsync(searchedProducts, pageNumber);
            gvProducts.DataSource = grid.FormatGridProduct(list.ToList());
            lblPageNumber.Text = $"Page {pageNumber} of {list.PageCount}";
            btnPrevious.Enabled = list.HasPreviousPage;
            btnNext.Enabled = list.HasNextPage;

        }

        private void CalculateTotals()
        {
            lblTotalRegistered.Text = allProducts.Count.ToString();
            decimal totalCost = allProducts.Sum(p => p.UnitCostPrice);
            decimal totalPrice = allProducts.Sum(p => p.UnitSellingPrice);
            decimal totalProfit = totalPrice - totalCost;
            lblTotalCostPrice.Text = $"{totalCost:c2}";
            lblTotalSellingPrice.Text = $"{totalPrice:c2}";
            lblTotalProfit.Text = $"{totalProfit:c2}";
            lblInStock.Text = $"{allProducts.Where(p => p.Instock == true).Count()}";
            lblOutOfStock.Text = $"{allProducts.Count(p => p.Instock == false)}";
        }

        private void LoadAllProducts()
        {
            allProducts = product.GetAllRecords().ToList();
            CalculateTotals();
        }
        public frmAllProducts()
        {
            InitializeComponent();
        }

 

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void frmAllProducts_Load(object sender, EventArgs e)
        {
            LoadAllProducts();
            LoadCategories();
            cboSearch.SelectedIndex = 0;
            await PopulateGridView();
        }

        private async void btnPrevious_Click(object sender, EventArgs e)
        {
            --pageNumber;
            await PopulateGridView();
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            ++pageNumber;
            await PopulateGridView();
        }


        private void lblTotalProfit_Click(object sender, EventArgs e)
        {

        }

        private void lblTotalSellingPrice_Click(object sender, EventArgs e)
        {

        }

        private async void cboSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            await PopulateGridView();
            ToggleComboSearch((SearchProductBy)cboSearch.SelectedIndex);
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (ValidateControl())
            {
                SearchProduct((SearchProductBy)(cboSearch.SelectedIndex));
                await PopulateSearchGridView();
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAllProducts();
            await PopulateGridView();
            txtSearch.Text = "";
        }

        private void btnReset_Click(object sender, EventArgs e)
        {

        }
    }
}
