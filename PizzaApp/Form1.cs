using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PizzaApp
{
    public partial class Form1 : Form
    {
        private List<string> orderItems = new List<string>(); // Списък с избраните продукти
        private decimal totalPrice = 0m; // Обща цена
        private Label totalPriceLabel;
        private ListBox orderListBox;

        public Form1()
        {
            InitializeComponent();
            SetupForm();

            this.WindowState = FormWindowState.Maximized;

        }

        private void SetupForm()
        {
            this.Text = "Поръчка на пица";
            this.Size = new Size(800, 600);

            Label titleLabel = new Label
            {
                Text = "Приложение за поръчка на пица",
                Font = new Font("Arial", 24, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 20)
            };
            this.Controls.Add(titleLabel);

            Label authorsLabel = new Label
            {
                Text = "Създадено от: Крум Иванов Настев 11Дклас №12",
                Font = new Font("Arial", 12, FontStyle.Italic),
                AutoSize = true,
                Location = new Point(20, 60)
            };
            this.Controls.Add(authorsLabel);

            // Списък с поръчани продукти
            orderListBox = new ListBox
            {
                Location = new Point(20, 100),
                Size = new Size(500, 300)
            };
            this.Controls.Add(orderListBox);

            // Общата цена
            totalPriceLabel = new Label
            {
                Text = "Обща цена: 0.00 лв.",
                Font = new Font("Arial", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 420)
            };
            this.Controls.Add(totalPriceLabel);

            // Бутон за избор на пица
            Button pizzaButton = new Button
            {
                Text = "Избери пица",
                Location = new Point(550, 100),
                Size = new Size(200, 50),
                BackColor = Color.LightYellow
            };
            pizzaButton.Click += (sender, e) => OpenPizzasForm();
            this.Controls.Add(pizzaButton);

            // Бутон за избор на напитки
            Button drinksButton = new Button
            {
                Text = "Избери напитка",
                Location = new Point(550, 170),
                Size = new Size(200, 50),
                BackColor = Color.LightBlue
            };
            drinksButton.Click += (sender, e) => OpenDrinksForm();
            this.Controls.Add(drinksButton);

            // Бутон за избор на десерти
            Button dessertsButton = new Button
            {
                Text = "Избери десерт",
                Location = new Point(550, 240),
                Size = new Size(200, 50),
                BackColor = Color.LightPink
            };
            dessertsButton.Click += (sender, e) => OpenDessertsForm();
            this.Controls.Add(dessertsButton);

            // Бутон за завършване на поръчката
            Button finishOrderButton = new Button
            {
                Text = "Завърши поръчката",
                Location = new Point(550, 400),
                Size = new Size(200, 50),
                BackColor = Color.LightGreen
            };
            finishOrderButton.Click += (sender, e) => FinishOrder();
            this.Controls.Add(finishOrderButton);
        }

        private void OpenPizzasForm()
        {
            PizzasForm pizzasForm = new PizzasForm(this);
            pizzasForm.ShowDialog();
        }

        private void OpenDrinksForm()
        {
            DrinksForm drinksForm = new DrinksForm(this);
            drinksForm.ShowDialog();
        }

        private void OpenDessertsForm()
        {
            DessertsForm dessertsForm = new DessertsForm(this);
            dessertsForm.ShowDialog();
        }

        public void AddToOrder(string itemName, decimal price)
        {
            orderItems.Add($"{itemName} - {price:F2} лв.");
            totalPrice += price;
            UpdateOrderDisplay();
        }

        private void UpdateOrderDisplay()
        {
            orderListBox.Items.Clear();
            orderListBox.Items.AddRange(orderItems.ToArray());
            totalPriceLabel.Text = $"Обща цена: {totalPrice:F2} лв.";
        }

        private void FinishOrder()
        {
            if (orderItems.Count == 0)
            {
                MessageBox.Show("Не сте избрали нищо за поръчка!", "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string orderSummary = string.Join("\n", orderItems);
            MessageBox.Show($"Вашата поръчка:\n{orderSummary}\n\nОбща сума: {totalPrice:F2} лв.", "Завършване на поръчката", MessageBoxButtons.OK, MessageBoxIcon.Information);
            orderItems.Clear();
            totalPrice = 0;
            UpdateOrderDisplay();
        }
    }
}
