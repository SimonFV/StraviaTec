using MongoDB.Driver;
using ApiMongo.Entities;
using ApiMongo.DTOs;
using ApiMongo.Configuration;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Options;

namespace ApiMongo.DAL
{
    public class CommentsDAL
    {
        private readonly IMongoCollection<Comment> _commentsCollection;

        public CommentsDAL(IOptions<CommentsDatabaseSettings> settings)
        {
            var mdbClient = new MongoClient(settings.Value.ConnectionString);
            var db = mdbClient.GetDatabase(settings.Value.DatabaseName);
            _commentsCollection = db.GetCollection<Comment>(settings.Value.CommentsCollectionName);
        }

        public List<Comment> GetByUser(string user)
        {
            return _commentsCollection.Find(comment => comment.User == user).ToList();
        }

        public List<Comment> GetByActivity(int activityId)
        {
            return _commentsCollection.Find(comment => comment.ActivityId == activityId).ToList();
        }

        public List<Comment> GetByUserAndActivity(string user, int activityId)
        {
            return _commentsCollection.Find(comment =>
                comment.ActivityId == activityId && comment.User == user).ToList();
        }

        public Comment GetComment(string user, int activityId, DateTime postTime)
        {
            return _commentsCollection.Find(comment =>
                comment.ActivityId == activityId &&
                comment.User == user &&
                comment.PostTime == postTime).FirstOrDefault();
        }

        public string InsertComment(CommentDto comment)
        {
            if (!(GetComment(comment.User, comment.ActivityId, comment.PostTime) is null))
                return "Taken";
            Comment newComment = new()
            {
                User = comment.User,
                ActivityId = comment.ActivityId,
                PostTime = comment.PostTime,
                Body = comment.Body
            };
            try
            {
                _commentsCollection.InsertOne(newComment);
            }
            catch (MongoWriteException e)
            {
                Console.WriteLine(e.Message);
                return "Error";
            }
            return "Done";
        }

        public string EditComment(CommentDto comment)
        {
            Comment tempComment = GetComment(comment.User, comment.ActivityId, comment.PostTime);
            if (tempComment is null)
                return "NotFound";
            tempComment.Body = comment.Body;
            try
            {
                _commentsCollection.ReplaceOne(com => com.Id == tempComment.Id, tempComment);
            }
            catch (MongoWriteException e)
            {
                Console.WriteLine(e.Message);
                return "Error";
            }
            return "Done";
        }

        public string DeleteComment(CommentDto comment)
        {
            Comment tempComment = GetComment(comment.User, comment.ActivityId, comment.PostTime);
            if (tempComment is null)
                return "NotFound";
            try
            {
                _commentsCollection.DeleteOne(com => com.Id == tempComment.Id);
            }
            catch (MongoWriteException e)
            {
                Console.WriteLine(e.Message);
                return "Error";
            }
            return "Done";
        }
    }

}