using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AnimatedText
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Budgets = GetBudgets();
            this.BindingContext = this;
        }

        private ObservableCollection<Budget> budgets;
        public ObservableCollection<Budget> Budgets
        {
            get { return budgets; }
            set
            {
                budgets = value;
                OnPropertyChanged();
            }
        }

        private float amount;
        public float SelectedAmount
        {
            get { return amount; }
            set
            {
                amount = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Budget> GetBudgets()
        {
            return new ObservableCollection<Budget>
            {
                new Budget { Name = "Food", Amount = 650, Color = Color.Blue, Image = "food.png" },
                new Budget { Name = "Groceries", Amount = 1350, Color = Color.SlateBlue, Image = "grocery.png" },
                new Budget { Name = "Transport", Amount = 170, Color = Color.Purple, Image = "transport.png" },
                new Budget { Name = "Utilities", Amount = 750, Color = Color.PeachPuff, Image = "utilities.png" },
            };
        }

        private void ItemTapped(object sender, EventArgs e)
        {
            SelectedAmount = 0.0f;
            var grid = sender as Grid;
            var selectedItem = grid.BindingContext as Budget;
            var parent = grid.Parent as StackLayout;

            ((parent.Parent) as ScrollView).ScrollToAsync(grid, ScrollToPosition.MakeVisible, true);

            foreach (var item in parent.Children)
            {
                var bg = item.FindByName<BoxView>("MainBg");
                var details = item.FindByName<StackLayout>("DetailsView");

                details.TranslateTo(-40, 0, 200, Easing.SinInOut);
                bg.IsVisible = false;
                details.IsVisible = false;
            }

            var selectionBg = grid.FindByName<BoxView>("MainBg");
            var selectionDetails = grid.FindByName<StackLayout>("DetailsView");

            selectionBg.IsVisible = true;
            selectionDetails.IsVisible = true;
            selectionDetails.TranslateTo(0, 0, 300, Easing.SinInOut);

            AnimatedText(selectedItem.Amount);
        }

        private void AnimatedText(float amount)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Device.StartTimer(TimeSpan.FromSeconds(1 / 100f), () =>
              {
                  double t = stopwatch.Elapsed.TotalMilliseconds % 500 / 500;

                  SelectedAmount = Math.Min((float)amount, (float)(10 * t) + SelectedAmount);

                  if (SelectedAmount >= (float)amount)
                  {
                      stopwatch.Stop();
                      return false;
                  }

                  return true;
              });
        }
    }

    public class Budget
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public float Amount { get; set; }
        public Color Color { get; set; }
    }
}
