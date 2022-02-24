using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace Employee_Rest_Sharp
{
    public class Employee
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string salary { get; set; }

    }
    [TestClass]
    public class RestSharp
    {

        RestClient client;
        [TestInitialize]
        public void SetUp()
        {
            client = new RestClient(" http://localhost:5000");
        }

        private RestResponse GetEmployeeList()
        {
            // arrange
            RestRequest request = new RestRequest("/employees/allemployees", Method.Get);

            // act
            RestResponse response = client.ExecuteAsync(request).Result;

            return response;
        }
        [TestMethod]
        public void onCallingGETApi_ReturnEmployeeList()
        {
            RestResponse response = GetEmployeeList();

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(7, dataResponse.Count);

            foreach (Employee emp in dataResponse)
            {
                System.Console.WriteLine("id: " + emp.id + ", Name: " + emp.first_name + ", Salary: " + emp.salary);
            }
        }
        //UC2
        [TestMethod]
        public void OnCallingPostAPI_ReturnEmployeeObject()
        {
            // Arrange
            // endpoint and POST method
            RestRequest request = new RestRequest("/employees/add_emp", Method.Post);
            
            request.AddHeader("Content-type", "application/json");
            request.AddJsonBody(
            new
            {
                first_name = "Clark",
                salary = "15000"
            });

            //Act
            RestResponse response = client.PostAsync(request).Result;

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Clark", dataResponse.first_name);
            Assert.AreEqual("15000", dataResponse.salary);
            System.Console.WriteLine(response.Content);
        }
        [TestMethod]
        public void addMultipleEmployee()
        {
            List<Employee> employees = new List<Employee>();
            employees.Add(new Employee { first_name = "Jackson", salary = "33000" });
            employees.Add(new Employee { first_name = "Mine", salary = "50000" });
            foreach (Employee emp in employees)
            {
                RestRequest request = new RestRequest("/employees/add_emp", Method.Post);
                request.AddHeader("Content-type", "application/json");
                request.AddJsonBody(
                new
                {
                    first_name = emp.first_name,
                    salary = emp.salary
                });
                //Act
                RestResponse response = client.PostAsync(request).Result;

                //Assert
                Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
                Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
                Assert.AreEqual(emp.first_name, dataResponse.first_name);
                Assert.AreEqual(emp.salary, dataResponse.salary);
                System.Console.WriteLine(response.Content);
            }

        }
        [TestMethod]
        public void UpdateEmployee()
        {
            // Arrange
            RestRequest request = new RestRequest("/employees/update_emp/9", Method.Put);
            request.AddHeader("Content-type", "application/json");
            request.AddJsonBody(
            new
            {
                first_name = "Raju",
                salary = "45000"
            });
            // Act
            RestResponse response = client.ExecuteAsync(request).Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Employee employee = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Raju", employee.first_name);
            Assert.AreEqual("45000", employee.salary);
            Console.WriteLine(response.Content);
        }

    }
}

