/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Portal.Helpers;

namespace Portal.ViewModels
{
    public class OrderViewModel
    {
        public OrderViewModel() { }
        public OrderViewModel(Models.Order order)
        {
            Groups = order.Group.Select(g => g.Id.ToString()).ToArray();
            Id = order.Id;
            StartNumber = order.StartNumber;

            Driver1FirstName = order.Driver.FirstName;
            Driver1LastName = order.Driver.LastName;
            var license = order.Driver.Licenses.OrderByDescending(l => l.IssuesOn).FirstOrDefault(l => l.Season == 2014 && l.Type == Models.LicenseType.Driver);
            Driver1License = license == null ? "" : license.Number;
            Driver1City = order.Driver.Location;
            Driver1BirthDate = order.Driver.BirthDate.HasValue? order.Driver.BirthDate.ToString(): "";
            Driver1Phone = order.Driver.Phone;
            Driver1Passport = order.Driver.Passport;
            Driver1Address = order.Driver.Address;
            Driver1Id = order.Driver.Id;
            Driver1Email = order.Driver.Email;

            Driver2FirstName = order.CoDriver.FirstName;
            Driver2LastName = order.CoDriver.LastName;
            license = order.CoDriver.Licenses.OrderByDescending(l => l.IssuesOn).FirstOrDefault(l => l.Season == 2014 && l.Type == Models.LicenseType.Driver);
            Driver2License = license == null ? "" : license.Number;
            Driver2City = order.CoDriver.Location;
            Driver2BirthDate = order.CoDriver.BirthDate.HasValue ? order.CoDriver.BirthDate.ToString() : "";
            Driver2Phone = order.CoDriver.Phone;
            Driver2Passport = order.CoDriver.Passport;
            Driver2Address = order.CoDriver.Address;
            Driver2Id = order.CoDriver.Id;

            EntrantPhone = order.Team.Phone;
            EntrantLicense = order.Team.License;
            EntrantFax = order.Team.Fax;
            EntrantEmail = order.Team.Email;
            EntrantCity = order.Team.City;
            EntrantASN = order.Team.ASN;
            EntrantAddress = order.Team.Address;
            Entrant = order.Team.Name;

            Mark = order.Car.Mark;
            Model = order.Car.Model;
            Engine = order.Car.Engine;
            Number = order.Car.RegistrationNumber;
            CarId = order.Car.Id;

            Comments = order.Comments;
        }
        public string[] Groups { get; set; }

        public string Entrant           { get; set; }
        public string EntrantCity{ get; set; }
        public string EntrantLicense{ get; set; }
        public string EntrantASN{ get; set; }
        public string EntrantAddress{ get; set; }
        public string EntrantPhone{ get; set; }
        public string EntrantFax{ get; set; }
        public string EntrantEmail { get; set; }

        public string Driver1License  { get; set; }
        public string Driver1LastName { get; set; }
        public string Driver1FirstName { get; set; }
        public string Driver1City { get; set; }
        public string Driver1BirthDate { get; set; }
        public string Driver1Phone { get; set; }
        public string Driver1Address { get; set; }
        public string Driver1Email { get; set; }
        public string Driver1Passport { get; set; }
        public string Driver1Id { get; set; }

        public string Driver2License { get; set; }
        public string Driver2LastName { get; set; }
        public string Driver2FirstName { get; set; }
        public string Driver2City { get; set; }
        public string Driver2BirthDate { get; set; }
        public string Driver2Phone { get; set; }
        public string Driver2Address { get; set; }
        public string Driver2Passport { get; set; }
        public string Driver2Id { get; set; }

        public string Mark { get; set; }
        public string Model { get; set; }
        public string Number { get; set; }
        public string Year { get; set; }
        public string Engine { get; set; }
        public string Comments { get; set; }

        internal Models.Team CreateTeam()
        {
            return new Models.Team()
            {
                Name = OrDefault(Entrant).ToUpper(),
                Address = OrDefault(EntrantAddress),
                ASN = OrDefault(EntrantASN),
                City = OrDefault(EntrantCity),
                Email = OrDefault(EntrantEmail),
                Fax = OrDefault(EntrantFax),
                License = OrDefault(EntrantLicense),
                Phone = OrDefault(EntrantPhone)
            };
        }

        private string OrDefault(string str)
        {
            return str.OrDefault();
        }

        internal Models.User CreateDriver1()
        {
            var user = new Models.User() {
                Id = Guid.NewGuid().ToString(),
                UserName = OrDefault(Driver1LastName).ToUpper() + " "+ OrDefault(Driver1FirstName),
                LastName = OrDefault(Driver1LastName).ToUpper(),
                FirstName = OrDefault(Driver1FirstName),
                Address = OrDefault(Driver1Address),
                BirthDate = ToDateTime(Driver1BirthDate),
                Location = OrDefault(Driver1City),
                Passport = OrDefault(Driver1Passport),
                Phone = OrDefault(Driver1Phone),
                Email = OrDefault(Driver1Email),
                Licenses = new List<Models.License>()
            };
            if (string.IsNullOrWhiteSpace(user.UserName)) user.UserName = user.Id;
            if(!string.IsNullOrWhiteSpace(Driver1License))
                user.Licenses.Add(new Models.License() {
                    Season = 2014,
                    Number = Driver1License,
                    IssuesOn = DateTime.Today
                });
            return user;
        }

        private DateTime? ToDateTime(string date)
        {
            try
            {
                return DateTime.Parse(date);
            }
            catch
            {
                return null;
            }
        }


        internal Models.Car CreateCar()
        {
            return new Models.Car()
            {
                Mark = OrDefault(Mark),
                Model = OrDefault(Model),
                RegistrationNumber = OrDefault(Number),
                Engine = OrDefault(Engine)                
            };
        }

        internal Models.User CreateDriver2()
        {
            var user = new Models.User()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = OrDefault(Driver2LastName).ToUpper() + " " + OrDefault(Driver2FirstName),
                LastName = OrDefault(Driver2LastName).ToUpper(),
                FirstName = OrDefault(Driver2FirstName),
                Address = OrDefault(Driver2Address),
                BirthDate = ToDateTime(Driver2BirthDate),
                Location = OrDefault(Driver2City),
                Passport = OrDefault(Driver2Passport),
                Phone = OrDefault(Driver2Phone),
                Licenses = new List<Models.License>()
            };
            if (string.IsNullOrWhiteSpace(user.UserName)) user.UserName = user.Id;
            if(!string.IsNullOrWhiteSpace(Driver2License))
                user.Licenses.Add(new Models.License() {
                    Season = 2014,
                    Number = Driver2License,
                    IssuesOn = DateTime.Today
                });
            return user;
        }

        public string StartNumber { get; set; }
        public int Id { get; set; }

        public int CarId { get; set; }
    }
}