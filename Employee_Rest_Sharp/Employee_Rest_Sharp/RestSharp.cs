﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Assert.AreEqual(4, dataResponse.Count);

            foreach (Employee emp in dataResponse)
            {
                System.Console.WriteLine("id: " + emp.id + ", Name: " + emp.first_name + ", Salary: " + emp.salary);
            }
        }
    }
}

