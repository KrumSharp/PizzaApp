using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PizzaApp
{
    public partial class PizzasForm : Form
    {
        private List<Pizza> pizzas;
        private List<Pizza> selectedPizzas = new List<Pizza>();
        private decimal totalPrice = 0;
        private Form1 mainForm;

        public PizzasForm(Form1 form)
        {
            InitializeComponent();
            LoadPizzas();
            DisplayPizzas();
            mainForm = form; // Запазваме препратка към главната форма

            this.WindowState = FormWindowState.Maximized;

        }

        private void LoadPizzas()
        {
            pizzas = new List<Pizza>
            {
                new Pizza("Маргарита", 8.99m, "Images/margarita.jpg"),
                new Pizza("Пеперони", 10.99m, "Images/pepperoni.jpg"),
                new Pizza("Капричоза", 9.99m, "Images/capricciosa.jpeg"),
                new Pizza("Четири сирена", 11.99m, "Images/quattro_formaggi.png"),
                new Pizza("Прошуто", 12.50m, "Images/prosciutto.jpg")
            };
        }

        private void DisplayPizzas()
        {
            int yOffset = 10;

            foreach (var pizza in pizzas)
            {
                Label pizzaLabel = new Label
                {
                    Text = pizza.Name + " - " + pizza.Price.ToString("C"),
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    Location = new Point(10, yOffset),
                    AutoSize = true
                };
                Controls.Add(pizzaLabel);

                PictureBox pizzaPicture = new PictureBox
                {
                    Image = Image.FromFile(pizza.ImagePath),
                    Size = new Size(100, 100),
                    Location = new Point(250, yOffset - 10),
                    SizeMode = PictureBoxSizeMode.StretchImage
                };
                Controls.Add(pizzaPicture);

                NumericUpDown quantityBox = new NumericUpDown
                {
                    Minimum = 0,
                    Maximum = 10,
                    Value = 0,
                    Location = new Point(370, yOffset),
                    Tag = pizza
                };
                Controls.Add(quantityBox);

                yOffset += 120;
            }

            Button addToOrderButton = new Button
            {
                Text = "Добави към поръчката",
                Location = new Point(10, yOffset),
                BackColor = Color.LightGreen,
                Width = 200,
                Height = 40
            };
            addToOrderButton.Click += AddToOrder;
            Controls.Add(addToOrderButton);
        }

        private void AddToOrder(object sender, EventArgs e)
        {
            selectedPizzas.Clear();
            totalPrice = 0;

            foreach (Control control in Controls)
            {
                if (control is NumericUpDown quantityBox && quantityBox.Tag is Pizza pizza)
                {
                    int quantity = (int)quantityBox.Value;
                    if (quantity > 0)
                    {
                        selectedPizzas.Add(new Pizza(pizza.Name, pizza.Price, pizza.ImagePath, quantity));
                        totalPrice += pizza.Price * quantity;
                    }
                }
            }

            // Ако има избрани пици, ги добавяме в главната форма
            if (selectedPizzas.Count > 0)
            {
                foreach (var pizza in selectedPizzas)
                {
                    mainForm.AddToOrder($"{pizza.Name} x{pizza.Quantity}", pizza.Price * pizza.Quantity);
                }
            }

            // Затваряме формата след избор
            this.Close();
        }

    }

    public class Pizza
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public int Quantity { get; set; }

        public Pizza(string name, decimal price, string imagePath, int quantity = 0)
        {
            Name = name;
            Price = price;
            ImagePath = imagePath;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return $"{Name} x{Quantity} - {Price * Quantity:C}";
        }
    }
}
