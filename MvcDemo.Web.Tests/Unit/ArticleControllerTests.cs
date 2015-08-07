using System.Net;

using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Tests;
using CMS.DocumentEngine.Types;
using CMS.Tests;

using MvcDemo.Web.Controllers;
using MvcDemo.Web.Infrastructure;
using MvcDemo.Web.Repositories;
using MvcDemo.Web.Tests.Extensions;

using NSubstitute;
using NUnit.Framework;
using TestStack.FluentMVCTesting;

namespace MvcDemo.Web.Tests.Unit
{
    [TestFixture]
    public class ArticlesControllerTests : UnitTests
    {
        private ArticlesController mController;
        private Article mArticle;
        private IOutputCacheDependencies mDependencies;
        private string mDocumentName = "Article1";


        [SetUp]
        public void SetUp()
        {
            Fake().DocumentType<Article>(Article.CLASS_NAME);
            mArticle = TreeNode.New<Article>().With(a => a.DocumentName = mDocumentName);
            mDependencies = Substitute.For<IOutputCacheDependencies>();
            
            var repository = Substitute.For<IArticleRepository>();
            repository.GetArticle(1).Returns(mArticle);
            
            mController = new ArticlesController(repository, mDependencies);
        }


        [Test]
        public void Index_RendersDefaultView()
        {
            mController.WithCallTo(c => c.Index())
                .ShouldRenderDefaultView();
        }


        [Test]
        public void Show_WithExistingArticle_RendersDefaultViewWithCorrectModel()
        {
            mController.WithCallTo(c => c.Show(1))
                .ShouldRenderDefaultView()
                .WithModelMatchingCondition<Article>(x => x.DocumentName == mDocumentName);
        }


        [Test]
        public void Show_WithoutExistingArticle_ReturnsHttpNotFoundStatusCode()
        {
            mController.WithCallTo(c => c.Show(2))
                .ShouldGiveHttpStatus(HttpStatusCode.NotFound);
        }
    }
}