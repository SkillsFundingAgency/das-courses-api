using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using NUnit.Framework;
using SFA.DAS.Courses.Api.Controllers;
using SFA.DAS.Courses.Api.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Api.UnitTests.Infrastructure
{
    public class WhenAddingAuthorizeFilterRoles
    {
        [Test, MoqAutoData]
        public void Then_The_Data_Load_Policy_Is_Added_To_The_Controllers_Starting_With_DataLoad(
            ControllerModel model,
            AuthorizeControllerModelConvention convention)
        {
            //Arrange
            var delegatingType = typeof(DataLoadController);
            model = new ControllerModel(new TypeDelegator(delegatingType), model.Attributes)
            {
                ControllerName = delegatingType.Name.Replace("Controller", "")
            };

            //Act
            convention.Apply(model);
            
            //Assert
            Assert.AreEqual(1,model.Filters.Count);
            var actual = ((AuthorizeFilter)model.Filters.FirstOrDefault())?.AuthorizeData.FirstOrDefault();
            Assert.IsNotNull(actual);
            Assert.AreEqual(PolicyNames.DataLoad,actual.Policy);
        }
        
        [Test, MoqAutoData]
        public void Then_The_Default_Policy_Is_Added_To_The_Controllers_That_Arent_In_The_Policy_Load_List(
            ControllerModel model,
            AuthorizeControllerModelConvention convention)
        {
            //Arrange
            var delegatingType = typeof(StandardsController);
            model = new ControllerModel(new TypeDelegator(delegatingType), model.Attributes)
            {
                ControllerName = delegatingType.Name.Replace("Controller", "")
            };
            
            //Act
            convention.Apply(model);
            
            //Assert
            Assert.AreEqual(1,model.Filters.Count);
            var actual = ((AuthorizeFilter)model.Filters.FirstOrDefault())?.AuthorizeData.FirstOrDefault();
            Assert.IsNotNull(actual);
            Assert.AreEqual(PolicyNames.Default,actual.Policy);
        }
    }
}