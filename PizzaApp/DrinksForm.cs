using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace PizzaApp
{
    public partial class DrinksForm : Form
    {
        private List<Drink> drinks;
        private List<Drink> selectedDrinks = new List<Drink>();
        private decimal totalPrice = 0;
        private Form1 mainForm;

        public DrinksForm(Form1 form)
        {
            InitializeComponent();
            LoadDrinks();
            DisplayDrinks();
            mainForm = form; // Запазваме препратка към главната форма

            this.WindowState = FormWindowState.Maximized;

        }

        private void LoadDrinks()
        {
            drinks = new List<Drink>
            {
                new Drink("Кока-Кола", 2.50m, "Images/coke.jpg"),
                new Drink("Фанта", 2.50m, "Images/fanta.jpg"),
                new Drink("Минерална вода", 1.50m, "Images/water.jpg"),
                new Drink("Студен чай", 3.00m, "Images/iced_tea.jpg"),
                new Drink("Кафе", 2.00m, "Images/coffee.jpg")
            };
        }

        private void DisplayDrinks()
        {
            int yOffset = 10;

            foreach (var drink in drinks)
            {
                Label drinkLabel = new Label
                {
                    Text = $"{drink.Name} - {drink.Price:C}",
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    Location = new Point(10, yOffset),
                    AutoSize = true
                };
                Controls.Add(drinkLabel);

                PictureBox drinkPicture = new PictureBox
                {
                    Image = Image.FromFile(drink.ImagePath),
                    Size = new Size(100, 100),
                    Location = new Point(250, yOffset - 10),
                    SizeMode = PictureBoxSizeMode.StretchImage
                };
                Controls.Add(drinkPicture);

                NumericUpDown quantityBox = new NumericUpDown
                {
                    Minimum = 0,
                    Maximum = 10,
                    Value = 0,
                    Location = new Point(370, yOffset),
                    Tag = drink
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
            selectedDrinks.Clear();
            totalPrice = 0;

            foreach (Control control in Controls)
            {
                if (control is NumericUpDown quantityBox && quantityBox.Tag is Drink drink)
                {
                    int quantity = (int)quantityBox.Value;
                    if (quantity > 0)
                    {
                        selectedDrinks.Add(new Drink(drink.Name, drink.Price, drink.ImagePath, quantity));
                        totalPrice += drink.Price * quantity;
                    }
                }
            }

            // Ако има избрани пици, ги добавяме в главната форма
            if (selectedDrinks.Count > 0)
            {
                foreach (var drink in selectedDrinks)
                {
                    mainForm.AddToOrder($"{drink.Name} x{drink.Quantity}", drink.Price * drink.Quantity);
                }
            }

            // Затваряме формата след избор
            this.Close();
        }
    }

    public class Drink
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public int Quantity { get; set; }

        public Drink(string name, decimal price, string imagePath, int quantity = 0)
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
