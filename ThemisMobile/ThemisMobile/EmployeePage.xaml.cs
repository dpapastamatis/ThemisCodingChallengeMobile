using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ThemisMobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ThemisMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmployeePage : ContentPage
    {
        public EventHandler<Employee> OnOKClickedEvent;
        private HttpClient _client = new HttpClient();
        private Employee _employee;
        private State _state;
        private const string Url = "https://dummy.restapiexample.com/api/v1/employees";


        public EmployeePage(Employee employee, State state)
        {
            _employee = employee;
            _state = state;           
            InitializeComponent();
            BindingContext = _employee;
            if (_state == State.New)
                Title = "New Employee";
            else
                Title = "Edit Employee";
        }

        private async void OnOKClicked(object sender, EventArgs e)
        {
            var content = JsonConvert.SerializeObject(_employee);
            if (_state == State.New)
                await _client.PostAsync(Url, new StringContent(content));
            else
                await _client.PutAsync(Url + "/" + _employee.ID, new StringContent(content));
            OnOKClickedEvent?.Invoke(this, _employee);
        }
    }
}