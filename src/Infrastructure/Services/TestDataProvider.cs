using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Domain.Enums;
using StoreKit.Shared.DTOs.Catalog;
using StoreKit.Shared.DTOs.Catalog.Comment;
using StoreKit.Shared.DTOs.Catalog.Page;
using StoreKit.Shared.DTOs.Catalog.StaticPage;

namespace StoreKit.Infrastructure.Services
{
    public class TestDataProvider
    {
        private readonly IServiceScopeFactory _serviceFactory;
        private INewsService _newsService;
        private ICategoryService _categoryService;
        private ICommentService _commentService;
        private IPageService _pageService;
        private IStaticPageService _staticPageService;
        private readonly Faker _faker = new();

        public TestDataProvider(IServiceScopeFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
            Init().Wait();
        }

        private Task Init()
        {
            using var scope = _serviceFactory.CreateScope();

            _newsService = scope.ServiceProvider.GetService<INewsService>();
            _categoryService = scope.ServiceProvider.GetService<ICategoryService>();
            _commentService = scope.ServiceProvider.GetService<ICommentService>();
            _pageService = scope.ServiceProvider.GetService<IPageService>();
            _staticPageService = scope.ServiceProvider.GetService<IStaticPageService>();

            var newsTask = CreateNews(100);
            var categoriesTask = CreateCategories(10);
            var commentsTask = CreateComments(100);
            var pagesTask = CreatePages();

            return Task.WhenAll(newsTask, categoriesTask, commentsTask, pagesTask);
        }

        private async Task CreatePages()
        {
            var createPageRequests = new List<CreatePageRequest>();
            createPageRequests.AddRange(await CreateStaticPage(2));
            createPageRequests.Add(CreateCommentsPage());
            createPageRequests.Add(CreateNewsPage());

            var tasks = createPageRequests.Select(c => _pageService.CreatePageAsync(c)).ToArray();
            await Task.WhenAll(tasks);
        }

        private CreatePageRequest CreateNewsPage()
        {
            return new CreatePageRequest
            {
                Name = "News",
                PageType = PageType.CommentsPage,
                Url = $"api/v1/News/"
            };
        }

        private CreatePageRequest CreateCommentsPage()
        {
            return new CreatePageRequest
            {
                Name = "Comments",
                PageType = PageType.CommentsPage,
                Url = $"api/v1/Comments/"
            };
        }

        private async Task<List<CreatePageRequest>> CreateStaticPage(int count)
        {
            var createStaticPageAsync = new Faker<CreatePageRequest>()
                .RuleFor(u => u.PageType, () => PageType.StaticPage)
                .RuleFor(u => u.Name, (f) => $"Static-{f.Lorem.Sentence()}")
                .RuleFor(u => u.Url, (f) => $"api/v1/StaticPage/")
                .Generate(count);

            foreach (var staticPage in createStaticPageAsync)
            {
                var paragraphs = _faker.Lorem.Paragraphs(_faker.Random.Int(2, 6));
                var body = $"<h1>{paragraphs}</h1>";
                var staticPageDto = new CreateStaticPageRequest { Body = body };
                var guid = await _staticPageService.CreateStaticPageAsync(staticPageDto);
                staticPage.Url += guid.ToString("N");
            }

            return createStaticPageAsync;
        }

        private Task CreateComments(int count)
        {
            var commentsFaker = new Faker<CreateCommentRequest>()
                .RuleFor(u => u.CommentatorName, (f, u) => f.Person.UserName)
                .RuleFor(u => u.Title, (f, u) => f.Lorem.Sentence())
                .RuleFor(u => u.Description, (f, u) => f.Lorem.Paragraph());
            var commentRequests = commentsFaker.Generate(count);

            var tasks = commentRequests.Select(c => _commentService.CreateCommentsAsync(c)).ToArray();
            return Task.WhenAll(tasks);
        }

        private Task CreateCategories(int count)
        {
            var categoryFaker = new Faker<CreateCategoryRequest>()
                .RuleFor(u => u.Name, (f, u) => f.Lorem.Sentence());

            var categoryRequests = categoryFaker.Generate(count);
            var tasks = categoryRequests.Select(c => _categoryService.CreateCategoryAsync(c)).ToArray();
            return Task.WhenAll(tasks);
        }

        private Task CreateNews(int count)
        {
            var newsFaker = new Faker<CreateNewsRequest>()
                .RuleFor(u => u.Title, (f, u) => f.Lorem.Sentence())
                .RuleFor(u => u.Description, (f, u) => f.Lorem.Paragraph());

            var newsRequests = newsFaker.Generate(count);
            var tasks = newsRequests.Select(n => _newsService.CreateNewsAsync(n)).ToArray();
            return Task.WhenAll(tasks);
        }
    }
}