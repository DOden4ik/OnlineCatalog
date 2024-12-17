using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OnlineCatalog
{
    public partial class Form1 : Form
    {
        private List<Product> products;
        private List<CartItem> cart;
        private ListView productsListView;
        private ListView cartListView;
        private Button addToCartButton;
        private Button viewCartButton;
        private Button checkoutButton;

        public Form1()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            InitializeComponent();
            InitializeData();
            SetupUI();
            DisplayProducts();
        }

        private void InitializeData()
        {
            products = new List<Product>
            {
                new Product { Id = 1, Name = "Смартфон", Description = "Смартфон останнього покоління", Price = 20000, Stock = 5 },
                new Product { Id = 2, Name = "Ноутбук", Description = "Потужний ноутбук для роботи та ігор", Price = 45000, Stock = 3 },
                new Product { Id = 3, Name = "Навушники", Description = "Бездротові навушники з шумозаглушенням", Price = 3000, Stock = 10 }
            };

            cart = new List<CartItem>();
        }

        private void SetupUI()
        {
            this.Text = "Онлайн каталог товарів";
            this.Width = 1000;
            this.Height = 700;

            productsListView = new ListView
            {
                View = View.Details,
                FullRowSelect = true,
                Width = 940,
                Height = 300,
                Location = new System.Drawing.Point(20, 20),
                Font = new System.Drawing.Font("Arial", 12)
            };

            productsListView.Columns.Add("ID", 50);
            productsListView.Columns.Add("Назва товару", 200);
            productsListView.Columns.Add("Опис", 400);
            productsListView.Columns.Add("Ціна (грн)", 150);
            productsListView.Columns.Add("На складі", 100);

            cartListView = new ListView
            {
                View = View.Details,
                FullRowSelect = true,
                Width = 940,
                Height = 200,
                Location = new System.Drawing.Point(20, 350),
                Font = new System.Drawing.Font("Arial", 12)
            };

            cartListView.Columns.Add("Назва товару", 300);
            cartListView.Columns.Add("Кількість", 150);
            cartListView.Columns.Add("Ціна за одиницю (грн)", 200);
            cartListView.Columns.Add("Загальна сума (грн)", 200);

            addToCartButton = new Button
            {
                Text = "Додати в кошик",
                Location = new System.Drawing.Point(20, 580),
                Width = 200,
                Height = 50,
                Font = new System.Drawing.Font("Arial", 12)
            };
            addToCartButton.Click += addToCartButton_Click;

            viewCartButton = new Button
            {
                Text = "Переглянути кошик",
                Location = new System.Drawing.Point(250, 580),
                Width = 200,
                Height = 50,
                Font = new System.Drawing.Font("Arial", 12)
            };
            viewCartButton.Click += viewCartButton_Click;

            checkoutButton = new Button
            {
                Text = "Оформити замовлення",
                Location = new System.Drawing.Point(480, 580),
                Width = 200,
                Height = 50,
                Font = new System.Drawing.Font("Arial", 12)
            };
            checkoutButton.Click += checkoutButton_Click;

            this.Controls.Add(productsListView);
            this.Controls.Add(cartListView);
            this.Controls.Add(addToCartButton);
            this.Controls.Add(viewCartButton);
            this.Controls.Add(checkoutButton);
        }

        private void DisplayProducts()
        {
            productsListView.Items.Clear();
            foreach (var product in products)
            {
                var listViewItem = new ListViewItem(product.Id.ToString());
                listViewItem.SubItems.Add(product.Name);
                listViewItem.SubItems.Add(product.Description);
                listViewItem.SubItems.Add(product.Price.ToString("0.00"));
                listViewItem.SubItems.Add(product.Stock.ToString());
                productsListView.Items.Add(listViewItem);
            }
        }

        private void DisplayCart()
        {
            cartListView.Items.Clear();
            foreach (var item in cart)
            {
                var listViewItem = new ListViewItem(item.Product.Name);
                listViewItem.SubItems.Add(item.Quantity.ToString());
                listViewItem.SubItems.Add(item.Product.Price.ToString("0.00"));
                listViewItem.SubItems.Add((item.Product.Price * item.Quantity).ToString("0.00"));
                cartListView.Items.Add(listViewItem);
            }
        }

        private void addToCartButton_Click(object sender, EventArgs e)
        {
            if (productsListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Виберіть товар для додавання в кошик.", "Попередження");
                return;
            }

            var selectedProductId = int.Parse(productsListView.SelectedItems[0].Text);
            var product = products.FirstOrDefault(p => p.Id == selectedProductId);

            if (product == null || product.Stock <= 0)
            {
                MessageBox.Show("Обраний товар недоступний.", "Попередження");
                return;
            }

            var cartItem = cart.FirstOrDefault(c => c.Product.Id == product.Id);
            if (cartItem == null)
            {
                cart.Add(new CartItem { Product = product, Quantity = 1 });
            }
            else
            {
                if (cartItem.Quantity < product.Stock)
                {
                    cartItem.Quantity++;
                }
                else
                {
                    MessageBox.Show("Недостатньо товару на складі.", "Попередження");
                    return;
                }
            }

            product.Stock--;
            DisplayProducts();
            MessageBox.Show($"Товар '{product.Name}' додано до кошика.", "Операція успішна");
        }

        private void viewCartButton_Click(object sender, EventArgs e)
        {
            DisplayCart();
        }

        private void checkoutButton_Click(object sender, EventArgs e)
        {
            if (cart.Count == 0)
            {
                MessageBox.Show("Ваш кошик порожній.", "Попередження");
                return;
            }

            decimal total = cart.Sum(item => item.Product.Price * item.Quantity);
            MessageBox.Show($"Загальна сума до оплати: {total:0.00} грн.", "Оформлення замовлення");
            cart.Clear();
            DisplayCart();
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }

    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
