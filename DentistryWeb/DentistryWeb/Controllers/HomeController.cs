using DentistryWeb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using DentistryWeb.Models;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using Dentistry_API.Models;

namespace DentistryWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        //Авторизация
        public IActionResult Auth()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Auth(Patient model)
        {
            string email = model.EmailPatient;
            string password = model.PasswordPatient;

            using (var httpClient = new HttpClient())
            {
                var requestData = new { Email = email, Password = password };
                var json = JsonConvert.SerializeObject(requestData);
                System.Diagnostics.Debug.WriteLine($"Request JSON: {json}");


                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var apiResponse = await httpClient.PostAsync("https://localhost:7015/login", content);

                if (apiResponse.IsSuccessStatusCode)
                {
                    var responseJson = await apiResponse.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Response JSON: {responseJson}");

                    var patient = JsonConvert.DeserializeObject<Patient>(responseJson);

                    if (patient != null && patient.RoleId == 1)
                    {
                        return RedirectToAction("MainPatient", "Home");
                    }
                    else if (patient != null && patient.RoleId == 2)
                    {
                        return RedirectToAction("MainDoctor", "Home");
                    }
                    else if (patient != null && patient.RoleId == 3)
                    {
                        return RedirectToAction("AdminService", "Home");
                    }

                }
                var responseContent = await apiResponse.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"Response JSON: {responseContent}");

                ViewBag.ErrorMessage = "Неверный логин или пароль";
                return View("Auth");
            }
        }

        //Регистрация
        public ViewResult Reg() => View();

        [HttpPost]
        public async Task<IActionResult> Reg(Patient patient)
        {
            Patient recivedpat = new Patient();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(patient), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:7015/api/Patients", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                    recivedpat = JsonConvert.DeserializeObject<Patient>(apiResponse);
                }
            }
            ViewBag.ErrorMessage = "Введены некорректные данные";
            return View("MainPatient");
        }

        //Окно врача
        public async Task<IActionResult> MainDoctor()
        {
            List<Appointment> appList = new List<Appointment>();
            List<Patient> patientList = new List<Patient>();
            List<DateAppointment> dateList = new List<DateAppointment>();

            using (var httpClient = new HttpClient())
            {

                using (var response = await httpClient.GetAsync("https://localhost:7015/api/Appointments"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    appList = JsonConvert.DeserializeObject<List<Appointment>>(apiResponse);
                }

                using (var response = await httpClient.GetAsync("https://localhost:7015/api/Patients"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    patientList = JsonConvert.DeserializeObject<List<Patient>>(apiResponse);
                }

                using (var response = await httpClient.GetAsync("https://localhost:7015/api/DateAppointments"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    dateList = JsonConvert.DeserializeObject<List<DateAppointment>>(apiResponse);
                }
            }

            var models = new Tuple<List<Appointment>, List<Patient>, List<DateAppointment>>(appList, patientList, dateList);

            return View(models);
        }

      
        public async Task<IActionResult> UpdateAppointment(int id)
        {
            Appointment appointment = new Appointment();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7015/api/Appointments/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    appointment = JsonConvert.DeserializeObject<Appointment>(apiResponse);
                }
            }
            return View(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAppointment(Appointment appointment, int id)
        {
            Appointment receivedapp = new Appointment();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(appointment), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync("https://localhost:7015/api/Appointments/" + id, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                    receivedapp = JsonConvert.DeserializeObject<Appointment>(apiResponse);
                }
            }
            return View(receivedapp);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAppointment(int IdAppointment)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:7015/api/Appointments/" + IdAppointment))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("MainDoctor");
        }
      





        //Окно Пациента
     
        public IActionResult MainPatient()
        {
            return View();
        }
        //Вывод в списки вариантов записи
        public async Task<IActionResult> CreateApp()
        {
            List<Service> serviceList = new List<Service>();
            List<Employee> empList = new List<Employee>();
            List<DateAppointment> dateList = new List<DateAppointment>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7015/api/Services"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    serviceList = JsonConvert.DeserializeObject<List<Service>>(apiResponse);
                }

                using (var response = await httpClient.GetAsync("https://localhost:7015/api/Employees"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    empList = JsonConvert.DeserializeObject<List<Employee>>(apiResponse);
                }

                using (var response = await httpClient.GetAsync("https://localhost:7015/api/DateAppointments"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    dateList = JsonConvert.DeserializeObject<List<DateAppointment>>(apiResponse);
                }
            }

            var models = new Tuple<List<Service>, List<Employee>, List<DateAppointment>>(serviceList, empList, dateList);

            return View(models);
        }

        //Создание записи 
        [HttpPost, ActionName("CreateApp")]
        public async Task<IActionResult> HandleCreateApp(Appointment app)
        {
            Appointment recivedapp = new Appointment();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(app), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:7015/api/Appointments", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    recivedapp = JsonConvert.DeserializeObject<Appointment>(apiResponse);
                    System.Diagnostics.Debug.WriteLine($"Response JSON: {apiResponse}");


                }
            }
            TempData["SuccessMessage"] = "Запись успешно создана";
            return RedirectToAction("CreateApp", "Home");
        }



        //Профиль Пациента
        public async Task<IActionResult> ProfilePatient()
        {
            List<Appointment> appList = new List<Appointment>();
            List<Service> servicaList = new List<Service>();
            List<Employee> empList = new List<Employee>();
            List<DateAppointment> dateList = new List<DateAppointment>();

            using (var httpClient = new HttpClient())
            {

                using (var response = await httpClient.GetAsync("https://localhost:7015/api/Appointments"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    appList = JsonConvert.DeserializeObject<List<Appointment>>(apiResponse);
                }

                using (var response = await httpClient.GetAsync("https://localhost:7015/api/Services"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    servicaList = JsonConvert.DeserializeObject<List<Service>>(apiResponse);
                }

                using (var response = await httpClient.GetAsync("https://localhost:7015/api/Employees"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    empList = JsonConvert.DeserializeObject<List<Employee>>(apiResponse);
                }

                using (var response = await httpClient.GetAsync("https://localhost:7015/api/DateAppointments"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    dateList = JsonConvert.DeserializeObject<List<DateAppointment>>(apiResponse);
                }
            }

            var models = new Tuple<List<Appointment>, List<Service>, List<Employee>, List<DateAppointment>> (appList,servicaList, empList, dateList);

            return View(models);
        }

        //Отмена записи

        [HttpPost]
        public async Task<IActionResult> DeleteAppointmentPatient(int IdAppointment)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:7015/api/Appointments/" + IdAppointment))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("ProfilePatient");
        }


        //Вывод услуг Пациент
        public async Task<IActionResult> Service()
        {
            List<Service> serviceList = new List<Service>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7015/api/Services"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    serviceList = JsonConvert.DeserializeObject<List<Service>>(apiResponse);
                }
            }
            return View(serviceList);
        }

        //Окно Админа
        //Вывод услуг Админ
        public async Task<IActionResult> AdminService()
        {
            List<Service> serviceList = new List<Service>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7015/api/Services"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    serviceList = JsonConvert.DeserializeObject<List<Service>>(apiResponse);
                }
            }
            return View(serviceList);
        }

        public ViewResult AddService() => View();

        [HttpPost]
        public async Task<IActionResult> AddService(Service service)
        {
            Service recivedservice = new Service();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(service), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:7015/api/Services", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                    recivedservice = JsonConvert.DeserializeObject<Service>(apiResponse);
                }
            }
            return View(recivedservice);
        }

        public async Task<IActionResult> UpdateService(int id)
        {
            Service service = new Service();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7015/api/Services/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    service = JsonConvert.DeserializeObject<Service>(apiResponse);
                }
            }
            return View(service);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateService(Service service, int id)
        {
            Service receivedservice = new Service();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(service), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync("https://localhost:7015/api/Services/" + id, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                    receivedservice = JsonConvert.DeserializeObject<Service>(apiResponse);

                    System.Diagnostics.Debug.WriteLine($"Response JSON: {apiResponse}");
                }
            }
            return View(receivedservice);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteService(int IdService)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:7015/api/Services/" + IdService))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("AdminService");
        }
       
        //Вывод дат Админ
        public async Task<IActionResult> AdminDateAppointment()
        {
            List<DateAppointment> dateAppList = new List<DateAppointment>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7015/api/DateAppointments"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    dateAppList = JsonConvert.DeserializeObject<List<DateAppointment>>(apiResponse);
                }
            }
            return View(dateAppList);
        }

        public ViewResult AddDateAppointment() => View();

        [HttpPost]
        public async Task<IActionResult> AddDateAppointment(DateAppointment dateAppointment)
        {
            DateAppointment reciveddateAppointment = new DateAppointment();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(dateAppointment), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:7015/api/DateAppointments", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                    reciveddateAppointment = JsonConvert.DeserializeObject<DateAppointment>(apiResponse);
                    System.Diagnostics.Debug.WriteLine($"Response JSON: {apiResponse}");
                }
            }
            return View(reciveddateAppointment);
        }

        public async Task<IActionResult> UpdateDateAppointment(int id)
        {
            DateAppointment dateAppointment = new DateAppointment();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7015/api/DateAppointments/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    dateAppointment = JsonConvert.DeserializeObject<DateAppointment>(apiResponse);
                }
            }
            return View(dateAppointment);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDateAppointment(DateAppointment dateAppointment, int id)
        {
            DateAppointment receiveddateAppointment = new DateAppointment();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(dateAppointment), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync("https://localhost:7015/api/DateAppointments/" + id, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                    receiveddateAppointment = JsonConvert.DeserializeObject<DateAppointment>(apiResponse);

                    System.Diagnostics.Debug.WriteLine($"Response JSON: {apiResponse}");
                }
            }
            return View(receiveddateAppointment);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDateAppointment(int IdDateAppointment)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:7015/api/DateAppointments/" + IdDateAppointment))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("AdminDateAppointment");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}