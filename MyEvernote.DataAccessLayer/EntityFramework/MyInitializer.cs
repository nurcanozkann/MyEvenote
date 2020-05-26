using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    // Database e data girilmesini sağlayan kodlar
    public class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            EvernoteUser admin = new EvernoteUser()
            {
                Name = "Can",
                Surname = "Aktı",
                Email = "canakti@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                Username = "canakti",
                ProfileImageFileName = "user_boy.png",
                Password = "123456",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now.AddMinutes(5),
                CreatedUserName = "canakti"
            };

            EvernoteUser standart = new EvernoteUser()
            {
                Name = "nazlı",
                Surname = "bayam",
                Email = "nazlibayam@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "nazlibayam",
                ProfileImageFileName = "user_boy.png",
                Password = "123456",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now.AddMinutes(5),
                CreatedUserName = "nazlibayam"
            };

            context.EvernoteUsers.Add(admin);
            context.EvernoteUsers.Add(standart);

            for (int i = 0; i < 8; i++)
            {
                EvernoteUser user = new EvernoteUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    Username = $"user{i}",
                    ProfileImageFileName = "user_boy.png",
                    Password = "123",
                    CreatedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    UpdatedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    CreatedUserName = $"user{i}"
                };

                context.EvernoteUsers.Add(user);
            }

            context.SaveChanges();


            List<EvernoteUser> userList = context.EvernoteUsers.ToList();

            //addin fake category data
            for (int i = 0; i < 10; i++)
            {
                Category category = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedUserName = "canakti"
                };

                context.Categories.Add(category);

                for (int k = 0; k < FakeData.NumberData.GetNumber(5, 9); k++)
                {
                    EvernoteUser owner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        Category = category,
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1, 9),
                        Owner = owner,
                        CreatedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        UpdatedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        CreatedUserName = owner.Username,
                    };

                    category.Notes.Add(note);

                    for (int j = 0; j < FakeData.NumberData.GetNumber(3, 5); j++)
                    {
                        EvernoteUser commentOwner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];
                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            Note = note,
                            Owner = commentOwner,
                            CreatedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            UpdatedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            CreatedUserName = commentOwner.Username
                        };

                        note.Comments.Add(comment);


                        for (int l = 0; l < note.LikeCount; l++)
                        {
                            Liked liked = new Liked();
                            liked.LikedUser = userList[l];

                            note.Likes.Add(liked);
                        }
                    }
                }

                context.SaveChanges();
            }
        }
    }
}