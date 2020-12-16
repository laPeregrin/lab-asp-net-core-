using BO_BusinessObjects_;
using ObjectContainer.Objects;
using BO_BusinessObjects_.ORMs;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using BO_BusinessObjects_.ORMs.Services;
using System.Text;
using GenerateRandomData.Scripts;

namespace ORM_test
{
    [TestFixture]
    public class Tests
    {
        private  IContactDAO _service;
        GenerateRandomLine generateRandomLine;
        [SetUp]
        public void Setup()
        {
            generateRandomLine = new GenerateRandomLine();
            _service = new ContactDAO();
        }

        [Test]
        public void Remove_ByGuidId_RetuntTrue()
        {
            bool res;
            //arrange
            string data = "092fa891-2a27-47ce-a033-89f447e694a9";
            Guid id = new Guid(data);
            //act

            res = _service.Delete(id);

            //assert
            Assert.AreEqual(true,res, "Dont delete correct contact by this id and return false");

        }

        [Test]
        public void Get_ByIdGuid_ReturnUserFromDataBase()
        {
            //arrange
            string data = "092fa891-2a27-47ce-a033-89f447e694a9";
            Guid id = new Guid(data);
            Contact user = new Contact();

            //act
            try
            {
                user = _service.Get(id);
            }
            //assert
            catch (Exception e)
            {
                string ExpectedName = "Default";
                Assert.AreEqual(ExpectedName, user.FirstName, "Dont take correct name by this id");
            }



        }
        [Test]
        public void GetByIdGuid_GUID_ReturnNULLUserFromDataBase()
        {
            //arrange
            string data = "fe9d144f-b84a-4aba-b0b1-50cbe930e57c";
            Guid id = new Guid(data);
            Contact user = new Contact();


            //act
            try
            {
                user = _service.Get(id);
                Assert.IsNotNull(user, "user is null");
            }
            //assert
            
            catch (Exception e)
            {
                Assert.AreEqual(null, user, $"Didnt take null user by not exist id{e.Message}");
            }


        }
        [Test]
        public void GetAll_noParameters_ReturnICollectionWithUser()
        {
            //arrange


            //act
            var user = (List<Contact>)_service.GetAll();
            bool succes = user.Count != 0;
            //assert

            Assert.IsTrue(succes, "Didnt take users from collection");
        }
        [Test]
        public void Add_newContactToDataBaseContact_ReturnTrue()
        {
            //arrange
            Guid id = new Guid("092fa891-2a27-47ce-a033-89f447e694a9");

            var contact = new Contact()
            {
                id = id,
                StateOrProvince = "Kiev",
                Address = "Podol 22A",
                Birthdate = DateTime.Today,
                City = "Kiev",
                Email = "ValeraBorow@mail.com",
                FirstName = "Valera",
                LastName = "Borrow",
                HomePhone = "148839",
                Phone = "+380-99-535-1339",
                Region = "Ukranian",
                Visible = true
            };

            //act
            var user = _service.Add(contact);
            //assert

            Assert.IsNotNull(user, "Didnt exist user wich was added");
        }

        //[Test]
        //public void Update_ContactFromDataBaseContact_ReturnTrue()
        //{
        //    Guid id = new Guid("092fa891-2a27-47ce-a033-89f447e694a9");
        //    //arrange
        //    StringBuilder sb = new StringBuilder();
           
        //    var user = _service.Get(id);
        //    user.Address = generateRandomLine.GenerateRandomString(10,true);
            

        //    //act
        //    _service.Update(user);
        //    Contact expectedContact = _service.Get(id);
        //    //assert

        //    Assert.AreEqual(expectedContact.Address, user.Address, "Didnt exist user wich was added");
        //}
    }
}