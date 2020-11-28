using BO_BusinessObjects_.ORMs;
using BO_BusinessObjects_.ORMs.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenerateRandomData.Scripts;
using ObjectContainer.Objects;

namespace ORM_test
{
    [TestFixture]
    public class ManagerDAOTest
    {
        private  GenerateRandomLine _generate;
        private  GenereateEmail _generateEmail;
        private IManagerDAO _service;
        [SetUp]
        public void SetUp()
        {
            _service = new ManagerDAO();
            _generate = new GenerateRandomLine();
            _generateEmail = new GenereateEmail();
        }

        [Test]
        public void AddNewManager_managerObj_ReturnFalse()
        {
            var id = Guid.NewGuid();
            //arrange
            var manager = new Manager() {
                id = id,
                Addres = _generate.GenerateRandomString(10, true),
                FullName = _generate.GenerateRandomString(20, false),
                Email = _generateEmail.genereateEmail("gmail.com", 10),
                Phone = _generate.GenerateRandomString(10, false),
                Position = _generate.GenerateRandomString(10, true),
                Remark = _generate.GenerateRandomString(50, true)
            };

            //act
            var res = _service.Add(manager);
            //assert
            Assert.IsTrue(res.Result,"Erro add new manager");
        }

        [Test]
        public void GetALL_noParameters_returnCollection()
        {
            //arrange

            //act
            List<Manager> collection = _service.GetAll().Result.ToList();

            //assert
            Assert.IsTrue(collection.Count>0);
        }

        //[Test]
        //public void UpdateManager_FromDataBaseContact_ReturnTrue()
        //{
        //    Guid id = new Guid("aa96fce1-c032-4872-8174-8ef1c06ada87");
        //    //arrange
        //    StringBuilder sb = new StringBuilder();
        //    var manager = _service.Get(id).Result;
        //    manager.Email = _generateEmail.genereateEmail("gmail");
        //    //act
        //    bool res = _service.Update(manager).Result;
        //    Manager expectedContact = _service.Get(id).Result;
        //    //assert
        //    Assert.AreEqual(expectedContact.Email, manager.Email, "Didnt exist user wich was added");
        //}
    }
}
