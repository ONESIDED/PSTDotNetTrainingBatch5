﻿namespace PSTDotNetTrainingBatch5.MinimalApi.Endpoints.Blog;

public static class BlogEndpoint
{
    //public static string Test (this string i)
    //{
    //    return i.ToString();
    //}

    public static void UseBlogEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/blogs", () =>
        {
            AppDbContext db = new AppDbContext();
            var model = db.TblBlogs.AsNoTracking().ToList();
            return Results.Ok(model);
        })
        .WithName("GetBlogs")
        .WithOpenApi();

        app.MapGet("/blogs/{id}", (int id) =>
        {
            AppDbContext db = new AppDbContext();
            var item = db.TblBlogs
            .AsNoTracking()
            .FirstOrDefault(x => x.BlogId == id);

            if (item is null)
            {
                return Results.BadRequest("No data found.");
            }

            return Results.Ok(item);
        })
        .WithName("GetBlog")
        .WithOpenApi();

        app.MapPost("/blogs", (TblBlog blog) =>
        {
            AppDbContext db = new AppDbContext();
            db.TblBlogs.Add(blog);
            db.SaveChanges();

            return Results.Ok(blog);
        })
        .WithName("CreateBlog")
        .WithOpenApi();

        app.MapPut("/blogs/{id}", (int id, TblBlog blog) =>
        {
            AppDbContext db = new AppDbContext();
            var item = db.TblBlogs
            .AsNoTracking()
            .FirstOrDefault(x => x.BlogId == id);

            if (item is null)
            {
                return Results.BadRequest("No data found.");
            }

            item.BlogTitle = blog.BlogTitle;
            item.BlogAuthor = blog.BlogAuthor;
            item.BlogContent = blog.BlogContent;

            // Require to tell stage changes because of AsNoTracking
            // AsNoTracking() is used to clone the object and not to track the changes
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();

            return Results.Ok(blog);
        })
        .WithName("UpdateBlog")
        .WithOpenApi();

        app.MapPatch("/blogs/{id}", (int id, TblBlog blog) =>
        {
            AppDbContext db = new AppDbContext();
            var item = db.TblBlogs
            .AsNoTracking()
            .FirstOrDefault(x => x.BlogId == id);

            if (item is null) { return Results.NotFound(); }

            if (!string.IsNullOrEmpty(blog.BlogTitle))
            {
                item.BlogTitle = blog.BlogTitle;
            }
            if (!string.IsNullOrEmpty(blog.BlogAuthor))
            {
                item.BlogAuthor = blog.BlogAuthor;
            }
            if (!string.IsNullOrEmpty(blog.BlogContent))
            {
                item.BlogContent = blog.BlogContent;
            }

            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();

            return Results.Ok(blog);
        })
        .WithName("PatchBlog")
        .WithOpenApi();

        app.MapDelete("/blogs/{id}", (int id) =>
        {
            AppDbContext db = new AppDbContext();
            var item = db.TblBlogs
            .AsNoTracking()
            .FirstOrDefault(x => x.BlogId == id);

            if (item is null)
            {
                return Results.BadRequest("No data found.");
            }

            db.Entry(item).State = EntityState.Deleted;
            db.SaveChanges();

            return Results.Ok();
        })
        .WithName("DeleteBlog")
        .WithOpenApi();
    }
        
}
