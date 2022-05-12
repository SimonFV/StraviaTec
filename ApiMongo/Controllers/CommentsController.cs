using Microsoft.AspNetCore.Mvc;
using System;
using ApiMongo.DAL;
using ApiMongo.DTOs;
using ApiMongo.Entities;
using System.Collections.Generic;

namespace ApiMongo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly CommentsDAL _commentsDAL;
        public CommentsController(CommentsDAL commentsDAL)
        {
            _commentsDAL = commentsDAL;
        }

        [HttpGet]
        [Route("byUser/{user}")]
        public IActionResult GetCommentByUser(string user)
        {
            List<Comment> comments = _commentsDAL.GetByUser(user);
            if (comments is null)
                return new JsonResult("Something went wrong") { StatusCode = 500 };
            return Ok(comments);
        }

        [HttpGet]
        [Route("byActivity/{activityId}")]
        public IActionResult GetCommentByActivity(int activityId)
        {
            List<Comment> comments = _commentsDAL.GetByActivity(activityId);
            if (comments is null)
                return new JsonResult("Something went wrong") { StatusCode = 500 };
            return Ok(comments);
        }

        [HttpGet]
        [Route("byUserAndActivity/{user}/{activityId}")]
        public IActionResult GetCommentByUserAndActivity(string user, int activityId)
        {
            List<Comment> comments = _commentsDAL.GetByUserAndActivity(user, activityId);
            if (comments is null)
                return new JsonResult("Something went wrong") { StatusCode = 500 };
            return Ok(comments);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult PostComment(CommentDto comment)
        {
            if (ModelState.IsValid)
            {
                string result = _commentsDAL.InsertComment(comment);
                if (result is "Error")
                {
                    return new JsonResult("Something went wrong saving the comment.") { StatusCode = 500 };
                }
                else if (result is "Taken")
                {
                    return new JsonResult("Comment already in the database.") { StatusCode = 409 };
                }
                return new JsonResult("Comment added.") { StatusCode = 201 };
            }
            return new JsonResult("Invalid model for a Comment.") { StatusCode = 400 };
        }

        [HttpPut]
        [Route("edit")]
        public IActionResult UpdateComment(CommentDto comment)
        {
            if (ModelState.IsValid)
            {
                string result = _commentsDAL.EditComment(comment);
                if (result is "Error")
                {
                    return new JsonResult("Something went wrong updating the comment.") { StatusCode = 500 };
                }
                else if (result is "NotFound")
                {
                    return new JsonResult("Comment does not exist in the database.") { StatusCode = 409 };
                }
                return new JsonResult("Comment updated.") { StatusCode = 200 };
            }
            return new JsonResult("Invalid model for a Comment.") { StatusCode = 400 };
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult RemoveComment(CommentDto comment)
        {
            if (ModelState.IsValid)
            {
                string result = _commentsDAL.DeleteComment(comment);
                if (result is "Error")
                {
                    return new JsonResult("Something went wrong deleting the comment.") { StatusCode = 500 };
                }
                else if (result is "NotFound")
                {
                    return new JsonResult("Comment does not exist in the database.") { StatusCode = 409 };
                }
                return new JsonResult("Comment deleted.") { StatusCode = 200 };
            }
            return new JsonResult("Invalid model for a Comment.") { StatusCode = 400 };
        }
    }
}