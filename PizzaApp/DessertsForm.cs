using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PizzaApp
{
    public partial class DessertsForm : Form
    {
        private List<Dessert> desserts;
        private List<Dessert> selectedDesserts = new List<Dessert>();
        private decimal totalPrice = 0;
        private Form1 mainForm;

        public DessertsForm(Form1 form)
        {
            InitializeComponent();
            LoadDesserts();
            DisplayDesserts();
            mainForm = form; // Запазваме препратка към главната форма

            this.WindowState = FormWindowState.Maximized;

        }

        private void LoadDesserts()
        {
            desserts = new List<Dessert>
            {
                new Dessert("Тирамису", 5.99m, "Images/tiramisu.jpg"),
                new Dessert("Шоколадов мус", 4.99m, "Images/chocolate_mousse.jpg"),
                new Dessert("Ябълков пай", 6.50m, "Images/apple_pie.jpg"),
                new Dessert("Палачинка с шоколад", 5.50m, "Images/pancake.jpg"),
                new Dessert("Чийзкейк", 6.99m, "Images/cheesecake.jpg")
            };
        }

        private void DisplayDesserts()
        {
            int yOffset = 10;

            foreach (var dessert in desserts)
            {
                Label dessertLabel = new Label
                {
                    Text = $"{dessert.Name} - {dessert.Price:C}",
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    Location = new Point(10, yOffset),
                    AutoSize = true
                };
                Controls.Add(dessertLabel);

                PictureBox dessertPicture = new PictureBox
                {
                    Image = Image.FromFile(dessert.ImagePath),
                    Size = new Size(100, 100),
                    Location = new Point(250, yOffset - 10),
                    SizeMode = PictureBoxSizeMode.StretchImage
                };
                Controls.Add(dessertPicture);

                NumericUpDown quantityBox = new NumericUpDown
                {
                    Minimum = 0,
                    Maximum = 10,
                    Value = 0,
                    Location = new Point(370, yOffset),
                    Tag = dessert
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
            selectedDesserts.Clear();
            totalPrice = 0;

            foreach (Control control in Controls)
            {
                if (control is NumericUpDown quantityBox && quantityBox.Tag is Dessert dessert)
                {
                    int quantity = (int)quantityBox.Value;
                    if (quantity > 0)
                    {
                        selectedDesserts.Add(new Dessert(dessert.Name, dessert.Price, dessert.ImagePath, quantity));
                        totalPrice += dessert.Price * quantity;
                    }
                }
            }

            // Ако има избрани пици, ги добавяме в главната форма
            if (selectedDesserts.Count > 0)
            {
                foreach (var dessert in selectedDesserts)
                {
                    mainForm.AddToOrder($"{dessert.Name} x{dessert.Quantity}", dessert.Price * dessert.Quantity);
                }
            }

            // Затваряме формата след избор
            this.Close();
        }

    }

    public class Dessert
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public int Quantity { get; set; }

        public Dessert(string name, decimal price, string imagePath, int quantity = 0)
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
