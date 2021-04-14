using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ThemisMobile.Interfaces;
using ThemisMobile.Models;
using Xamarin.Forms;

namespace ThemisMobile
{

    public partial class MainPage : ContentPage
    {
		private const string Url = "https://dummy.restapiexample.com/api/v1/employees";
		//private const string Url = "http://10.0.2.2:44315/api/employees";
		private HttpClient _client;
		private ObservableCollection<Employee> _employees;


		public MainPage()
		{
			InitializeComponent();

			//_client = new HttpClient(DependencyService.Get<IHTTPClientHandlerCreationService>().GetInsecureHandler());
			_client = new HttpClient();
		}

		protected override async void OnAppearing()
		{
			var content = await _client.GetStringAsync(Url);
			var parsedObject = JObject.Parse(content);
			var jsonData = parsedObject["data"].ToString();
			var employees = JsonConvert.DeserializeObject<IEnumerable<Employee>>(jsonData);
			_employees = new ObservableCollection<Employee>(employees);
			listView.ItemsSource = _employees;

			base.OnAppearing();
		}

		private async void OnAdd(object sender, System.EventArgs e)
		{
			var page = new EmployeePage(new Employee(), State.New);
			page.OnOKClickedEvent = (source, args) =>
			{
				_employees.Add(args);
				Navigation.PopAsync();
			};
			await Navigation.PushAsync(page);
		}

		private async void OnEdit(object sender, EventArgs e)
        {
			var employee = ((sender as MenuItem).CommandParameter) as Employee;
			var page = new EmployeePage(employee, State.Existing);
			page.OnOKClickedEvent = (source, args) =>
			{
				Navigation.PopAsync();
			};
			await Navigation.PushAsync(page);
		}

		private async void OnDelete(object sender, EventArgs e)
        {
			var employee = ((sender as MenuItem).CommandParameter) as Employee;
			var response = await DisplayAlert("Info!", "Are you sure you want to delete employee " + employee.Employee_name + "?", "Yes", "No");
			if (response)
            {
				await _client.DeleteAsync(Url + employee.ID);
				_employees.Remove(employee);
            }
		}
	}
}
